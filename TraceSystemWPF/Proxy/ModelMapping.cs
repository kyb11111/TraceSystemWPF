using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TraceSystemWPF.Proxy
{
    public class PropertyFieldPair
    {
        public string FieldName
        {
            get;
            set;
        }

        public PropertyInfo Property
        {
            get;
            set;
        }
    }

    public class ModelMapping
    {
        private bool m_init;
        private ModelMapping m_parent;
        private ModelMapping m_root;
        private Type m_type;
        private PropertyFieldPair[] m_field;

        public ModelMapping(Type modelType)
        {
            m_init = false;
            m_type = modelType;
            m_field = GetFields(modelType);
        }

        public Type ModelType
        {
            get { return m_type; }
        }

        public PropertyFieldPair[] PropertyFields
        {
            get { return m_field; }
        }

        public ModelMapping Parent
        {
            get
            {
                if (!m_init)
                {
                    if (m_type.BaseType.IsDefined(typeof(DataContractAttribute), false))
                        m_parent = new ModelMapping(m_type.BaseType);
                    m_init = true;
                }
                return m_parent;
            }
        }

        public ModelMapping Root
        {
            get
            {
                if (m_root == null)
                {
                    m_root = this;
                    while (m_root.Parent != null)
                    {
                        m_root = m_root.Parent;
                    }
                }
                return m_root;
            }
        }

        public static PropertyFieldPair[] GetFields(Type modelType)
        {
            List<PropertyFieldPair> list = new List<PropertyFieldPair>();
            PropertyInfo[] properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                if (property.IsDefined(typeof(DataMemberAttribute), false))
                {
                    PropertyFieldPair mf = new PropertyFieldPair();
                    mf.FieldName = property.Name;
                    mf.Property = property;
                    list.Add(mf);
                }
            }
            return list.ToArray();
        }
    }
}
