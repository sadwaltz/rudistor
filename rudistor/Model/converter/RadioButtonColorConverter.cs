using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace rudistor.Model.converter
{
    public class RadioButtonColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            bool isChecked = checkValue.Equals(targetValue,
                StringComparison.InvariantCultureIgnoreCase);
            if (isChecked) {
                return "Red";
            }

            return "Black";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
