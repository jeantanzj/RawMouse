using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EGClassroom.Helper
{
   
        [ValueConversion(typeof(bool), typeof(bool))]
        public class InverseBoolConverter : IValueConverter
        {
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
            {
                 return !(bool)value;
            }

            public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
            {
                throw new NotSupportedException();
            }

            #endregion
        }
    }

