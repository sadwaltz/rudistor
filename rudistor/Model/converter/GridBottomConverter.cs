using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace rudistor.Model.converter
{
    class GridBottomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Strategy strategy = (Strategy)value;
            String xc = "腿1超价" + strategy.t1cj + "|腿1等待" + strategy.t1dd + "|腿2超价" + strategy.t2cj + "|腿2等待" + strategy.t2dd ;
            
            return xc;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
