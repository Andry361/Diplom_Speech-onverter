   M S B A M L   `   `   `  ����  Diploma_SpeechСonverter!    SpeechСonverter.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      H DMahApps.Metro, Version=1.1.3.5, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  5      Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923fb ^DynamicDataDisplay.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f?d34http://research.microsoft.com/DynamicDataDisplay/1.0   5      -   5          � Title$  Speech Сonverter��5      $	��350��5      $	��525��5         AccentColorBrush#���   5	      $��1q�5
       � WindowStartupLocation$ CenterScreen=�5      (  "MahApps.Metro.Controls.MetroWindow   EnableDWMDropShadow .  � 
ResizeMode$ CanResizeWithGrip=�5         WindowTransitionsEnabled .  $	��600��5      $
��1000��5         RightWindowCommands 5      +  %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5        $��OpenMenuMusicManagement��5       � ToolTip$a [Окрыть меню по упровленю музыкальной компазицией��5      .��5      �� $��22��6   $��22��5      ��5      ��  �� RelativeSource ��  �� AncestorType$ ButtonL�	 �� Path$	 
Foreground�
 J� OpacityMask
 5      !� 5       j� Stretch$ Uniform=�6&     appbar_music !� Visual# [ 5   &   5!      5"      �� 5#      -   5        $��SettingsButton��5#      $��Настройки��6+   6?   5$         Flyouts 5&      +  %MahApps.Metro.Controls.FlyoutsControl  5'   
   -   5          Name$ flyoutsControl��5'   "   .@�5(      #  MahApps.Metro.Controls.Flyout     Header$ Настройки��6      Position$ Right=�5)         AnimateOpacity . ��5+      ��  �� *FindAncestor+$ Controls:MetroWindowL�$	 ActualWidth� �� Mode$ OneWay=�.��5,      � $��Center=�5-      $��Center=�5.       � ColumnDefinitions 5/      �� 50      $
��auto �6+   �� 51      $
��auto �6+    � RowDefinitions 53      �� 54      $
��auto �6(   �� 55      $
��auto �6(   .+�57      !  MahApps.Metro.Controls.Tile  -   5        $��OpenFileTile��58      $��0��59      $��0��5:         Title$
 .wav��5;      $��Center=�5<      $	��200��5=      $	��200��5>      $��Открыть файл��5?      $��20(�5A      5B        5C      $��1��5D      $��0��5E      $ утилиты��5F      $��Center=�5G      $	��200��5H      $	��200��5I      $��Инструменты��5J      $��20(�5K      5M        5N      $��0��5O      $��1��5P      $
 .wav��5Q      $��Center=�5R      $	��200��5S      $	��200��5T      $��Сохрать Фаил��5U      $��20(�5V      5X        5Y      $��1��5Z      $��1��5[      $ помощь��5\      $��Center=�5]      $	��200��5^      $	��200��5_      $#��Открыть справку��5`      $��20(�5a      5c      5d      5e        5f      $ Музыка��6   $ Bottom=�5g      .��5h      �� $��80��6   $��5,5,5,5q�5i      $��Right=�5j       �� Orientation$ 
Horizontal=�5k      .+�5l      �� -   5        $��PlayWaveButton��5m      $L FВоспроизвести музыкальную компазицию��5n      $��50��5p      $��50��5q        MetroCircleButtonStyle#���  5r      .��5s      �� $��20��6$   $��20��5t   $   ��5u   $   �� $	 
Foreground� �� *FindAncestor+" ��
 5v      !� 5w   "   $
 Fill=�6.     appbar_control_play# �  5x   .   5z      5{      d� 5|      -   5        $��PauseWaveButton��5}      $F @Остановить музыкальную компазицию��5~      $��50��5�      $��50��5�      $��Center=�5�         MetroCircleToggleButtonStyle#���  5�      .��5�      �� $��20��5�       $��20��5�       ��5�       �� $	 
Foreground� �� *FindAncestor+" d�
 5�      !� 5�   "   $
 Fill=�6.     appbar_control_pause# �  5�   *   5�      5�      d� 5�      -   5        $��WaveRecodingButton��5�      $��50��5�   #   $ Записать речь��5�      $��50��5�      $��Center=�5�      #���  5�      .��5�      �� $��20��5�      $��25��5�      ��5�      �� $	 
Foreground� �� *FindAncestor+" d�
 5�      !� 5�   "   $
 Fill=�6.     appbar_microphone_google# �  5�   &   5�      5�      �� 5�      -	   5        $��SaveWaveFileButton��5�      $6 0Сохранить записанную речь��5�      $��50��5�      $��50��5�      #���  5�      .��5�      �� $��20��6$   $��20��5�      ��5�      �� $	 
Foreground� �� *FindAncestor+" ��
 5�      !� 5�   "   $
 Fill=�6.     appbar_save# �  5�   &   5�      5�      5�      5�      5�      .��5�      �  5�   
   �� 5�      .+�5�   
   �� $��0��6   .@�5�      )  #MahApps.Metro.Controls.MetroTabItem  $ ��Осциллограмма��6$   .��5�      ]� .+�5�      �� $ Vertical=�5�      .+�5�      d� -
   5        $��BildOscillogram��5�      $��50��5�   '   $3 -Построить осциллограмму��5�      $��50��5�      $��0, 10, 0, 0q�5�      #���  5�      .��5�      �� -   5        $��RectangleIconGraphs��5�      $��20��5�      $��20��5�      ��5�      �� $	 
Foreground� �� *FindAncestor+" d�
 5�   "   !� 5�   &   $
 Fill=�62     appbar_graph_line# �  5�   &   5�      5�      5�      8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  5�      -   5        $��plotter��5�      3  -Microsoft.Research.DynamicDataDisplay.Plotter   IsDefaultAxis .  $��
0,10,40,10q�5�         Children. 5�      2	  ,Microsoft.Research.DynamicDataDisplay.Header	  $��20(�6$   .��6<   Осциллограмма5�      =
  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle
  5�      .��6/   Амплитуда6:   ?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  5�      .��61   
Время68   S  MMicrosoft.Research.DynamicDataDisplay.Charts.Navigation.CursorCoordinateGraph  5�      5�      5�      5�        5�      $ ��Спектрограмма��6$   .��5�      �� $��30(�6   $��More cool transition��6*   5�        5�      $��Запись речи��6$   .��5�      �� -   5        $��ListMicrophones��5�      $	��200��5�      $��0, 10, 0, 0q�5�      $��0q�5�       �� SelectedIndex$ 0��5�      ��5�      �� $	 Microphones� �� View 5�      �� 5�      .P�5�      �� $��ID��5�   )    �� DisplayMemberBinding 5�   -   �� *MicrophoneId+5�   )   �� 5�      $��artist��5�   )    5�   -   �� *NameMicrophone+5�   )   5�      5�      5�      5�      5�      5�      