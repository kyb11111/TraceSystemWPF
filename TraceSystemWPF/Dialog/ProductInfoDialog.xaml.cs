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
    /// ProductInfoDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ProductInfoDialog : MetroWindow
    {
        public ProductInfoDialog()
        {
            InitializeComponent();
        }

        private Product m_Product;

        public Product Product
        {
            get { return m_Product; }
            set 
            { 
                m_Product = value;
                txtName.Text = m_Product.Name;
                ProductType type = ModelCacheManager.Instance[typeof(ProductType), m_Product.ProductType] as ProductType;
                txtProductType.Text = type.Name;
                txtRemark.Text = m_Product.Remark;
                TraceClientProxy.Instance.Proxy.DownloadTextFileCompleted += new EventHandler<DownloadTextFileCompletedEventArgs>(Proxy_DownloadTextFileCompleted);
                TraceClientProxy.Instance.Proxy.DownloadTextFileAsync(m_Product.UserInfo.ToString(), m_Product.Name.ToString());
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
