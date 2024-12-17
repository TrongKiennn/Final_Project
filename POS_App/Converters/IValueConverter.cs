using System;
using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace POS_App.Converters
{
    public class RadioButtonCheckedConverter : IValueConverter
    {
        // Convert from boolean to the value used in RadioButton
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString() == parameter?.ToString();
        }

        // Convert from RadioButton's check state back to the actual value
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? parameter?.ToString() : null; // null thay vì Binding.DoNothing
        }
    }

    //public class AddNewVisibilityConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        return (int)value == 0 ? Visibility.Visible : Visibility.Collapsed;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class NormalTableVisibilityConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        return (int)value != 0 ? Visibility.Visible : Visibility.Collapsed;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
