using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POS_App.View;
using Windows.Networking.Proximity;
using Windows.Storage;

namespace POS_App
{
    public sealed partial class MainPage : Page
    {
        private bool IsLoggedIn { get; set; } = false;
        public MainPage()
        {
            this.InitializeComponent();
            MainFrame.Navigate(typeof(DashBoardPage));
        }

        private async void NavigationView_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as NavigationViewItem;
            string UserRole = "";
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("Role"))
            {
                UserRole = localSettings.Values["Role"] as string;
            }

            if (selectedItem != null)
            {
                string content = selectedItem.Content.ToString();

                if (UserRole == "manager" || UserRole=="admin")
                {
                    switch (content)
                    {
                        case "Order":
                            MainFrame.Navigate(typeof(OrderPage));
                            break;
                        case "Drink IngredientPage":
                            MainFrame.Navigate(typeof(DrinkIngredientPage));
                            break;
                        case "Material Management":
                            MainFrame.Navigate(typeof(MaterialManagement));
                            break;
                        case "Event Scheduling":
                            MainFrame.Navigate(typeof(EventScheduling));
                            break;
                        case "Statistics":
                            MainFrame.Navigate(typeof(Statistic));
                            break;
                        case "Table Manager":
                            MainFrame.Navigate(typeof(TableManagement));
                            break;
                        case "Employee Management":
                            MainFrame.Navigate(typeof(EmployeeManagement));
                            break;
                        case "VIP":
                            MainFrame.Navigate(typeof(VIPCustomer));
                            break;
                        case "Update Menu":
                            MainFrame.Navigate(typeof(UpdateProduct));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (content)
                    {
                        case "Order":
                            MainFrame.Navigate(typeof(OrderPage));
                            break;
                        case "Drink IngredientPage":
                            MainFrame.Navigate(typeof(DrinkIngredientPage));
                            break;
                        case "Event Scheduling":
                            MainFrame.Navigate(typeof(EventScheduling));
                            break;
                        case "Material Management":
                            MainFrame.Navigate(typeof(MaterialManagement));
                            break;
                        case "Table Manager":
                            MainFrame.Navigate(typeof(TableManagement));
                            break;
                        case "VIP":
                            MainFrame.Navigate(typeof(VIPCustomer));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Logout_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Clear();
            IsLoggedIn = false;
            UpdateUI();
        }

        private void UpdateUI()
        {
            MyNavigationView.Visibility = IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            if (!IsLoggedIn)
            {
                Frame.Navigate(typeof(Login));
            }
        }
    }
}
