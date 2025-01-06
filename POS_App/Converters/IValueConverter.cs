using System;
using System.Globalization;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace POS_App.Converters
{
    public class RadioButtonCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString() == parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? parameter?.ToString() : null;
        }
    }

    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "occupied":
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0x8c, 0x00));
                    case "available":
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x95, 0xd5, 0xb2));
                    case "reserved":
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0x63, 0x47));
                    case "low":
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0x8c, 0x00));
                    case "out_soon":
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0x63, 0x47));
                    default:
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9));
                }
            }
            return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    public class StatusDrinkToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "available":
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9));
                    case "unavailable":
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0x63, 0x47));
                    default:
                        return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9));
                }
            }
            return new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

       
    }

    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {

                return decimalValue.ToString("F2", CultureInfo.InvariantCulture);
            }
            return "0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (decimal.TryParse(value as string, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            return 0m;
        }
    }
}