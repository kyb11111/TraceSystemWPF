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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;

namespace TraceSystemWPF.Proxy
{
    public class ChidrenModelCollection<T> : ModelCollection<T> where T : IModelBase
    {
        public ChidrenModelCollection()
        {
            ModelCollection collection = ModelCacheManager.Instance[typeof(T)];
            collection.CollectionChanged += new NotifyCollectionChangedEventHandler(ModelCacheCollection_CollectionChanged);
        }

        void ModelCacheCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (T t in e.OldItems)
                    {
                        this.Remove(t.Rid); 
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.Clear();
                    break;
                default:
                    break;
            }
        }

        public T this[string altKey]
        {
            get
            {
                foreach (T model in this)
                {
                    if (!string.IsNullOrWhiteSpace(model.AlternateKey))
                    {
                        if (model.AlternateKey == altKey)
                            return model;
                    }
                }
                return default(T);
            }
        }
    }

    /// <summary>
    /// 用于观察一些列集合是否发生变化的总集合
    /// </summary>
    public class ChildrenSummaryModelCollection : ObservableCollection<IModelBase>
    {
        private HashSet<ICollection> m_list;

        public ChildrenSummaryModelCollection()
        {
            m_list = new HashSet<ICollection>();
        }

        /// <summary>
        /// 添加子集合
        /// </summary>
        /// <param name="childrenCollection">子集合</param>
        public void Add(ICollection childrenCollection)
        {
            m_list.Add(childrenCollection);
            foreach (IModelBase model in childrenCollection)
                this.Add(model);
            INotifyCollectionChanged changedCollection = childrenCollection as INotifyCollectionChanged;
            if (changedCollection != null)
                changedCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(childrenCollection_CollectionChanged);
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            foreach (ICollection collection in m_list)
            {
                INotifyCollectionChanged changedCollection = collection as INotifyCollectionChanged;
                if (changedCollection != null)
                    changedCollection.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(childrenCollection_CollectionChanged);
            }
            m_list.Clear();
        }

        void childrenCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IModelBase model in e.NewItems)
                        this.Add(model);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IModelBase model in e.OldItems)
                        this.Remove(model);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.Clear();
                    foreach (ICollection collection in m_list)
                        foreach (IModelBase model in collection)
                            this.Add(model);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();
                default:
                    break;
            }
        }
    }
}
