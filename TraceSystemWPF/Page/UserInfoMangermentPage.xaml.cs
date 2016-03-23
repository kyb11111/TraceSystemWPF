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

namespace TraceSystemWPF.Page
{
    /// <summary>
    /// UserInfoMangermentPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoMangermentPage : UserControl
    {
        public UserInfoMangermentPage()
        {
            InitializeComponent();
        }       

        private UserInfo m_UserInfo;
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbUserType.SelectedIndex == -1)
            {
                MessageBox.Show("选择用户类型");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入用户名");
                return;
            }

            foreach (UserInfo usr in ModelCacheManager.Instance[typeof(UserInfo)])
            {
                if (usr.UserName == txtName.Text)
                {
                    MessageBox.Show("已存在用户名");
                    return;
                }
            }
            m_UserInfo = new UserInfo();
            m_UserInfo.UserName = txtName.Text;
            m_UserInfo.UserType = Convert.ToInt32(cmbUserType.SelectedValue);
            m_UserInfo.PassWord = "123456";
            m_UserInfo.Remark = "";
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Insert;
            action.ExcuteObject = m_UserInfo;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }


        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (m_UserInfo == null)
            {
                MessageBox.Show("请选择需要删除的对象");
                return;
            }
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Delete;
            action.ExcuteObject = m_UserInfo;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_UserInfo = dgUserInfo.SelectedItem as UserInfo;
            if (m_UserInfo != null)
            {
                txtName.Text = m_UserInfo.UserName;
                cmbUserType.SelectedValue = m_UserInfo.UserType;
            }            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgUserInfo.SelectedIndex = 0;
            dgUserInfo.ItemsSource = ModelCacheManager.Instance[typeof(UserInfo)];
            List<UserTypeEnum> EnumList = new List<UserTypeEnum>();
            EnumList.Add(new UserTypeEnum() { EnumValue = 1, EnumString = "用户" });
            EnumList.Add(new UserTypeEnum() { EnumValue = 2, EnumString = "农户" });
            EnumList.Add(new UserTypeEnum() { EnumValue = 3, EnumString = "管理" });
            cmbUserType.ItemsSource = EnumList;
        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbUserType.SelectedIndex == -1)
            {
                MessageBox.Show("选择用户类型");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入用户名");
                return;
            }

            foreach (UserInfo usr in ModelCacheManager.Instance[typeof(UserInfo)])
            {
                if (usr.UserName == txtName.Text && m_UserInfo.Rid != usr.Rid)
                {
                    MessageBox.Show("已存在用户名");
                    return;
                }
            }
            m_UserInfo.UserName = txtName.Text;
            m_UserInfo.UserType = Convert.ToInt32(cmbUserType.SelectedValue);
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Update;
            action.ExcuteObject = m_UserInfo;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }
    }
}
