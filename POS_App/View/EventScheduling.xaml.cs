using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using POS_App.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventScheduling : Page
    {

        public EventSchedulingViewModel ESVm { get; set; }

        public EventScheduling()
        {
            this.InitializeComponent();
            ESVm = new EventSchedulingViewModel();
            DataContext = ESVm;
        }
       
    }
}
