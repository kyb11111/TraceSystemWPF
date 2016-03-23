using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TraceSystemWPF.Proxy;

namespace TraceSystemWPF.Converter
{
    public class DepartmentConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Department dept = ModelCacheManager.Instance[typeof(Department), (int)value] as Department;
            if (dept != null)
            {
                return dept.Name;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
