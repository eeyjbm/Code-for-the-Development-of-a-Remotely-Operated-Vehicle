   M S B A M L   `   `   `  ����H  DROV Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ROV_InterfaceROV_Interface  5   	   W0clr-namespace:ROV_Interface.Views.TelemetryViews"ROV_Interface.Views.TelemetryViews  5   	   k:clr-namespace:ROV_Interface.ViewModels.TelemertyViewModels,ROV_Interface.ViewModels.TelemertyViewModels  5	   	   U/clr-namespace:ROV_Interface.Views.SettingsViews!ROV_Interface.Views.SettingsViews  5
   	   i9clr-namespace:ROV_Interface.ViewModels.SettingsViewModels+ROV_Interface.ViewModels.SettingsViewModels  5   	       ROV_Interface.MainWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:ROV_Interface     5   	   KTelemetryViews0clr-namespace:ROV_Interface.Views.TelemetryViews     5   	   ZTelemertyViewModels:clr-namespace:ROV_Interface.ViewModels.TelemertyViewModels     5	   	   ISettingsViews/clr-namespace:ROV_Interface.Views.SettingsViews     5
   	   XSettingsViewModels9clr-namespace:ROV_Interface.ViewModels.SettingsViewModels     5   	     � Title$  
MainWindow��5   	   $	��600��6   $
��1000��6,    � WindowState$ 	Maximized=�6<    � WindowStyle$
 None=�6T    � 	Resources 5         Button.Static.Background �� Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border  Button.Disabled.Foreground 	 ButtonStyleWhite �� 
TargetType �� Property �� Value �� 
TargetType	 ��Name
 J� SnapsToDevicePixels ��Name �� Triggers P� Property P� Value �� 
TargetName 
 Button.Static.Background1  Button.Static.Border1   Button.MouseOver.Background1  Button.MouseOver.Border1  Button.Pressed.Background1  Button.Pressed.Border1  Button.Disabled.Background1  Button.Disabled.Border1  Button.Disabled.Foreground1  BasicButton  FocusVisual �� StrokeDashArray  Button.Static.Background2  Button.Static.Border2  Disabled��%�  &	        &	 %     &	 D     &	 g     &	 �     &	 �     &	 �     &	 �     &	     &		 +    7   7  7  7  7  7  7  7  7  &	
 �    &	 !    &	 @    &	 e    &	 �    &	 �    &	 �    &	 �    &	     &	 C    7 
 7  7  7  7  7  7  7  7  &	     &	 �    &	 �    &	 	    7  7  7  �� 5   
   $ Transparent��6;   �� 5   
   $ White��67   �� 5   
   $ 	#FFB8B8B8��6>   �� 5   
   $ White��6:   �� 5   
   $ 	#FFB8B8B8��6<   �� 5   
   $ White��68   �� 5   
   $ Transparent��6=   �� 5   
   $ White��69   �� 5   
   $ White��6=   �� 5   
   " ��6)   .�5      ��  � ��8   6+   �� 5       � ��8  6,   �� 5       � ��# � ��6+   �� 5       � ��$ 1q�60   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5        � ��$ 1q�6(   �� 5!       � �� 5"      �� 5#      " ��6&   .R�5$      �� $	 border��6!   #��z��61   #��z��6[   #��z��6�   $

 true��6�   ._�5%      �� $ contentPresenter��6/   $|�False��6I   #��z��6[   #��z��6�   $
��True��6�   #
 z
 6�   #��z��6  5&       5'      P� 5(       �@��IsDefaulted$
 true��6=   . �5)   "   �� $ border��6@   # � ��6T    � ��5*      P� 5+       �@J�IsMouseOver$
 true��6=   . �5,   "   �� $ border��6?   8   � ��6S   �� 5-   "   $ border��6@   8   � ��6T   5.      P� 5/       � ��$
 true��6;   . �50   "   �� $ border��6?   8   � ��6S   �� 51   "   $ border��6@   8   � ��6T   52      P� 53       � {�$ false��6;   . �54   "   �� $ border��6?   8   � ��6S   �� 55   "   $ border��6@   8   � ��6T   �� 56   "   $ contentPresenter��6K   8   � ��6i   57      59      5;      5<      �� 5?   
   $ Transparent��6<   �� 5@   
   $ White��68   �� 5A   
   $ Transparent��6?   �� 5B   
   $ Transparent��6;   �� 5C   
   $ Transparent��6=   �� 5D   
   $ Transparent��69   �� 5E   
   $ Transparent��6>   �� 5F   
   $ Transparent��6:   �� 5G   
   $ Transparent��6>   �� 5H   
   " ��6$   .�5I      ��  � ��8   6+   �� 5J       � ��8  6,   �� 5K       � ��# � ��6+   �� 5L       � ��$ 1q�60   �� 5M       � ��$ Center=�6;   �� 5N       � ��$ Center=�69   �� 5O       � ��$ 1q�6(   �� 5P       � �� 5Q      �� 5R      " ��6&   .R�5S      �� $	 border��6!   #��z��61   #��z��6[   #��z��6�   $

 true��6�   ._�5T      �� $ contentPresenter��6/   $|�False��6I   #��z��6[   #��z��6�   $
��True��6�   #
 z
 6�   #��z��6  5U       5V      P� 5W       �@��IsDefaulted$
 true��6=   . �5X   "   �� $ border��6@   # � ��6T    � ��5Y      P� 5Z       �@J�IsMouseOver$
 true��6=   . �5[   "   �� $ border��6?   8   � ��6S   �� 5\   "   $ border��6@   8   � ��6T   5]      P� 5^       � ��$
 true��6;   . �5_   "   �� $ border��6?   8   � ��6S   �� 5`   "   $ border��6@   8   � ��6T   5a      P� 5b       � {�$ false��6;   . �5c   "   �� $ border��6?   8   � ��6S   �� 5d   "   $ border��6@   8   � ��6T   �� 5e   "   $ contentPresenter��6K   8   � ��6i   5f      5h      5j      5k      �� 5l   
   .�5m      ��  � �� 5n      �� 5o      .R�5p      �� $��2q�6$   $	 1 2R�6/   $

 true��6E   $��1��6`   #��� ��6t   5q      5s      5t      �� 5v   
   $ Transparent��6<   �� 5w   
   $	 Red��68   �� 5x   
   " ��6!   .�5y      ��  �@�FocusVisualStyle8   61   �� 5z       � ��8  6+   �� 5{       � ��8  6,   �� 5|       � ��# � ��6+   �� 5}       � ��$ 1q�60   �� 5~       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5�       � ��$ 1q�6(   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� $	 border��6!   #��z��61   #��z��6[   #��z��6�   $

 true��6�   ._�5�      �� $ contentPresenter��6/   $|�False��6I   #��z��6[   #��z��6�   $
��True��6�   #
 z
 6�   #��z��6  5�      5�      5�      5�      5�      '  !System.Windows.Shell.WindowChrome   WindowChrome 5�        5�   
      CaptionHeight$ 30Q�6   ��5�      �� 5�   
    �� ImageSource$ Media/Reef.jfif��6    j� Stretch$ UniformToFill=�63   .��5�      �  � ColumnDefinitions 5�   
   �� 5�      $��20 �6   �� 5�      $	��201 �6   �� 5�      $��3* �6   �� 5�      $	��201 �6   �� 5�      $��20 �6    � RowDefinitions 5�   
   �� 5�      $��30 �6   �� 5�      $��20 �6   �� 5�      $	��380 �6   �� 5�      $��3* �6   .+�5�   
   �� -   5         ��Name$ streamContent��5�      $��2��60   $��2��6@   $��2��6M   6`   � 5�   
   $��5��6   $��30��6#   .+�5�      �� $��0��6   $��0��6%   $��5��65   
���iii� J� Opacity$	 0.9Q�6X   �� 5�       �� Orientation$ 
Horizontal=�6   $��0��62   $��0��6?   $��2��6O   $
��left=�6c   .+�5�      �� $ 
Horizontal=�6      IsHitTestVisibleInChrome$
 True��66   .+�5�      �� #���  6   $��5,0,5,0q�6e   $��Center=�6v   $��1��6�   
������ 
������ ��6A   �� *SettingsCommand+.��5�      �� $"��Media/icons8_settings_64.png��6   5�      �� 5�      #���  6   $��1��6f   
������ 
������ $��0,1,0,1q�6�   ��6@   �� *TelemetryCommand+.��5�      �� $��Media/compass_50px.png��6   5�      �� 5�      -   5        #��� 	 5�      $��Connect��6i   $��Center=�6{   $��1��6�   
������ 
�������$��DemiBold"�6�   $��14(�6�   $��2q�6�   
�������$��5,0,3,0q�6  $��Stretch=�6.  ��6F   �� *ConnectCommand+ 5�      ��%^   '��       �� 5�      " ��6    .�5�      ��  �@��CornerRadius$ 5��6=   5�      5�      5�      �� 5�      -   5        #��� 	 5�      $��Refresh��6j   $��Center=�6|   $��1��6�   $��2��6�   
������ 
�������$��DemiBold"�6�   $��14(�6�   $��2q�6  
�������$��5,0,5,0q�6-  $��Stretch=�6>  ��6E   �� *TelemetryCommand+ 5�      ��%^   '��       �� 5�      " ��6    .�5�      ��  �@��CornerRadius$ 5��6=   5�      5�      5�      5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� $��Gear ��6    $��5,0,0,0q�6-   
�������$��14(�6Q   $��DemiBold"�6_   $��Center=�6u   $��Center=�6�   �� 5�      
�������$��15(�6I   $��DemiBold"�6W   $��Center=�6m   $��Center=�6�   ��6    �� *Gear+6�   5�      5�      �� 5�      $ 
Horizontal=�6   $��0��63   $��2��6@   $��Center=�6P   .+�5�      �� $��Media/Controller_64.png��6   $��2q�65   �� 5�      $��40��6   $��6��6#   $��8,0,8,0q�6S    �� RadiusX$ 3��6d    �� RadiusY$ 3��6p   ��6.   �� *RemoteConnectColour+6p   �� 5�      $ ��Media/icons8_laptop_64.png��6   $��2q�68   �� 5�      $��40��6   $��6��6#   $��8,0,8,0q�6P   $ 3��6a   $ 3��6m   ��6.   �� *ROVConnectColour+6m   �� 5�      $	��ROV��6   $��Center=�6#   
�������$��DemiBold"�6Q   $��14(�6g   5�      �� 5�   
   $ 
Horizontal=�6   $��0��6/   $��2��6<   $��3��6L   $��right=�6`   .+�5�      �� $
 True��6   $��Center=�6J   $��Right=�6e   $ 
Horizontal=�5�      $��35��62   .+�5�      �� #���  6   
���  ��$��LOW BATTERY��6v   $��Center=�6�   $��1��6�   $��2��6�   $��DemiBold"�6�   $��17(�6�   $��2q�6�   $��5,0,20,0q�6�   $��Stretch=�6  ${�False��6/  y�6@   �� *BatteryStatus+ 5�      ��%^   '��       �� 5�      " ��6$   .�5�   "   ��  �@��CornerRadius$ 5��6A   5�      5�      5�      �� 5�      -   5        $��MinimizeButton��5�   (   #��� 	 5�      
�������$��35��6\   $��35��6g   $��-��6s   $
��Bold"�6�   $��23(�5�      
������ $��0q�6W    � ToolTip$ Minimize��6k   �� 5�      -   5        $��MaximizeButton��5�   '   #��� 	 5�      
�������$��35��6[   $��35��6f   $��+��6s   $��DemiBold"�6   $��21(�5�      
������ $��0q�6V   $ Restore Down��6j   �� 5�      -   5        $��CloseButton��5�   '   #��� 	 5�      
�������$��35��6[   $��35��6f   $��x��6r   $��DemiBold"�6~   $��18(�5�      
������ $��0q�6S   $ Close��6g   6w   5�      5�      5�      �� 5�   
   $��2��6   $��3��6&   ��66   �� *CurrentTelemetryViewModel+y�6e   �� *TelemetryPanelVisibility+ 5�       �� DataTypeN   HROV_Interface.ViewModels.TelemertyViewModels.TelemetryPanelMainViewModel?   9ROV_Interface.Views.TelemetryViews.TelemetryPanelMainView � DataContextN   HROV_Interface.ViewModels.TelemertyViewModels.TelemetryPanelSubViewModel1?   9ROV_Interface.Views.TelemetryViews.TelemetryPanelSubView1��%�   (��       5�   G   *, +)(�� c     5�   G   *, +)�� 5�      "TelemetryPanelMainViewTemplate��"  6G   .R�5�         6<   �� 5�      �� 5�      "TelemetryPanelSubView1Template��"  6G   .R�5�         6<   �� 5�      5�      5�      �� 5�   
   $��2��6   $��1��6&   ��66   �� *CurrentSettingsViewModel+y�6c   �� *SettingsPanelVisibility+ 5�      L   FROV_Interface.ViewModels.SettingsViewModels.SettingsPanelMainViewModel=   7ROV_Interface.Views.SettingsViews.SettingsPanelMainViewL   FROV_Interface.ViewModels.SettingsViewModels.SettingsPanelSubViewModel1=	   7ROV_Interface.Views.SettingsViews.SettingsPanelSubView1L
   FROV_Interface.ViewModels.SettingsViewModels.SettingsPanelSubViewModel2=   7ROV_Interface.Views.SettingsViews.SettingsPanelSubView2��%q  (��       5�   F   *, +)(�� b     5�   F   *, +)(�� �     5�   F   *,
 +)�� 5�      !SettingsPanelMainViewTemplate��"  6F   .R�5�         6:   �� 5�      �� 5�      !SettingsPanelSubView1Template��"  6F   .R�5�      	   6:   �� 5�      �� 5�      !SettingsPanelSubView2Template��" 
 6F   .R�5�         6:   �� 5�      5�      5�      5�      5�      