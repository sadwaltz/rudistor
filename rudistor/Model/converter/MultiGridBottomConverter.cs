using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace rudistor.Model.converter
{
    class MultiGridBottomConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //String xc = "腿1超价" + values[0] + "|腿1等待" + values[1] + "|腿2超价" + values[2] + "|腿2等待" + values[3];
            String xc = String.Format("{0}    {1}    {2}    {3}", values);
            return xc;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
