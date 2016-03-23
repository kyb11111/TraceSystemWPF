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

namespace TraceSystemWPF.Page
{
    /// <summary>
    /// UserInfoMangermentPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductInsertAdminPage : UserControl
    {
        public ProductInsertAdminPage()
        {
            InitializeComponent();
        }

        Product m_Product;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPruductType.ItemsSource = ModelCacheManager.Instance[typeof(ProductType)];
            TraceClientProxy.Instance.Proxy.ExcuteCompleted += new EventHandler<ExcuteCompletedEventArgs>(Proxy_ExcuteCompleted);
            Binding();
        }

        void Proxy_ExcuteCompleted(object sender, ExcuteCompletedEventArgs e)
        {
                //throw new NotImplementedException();
            if (e.Result)
            {
                Binding();
            }
        }

        private void Binding()
        {
            Product[] Array = (from m in ModelCacheManager.GetList<Product>()
                               select m).ToArray();
            dg.ItemsSource = Array;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入产品名称");
                return;
            }

            if (cmbPruductType.SelectedIndex == -1)
            {
                MessageBox.Show("请选择产品类别");
            }

            m_Product = new Product();
            m_Product.Name = txtName.Text;
            m_Product.UserInfo = TraceClientProxy.Instance.UserRid;
            m_Product.ProductType = Convert.ToInt32(cmbPruductType.SelectedValue);
            m_Product.Remark = "";

            TraceClientProxy.Instance.Proxy.SaveQRCodeAsync(m_Product.UserInfo.ToString(), m_Product.Name.ToString());

            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Insert;
            action.ExcuteObject = m_Product;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入产品名称");
                return;
            }

            if (cmbPruductType.SelectedIndex == -1)
            {
                MessageBox.Show("请选择产品类别");
            }

            TraceClientProxy.Instance.Proxy.DeleteFileAsync(m_Product.UserInfo.ToString(), m_Product.Name.ToString());
            m_Product.Name = txtName.Text;
            m_Product.ProductType = Convert.ToInt32(cmbPruductType.SelectedValue);
            m_Product.UserInfo = TraceClientProxy.Instance.UserRid;
            TraceClientProxy.Instance.Proxy.SaveQRCodeAsync(m_Product.UserInfo.ToString(), m_Product.Name.ToString());
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Update;
            action.ExcuteObject = m_Product;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (m_Product == null)
            {
                MessageBox.Show("请选择需要删除的对象");
                return;
            }
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Delete;
            action.ExcuteObject = m_Product;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
            foreach (TraceInfo info in ModelCacheManager.Instance[typeof(TraceInfo)])
            {
                if (info.Product == m_Product.Rid)
                {
                    ExcuteAction act = new ExcuteAction();
                    act.ExcuteType = ExcuteType.Delete;
                    act.ExcuteObject = info;
                    TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Product = dg.SelectedItem as Product;
            if (m_Product != null)
            {
                txtName.Text = m_Product.Name;
                cmbPruductType.SelectedValue = m_Product.ProductType;
            }          
        }

        private void btnInf_Click(object sender, RoutedEventArgs e)
        {
            ProductInfoDialog dialog = new ProductInfoDialog();
            dialog.Product = m_Product;
            dialog.Show();
        }
        
    }
}
