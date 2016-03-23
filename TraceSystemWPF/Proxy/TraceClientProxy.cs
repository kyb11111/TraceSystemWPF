using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace TraceSystemWPF.Proxy
{
    public class TraceClientProxy
    {
        private bool _isConnected = false;

        public bool IsConnected
        {
            get { return _isConnected; }
        }

        private static TraceClientProxy s_instance = null;
        public static TraceClientProxy Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new TraceClientProxy();
                }
                return s_instance;
            }
        }

        public int UserRid { get; set; }

        private TraceServiceClient m_clientProxy = null;
        public TraceServiceClient Proxy
        {
            get
            {
                if (m_clientProxy == null)
                {
                    TraceServiceCallback callback = new TraceServiceCallback();
                    InstanceContext clientContext = new InstanceContext(callback);
                    m_clientProxy = new TraceServiceClient(clientContext);
                }
                return m_clientProxy;
            }
        }

        private TraceClientProxy()
        {
            TraceServiceCallback callback = new TraceServiceCallback();
            InstanceContext clientContext = new InstanceContext(callback);
            m_clientProxy = new TraceServiceClient(clientContext);
            m_clientProxy.Open();
            _isConnected = true;
            m_clientProxy.Verify("123","456");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            LoadModel();
        }

        public void LoadModel()
        {
            ObservableCollection<string> ModelType = m_clientProxy.GetClientChcheModelTypeName();
            foreach (string type in ModelType)
            {
                ObservableCollection<ModelBase> ModelCollection = m_clientProxy.GetListAll(type);
                foreach (ModelBase model in ModelCollection)
                {
                    ModelCacheManager.Instance.Save(model);
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (m_clientProxy.State.HasFlag(CommunicationState.Faulted)
                || m_clientProxy.State.HasFlag(CommunicationState.Closed))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                TraceServiceCallback callback = new TraceServiceCallback();
                InstanceContext clientContext = new InstanceContext(callback);
                m_clientProxy = new TraceServiceClient(clientContext);
                m_clientProxy.Open();
                m_clientProxy.Verify("123", "456");
                LoadModel();
            }
            else
            {
                LoadModel();
            }
        }
    }
}
