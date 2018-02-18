using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;

namespace rudistor.Model.converter
{
    class MultiGridHeaderConverter : IMultiValueConverter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            String clToQ = values[0].ToString();
            String t2clToQ = values[1].ToString();
            String t2vol = values[2].ToString();
            String xc = null;

            try
            {
                XElement root = XElement.Load("Config/tactics.xml");
                IEnumerable<XElement> cl = from el in root.Elements("Tactics")
                                           where (string)el.Attribute("type") == "T1" && (string)el.Element("value") == clToQ
                                           select el;
                IEnumerable<XElement> t2cl = from el in root.Elements("Tactics")
                                             where (string)el.Attribute("type") == "T2" && (string)el.Element("value") == t2clToQ
                                             select el;

                xc = cl.ElementAt(0).Element("name").Value.Split('-')[1] + " " + t2cl.ElementAt(0).Element("name").Value.Split('-')[1] + " PVOL " + t2vol;
            }
            catch (Exception e)
            {
                logger.Error(e);
                logger.Error("tactics.xml parse failed!");
            }
            return xc;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
