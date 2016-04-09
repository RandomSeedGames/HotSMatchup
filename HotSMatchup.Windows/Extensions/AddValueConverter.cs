using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HotSMatchup
{
    public class AddValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
              object parameter, CultureInfo culture)
        {
            if (value is double && double.IsNaN((double)value))
                return value;

            int addnumber = 0;
            if (parameter is string)
                int.TryParse((string)parameter, out addnumber);

            return ((double)value + addnumber);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
