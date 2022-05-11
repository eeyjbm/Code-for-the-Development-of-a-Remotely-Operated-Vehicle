
#include <Arduino.h>
#include <i2c_driver.h>
#include <i2c_driver_wire.h>
#include "MS5837.h"
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>
#include <String>
#include <Servo.h>
#include <EEPROM.h>
#include "ArduPID.h"

ArduPID myController;


#define LeftLED 4
#define RightLED 5

#define BNO055_SAMPLERATE_DELAY_MS (50)
Adafruit_BNO055 bno = Adafruit_BNO055(55, 0x28);
sensors_event_t event;

MS5837 sensor;

Servo TiltServo;  // create servo object to control a servo
Servo PanServo;  // create servo object to control a servo
Servo LeftVThrustor;
Servo RightVThrustor;
Servo LeftHThrustor;
Servo RightHThrustor;

bool Connected = false;
int RecoveryCounter = 0;


int LeftVThrustorValuePrevious = 1500;
int RightVThrustorValuePrevious = 1500;
int LeftHThrustorValuePrevious = 1500;
int RightHThrustorValuePrevious = 1500;

int MotorHeave = 499;
int MotorSurge = 499;
int MotorYaw = 499;
int CameraPan = 90;
int CameraTilt = 90;
int Lights = 0;
bool DepthHold = false;
bool ARMED = false;

bool RecoveryON = false;
int RecoveryTimeDelay = 5;
int RecoveryThrustorPower = 100;
int WaterDensity = 997;
double PTerm = 1;
double DTerm = 1;
double ITerm = 1;
int MaxThrustorChange =  int(0.4 * BNO055_SAMPLERATE_DELAY_MS) ; // 400 per second - 20 per 50 ms

bool setPIDInput = true;
double PIDinput;
double PIDoutput;
double PIDsetpoint;


void setup()
{

  if (EEPROM.read(0) == 255)
  {
    EEPROM.write(1, (WaterDensity >> 0) & 0xFF);
    EEPROM.write(2, (WaterDensity >> 8) & 0xFF);
    EEPROM.write(3, MaxThrustorChange);
    EEPROM.write(4, (1 >> 0) & 0xFF);
    EEPROM.write(5, (1 >> 8) & 0xFF);
    EEPROM.write(6, (1 >> 0) & 0xFF);
    EEPROM.write(7, (1 >> 8) & 0xFF);
    EEPROM.write(8, (1 >> 0) & 0xFF);
    EEPROM.write(9, (1 >> 8) & 0xFF);
    EEPROM.write(10, ((int)RecoveryON));
    EEPROM.write(11, (RecoveryTimeDelay));
    EEPROM.write(12, RecoveryThrustorPower);
    EEPROM.write(0, 0);
  }
  else
  {
    WaterDensity = (EEPROM.read(2) << 8 ) | (EEPROM.read(1) & 0xff);
    MaxThrustorChange = EEPROM.read(3);
    PTerm = ((double)((EEPROM.read(4) << 8 ) | (EEPROM.read(5) & 0xff))) / 1000;
    DTerm = ((double)((EEPROM.read(6) << 8 ) | (EEPROM.read(7) & 0xff))) / 1000;
    ITerm = ((double)((EEPROM.read(8) << 8 ) | (EEPROM.read(9) & 0xff))) / 1000;
    RecoveryON = (bool)EEPROM.read(10);
    RecoveryTimeDelay = EEPROM.read(11);
    RecoveryThrustorPower = EEPROM.read(12);

  }

  Wire.begin();
  Wire1.begin(9);                // join i2c bus with address #4
  Wire1.onReceive(receiveEvent); // register event
  Wire1.onRequest(requestEvent);
  Serial.begin(9600);
  TiltServo.attach(0);  // attaches tilt servo
  PanServo.attach(1);  // attaches pan servo
  pinMode(4, OUTPUT); // lights
  pinMode(5, OUTPUT);
  analogWrite(LeftLED, 0);
  analogWrite(RightLED, 0);

  LeftVThrustor.attach(9);  //attach thrustors
  RightVThrustor.attach(10);
  LeftHThrustor.attach(11);
  RightHThrustor.attach(8);

  LeftVThrustor.writeMicroseconds(1500); // set esc to 0 (1100-1900)
  RightVThrustor.writeMicroseconds(1500);
  LeftHThrustor.writeMicroseconds(1500);
  RightHThrustor.writeMicroseconds(1500);

  TiltServo.write(90);              // tell servo to go to position in variable 'pos'
  PanServo.write(90);              // tell servo to go to position in variable 'pos'
  delay(7000); // ESC delay

  bno.begin();
  bno.setExtCrystalUse(true);

  while (!sensor.init()) {
    Serial.println("Init failed!");
    Serial.println("Are SDA/SCL connected correctly?");
    Serial.println("Blue Robotics Bar30: White=SDA, Green=SCL");
    Serial.println("\n\n\n");
    delay(1000);
  }


  sensor.setModel(MS5837::MS5837_30BA);
  sensor.setFluidDensity(WaterDensity); // kg/m^3 (freshwater, 1029 for seawater)


  myController.begin(&PIDinput, &PIDoutput, &PIDsetpoint, PTerm, ITerm, DTerm);
  myController.setOutputLimits(-100, 100);
  myController.start();
}

void loop()
{
  bno.getEvent(&event);
  sensor.read();
  static bool RecoveryActivated = false;
  
  if ((RecoveryCounter > (RecoveryTimeDelay * 1000) / BNO055_SAMPLERATE_DELAY_MS) && Connected && RecoveryON)
  {
    RecoveryCounter = ((RecoveryTimeDelay * 1000) / BNO055_SAMPLERATE_DELAY_MS) + 1;
    ARMED = false;

    int LeftVThrustorValue = map(RecoveryThrustorPower, 0, 100, 1500, 1100);
    if ((LeftVThrustorValue - LeftVThrustorValuePrevious) > MaxThrustorChange)
    {
      LeftVThrustorValue = LeftVThrustorValuePrevious + MaxThrustorChange;
    }
    if ((LeftVThrustorValue - LeftVThrustorValuePrevious) < -MaxThrustorChange)
    {
      LeftVThrustorValue = LeftVThrustorValuePrevious - MaxThrustorChange;
    }
    LeftVThrustor.writeMicroseconds(LeftVThrustorValue); // Send signal to ESC.
    LeftVThrustorValuePrevious = LeftVThrustorValue;

    int RightVThrustorValue = map(RecoveryThrustorPower, 0, 100, 1500, 1100);
    if (RightVThrustorValue - RightVThrustorValuePrevious > MaxThrustorChange)
  {
    RightVThrustorValue = RightVThrustorValuePrevious + MaxThrustorChange;
  }
  if ((RightVThrustorValue - RightVThrustorValuePrevious) < -MaxThrustorChange)
    {
      RightVThrustorValue = RightVThrustorValuePrevious - MaxThrustorChange;
    }
    RightVThrustor.writeMicroseconds(RightVThrustorValue); // Send signal to ESC.
    RightVThrustorValuePrevious = RightVThrustorValue;

    RecoveryActivated = true;

  }
  else
  {

    RecoveryActivated = false;
    
    if (ARMED)
    {
      TiltServo.write(constrain(CameraTilt, 35, 145));
      PanServo.write(constrain(CameraPan, 0, 180));
      // analogWrite(LeftLED, constrain(Lights, 0, 255));
      analogWrite(RightLED, constrain(Lights, 0, 255));

      if(DepthHold && MotorHeave == 499)
      {
        if(setPIDInput)
        {
          PIDsetpoint = sensor.depth();
          setPIDInput = false;
        }

       PIDinput = sensor.depth();
       myController.compute();
       
      int LeftVThrustorValue = map(PIDoutput, -100, 100, 1100, 1900);
      if ((LeftVThrustorValue - LeftVThrustorValuePrevious) > MaxThrustorChange)
      {
        LeftVThrustorValue = LeftVThrustorValuePrevious + MaxThrustorChange;
      }
      if ((LeftVThrustorValue - LeftVThrustorValuePrevious) < -MaxThrustorChange)
      {
        LeftVThrustorValue = LeftVThrustorValuePrevious - MaxThrustorChange;
      }
      LeftVThrustor.writeMicroseconds(LeftVThrustorValue); // Send signal to ESC.
      LeftVThrustorValuePrevious = LeftVThrustorValue;

      int RightVThrustorValue = map(PIDoutput, -100, 100, 1100, 1900);
      if (RightVThrustorValue - RightVThrustorValuePrevious > MaxThrustorChange)
      {
        RightVThrustorValue = RightVThrustorValuePrevious + MaxThrustorChange;
      }
      if ((RightVThrustorValue - RightVThrustorValuePrevious) < -MaxThrustorChange)
      {
        RightVThrustorValue = RightVThrustorValuePrevious - MaxThrustorChange;
      }
      RightVThrustor.writeMicroseconds(RightVThrustorValue); // Send signal to ESC.
      RightVThrustorValuePrevious = RightVThrustorValue;

       Serial.print(PIDsetpoint);
       Serial.print(" "); // a space ' ' or  tab '\t' character is printed between the two values.
  Serial.print(PIDinput);
  Serial.print(" "); // a space ' ' or  tab '\t' character is printed between the two values.
  Serial.print(PIDoutput);
  Serial.print(" "); // a space ' ' or  tab '\t' character is printed between the two values.
  Serial.println(PIDoutput - PIDinput); // the last value is followed by a carriage return and a newline characters.


      }
      else
      {
      setPIDInput = true;
      int LeftVThrustorValue = map(MotorHeave, 0, 998, 1100, 1900);
      if ((LeftVThrustorValue - LeftVThrustorValuePrevious) > MaxThrustorChange)
      {
        LeftVThrustorValue = LeftVThrustorValuePrevious + MaxThrustorChange;
      }
      if ((LeftVThrustorValue - LeftVThrustorValuePrevious) < -MaxThrustorChange)
      {
        LeftVThrustorValue = LeftVThrustorValuePrevious - MaxThrustorChange;
      }
      LeftVThrustor.writeMicroseconds(LeftVThrustorValue); // Send signal to ESC.
      LeftVThrustorValuePrevious = LeftVThrustorValue;

      int RightVThrustorValue = map(MotorHeave, 0, 998, 1100, 1900);
      if (RightVThrustorValue - RightVThrustorValuePrevious > MaxThrustorChange)
      {
        RightVThrustorValue = RightVThrustorValuePrevious + MaxThrustorChange;
      }
      if ((RightVThrustorValue - RightVThrustorValuePrevious) < -MaxThrustorChange)
      {
        RightVThrustorValue = RightVThrustorValuePrevious - MaxThrustorChange;
      }
      RightVThrustor.writeMicroseconds(RightVThrustorValue); // Send signal to ESC.
      RightVThrustorValuePrevious = RightVThrustorValue;
      }
      int LeftHThrustorValue = map(constrain((MotorSurge - 499) - (MotorYaw - 499), -499, 499), -499 , 499, 1100, 1900);
      if ((LeftHThrustorValue - LeftHThrustorValuePrevious) > MaxThrustorChange)
      {
        LeftHThrustorValue = LeftHThrustorValuePrevious + MaxThrustorChange;
      }
      if ((LeftHThrustorValue - LeftHThrustorValuePrevious) < -MaxThrustorChange)
      {
        LeftHThrustorValue = LeftHThrustorValuePrevious - MaxThrustorChange;
      }
      LeftHThrustor.writeMicroseconds(LeftHThrustorValue);
      LeftHThrustorValuePrevious = LeftHThrustorValue;

      int RightHThrustorValue = map(constrain((MotorSurge - 499) + (MotorYaw - 499), -499, 499), -499 , 499, 1100, 1900);
      if ((RightHThrustorValue - RightHThrustorValuePrevious) > MaxThrustorChange)
      {
        RightHThrustorValue = RightHThrustorValuePrevious + MaxThrustorChange;
      }
      if ((RightHThrustorValue - RightHThrustorValuePrevious) < -MaxThrustorChange)
      {
        RightHThrustorValue = RightHThrustorValuePrevious - MaxThrustorChange;
      }
      RightHThrustor.writeMicroseconds(RightHThrustorValue);
      RightHThrustorValuePrevious = RightHThrustorValue;

    }
    else
    {

      int LeftVThrustorValue = 1500;
      if ((LeftVThrustorValue - LeftVThrustorValuePrevious) > MaxThrustorChange)
      {
        LeftVThrustorValue = LeftVThrustorValuePrevious + MaxThrustorChange;
      }
      if ((LeftVThrustorValue - LeftVThrustorValuePrevious) < -MaxThrustorChange)
      {
        LeftVThrustorValue = LeftVThrustorValuePrevious - MaxThrustorChange;
      }
      LeftVThrustor.writeMicroseconds(LeftVThrustorValue); // Send signal to ESC.
      LeftVThrustorValuePrevious = LeftVThrustorValue;

      int RightVThrustorValue = 1500;
      if (RightVThrustorValue - RightVThrustorValuePrevious > MaxThrustorChange)
      {
        RightVThrustorValue = RightVThrustorValuePrevious + MaxThrustorChange;
      }
      if ((RightVThrustorValue - RightVThrustorValuePrevious) < -MaxThrustorChange)
      {
        RightVThrustorValue = RightVThrustorValuePrevious - MaxThrustorChange;
      }
      RightVThrustor.writeMicroseconds(RightVThrustorValue); // Send signal to ESC.
      RightVThrustorValuePrevious = RightVThrustorValue;

    }
  }


  delay(BNO055_SAMPLERATE_DELAY_MS);
  RecoveryCounter ++;
}


void receiveEvent(int howMany)
{
  char InputBuffer[30];
  int LenghtOfData = 0;
  int InputBufferPosition;
  int Temp;
  while (1 < Wire1.available()) // loop through all but the last
  {
    InputBuffer[LenghtOfData] = Wire1.read(); // receive byte as a character
    LenghtOfData++;
  }
  InputBuffer[LenghtOfData] = Wire1.read();    // receive byte as an integer

  switch (InputBuffer[0]) {
    case 'H':
      Connected = true;
      RecoveryCounter = 0;

      InputBufferPosition = 2;
      Temp = 0;
      Temp =  InputBuffer[1] - '0';
      if (InputBuffer[InputBufferPosition] != 'S')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'S')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
        }
      }
      MotorHeave = Temp;
      InputBufferPosition++;

      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBuffer[InputBufferPosition] != 'Y')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');

        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'Y')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
        }
      }
      MotorSurge = Temp;
      InputBufferPosition++;

      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBuffer[InputBufferPosition] != 'T')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'T')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
        }
      }
      MotorYaw = Temp;
      InputBufferPosition++;

      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBuffer[InputBufferPosition] != 'P')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
      }
      CameraTilt = Temp * 10;
      InputBufferPosition++;

      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBuffer[InputBufferPosition] != 'L')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
      }
      CameraPan = Temp * 5 + 35;
      InputBufferPosition++;

      Lights =  (InputBuffer[InputBufferPosition] - '0') * 51;
      InputBufferPosition++;
      ARMED = InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      DepthHold = InputBuffer[InputBufferPosition] - '0';


      break;

    case 'T':
      InputBufferPosition = 4;
      Temp = 0;
      RecoveryTimeDelay = InputBuffer[1] - '0';
      EEPROM.write(11, (RecoveryTimeDelay));

      Temp =  InputBuffer[3] - '0';
      if (InputBuffer[InputBufferPosition] != 'W')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'W')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
        }
      }
      RecoveryThrustorPower = Temp;
      EEPROM.write(12, Temp);
      InputBufferPosition++;

      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBuffer[InputBufferPosition] != 'R')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');

        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'R')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
          if (InputBuffer[InputBufferPosition] != 'R')
          {
            Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
            InputBufferPosition++;
          }
        }
      }
      WaterDensity = Temp;
      EEPROM.write(1, (WaterDensity >> 0) & 0xFF);
      EEPROM.write(2, (WaterDensity >> 8) & 0xFF);
      InputBufferPosition++;

      RecoveryON = InputBuffer[InputBufferPosition] - '0';
      EEPROM.write(10, ((int)RecoveryON));


      break;

    case 'P':
      InputBufferPosition = 2;
      Temp = 0;
      Temp =  InputBuffer[1] - '0';
      if (InputBuffer[InputBufferPosition] != 'D')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'D')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
          if (InputBuffer[InputBufferPosition] != 'D')
          {
            Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
            InputBufferPosition++;
          }
        }
      }
      PTerm = (double(Temp) / 1000);
      EEPROM.write(4, (Temp >> 0) & 0xFF);
      EEPROM.write(5, (Temp >> 8) & 0xFF);
      InputBufferPosition++;

      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBuffer[InputBufferPosition] != 'I')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'I')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
          if (InputBuffer[InputBufferPosition] != 'I')
          {
            Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
            InputBufferPosition++;
          }
        }
      }
      DTerm = (double(Temp) / 1000);
      EEPROM.write(6, (Temp >> 0) & 0xFF);
      EEPROM.write(7, (Temp >> 8) & 0xFF);
      InputBufferPosition++;

      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBuffer[InputBufferPosition] != 'T')
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
        if (InputBuffer[InputBufferPosition] != 'T')
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
          if (InputBuffer[InputBufferPosition] != 'T')
          {
            Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
            InputBufferPosition++;
          }
        }
      }
      ITerm = (double(Temp) / 1000);
      EEPROM.write(8, (Temp >> 0) & 0xFF);
      EEPROM.write(9, (Temp >> 8) & 0xFF);
      InputBufferPosition++;


      Temp =  InputBuffer[InputBufferPosition] - '0';
      InputBufferPosition++;
      if (InputBufferPosition <= LenghtOfData)
      {
        Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
        InputBufferPosition++;
        if (InputBufferPosition <= LenghtOfData)
        {
          Temp = Temp * 10 + (InputBuffer[InputBufferPosition] - '0');
          InputBufferPosition++;
        }
      }
      MaxThrustorChange = (Temp * BNO055_SAMPLERATE_DELAY_MS) / 100;
      EEPROM.write(3, MaxThrustorChange);


      break;
    case 'Z':
      Connected = false;
      ARMED = false;
      break;
  }
}

void requestEvent()
{
  char OutputBuffer[23];
  const String data("P" + String((event.orientation.z + 180), 0) + "Y" + String(event.orientation.x, 0) + "R" + String((event.orientation.y + 90), 0) + "D" + String(sensor.depth(), 0) + "T" + String(sensor.temperature() * 10, 0));
  data.toCharArray(OutputBuffer, 23);
  Wire1.write(&OutputBuffer[0], 23);
}
