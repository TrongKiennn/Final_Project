using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices.WindowsRuntime;
using DemoListBinding1610.Helper;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DemoListBinding1610;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get; set;
    }
    public MainPage()
    {
        this.InitializeComponent();
        ViewModel = new MainViewModel();
        // DataContext = this;
    }
    private void getAllCategoriesButton_Click(object sender, RoutedEventArgs e)
    {
        string data = "";
        if (data.IsEmpty()) {
            double value = 5.0.mm();
            Debug.WriteLine(value);
        }

        var character1= new {
            Name = "Kieu Phong",
            ATK = 9999,
            Speed = 9000,
        };

        var character2= new {
            Name = "Doan Du",
            ATK = 5000,
            Speed = 8999
        };

        var character3= new {
            Name = "Hu Truc",
            ATK = 7500,
            Lucky = 9999
        };

        Debug.WriteLine(character1.GetType().Name);
        Debug.WriteLine(character2.GetType().Name);
        Debug.WriteLine(character3.GetType().Name);
    }

    private void addCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(EmployeeAddPage));
    }

    private async void deleteCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        var employee = itemsComboBox.SelectedItem as Employee;
        bool success = ViewModel.Remove(employee);

        if (success) {
            await new ContentDialog {
                XamlRoot = this.Content.XamlRoot,
                Title = "Delete",
                Content = "Delete successfully",
                CloseButtonText = "OK"
            }.ShowAsync();
        } else {
            await new ContentDialog {
                XamlRoot = this.Content.XamlRoot,
                Title = "Delete",
                Content = "Delete failed",
                CloseButtonText = "Cannot delete employee with id: " + employee.ID
            }.ShowAsync();
        }
    }

    private void updateCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        var employee = itemsComboBox.SelectedItem as Employee;

        Frame.Navigate(typeof(EmployeeEditPage), employee);
    }

    private void replaceCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Employees = new ObservableCollection<Employee>()
        {
            new Employee()
            {
                ID = 171,
                Name = "Jason Bourne",
                Avatar = "/Assets/avatar07.jpg"
            },
            new Employee()
            {
                ID = 172,
                Name = "Jason Statham",
                Avatar = "/Assets/avatar08.jpg"
            },
            new Employee()             {
                ID = 173,
                Name = "Jason Momoa",
                Avatar = "/Assets/avatar09.jpg"
            },
        };
    }

    private void previousButton_Click(object sender, RoutedEventArgs e) {

    }

    private void nextButton_Click(object sender, RoutedEventArgs e) {
        ViewModel.GoToNextPage();
    }

    bool init = false;
    private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (init == false) {
            init = true;
            return;
        }
        if (pagesComboBox.SelectedIndex >= 0) {
            var item = pagesComboBox.SelectedItem as PageInfo;
            ViewModel.GoToPage(item.Page);
        }
    }

    private void searchButton_Click(object sender, RoutedEventArgs e) {
        ViewModel.Search();
    }

    private void goToOrderPage_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(OrderPage), ViewModel);
    }

}
