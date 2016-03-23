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
    public partial class DocumentSearchPage : UserControl
    {
        public DocumentSearchPage()
        {
            InitializeComponent();
        }
        private Document m_Document;

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Document = dgDocument.SelectedItem as Document;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgDocument.SelectedIndex = 0;
            dgDocument.ItemsSource = ModelCacheManager.Instance[typeof(Document)];
            cmbDocumentType.ItemsSource = ModelCacheManager.Instance[typeof(DocumentType)];
            cmbUserinfo.ItemsSource = ModelCacheManager.Instance[typeof(UserInfo)];
            cmbDepartment.ItemsSource = ModelCacheManager.Instance[typeof(Department)];
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Binding();
        }

        private void cmbDocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Binding();
        }

        private void cmbUserinfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Binding();
        }

        private void Binding()
        {
            ModelCollection<Document> Collection = new ModelCollection<Document>();
            Document[] DocArray = (from m in ModelCacheManager.GetList<Document>()
                                where (cmbDocumentType.SelectedValue == null || m.DocumentType == (int)cmbDocumentType.SelectedValue) &&
                                (cmbUserinfo.SelectedValue == null || m.UserInfo == (int)cmbUserinfo.SelectedValue) &&
                                (cmbDepartment.SelectedValue == null || m.Department == (int)cmbDepartment.SelectedValue) &&
                                (txtName.Text == "" || m.Name.Contains(txtName.Text))
                                select m).ToArray();
            dgDocument.ItemsSource = DocArray ;
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Binding();
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
            cmbDepartment.SelectedIndex = -1;
            cmbDocumentType.SelectedIndex = -1;
            cmbUserinfo.SelectedIndex = -1;
        }      
 
       
    }
}

