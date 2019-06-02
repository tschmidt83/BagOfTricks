using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BagOfTricks.Converters
{

    [ValueConversion(typeof(bool), typeof(System.Windows.Media.SolidColorBrush))]
    public class EditModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
                return System.Windows.Media.Brushes.Wheat;
            else
                return System.Windows.Media.Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

}
