﻿#pragma checksum "..\..\PreOptimizationWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "52DB7EBC80CC5308BE6747198647FB0359595F88"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using VM_Optimization_Tool;


namespace VM_Optimization_Tool {
    
    
    /// <summary>
    /// PreOptimizationWindow
    /// </summary>
    public partial class PreOptimizationWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridName;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkCleanMgr;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkDism;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkDefrag;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkSDelete;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button start_Button;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button abort_Button;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackPanel;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radioButton1;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radioButton2;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\PreOptimizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radioButton3;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VM_Optimization_Tool;component/preoptimizationwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\PreOptimizationWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.gridName = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.checkCleanMgr = ((System.Windows.Controls.CheckBox)(target));
            
            #line 10 "..\..\PreOptimizationWindow.xaml"
            this.checkCleanMgr.Unchecked += new System.Windows.RoutedEventHandler(this.CheckCleanMgr_Checked);
            
            #line default
            #line hidden
            
            #line 10 "..\..\PreOptimizationWindow.xaml"
            this.checkCleanMgr.Checked += new System.Windows.RoutedEventHandler(this.CheckCleanMgr_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.checkDism = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.checkDefrag = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.checkSDelete = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.start_Button = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\PreOptimizationWindow.xaml"
            this.start_Button.Click += new System.Windows.RoutedEventHandler(this.StartButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.abort_Button = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\PreOptimizationWindow.xaml"
            this.abort_Button.Click += new System.Windows.RoutedEventHandler(this.AbortButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.stackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 9:
            this.radioButton1 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 10:
            this.radioButton2 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 11:
            this.radioButton3 = ((System.Windows.Controls.RadioButton)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

