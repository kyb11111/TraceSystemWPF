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
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using TraceSystemWPF.Proxy;
using System.ServiceModel;
using System.Collections.ObjectModel;


namespace TraceSystemWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton== MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        System.Windows.Threading.DispatcherTimer timer;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool flag = TraceClientProxy.Instance.IsConnected;
            //TraceClientProxy.Instance.LoadModel();
            Storyboard sbd = Resources["leafLeave"] as Storyboard;
            sbd.Begin();
            Storyboard baiyun = Resources["cloudMove"] as Storyboard;
            baiyun.Begin();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool IsVaild = false;
            int type = 0;
            foreach (UserInfo user in ModelCacheManager.Instance[typeof(UserInfo)])
            {
                if (user.UserName == txtUserName.Text && user.PassWord == txtPassWord.Password)
                {
                    IsVaild = true;
                    type = user.UserType;
                    TraceClientProxy.Instance.UserRid = user.Rid;
                    break;
                }
            }
            if (IsVaild)
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
                if (type == 3)
                {
                    MainWindow wind = new MainWindow();
                    wind.ShowDialog();
                }
                if (type == 2)
                {
                    NHWindow wind1 = new NHWindow();
                    wind1.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("请输入正确用户名和密码");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {

        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement focusElement = Keyboard.FocusedElement as UIElement;
                if (focusElement != null)
                {
                    focusElement.MoveFocus(request);
                }
                e.Handled = true;
            }
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
