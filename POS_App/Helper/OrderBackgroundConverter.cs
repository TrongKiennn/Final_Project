using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Helpers
{
    public class OrderBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Check OrderCount value
            int orderCount = value is int count ? count : 0;
            var customColor = Microsoft.UI.ColorHelper.FromArgb(255, 255, 255, 217); // ARGB for #ffffd9
            var customColorFor0item = Microsoft.UI.ColorHelper.FromArgb(255, 152, 102, 80); // ARGB for #986650

            // Return appropriate Brush based on OrderCount
            return orderCount > 0
                ? new SolidColorBrush(customColor)  // Background for tables with orders
                : new SolidColorBrush(customColorFor0item);  // Default background
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
