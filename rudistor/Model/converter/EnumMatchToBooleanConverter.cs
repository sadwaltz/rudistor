﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace rudistor.Model.converter
{
    

    class EnumMatchToBooleanConverter:IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();

            
            string targetValue = parameter.ToString();
            bool isChecked = checkValue.Equals(targetValue,
                     StringComparison.InvariantCultureIgnoreCase);
            
            return isChecked;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {

            // return Binding.DoNothing instead of null when uncheck radiobuttion
            if (value == null || parameter == null)
                return Binding.DoNothing;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return targetValue;

            return Binding.DoNothing;
            
        }
    }
}
