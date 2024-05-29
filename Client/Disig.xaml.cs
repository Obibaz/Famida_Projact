using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Models;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Disig.xaml
    /// </summary>
    public partial class Disig : Window
    {
        public Disig( string tmp)
        {
            InitializeComponent();
            string qw = "https://reyestr.court.gov.ua/Review/" + tmp;
            webBrowser.Navigate(qw);
            webBrowser.Navigated += (sender, args) => { HideScriptErrors((WebBrowser)sender, true); };
        }

        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }

    }
}

