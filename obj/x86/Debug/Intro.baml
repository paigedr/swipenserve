   M S B A M L   `   `   `  ����^  ZMicrosoft.Samples.Kinect.WpfViewers, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null�^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers#Microsoft.Samples.Kinect.WpfViewers  5   7    WpfApplication1   Controls.Intro   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      mmy^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers     5   7   .��5      �� -   5        $��
MainCanvas��5      .+�5
   
   �� -   5        $��slider��5
   P   $	��125��6   $
��Left=�6!   $��
95,333,0,0q�6<   
����  �
�������$	��Top=�6�   $	��564��6�     �� Left$	  -56��6�   
 �� Top$ 1��6�   �� 5   
   -   5        $��label1��5   |   $��Welcome to Pajama Foods!��6   $��36(�63   $��65��6A   $
��Left=�6M   $��
178,20,0,0q�6h   $	��Top=�6�   $	��444��6�   $	  -71��6�   $ -8��6�   r� 5   
   -   5        $��
introVideo��5       r� Source$ Images\intro.m4v?�6)    K� ZIndex$ 2��6C   $	��165��6T   $	��259��6a   $��200,150,0,0q�6m    � Triggers 5      :� 5       :� RoutedEvent$ MediaElement.Loaded��6   [�5      �� 5      .��5      �� .��5   "   p� -   5         g�Name$	 GRR��5   0    p� Source$Y SC:\Users\Mac2492\Documents\Visual Studio 2010\Projects\swipenserve\Images\intro.m4v?�6;    �� 
TargetName$ 
introVideo��6�   	 g� RepeatBehavior$	 Forever��6�   5      5      5      5      �� 5   
   -   5        $��label2��5   �   $+��%To order with gestures, please swipe.��6   
�������$��24(�6R   $��44��6`   $
��Left=�6l   $��
103,68,0,0q�6�   $	��Top=�6�   $	��398��6�   $  16��6�   $ 15��6�   �� 5   
   $$��Swipe with left hand to order!��6   $��78��69   $��40(�6E   $	 360��6S   $	��535��6d   $  53��6p   6�   5      5      