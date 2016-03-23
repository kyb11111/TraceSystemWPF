using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TraceSystemWPF.Proxy;

namespace TraceSystemWPF.Converter
{
    public class UserInfoUserTypeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int UserTypeValue = (int)value;
            string EnumString = "";
            switch (UserTypeValue)
            {
                case 1:
                    EnumString = "用户";
                    break;
                case 2:
                    EnumString = "农户";
                    break;
                case 3:
                    EnumString = "管理";
                    break;
            }
            return EnumString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
