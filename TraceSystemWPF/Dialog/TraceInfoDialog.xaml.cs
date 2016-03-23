using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using TraceSystemWPF.Proxy;
using System.IO;
using System.Drawing;

namespace TraceSystemWPF.Dialog
{
    /// <summary>
    /// TraceInfoInfoDialog.xaml 的交互逻辑
    /// </summary>
    public partial class TraceInfoDialog : MetroWindow
    {
        public TraceInfoDialog()
        {
            InitializeComponent();
        }

        private TraceInfo m_TraceInfo;

        public TraceInfo TraceInfo
        {
            get { return m_TraceInfo; }
            set 
            { 
                m_TraceInfo = value;
                txtName.Text = m_TraceInfo.Name;
                Product p = ModelCacheManager.Instance[typeof(Product),m_TraceInfo.Product] as Product;
                txtProductType.Text = p.Name + " " + m_TraceInfo.DateTime.ToString();
                txtRemark.Text = m_TraceInfo.TextInfo;
                TraceClientProxy.Instance.Proxy.DownloadTextFileCompleted += new EventHandler<DownloadTextFileCompletedEventArgs>(Proxy_DownloadTextFileCompleted);
                TraceClientProxy.Instance.Proxy.DownloadTextFileAsync(p.UserInfo + "\\Trace", m_TraceInfo.Name.ToString());
            }
        }

        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }

        void Proxy_DownloadTextFileCompleted(object sender, DownloadTextFileCompletedEventArgs e)
        {
            TraceClientProxy.Instance.Proxy.DownloadTextFileCompleted -= new EventHandler<DownloadTextFileCompletedEventArgs>(Proxy_DownloadTextFileCompleted);
            if (e.Result == true)
            {
                img.Source = ByteArrayToBitmapImage(e.fileContent);
            }
        }
    }
}
