using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceSystemWPF.Proxy
{
    class TraceServiceCallback : Proxy.ITraceServiceCallback
    {
        public void ExcuteNotify(System.Collections.ObjectModel.ObservableCollection<Proxy.ExcuteAction> actions)
        {
            foreach (ExcuteAction action in actions)
            {
                switch (action.ExcuteType)
                {
                    case ExcuteType.Insert:
                        ModelCacheManager.Instance.Save(action.ExcuteObject as ModelBase);
                        break;
                    case ExcuteType.Update:
                        ModelCacheManager.Instance.Save(action.ExcuteObject as ModelBase);
                        break;
                    case ExcuteType.Delete:
                        ModelCacheManager.Instance.Remove(action.ExcuteObject as ModelBase);
                        break;
                }
            }
        }

        public void RealTimeNotify(string modelType, System.Collections.ObjectModel.ObservableCollection<Proxy.RealtimeData> data)
        {

        }

        public void ErrorNotify(string serviceName, string message, Proxy.ExcuteType type)
        {
        }

        public void ping()
        {

        }

        public void CloseSession()
        {

        }


        public IAsyncResult BeginExcuteNotify(System.Collections.ObjectModel.ObservableCollection<ExcuteAction> actions, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndExcuteNotify(IAsyncResult result)
        {
        }

        public IAsyncResult BeginRealTimeNotify(string modelType, System.Collections.ObjectModel.ObservableCollection<RealtimeData> data, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndRealTimeNotify(IAsyncResult result)
        {
        }

        public IAsyncResult BeginErrorNotify(string serviceName, string message, ExcuteType type, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndErrorNotify(IAsyncResult result)
        {
        }

        public IAsyncResult Beginping(AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void Endping(IAsyncResult result)
        {
        }

        public IAsyncResult BeginCloseSession(AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndCloseSession(IAsyncResult result)
        {
        }
    }
}
