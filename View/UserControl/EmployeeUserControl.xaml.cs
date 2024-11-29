using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DemoListBinding1610; 
public sealed partial class EmployeeUserControl : UserControl {
    public static readonly DependencyProperty InfoProperty 
        = DependencyProperty.Register(
            "Info", typeof(Employee),
            typeof(EmployeeUserControl),
            new PropertyMetadata(null)
    );

    public Employee Info 
    {
        get => (Employee)GetValue(InfoProperty);
        set => SetValue(InfoProperty, value);
    }

    
    public EmployeeUserControl() {
        this.InitializeComponent();
    }
}
