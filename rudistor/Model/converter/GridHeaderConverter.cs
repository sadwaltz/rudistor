﻿using NLog;
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
    class GridHeaderConverter : IValueConverter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Strategy strategy = (Strategy)value;
            String xc = null;
            try
            {
                XElement root = XElement.Load("Config/tactics.xml");
                IEnumerable<XElement> cl = from el in root.Elements("Tactics")
                                           where (string)el.Attribute("type") == "T1" && (string)el.Element("value") == strategy.cl
                                           select el;
                IEnumerable<XElement> t2cl = from el in root.Elements("Tactics")
                                             where (string)el.Attribute("type") == "T2" && (string)el.Element("value") == strategy.t2cl
                                             select el;

                xc = strategy.StageId + " " + cl.ElementAt(0).Element("name").Value.Split('-')[1] + " " + t2cl.ElementAt(0).Element("name").Value.Split('-')[1] + " PVOL " + strategy.t2vol;
            }
            catch(Exception e)
            {
                logger.Error(e);
                logger.Error("tactics.xml parse failed!");
            }
            return xc;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
