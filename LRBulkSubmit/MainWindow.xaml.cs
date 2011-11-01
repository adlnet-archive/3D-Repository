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
using System.IO;
using System.Web;
using System.Data.Common;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Windows.Threading;
using LR;
using vwarDAL;
namespace LRTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ContentObject co = new ContentObject();
            co.PID = "adl:1";
            co.SubmitterEmail = "robert.chadwick.ctr@adlnet.gov";
            co.Keywords="test,boat,model,gun";
            string result = LR_3DR_Bridge.ModelUploadedInternal(co);   
        }
        void AllowUIToUpdate()
        {

            DispatcherFrame frame = new DispatcherFrame();

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate(object parameter)
            {

                frame.Continue = false;

                return null;

            }), null);

            Dispatcher.PushFrame(frame);

        }
        private void DoSubmitAll()
        {
            vwarDAL.DataAccessFactory df = new DataAccessFactory();
            vwarDAL.IDataRepository dal = df.CreateDataRepositorProxy();
            IEnumerable<ContentObject> objects = dal.GetAllContentObjects();

            int total = 0;
            int current = 0;
            foreach (ContentObject co in objects)
            {
                total++;
            }


            progressBar1.Maximum = total;
            progressBar2.Maximum = 4;
            
            foreach (ContentObject co in objects)
            {
                
                current++;
                progressBar1.Value = current;
                progressBar2.Value = 0;
                AllowUIToUpdate();
                textBlock1.Text = LR_3DR_Bridge.ModelDownloadedInternal(co);
                progressBar2.Value = 1;
                AllowUIToUpdate();
                ContentObject co2 = dal.GetContentObjectById(co.PID, false, true);
                textBlock1.Text = LR_3DR_Bridge.ModelRatedInternal(co2);
                progressBar2.Value = 2;
                AllowUIToUpdate();
                textBlock1.Text = LR_3DR_Bridge.ModelUploadedInternal(co);
                progressBar2.Value = 3;
                AllowUIToUpdate();
                textBlock1.Text = LR_3DR_Bridge.ModelViewedInternal(co);
                progressBar2.Value = 4;
                AllowUIToUpdate();
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DoSubmitAll();
        }
    }
}
