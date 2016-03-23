using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceSystemWPF.Proxy;
using System.Windows.Threading;

namespace TraceSystemWPF
{
    public partial class TraceClientProxy
    {
        private bool _isConnected = false;

        public bool IsConnected
        {
            get { return _isConnected; }
        }

        private static TraceClientProxy s_instance;
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

        private TraceClientProxy()
        {
            m_clientProxy = ServiceUtil.GetServiceClient<RegistrationServiceClient>();
        }


        private RegistrationServiceClient m_clientProxy;
        public RegistrationServiceClient Proxy
        {
            get
            {
                if (m_clientProxy == null)
                {
                    //m_clientProxy = new RegistrationServiceClient();
                    m_clientProxy = ServiceUtil.GetServiceClient<RegistrationServiceClient>();
                }
                return m_clientProxy;
            }
        }
       
    }
}
