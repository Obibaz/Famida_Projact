using System;
using System.Collections;
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
using mshtml;


namespace Server_1
{
    /// <summary>
    /// Логика взаимодействия для Pars.xaml
    /// </summary>
    public partial class Pars : Window
    {
        private string _num;
        public bool _tmp;
        public List<string> _dis;
        public Pars(string num)
        {
            _dis = new List<string>();
            _tmp = false;
            InitializeComponent();
            _num = num;
            webBrowser.Navigate("https://reyestr.court.gov.ua/");
            webBrowser.Navigated += (sender, args) => { HideScriptErrors((WebBrowser)sender, true); };
        }

        public Pars()
        { }

            public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadAndClickAsync();
        }


        private async void LoadAndClickAsync()
        {
            IHTMLDocument2 doc = (IHTMLDocument2)webBrowser.Document;
            IHTMLInputElement caseNumberInput = (IHTMLInputElement)doc.all.item("CaseNumber");
            if (caseNumberInput != null)
            {
                caseNumberInput.value = _num;

                // Викликати метод "click" через виконання JavaScript
                webBrowser.InvokeScript("eval", "document.getElementById('btn').click();");
            }

            // Очікування завершення навігації
            webBrowser.Navigating += WebBrowser_Navigating;
            _dis = ParseData();
            if (_dis.Count != 0) { _tmp = true;  this.Close(); }
            

        }

        private void WebBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            // Перевірка, чи навігація завершилася
            if (e.Uri == null || e.Uri.AbsoluteUri == "about:blank")
            {
                // Навігація завершилася, можна викликати метод
                ParseDataAndNavigate();
            }
        }

        private List<string> ParseData()
        {
            // Створити список для зберігання всіх знайдених значень
            List<string> reviewNumbers = new List<string>();

            // Отримати всі елементи <a> на сторінці
            IHTMLDocument2 doc = (IHTMLDocument2)webBrowser.Document;

            if (doc != null)
            {
                var elements = doc.all.tags("a");

                foreach (IHTMLElement element in (IEnumerable)elements)
                {
                    // Перевірити, чи це посилання з класом "doc_text2"
                    if (element.className == "doc_text2")
                    {
                        // Отримати текст посилання (наприклад, номер огляду)
                        string reviewNumber = element.innerText;
                        reviewNumbers.Add(reviewNumber);
                    }
                }
            }

            // Повернути список зі всіма знайденими значеннями
            return reviewNumbers;
        }


        private void ParseDataAndNavigate()
        {
            System.Windows.Forms.MessageBox.Show("Test");
        }
    }
}
