using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HtmlAgilityPack;
using Models;
using mshtml;


namespace Server_1
{
    /// <summary>
    /// Логика взаимодействия для Pars.xaml
    /// </summary>
    public partial class Pars : Window
    {
        private string _num;
        public List<string> _dis;
        public List<Decision> _decisions;
        public string _dis_out;
        public string _disс;
        public bool _tmp;
        public Pars(string num)
        {
            _dis = new List<string>();
            _tmp = false;
            InitializeComponent();
            _num = num;
            webBrowser.Navigate("https://reyestr.court.gov.ua/");
            webBrowser.Navigated += (sender, args) => { HideScriptErrors((WebBrowser)sender, true); };
        }


        public Pars(string count, bool tmp)
        {
            _tmp = false;

            InitializeComponent();
            _disс = count;
            string qw = "https://reyestr.court.gov.ua/Review/" + count;
            System.Windows.Forms.MessageBox.Show(qw);
            webBrowser.Navigate(qw);
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
            if (_num != null)
                LoadAndClickAsync();
            else if(_disс != null)
                ParsDis();
            
        }
        private async void ParsDis()
        {
            _dis_out = Parsdis_text();
            if (_dis_out != null) { _tmp = true; this.Close();} ///не закривається 
        }

            private string Parsdis_text()
        {
            var htmlDocument = webBrowser.Document as mshtml.HTMLDocument;
            var htmlContent = htmlDocument.body.outerHTML;
            string decodedText = WebUtility.HtmlDecode(htmlContent); ////РАЗКОДИРОВКА ДОЛГО ИСКАЛЛЛЛ
            string bodyPattern = @"<body[^>]*>[\s\S]*?<\/body>";
            Match match = Regex.Match(decodedText, bodyPattern);
            if (match.Success)
            {
                string extractedText1 = match.Value;
                webBrowser.NavigateToString(extractedText1);
                //текст рішенні .
                
                string extractedText = Regex.Replace(extractedText1, "<.*?>", string.Empty);
                return extractedText;
            }
            return null;
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
                var elements = doc.all.tags("font");

                foreach (IHTMLElement element in (IEnumerable)elements)
                {
                    if (element.innerText.Contains ("Ваш запит не знайдено жодного документа"))
                    {

                        this.Close();

                    }
                }
            }

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
            //////ПЕРЕВІРКА
            
            
            List<string> tdValues = new List<string>();
            if (doc != null)
            {
                var elements = doc.all.tags("td");

                foreach (IHTMLElement element in (IEnumerable)elements)
                {
                    string className = element.className;

                    if (IsMatchingClass(className))
                    {
                        string tdValue = element.innerText;
                        tdValues.Add(tdValue);
                    }
                }
            }
            _decisions = ConvertToDecisions(tdValues);
            // значення для перевірки
            //foreach (var decision in tmp)
            //{
            //    System.Windows.Forms.MessageBox.Show($"Type: {decision.Type}, Date: {decision.Date}, Form: {decision.Form}"); /////ТУТ ДОБОВЛЯТЬ ДЛЯ ПАРСИНГА
            //}


            ////////


            // Повернути список зі всіма знайденими значеннями
            return reviewNumbers;
        }

        private List<Decision> ConvertToDecisions(List<string> tdValues)/////ТУТ ДОБОВЛЯТЬ ДЛЯ ПАРСИНГА
        {
            List<Decision> decisions = new List<Decision>();


            for (int i = 0; i < tdValues.Count + 5; i += 5) // Припускаємо, що дані йдуть групами по 3
            {
                if (i + 5 < tdValues.Count+1)
                {
                    Decision decision = new Decision
                    {
                        Type = tdValues[i],
                        Date = DateTime.Parse(tdValues[i + 1]),
                        Form = tdValues[i + 2],
                        NameCourt = tdValues[i + 3],
                        Namejudge = tdValues[i + 4]
                    };
                    decisions.Add(decision);
                }
            }
            return decisions;
        }

        private bool IsMatchingClass(string className)
        {
            string[] prefixes = { "VRType", "RegDate", "CSType", "CourtName", "ChairmenName",
                          "ChairmenName tr1 tdRightBottomBorder","ChairmenName tdRightBottomBorder", "tr1 tdRightBottomBorder" };

            foreach (string prefix in prefixes)
            {
                if (!string.IsNullOrEmpty(className) && className.Contains(prefix))
                {
                    return true;
                }
            }
            return false;
        }

        private void ParseDataAndNavigate()
        {
            System.Windows.Forms.MessageBox.Show("Test");
        }
    }
}
