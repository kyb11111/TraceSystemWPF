using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TraceSystemWPF.Proxy
{
    /// <summary>
    /// 存根发生变化时触发事件的事件参数
    /// </summary>
    public class ModelCacheChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 引起变化的模型对象
        /// </summary>
        public readonly IModelBase Model;

        /// <summary>
        /// 给删除用的构造函数
        /// </summary>
        /// <param name="model">删除的对象</param>
        internal ModelCacheChangedEventArgs(IModelBase model)
        {
            Model = model;
        }

        /// <summary>
        /// 给添加修改用的构造函数
        /// </summary>
        /// <param name="modelType">已添加到Cache里的对象类型</param>
        /// <param name="rid">已添加到Cache里的对象Rid</param>
        internal ModelCacheChangedEventArgs(Type modelType, int rid)
        {
            Model = ModelCacheManager.Instance[modelType, rid];
        }
    }

    public class ModelCacheManager
    {
        private Dictionary<string, ModelCollection> typeDictionary = new Dictionary<string, ModelCollection>();
        private static ModelCacheManager instance;

        /// <summary>
        /// 声明为受保护的构造函数
        /// </summary>
        protected ModelCacheManager()
        {
        }

        /// <summary>
        /// 获取模型对象存根管理器的实例
        /// </summary>
        public static ModelCacheManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ModelCacheManager();
                return instance;
            }
        }

        /// <summary>
        /// 清空Cache
        /// </summary>
        public void Clear()
        {
            foreach (ModelCollection collection in typeDictionary.Values)
                collection.Clear();
            if (CacheClear != null)
                CacheClear(this, null);
            //typeDictionary.Clear();
        }

        /// <summary>
        /// 模型存入存根时触发
        /// </summary>
        public event EventHandler<ModelCacheChangedEventArgs> ModelSaved;
        /// <summary>
        /// 模型移除存根时触发
        /// </summary>
        public event EventHandler<ModelCacheChangedEventArgs> ModelRemoved;
        /// <summary>
        /// 存根清空时触发
        /// </summary>
        public event EventHandler CacheClear;

        /// <summary>
        /// 存储一个模型对象，如果存根管理器中没有该对象则增加该对象
        /// </summary>
        /// <param name="value">要存储的模型对象</param>
        public virtual void Save(IModelBase value)
        {
            Type type = value.GetType();
            ModelCollection modelCollection = this[type];
            if (modelCollection == null)
                return;
            if (modelCollection.Contains(value.Rid))
            {
                IModelBase model = modelCollection[value.Rid];
                ModelMapping mf = modelCollection.ModelField;
                foreach (PropertyFieldPair pfp in mf.PropertyFields)
                {
                    object obj = pfp.Property.GetValue(value, null);
                    pfp.Property.SetValue(model, obj, null);
                }
                while (mf.Parent != null)
                {
                    foreach (PropertyFieldPair pfp in mf.Parent.PropertyFields)
                    {
                        object obj = pfp.Property.GetValue(value, null);
                        pfp.Property.SetValue(model, obj, null);
                    }
                    mf = mf.Parent;
                }
                modelCollection.AddAltKey(value);
            }
            else
            {
                modelCollection.Add(value);
                modelCollection.AddAltKey(value);
                type = type.BaseType;
                while (type.BaseType != typeof(object) && type.Name != "ScadaModel")
                {
                    modelCollection = this[type.Name];
                    if (modelCollection.Contains(value.Rid))
                    {
                        IModelBase model = modelCollection[value.Rid];
                        ModelMapping mf = modelCollection.ModelField;
                        foreach (PropertyFieldPair pfp in mf.PropertyFields)
                        {
                            object obj = pfp.Property.GetValue(model, null);
                            pfp.Property.SetValue(value, obj, null);
                        }
                        modelCollection.Remove(value.Rid);
                        modelCollection.RemoveAltKey(value);
                    }
                    modelCollection.Add(value);
                    modelCollection.AddAltKey(value);
                    type = type.BaseType;
                }
            }
            value.Initialize();
            if (ModelSaved != null)
                ModelSaved(this, new ModelCacheChangedEventArgs(value.GetType(), value.Rid));
        }

        /// <summary>
        /// 存储多个模型对象
        /// </summary>
        /// <param name="values">要存储的模型对象集合的枚举器</param>
        public void SaveRange(IEnumerable<IModelBase> values)
        {
            foreach (IModelBase value in values)
            {
                Save(value);
            }
        }

        /// <summary>
        /// 移除一个模型对象
        /// </summary>
        /// <param name="value">要移除的模型对象</param>
        public virtual void Remove(IModelBase value)
        {
            if (value == null)//于艳辉添加
                return;
            Type type = value.GetType();
            while (type.BaseType != typeof(object))
            {
                ModelCollection modelCollection = this[type.Name];
                if (modelCollection != null &&
                    modelCollection.Contains(value.Rid))
                {
                    IModelBase model = modelCollection[value.Rid];
                    modelCollection.Remove(model);
                    modelCollection.RemoveAltKey(model);
                }
                type = type.BaseType;
            }
            if (ModelRemoved != null)
                ModelRemoved(this, new ModelCacheChangedEventArgs(value));
        }

        /// <summary>
        /// 移除多个模型对象
        /// </summary>
        /// <param name="values">要移除的模型对象集合的枚举器</param>
        public void RemoveRange(IEnumerable<IModelBase> values)
        {
            foreach (IModelBase value in values)
            {
                Remove(value);
            }
        }

        /// <summary>
        /// 获取模型对象的字典集合，如果没有该集合则创建该字典集合以及此类对象的父类模型对象的字典集合
        /// </summary>
        /// <param name="typeName">模型对象的类型名称</param>
        /// <returns>模型对象字典集合</returns>
        public ModelCollection this[string typeName]
        {
            get
            {
                ModelCollection modelCollection = null;
                typeDictionary.TryGetValue(typeName, out modelCollection);
                return modelCollection;
            }
        }

        /// <summary>
        /// 获取模型对象的字典集合，如果没有该集合则创建该字典集合以及此类对象的父类模型对象的字典集合
        /// </summary>
        /// <param name="type">模型对象的类型</param>
        /// <returns>模型对象字典集合</returns>
        public ModelCollection this[Type type]
        {
            get
            {
                ModelCollection modelCollection;
                if (!typeDictionary.TryGetValue(type.Name, out modelCollection))
                {
                    modelCollection = CreateModelCollection(type);
                }
                return modelCollection;
            }
        }

        /// <summary>
        /// 获取一个模型对象
        /// </summary>
        /// <param name="typeName">模型对象的类型名称</param>
        /// <param name="rid">模型对象的Rid</param>
        /// <returns>模型对象</returns>
        public IModelBase this[string typeName, int rid]
        {
            get
            {
                ModelCollection modelDictionary = this[typeName];
                if (modelDictionary != null && modelDictionary.Contains(rid))
                    return modelDictionary[rid];
                return null;
            }
        }

        /// <summary>
        /// 获取一个模型对象
        /// </summary>
        /// <param name="type">模型对象的类型</param>
        /// <param name="rid">模型对象的Rid</param>
        /// <returns>模型对象</returns>
        public IModelBase this[Type type, int rid]
        {
            get
            {
                ModelCollection modelDictionary = this[type];
                if (modelDictionary != null && modelDictionary.Contains(rid))
                    return modelDictionary[rid];
                return null;
            }
        }

        /// <summary>
        /// 获取一个模型对象
        /// </summary>
        /// <param name="typeName">模型对象的类型名称</param>
        /// <param name="altKey">模型对象的可选索引键</param>
        /// <returns>模型对象</returns>
        public IModelBase this[string typeName, string altKey]
        {
            get
            {
                ModelCollection modelDictionary = this[typeName];
                if (modelDictionary != null)
                    return modelDictionary[altKey];
                return null;
            }
        }

        /// <summary>
        /// 获取一个模型对象
        /// </summary>
        /// <param name="type">模型对象的类型</param>
        /// <param name="altKey">模型对象的可选索引键</param>
        /// <returns>模型对象</returns>
        public IModelBase this[Type type, string altKey]
        {
            get
            {
                ModelCollection modelDictionary = this[type];
                if (modelDictionary != null)
                    return modelDictionary[altKey];
                return null;
            }
        }

        /// <summary>
        /// 通过远程传输的模型对象获取本地对应的模型对象
        /// </summary>
        /// <param name="value">远程传输来的模型对象</param>
        /// <returns>本地模型对象</returns>
        public IModelBase this[IModelBase value]
        {
            get
            {
                ModelCollection modelDictionary = this[value.GetType()];
                if (modelDictionary != null && modelDictionary.Contains(value.Rid))
                    return modelDictionary[value.Rid];
                return null;
            }
        }

        /// <summary>
        /// 返回指定类型的对象列
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>对象集合索引器</returns>
        public static IEnumerable<T> GetList<T>() where T : IModelBase
        {
            return Instance[typeof(T)].ToArray<T>();
        }

        /// <summary>
        /// 返回指定类型的对象列
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>对象集合索引器</returns>
        public static T GetModel<T>(int rid) where T : IModelBase
        {
            try
            {
                object o = Instance[typeof(T), rid];
                if (o != null && o is T)
                    return (T)o;
                else
                    return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        private ModelCollection CreateModelCollection(Type type)
        {
            if (type.BaseType == typeof(object))
                return null;
            ModelCollection modelCollection;
            if (!typeDictionary.TryGetValue(type.Name, out modelCollection))
            {
                modelCollection = new ModelCollection();
                modelCollection.ModelType = type;
                typeDictionary.Add(type.Name, modelCollection);
                modelCollection.BaseCollection = CreateModelCollection(type.BaseType);
                return modelCollection;
            }
            return modelCollection;
        }
    }
}
