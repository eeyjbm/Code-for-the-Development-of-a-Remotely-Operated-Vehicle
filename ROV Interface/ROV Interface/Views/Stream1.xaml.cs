using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GLib;
using Gst;
using Gst.App;
using Gst.Video;
using Thread = System.Threading.Thread;

namespace ROV_Interface.Views
{
    /// <summary>
    /// Interaction logic for Stream1.xaml
    /// </summary>



        /// <summary>
        /// Interaction logic for Stream1.xaml
        /// </summary>
        public partial class Stream1 : UserControl
        {
            private MainLoop _mainLoop;
            private Thread _mainGLibThread;
            private IntPtr _windowHandle;
            private VideoOverlayAdapter _adapter;
            private Pipeline _pipe;

            private const string BusMessageError = "message::error";
            private const string BusMessageEos = "message::eos";
            private const string BusMessageStateChanged = "message::state-changed";
            private const string BusMessageApplication = "message::application";

            public Stream1()
            {
                InitializeComponent();

                Gst.Application.Init();
                GtkSharp.GstreamerSharp.ObjectManager.Initialize();
                _mainLoop = new GLib.MainLoop();
                _mainGLibThread = new System.Threading.Thread(_mainLoop.Run);
                _mainGLibThread.Start();

                InitGStreamerPipeline();
            }

            public void Closed()
            {
                _pipe.SetState(State.Null);
                _pipe.Dispose();
                _mainLoop.Quit();
            }

            public void Activated()
            {
                _windowHandle = VideoPanel.Handle;
            }

            private void InitGStreamerPipeline()
            {
                _pipe = (Pipeline)Parse.Launch("udpsrc port=5200 ! application/x-rtp, media=video, clock-rate=90000, payload=96 ! rtpjpegdepay ! jpegdec ! videoconvert ! autovideosink");//tcpclientsrc host=192.168.8.185 port=5000 ! gdpdepay ! rtph264depay ! avdec_h264 ! videoconvert ! autovideosink sync=false");             //   "rtspsrc location=rtsp://localhost:8554/test latency=100 ! queue ! rtph264depay ! h264parse ! avdec_h264 ! videoconvert ! videoscale ! video/x-raw,width=640,height=480 ! autovideosink"); 

                _pipe.Connect("video-tags-changed", TagsCb);
                _pipe.Connect("audio-tags-changed", TagsCb);
                _pipe.Connect("text-tags-changed", TagsCb);

                var bus = _pipe.Bus;
                bus.AddSignalWatch();
                bus.EnableSyncMessageEmission();
                bus.SyncMessage += OnBusSyncMessage;
                bus.Connect(BusMessageError, ErrorCb);
                bus.Connect(BusMessageEos, EosCb);
                bus.Connect(BusMessageStateChanged, StateChangedCb);
                bus.Connect(BusMessageApplication, ApplicationCb);

                GLib.Timeout.Add(1000, RefreshPlaybinInfo);
            }

            private void OnBusSyncMessage(object o, SyncMessageArgs sargs)
            {
                var message = sargs.Message;

                if (!Gst.Video.Global.IsVideoOverlayPrepareWindowHandleMessage(message))
                    return;

                var src = message.Src as Element;
                if (src == null)
                    return;

                try
                {
                    src["force-aspect-ratio"] = true;
                }
                catch { /* ignored */ }

                _adapter = new VideoOverlayAdapter(src.Handle);
                _adapter.WindowHandle = _windowHandle;
                _adapter.HandleEvents(true);
            }

            private bool RefreshPlaybinInfo()
            {
                _pipe.QueryDuration(Format.Time, out var durationTime);
                _pipe.QueryPosition(Format.Time, out var pos);

                var stateChangeReturn = _pipe.GetState(out var state, out var pending, 100);



                return true;
            }



            private void TagsCb(object o, SignalArgs args)
            {
                var playbin = (Gst.Element)o;
                var s = new Structure("tags-changed");
                playbin.PostMessage(Gst.Message.NewApplication(playbin, s));
            }

            private void ApplicationCb(object o, SignalArgs args)
            {
                var msg = (Gst.Message)args.Args[0];
                if (msg.Structure.Name == "tags-changed")
                {
                    // Handle re-analysis of the streams
                    // See https://github.com/ttustonic/GStreamerSharpSamples/blob/master/WpfSamples/BasicTutorial05.xaml.cs#L173
                    // This may be required when we want to broadcast and select between multiple video streams from the same source
                }
            }

            private void StateChangedCb(object o, SignalArgs args)
            {
                var msg = (Gst.Message)args.Args[0];
                msg.ParseStateChanged(out var oldstate, out var newstate, out var pending);
            }

            private void EosCb(object o, SignalArgs args)
            {
                Console.WriteLine("End of stream");
                _pipe.SetState(State.Ready);
            }

            private void ErrorCb(object o, SignalArgs args)
            {
                var msg = (Gst.Message)args.Args[0];
                msg.ParseError(out var exc, out var debug);

                Console.WriteLine($"Error received from element {msg.Src}: {exc.Message}");
                Console.WriteLine($"Debug info: {debug ?? "None"}");

                _pipe.SetState(State.Ready);
            }

        public void PlayClicked()
            {
                var state = _pipe.SetState(Gst.State.Playing);
                Console.WriteLine(state.ToString());
            }

        public void PauseClicked()
            {
                var state = _pipe.SetState(Gst.State.Paused);
                Console.WriteLine(state.ToString());
            }

            public void StopClicked()
            {
                var state = _pipe.SetState(Gst.State.Ready);
                Console.WriteLine(state.ToString());
            }
        }
    

}


