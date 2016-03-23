using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TraceSystemWPF.Proxy;

namespace TraceSystemWPF.Converter
{
    public class ProductTypeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ProductType p = ModelCacheManager.Instance[typeof(ProductType), (int)value] as ProductType;
            if (p != null)
            {
                return p.Name;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
