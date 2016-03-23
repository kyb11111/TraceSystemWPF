using System;
using System.Net;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace TraceSystemWPF.Proxy
{
    public partial class ModelBase : IModelBase
    {
        public virtual string AlternateKey
        {
            get
            {
                return string.Empty;
            }
        }

        public string Type
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public string FullTypeName
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        internal int RefCount
        {
            get;
            set;
        }

        internal void SetRealtimeData(RealtimeData data)
        {
            int index = 0;
            SetRealtimeData(data,ref index);
        }

        protected virtual void SetRealtimeData(RealtimeData data, ref int index)
        {
        }

        public virtual void Initialize()
        {
          
        }

        public virtual object HeaderSigns
        {
            get { return string.Format("[{0}]", this.GetType().Name); }
        }

        #region 关联属性
        private ParentModel parent;
        private ChildrenModel children;

        /// <summary>
        /// 获取多对一关联中的关联父对象
        /// </summary>
        public ParentModel ParentModel
        {
            get
            {
                if (parent == null)
                    parent = new ParentModel(this);
                return parent;
            }
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        public ChildrenModel ChildrenModel
        {
            get
            {
                if (children == null)
                    children = new ChildrenModel(this);
                return children;
            }
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <typeparam name="T">关联子对象类型</typeparam>
        /// <returns>关联子对象集合</returns>
        public IEnumerable<T> GetChildren<T>() where T : IModelBase
        {
            return ChildrenModel.GetChildren<T>();
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <typeparam name="T">关联子对象类型</typeparam>
        /// <param name="childPropertyName">关联子对象对应的成员属性名称</param>
        /// <returns>关联子对象集合</returns>
        public IEnumerable<T> GetChildren<T>(string childPropertyName) where T : IModelBase
        {
            return ChildrenModel.GetChildren<T>(childPropertyName);
        }

        /// <summary>
        /// 获取多对一关联中的关联父对象
        /// </summary>
        /// <typeparam name="T">关联父对象类型</typeparam>
        /// <returns>关联父对象</returns>
        public T GetParent<T>() where T : IModelBase
        {
            return ParentModel.GetParent<T>();
        }

        /// <summary>
        /// 获取多对一关联中的关联父对象
        /// </summary>
        /// <typeparam name="T">关联父对象类型</typeparam>
        /// <param name="parentPropertyName">关联父对象对应的成员属性名称</param>
        /// <returns>关联父对象</returns>
        public T GetParent<T>(string parentPropertyName) where T : IModelBase
        {
            return ParentModel.GetParent<T>(parentPropertyName);
        }
        #endregion

        public IModelBase Clone()
        {
            //克隆会把事件也一起克隆，在这里需要清除事件
            Type type = this.GetType();
            IModelBase model = Activator.CreateInstance(type) as IModelBase;
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public|BindingFlags.Instance);
            foreach(PropertyInfo property in properties)
            {
                if (Attribute.IsDefined(property, typeof(DataMemberAttribute)))
                {
                    object o = property.GetValue(this, null);
                    property.SetValue(model, o, null);
                }
            }
            return model;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.AlternateKey))
                return base.ToString();
            return this.AlternateKey;
        }
    }
}
