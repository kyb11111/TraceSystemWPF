using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TraceSystemWPF.Proxy;

namespace TraceSystemWPF.Converter
{
    public class DocumentTypeNameConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DocumentType m_DocumentType = ModelCacheManager.Instance[typeof(DocumentType), (int)value] as DocumentType;
            if (m_DocumentType != null)
            {
                return m_DocumentType.TypeName;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
