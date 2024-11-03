using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service
{
    public interface INavigationService
    {
        void NavigateToDashboard(Model.User user);
        void NavigateToRegister();
    }
}
