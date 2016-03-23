using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;

namespace TraceSystemWPF.Proxy
{
    public interface IModelBase : INotifyPropertyChanged
    {
        int Rid
        {
            get;
        }

        string AlternateKey
        {
            get;
        }

        string Type
        {
            get;
        }

        //void SetRealtimeData(RealtimeData data)
        //{
        //}

        void Initialize();

        IModelBase Clone();

        #region 关联属性
        /// <summary>
        /// 获取多对一关联中的关联父对象
        /// </summary>
        ParentModel ParentModel
        {
            get;
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        ChildrenModel ChildrenModel
        {
            get;
        }

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <typeparam name="T">关联子对象类型</typeparam>
        /// <returns>关联子对象集合</returns>
        IEnumerable<T> GetChildren<T>() where T : IModelBase;

        /// <summary>
        /// 获取一对多关联中的子对象集合
        /// </summary>
        /// <typeparam name="T">关联子对象类型</typeparam>
        /// <param name="childPropertyName">关联子对象对应的成员属性名称</param>
        /// <returns>关联子对象集合</returns>
        IEnumerable<T> GetChildren<T>(string childPropertyName) where T : IModelBase;

        /// <summary>
        /// 获取多对一关联中的关联父对象
        /// </summary>
        /// <typeparam name="T">关联父对象类型</typeparam>
        /// <returns>关联父对象</returns>
        T GetParent<T>() where T : IModelBase;

        /// <summary>
        /// 获取多对一关联中的关联父对象
        /// </summary>
        /// <typeparam name="T">关联父对象类型</typeparam>
        /// <param name="parentPropertyName">关联父对象对应的成员属性名称</param>
        /// <returns>关联父对象</returns>
        T GetParent<T>(string parentPropertyName) where T : IModelBase;
        #endregion
    }

    public class ModelNull : IModelBase
    {
        private static ModelNull s_null;
        public static IModelBase Null
        {
            get
            {
                if (s_null != null)
                    s_null = new ModelNull();
                return s_null;
            }
        }

        private ModelNull()
        {
        }

        public int Rid
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string AlternateKey
        {
            get { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { throw new NotImplementedException(); }
        }

        public int RefCount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public ParentModel ParentModel
        {
            get { throw new NotImplementedException(); }
        }

        public ChildrenModel ChildrenModel
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<T> GetChildren<T>() where T : IModelBase
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetChildren<T>(string childPropertyName) where T : IModelBase
        {
            throw new NotImplementedException();
        }

        public T GetParent<T>() where T : IModelBase
        {
            throw new NotImplementedException();
        }

        public T GetParent<T>(string parentPropertyName) where T : IModelBase
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public IModelBase Clone()
        {
            return null;
        }
    }
}
