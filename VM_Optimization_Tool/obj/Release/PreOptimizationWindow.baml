   M S B A M L   `   `   `  ���� O  KVM_Optimization_Tool, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null;"clr-namespace:VM_Optimization_ToolVM_Optimization_Tool  0    *VM_Optimization_Tool.PreOptimizationWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:VM_Optimization_Tool       � Title$  PreOptimizationWindow��$��366.763��$��339.595�� � 
ResizeMode$ NoResize=�.��� -   $��gridName��.+��� -   $��checkCleanMgr�� � ToolTip$ Clears unnecessary files��$��Clean Manager��$
��Left=�$��	99,82,0,0q�$	��Top=� d� 	IsChecked$
 TrueY��� -   $��	checkDism��$ Clears old updated files��$
��DISM��$
��Left=�$��
99,148,0,0q�$	��Top=�$
 TrueY��� -   $��checkDefrag��$ Defragments files��$��Defragmentation��$
��Left=�$��
99,168,0,0q�$	��Top=�$
 TrueY��� -   $��checkSDelete��$ Zero free space��$��SDelete��$
��Left=�$��
99,188,0,0q�$	��Top=�$
 TrueY��� -   $��start_Button��$��Start��$
��Left=�$��200,276,0,0q�$	��Top=�$��75�� J� RenderTransformOrigin$ -1.043,-2.397&��� -   $��abort_Button��$��Abort��$
��Left=�$��
58,276,0,0q�$	��Top=�$��75���� $ ��Select Optimization Tasks:��$
��Left=�$��	26,51,0,0q�$	��Top=��� -   $��
stackPanel��$��112,99,21,183q�.+�	� -	   $��radioButton1��$( "Clear most common unnecessary data�� 	� 	GroupName$ CleanMgr��$ ��Clear automatic (standard)��$
 TrueY�	� -
   $��radioButton2��$  Clear all unnecessary data��$ CleanMgr��$��Clear automatic (full)��	� -   $��radioButton3��$! Select manual what to clear��$ CleanMgr��$��Clear manual��