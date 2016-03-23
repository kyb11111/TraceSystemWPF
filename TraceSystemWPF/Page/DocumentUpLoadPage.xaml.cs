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
using Microsoft.Win32;
using System.IO;
using TraceSystemWPF.Proxy;
using TraceSystemWPF.Twain;
using System.Drawing;

namespace TraceSystemWPF.Page
{
    /// <summary>
    /// DocumentUpLoadPage.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentUpLoadPage : UserControl
    {
        public DocumentUpLoadPage()
        {
            InitializeComponent();
        }

        private void cmbDocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnFileLoad_Click(object sender, RoutedEventArgs e)
        {
            if (WpfTwain.Instance.Select())
            {
                btnFile.IsEnabled = true;
                WpfTwain.Instance.Acquire(true);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WpfTwain.Instance.TwainTransferReady += new TwainTransferReadyHandler(TwainInterface_TwainTransferReady);
            WpfTwain.Instance.TwainCloseRequest += new TwainEventHandler(TwainInterface_TwainCloseRequest);
        }

        void TwainInterface_TwainCloseRequest(WpfTwain sender)
        {

        }

        void TwainInterface_TwainTransferReady(WpfTwain sender, List<ImageSource> imageSources)
        {
            image.Source = imageSources.First();
            byte[] buffer = ReadImageMemory(image.Source as BitmapSource);
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            WpfTwain.Instance.Acquire(false);
        }

        private static byte[] ReadImageMemory( BitmapSource source)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memoryStream);
            return memoryStream.GetBuffer();
        }



        public static byte[] bitmapimagetobytearray(BitmapImage bmp)
        {
            byte[] bytearray = null;

            try
            {
                Stream smarket = bmp.StreamSource;

                if (smarket != null && smarket.Length > 0)
                {
                    smarket.Position = 0;
                    using (BinaryReader br = new BinaryReader(smarket))
                    {
                        bytearray = br.ReadBytes((int)smarket.Length);
                    }
                }
            }
            catch
            {
                //other exception handling
            }

            return bytearray;
        }
    }
}
