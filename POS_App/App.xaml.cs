using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using POS_App.Service.DataAccess;
using POS_App.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            ServiceFactory.Register(typeof(IDao_Drinks),typeof(Dao_Drinks));
            ServiceFactory.Register(typeof(IDao_Order_Item), typeof(Dao_Order_Item));
            ServiceFactory.Register(typeof(IDao_Order), typeof(Dao_Order));
            ServiceFactory.Register(typeof(IDao_Tables), typeof(Dao_Tables));
            ServiceFactory.Register(typeof(IDao_Events), typeof(Dao_Events));
            ServiceFactory.Register(typeof(IDao_Ingredients), typeof(Dao_Ingredients));
            ServiceFactory.Register(typeof(IDao_Drink_Ingredient), typeof(Dao_Drink_Ingredient));
            ServiceFactory.Register(typeof(IDao_Employee_Attendance), typeof(Dao_Employee_Attendance));
            ServiceFactory.Register(typeof(IDao_Employee_Information), typeof(Dao_Employee_Information));
            ServiceFactory.Register(typeof(IDao_User), typeof(Dao_User));
            ServiceFactory.Register(typeof(IDao_Customer), typeof(Dao_Customer));
            ServiceFactory.Register(typeof(IDao_Rank), typeof(Dao_Rank));
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }
}
