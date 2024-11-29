using DemoListBinding1610.Service;
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
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EmployeeAddPage : Page {
    public class EmployeeAddPageViewModel {
        public EmployeeAddPageViewModel() {            
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public Employee Info { get; set;} = new Employee();
        IDao _dao;

        public bool AddEmployee() {
            Info.Avatar = "/Assets/avatar07.jpg";
            return _dao.AddEmployee(Info);      
        }
    }

    public EmployeeAddPageViewModel ViewModel { get; set;} = new EmployeeAddPageViewModel();

    public EmployeeAddPage() {
        this.InitializeComponent();
    }

    private async void submitButton_Click(object sender, RoutedEventArgs e) {
        bool success = ViewModel.AddEmployee();

        if (success) { 
            await new ContentDialog() {
                XamlRoot = this.Content.XamlRoot,
                Title = "Insert new employee",
                Content = "Successfully inserted employee:" + ViewModel.Info.Name,
                CloseButtonText = "OK"
            }.ShowAsync();
            Frame.GoBack();
        }
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e) {
        Frame.GoBack();
    }
}
