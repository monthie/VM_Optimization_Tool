   M S B A M L   `   `   `  ����O  KVM_Optimization_Tool, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null;"clr-namespace:VM_Optimization_ToolVM_Optimization_Tool  5   	   0    *VM_Optimization_Tool.PreOptimizationWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:VM_Optimization_Tool     5   	     � Title$  Select Optimization Steps��5   	   $��366.763��6+   $��339.595��6<    � 
ResizeMode$ CanMinimize=�6L   .��5	      � -   5        $��gridName��5	      .+�5
   
   �� -   5        $��checkCleanMgr��5
   6    � ToolTip$ Clears unnecessary files��6   $��Clean Manager��6K   $
��Left=�6c   $��	99,82,0,0q�6~   $	��Top=�6�    d� 	IsChecked$
 TrueY�6�   �� 5   
   -   5        $��	checkDism��5   6   $ Clears old updated files��6   $
��DISM��6G   $
��Left=�6V   $��
99,148,0,0q�6q   $	��Top=�6�   $
 TrueY�6�   �� 5   
   -   5        $��checkDefrag��5   /   $ Defragments files��6   $��Defragmentation��6B   $
��Left=�6\   $��
99,168,0,0q�6w   $	��Top=�6�   $
 TrueY�6�   �� 5   
   -   5        $��checkSDelete��5   -   $ Zero free space��6   $��SDelete��6A   $
��Left=�6S   $��
99,188,0,0q�6n   $	��Top=�6�   $
 TrueY�6�   �� 5   
   -   5        $��start_Button��5      $��Start��6%   $
��Left=�65   $��200,276,0,0q�6P   $	��Top=�6e   $��75��6}    J� RenderTransformOrigin$ -1.043,-2.397&�6�   6�   �� 5   
   -   5        $��abort_Button��5      $��Abort��6%   $
��Left=�65   $��
58,276,0,0q�6P   $	��Top=�6d   $��75��6|   6�   �� 5   
   $ ��Select Optimization Tasks:��6   $
��Left=�65   $��	26,51,0,0q�6P   $	��Top=�6c   �� 5   
   -   5        $��
stackPanel��5      $��112,99,21,183q�6'   .+�5      	� -	   5        $��radioButton1��5   G   $( "Clear most common unnecessary data��6    	� 	GroupName$ CleanMgr��6[   $ ��Clear automatic (standard)��6p   $
 TrueY�6�   	� 5      -
   5        $��radioButton2��5   ?   $  Clear all unnecessary data��6   $ CleanMgr��6S   $��Clear automatic (full)��6h   	� 5      -   5        $��radioButton3��5   @   $! Select manual what to clear��6   $ CleanMgr��6T   $��Clear manual��6i   5      5      5      