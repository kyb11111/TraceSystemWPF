using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace TraceSystemWPF.Proxy
{
    /// <summary>
    /// 获取一对多关联中的子对象集合
    /// </summary>
    public class ChildrenModel
    {
        private IModelBase model;
        /// <summary>
        ///  创建ChildrenModel对象
        /// </summary>
        /// <param name="value">模型对象</param>
        public ChildrenModel(IModelBase value)
        {
            model = value;
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <param name="childTypeName">关联子对象类型名称</param>
        /// <returns>关联子对象集合</returns>
        public ICollection<IModelBase> this[string childTypeName]
        {
            get
            {
                return this[childTypeName, model.GetType().Name];
            }
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <param name="childTypeName">关联子对象类型名称</param>
        /// <param name="childPropertyName">关联子对象对应的成员属性名称</param>
        /// <returns>关联子对象集合</returns>
        public ICollection<IModelBase> this[string childTypeName, string childPropertyName]
        {
            get
            {
                ChildrenModelCollection children = new ChildrenModelCollection();
                ModelCollection rootCollection = ModelCacheManager.Instance[childTypeName];
                if (rootCollection == null)
                    return children;

                Type childType = rootCollection.ModelType;
                PropertyInfo property = childType.GetProperty(childPropertyName);
                if (property == null || property.PropertyType != typeof(int))
                    return children;

                children.BuildFromRootModelCollection(rootCollection, property, model.Rid);
                return children;
            }
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <param name="childTypeName">关联子对象类型</param>
        /// <returns>关联子对象集合</returns>
        public ICollection<IModelBase> this[Type childType]
        {
            get
            {
                return this[childType, model.GetType().Name];
            }
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <param name="childTypeName">关联子对象类型</param>
        /// <param name="childPropertyName">关联子对象对应的成员属性名称</param>
        /// <returns>关联子对象集合</returns>
        public ICollection<IModelBase> this[Type childType, string childPropertyName]
        {
            get
            {
                return this[childType.Name, childPropertyName];
            }
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <typeparam name="T">关联子对象类型</typeparam>
        /// <returns>关联子对象集合</returns>
        public IEnumerable<T> GetChildren<T>() where T : IModelBase
        {
            return GetChildren<T>(this.GetType().Name);
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <typeparam name="T">关联子对象类型</typeparam>
        /// <param name="childPropertyName">关联子对象对应的成员属性名称</param>
        /// <returns>关联子对象集合</returns>
        public IEnumerable<T> GetChildren<T>(string childPropertyName) where T : IModelBase
        {
            return this[typeof(T), childPropertyName].Select(model => 
            {
                if (model is T)
                    return (T)model;
                return default(T);
            });
        }
    }

    public class ChildrenModelCollection : ObservableCollection<IModelBase>, IDisposable
    {
        private ModelCollection rootCollection;
        private int parentRid;
        private PropertyInfo m_childProperty = null;

        internal ChildrenModelCollection()
        {
        } 

        internal void BuildFromRootModelCollection(ModelCollection rootCollection, PropertyInfo childProperty, int parentRid)
        {
            m_childProperty = childProperty;
            this.rootCollection = rootCollection;
            this.parentRid = parentRid;
            rootCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(RootCollection_CollectionChanged);
            foreach (IModelBase child in rootCollection)
            {
                int rid = (int)childProperty.GetValue(child, null);
                if (rid == parentRid)
                    this.Add(child);
            }
        }

        void RootCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int rid = 0;
            if (m_childProperty != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        IModelBase newModel = e.NewItems[0] as IModelBase;
                        rid = (int)m_childProperty.GetValue(newModel, null);
                        if (rid == parentRid)
                            this.Add(newModel);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        IModelBase oldModel = e.OldItems[0] as IModelBase;
                        rid = (int)m_childProperty.GetValue(oldModel, null);
                        if (rid == parentRid)
                            this.Remove(oldModel);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        this.Clear();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Dispose()
        {
            rootCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(RootCollection_CollectionChanged);
        }
    }
}
