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
    /// DapartmentManagmentPage.xaml 的交互逻辑
    /// </summary>
    public partial class DapartmentManagmentPage : UserControl
    {
        public DapartmentManagmentPage()
        {
            InitializeComponent();
        }

        private Department m_Department;

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Department = dgDeparment.SelectedItem as Department;
            if (m_Department != null)
            {
                txtName.Text = m_Department.Name;
                txtRemark.Text = m_Department.Remark;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgDeparment.SelectedIndex = 0;
            dgDeparment.ItemsSource = ModelCacheManager.Instance[typeof(Department)];
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入部门名称");
                return;
            }

            foreach (Department dept in ModelCacheManager.Instance[typeof(Department)])
            {
                if (txtName.Text == dept.Name)
                {
                    MessageBox.Show("部门名已被占用，请重新添加");
                    return;
                }
            }

            Department newModel = new Department();
            newModel.Name = txtName.Text;
            newModel.Remark = txtRemark.Text;
            
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Insert;
            action.ExcuteObject = newModel;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入部门名称");
                return;
            }
            if (m_Department == null)
            {
                MessageBox.Show("请选择需要修改的对象");
                return;
            }
            foreach (Department dept in ModelCacheManager.Instance[typeof(Department)])
            {
                if (dept.Name == txtName.Text && dept.Rid != m_Department.Rid)
                {
                    MessageBox.Show("部门名已被占用，请重新添加");
                    return;
                }
            }
            m_Department.Name = txtName.Text;
            m_Department.Remark = txtRemark.Text;
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Update;
            action.ExcuteObject = m_Department;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (m_Department == null)
            {
                MessageBox.Show("请选择需要删除的对象");
                return;
            }
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Delete;
            action.ExcuteObject = m_Department;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }
    }
}
