   M S B A M L   `   `   `  ����O  KVM_Optimization_Tool, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null;"clr-namespace:VM_Optimization_ToolVM_Optimization_Tool  5   	   -    'VM_Optimization_Tool.SelectOptimization   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4local"clr-namespace:VM_Optimization_Tool     5   	   -   5          � Title$  Optimization Options��5   	   $	��526��5   	   $	��726��6   $	��500��6"   $	��680��62    � WindowStartupLocation$ CenterScreen=�6A    � 	Resources 5      6  0VM_Optimization_Tool.TrueFalseToBooleanConverter   TrueFalseToBooleanConverter  xmlData � Source � XPath  NodeTemplate �� ItemsSource �� XPath �� Orientation ��Name	 d� 	IsChecked
 �� Mode �� 	Converter ��Name �� Triggers �� Value �� Binding �� Path �� 
TargetName �� Value �� Property��%F  &	        &	      &	 N     7     5   
   6,   � 5	   
   $  ?�6*   $ /sequence/group/group��64   �� 5   
    5      �� $ .//step��.R�5      �� $��
stackpanel��6   $ 
Horizontal=�6+   .+�5      �� $ 	nodecheck��6   -   5        	 5   e   �� $ @defaultSelected��$
 TwoWay=�8   .��5      �� $ nodetext��6    $	��5,0q�62   5      5       5      �� 5      $ Element��6@    6   �� $ NodeType�.a�5      �� $ nodetext��6    5      �� $ Attributes[1].Value� � ��5      5      5      .��5      �  � DataContext# [ 6   $��4q�62    � RowDefinitions 5   
   �� 5       $
��100* �6   �� 5!      $��50��6   $	��10* �6+   �� 5"      $��50��6   $	��10* �6+   .+�5%   
   R� -   5        $��trvItems��5%      $��0��6#   #��[ 5&      $��Stretch=�5'      $��Stretch=�5(       %� IsVirtualizing .   %� VirtualizationMode$ Standard=�5+      ��5)      �� ��5,      �� 5-       �� 
TargetType" Q�6   .�5.      ��  �@Q�
IsExpanded . 5/      50      52      �� 54   
   $��50��6   $ 
Horizontal=�6$   $��2��6=   $��Center=�55      .+�56      �� -   5        $��btnLoad��56      $��1��6$   $��4q�61   $��Start��57      $	��100��6)   58      �� 59      -   5        $��btnAbort��59      $��1��6%   $��4q�62   $��Abort��5:      $	��100��6)   5;      5<      � 5>   
   -   5         �Name$ progressBar��5>      $��1��6+   $��Stretch=�68   $��30��6V   $��0,4,0,0q�6e   $��Stretch=�6v    � Minimum$ 0Q�6�    � Maximum$	 100Q�6�   5A      5B      