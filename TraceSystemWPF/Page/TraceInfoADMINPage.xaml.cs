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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TraceSystemWPF.Proxy;
using System.Collections.ObjectModel;
using TraceSystemWPF.Units;
using TraceSystemWPF.Dialog;
using System.IO;

namespace TraceSystemWPF.Page
{
    /// <summary>
    /// UserInfoMangermentPage.xaml 的交互逻辑
    /// </summary>
    public partial class TraceInfoADMINPage : UserControl
    {
        public TraceInfoADMINPage()
        {
            InitializeComponent();
        }

        Product m_Product;
        TraceInfo m_TraceInfo;
        byte[] PicByte;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TraceClientProxy.Instance.Proxy.ExcuteCompleted += new EventHandler<ExcuteCompletedEventArgs>(Proxy_ExcuteCompleted);
            dg.ItemsSource = ModelCacheManager.Instance[typeof(Product)];
            dg.SelectedIndex = 0;
        }

        void Proxy_ExcuteCompleted(object sender, ExcuteCompletedEventArgs e)
        {
            if (e.Result)
            {
                Binding();
            }
        }

        private void Binding()
        {
            if (m_Product.Rid == null) return;
            TraceInfo[] Array = (from m in ModelCacheManager.GetList<TraceInfo>()
                                 where m.Product == m_Product.Rid
                               select m).ToArray();
            dgTrace.ItemsSource = Array;
        }



        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (m_TraceInfo == null)
            {
                MessageBox.Show("请选择需要删除的对象");
                return;
            }
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Delete;
            action.ExcuteObject = m_TraceInfo;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
            TraceClientProxy.Instance.Proxy.DeleteFileAsync(m_Product.UserInfo.ToString() + "//Trace", m_TraceInfo.Name);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Product = dg.SelectedItem as Product;
            if (m_Product != null)
            {
                Binding();
            }          
        }

        private void btnInf_Click(object sender, RoutedEventArgs e)
        {
            if (m_TraceInfo != null)
            {
                TraceInfoDialog dialog = new TraceInfoDialog();
                dialog.TraceInfo = m_TraceInfo;
                dialog.Show();
            }
        }


        private void dgTrace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_TraceInfo = dgTrace.SelectedItem as TraceInfo;  
        }
        
    }
}
