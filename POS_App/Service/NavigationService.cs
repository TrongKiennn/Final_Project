using POS_App.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace POS_App.Service
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToDashboard(Model.User user)
        {
            DashBoardPage dashboardPage = new DashBoardPage();
            _frame.Navigate(typeof(DashBoardPage), user);
        }

        public void NavigateToRegister()
        {
            _frame.Navigate(typeof(Register));
        }
    }
}
