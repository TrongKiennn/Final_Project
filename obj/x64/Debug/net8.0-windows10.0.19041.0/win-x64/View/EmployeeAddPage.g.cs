﻿#pragma checksum "C:\Users\MK\Downloads\DemoListBinding3110\DemoListBinding2410\View\EmployeeAddPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7F9D836B4097E2929A936E81551A857277CC7765E60990737DB918779C24208D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoListBinding1610
{
    partial class EmployeeAddPage : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_DemoListBinding1610_EmployeeUserControl_Info(global::DemoListBinding1610.EmployeeUserControl obj, global::DemoListBinding1610.Employee value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::DemoListBinding1610.Employee) global::Microsoft.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::DemoListBinding1610.Employee), targetNullValue);
                }
                obj.Info = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private partial class EmployeeAddPage_obj1_Bindings :
            global::Microsoft.UI.Xaml.Markup.IDataTemplateComponent,
            global::Microsoft.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Microsoft.UI.Xaml.Markup.IComponentConnector,
            IEmployeeAddPage_Bindings
        {
            private global::DemoListBinding1610.EmployeeAddPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::DemoListBinding1610.EmployeeUserControl obj2;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj2InfoDisabled = false;

            private EmployeeAddPage_obj1_BindingsTracking bindingsTracking;

            public EmployeeAddPage_obj1_Bindings()
            {
                this.bindingsTracking = new EmployeeAddPage_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 15 && columnNumber == 36)
                {
                    isobj2InfoDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 2: // View\EmployeeAddPage.xaml line 14
                        this.obj2 = global::WinRT.CastExtensions.As<global::DemoListBinding1610.EmployeeUserControl>(target);
                        this.bindingsTracking.RegisterTwoWayListener_2(this.obj2);
                        break;
                    default:
                        break;
                }
            }
                        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
                        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target) 
                        {
                            return null;
                        }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                nextPhase = -1;
            }

            public void Recycle()
            {
                return;
            }

            // IEmployeeAddPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                if (newDataRoot != null)
                {
                    this.dataRoot = global::WinRT.CastExtensions.As<global::DemoListBinding1610.EmployeeAddPage>(newDataRoot);
                    return true;
                }
                return false;
            }

            public void Activated(object obj, global::Microsoft.UI.Xaml.WindowActivatedEventArgs data)
            {
                this.Initialize();
            }

            public void Loading(global::Microsoft.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::DemoListBinding1610.EmployeeAddPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel(obj.ViewModel, phase);
                    }
                }
            }
            private void Update_ViewModel(global::DemoListBinding1610.EmployeeAddPage.EmployeeAddPageViewModel obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_Info(obj.Info, phase);
                    }
                }
            }
            private void Update_ViewModel_Info(global::DemoListBinding1610.Employee obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel_Info(obj);
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // View\EmployeeAddPage.xaml line 14
                    if (!isobj2InfoDisabled)
                    {
                        XamlBindingSetters.Set_DemoListBinding1610_EmployeeUserControl_Info(this.obj2, obj, null);
                    }
                }
            }
            private void UpdateTwoWay_2_Info()
            {
                if (this.initialized)
                {
                    if (this.dataRoot != null)
                    {
                        if (this.dataRoot.ViewModel != null)
                        {
                            this.dataRoot.ViewModel.Info = this.obj2.Info;
                        }
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class EmployeeAddPage_obj1_BindingsTracking
            {
                private global::System.WeakReference<EmployeeAddPage_obj1_Bindings> weakRefToBindingObj; 

                public EmployeeAddPage_obj1_BindingsTracking(EmployeeAddPage_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<EmployeeAddPage_obj1_Bindings>(obj);
                }

                public EmployeeAddPage_obj1_Bindings TryGetBindingObject()
                {
                    EmployeeAddPage_obj1_Bindings bindingObject = null;
                    if (weakRefToBindingObj != null)
                    {
                        weakRefToBindingObj.TryGetTarget(out bindingObject);
                        if (bindingObject == null)
                        {
                            weakRefToBindingObj = null;
                            ReleaseAllListeners();
                        }
                    }
                    return bindingObject;
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_ViewModel_Info(null);
                }

                public void PropertyChanged_ViewModel_Info(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    EmployeeAddPage_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::DemoListBinding1610.Employee obj = sender as global::DemoListBinding1610.Employee;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                        }
                        else
                        {
                            switch (propName)
                            {
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::DemoListBinding1610.Employee cache_ViewModel_Info = null;
                public void UpdateChildListeners_ViewModel_Info(global::DemoListBinding1610.Employee obj)
                {
                    if (obj != cache_ViewModel_Info)
                    {
                        if (cache_ViewModel_Info != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_ViewModel_Info).PropertyChanged -= PropertyChanged_ViewModel_Info;
                            cache_ViewModel_Info = null;
                        }
                        if (obj != null)
                        {
                            cache_ViewModel_Info = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_ViewModel_Info;
                        }
                    }
                }
                public void RegisterTwoWayListener_2(global::DemoListBinding1610.EmployeeUserControl sourceObject)
                {
                    sourceObject.RegisterPropertyChangedCallback(global::DemoListBinding1610.EmployeeUserControl.InfoProperty, (sender, prop) =>
                    {
                        var bindingObj = this.TryGetBindingObject();
                        if (bindingObj != null)
                        {
                            bindingObj.UpdateTwoWay_2_Info();
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 3: // View\EmployeeAddPage.xaml line 17
                {
                    this.submitButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.submitButton).Click += this.submitButton_Click;
                }
                break;
            case 4: // View\EmployeeAddPage.xaml line 19
                {
                    this.cancelButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.cancelButton).Click += this.cancelButton_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }


        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2409")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // View\EmployeeAddPage.xaml line 2
                {                    
                    global::Microsoft.UI.Xaml.Controls.Page element1 = (global::Microsoft.UI.Xaml.Controls.Page)target;
                    EmployeeAddPage_obj1_Bindings bindings = new EmployeeAddPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                    global::Microsoft.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element1, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}
