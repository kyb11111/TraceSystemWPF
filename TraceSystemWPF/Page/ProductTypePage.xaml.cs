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

namespace TraceSystemWPF.Page
{
    /// <summary>
    /// ProductTypePage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductTypePage : UserControl
    {
        public ProductTypePage()
        {
            InitializeComponent();
        }
        ProductType m_ProductType;
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_ProductType = dg.SelectedItem as ProductType;
            if (m_ProductType != null)
            {
                txtName.Text = m_ProductType.Name;
            }            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入产品分类名称");
                return;
            }
            foreach (ProductType p in ModelCacheManager.Instance[typeof(ProductType)])
            {
                if (p.Name == txtName.Text)
                {
                    MessageBox.Show("类型名称已重复");
                    return;
                }
            }
            m_ProductType = new ProductType();
            m_ProductType.Name = txtName.Text;
            m_ProductType.Remark = "";
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Insert;
            action.ExcuteObject = m_ProductType;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入产品分类名称");
                return;
            }
            foreach (ProductType p in ModelCacheManager.Instance[typeof(ProductType)])
            {
                if (p.Name == txtName.Text && p.Rid != m_ProductType.Rid)
                {
                    MessageBox.Show("类型名称已重复");
                    return;
                }
            }
            m_ProductType.Name = txtName.Text;
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Insert;
            action.ExcuteObject = m_ProductType;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (m_ProductType == null)
            {
                MessageBox.Show("请选择需要删除的对象");
                return;
            }
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Delete;
            action.ExcuteObject = m_ProductType;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dg.SelectedIndex = 0;
            dg.ItemsSource = ModelCacheManager.Instance[typeof(ProductType)];
        }
    }
}
