﻿#pragma checksum "..\..\SelectOptimization.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7DDEA5E91D78CFDCE00B2E04347C1B77EEE8067C"
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
    /// SelectOptimization
    /// </summary>
    public partial class SelectOptimization : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 34 "..\..\SelectOptimization.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView trvItems;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\SelectOptimization.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLoad;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\SelectOptimization.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAbort;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\SelectOptimization.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressBar;
        
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
            System.Uri resourceLocater = new System.Uri("/VM_Optimization_Tool;component/selectoptimization.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SelectOptimization.xaml"
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
            case 2:
            this.trvItems = ((System.Windows.Controls.TreeView)(target));
            return;
            case 3:
            this.btnLoad = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\SelectOptimization.xaml"
            this.btnLoad.Click += new System.Windows.RoutedEventHandler(this.loadXML_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnAbort = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\SelectOptimization.xaml"
            this.btnAbort.Click += new System.Windows.RoutedEventHandler(this.abort_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.progressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 14 "..\..\SelectOptimization.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBoxChanged);
            
            #line default
            #line hidden
            
            #line 14 "..\..\SelectOptimization.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBoxChanged);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

