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
    /// DocumentSearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentTypeManagerPage : UserControl
    {
        public DocumentTypeManagerPage()
        {
            InitializeComponent();
        }
        private DocumentType m_DocumentType;

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_DocumentType = dgDocumentType.SelectedItem as DocumentType;
            if (m_DocumentType != null)
            {
                txtName.Text = m_DocumentType.TypeName;
                txtRemark.Text = m_DocumentType.Remark;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgDocumentType.SelectedIndex = 0;
            dgDocumentType.ItemsSource = ModelCacheManager.Instance[typeof(DocumentType)];
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入文档类型名称");
                return;
            }

            foreach (DocumentType DocumentType in ModelCacheManager.Instance[typeof(DocumentType)])
            {
                if (txtName.Text == DocumentType.TypeName)
                {
                    MessageBox.Show("名称已被占用，请重新添加");
                    return;
                }
            }

            DocumentType newModel = new DocumentType();
            newModel.TypeName = txtName.Text;
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
                MessageBox.Show("请输入类型名称");
                return;
            }
            if (m_DocumentType == null)
            {
                MessageBox.Show("请选择需要修改的对象");
                return;
            }
            foreach (DocumentType DocumentType in ModelCacheManager.Instance[typeof(DocumentType)])
            {
                if (DocumentType.TypeName == txtName.Text && DocumentType.Rid != m_DocumentType.Rid)
                {
                    MessageBox.Show("部门名已被占用，请重新添加");
                    return;
                }
            }
            m_DocumentType.TypeName = txtName.Text;
            m_DocumentType.Remark = txtRemark.Text;
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Update;
            action.ExcuteObject = m_DocumentType;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (m_DocumentType == null)
            {
                MessageBox.Show("请选择需要删除的对象");
                return;
            }
            ExcuteAction action = new ExcuteAction();
            action.ExcuteType = ExcuteType.Delete;
            action.ExcuteObject = m_DocumentType;
            TraceClientProxy.Instance.Proxy.ExcuteAsync(new ObservableCollection<ExcuteAction>() { action });
        }
    }
}
