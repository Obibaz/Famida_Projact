using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Azure.Core;
using Models;
using Newtonsoft.Json;

namespace Client.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private User _loginData;

        public LoginViewModel()
        {
            _loginData = new User();

            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        public string Username
        {
            get => _loginData.Name;
            set
            {
                _loginData.Name = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _loginData.Pass;
            set
            {
                _loginData.Pass = value;
                OnPropertyChanged(nameof(Password));
            }
        }


        public ICommand LoginCommand { get; }

        private bool CanLogin() => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);


        private void Login()
        {
            var request = new MyRequst() { User_1 = _loginData, Header = "Login" };

            MyResponse myResponse = Universal_TCP.SERVER_PROSTO(request);

            if (myResponse.Massage == "SUCCESS")
            {
                if (myResponse.Userss.Status == "Admin")
                {
                    var NewWIN = new Admin(myResponse.Userss);
                    CloseCurrentWindow();
                    NewWIN.ShowDialog();
                }
                if (myResponse.Userss.Status == "Worker")
                {
                    var NewWIN = new Worker(myResponse.Userss);
                    CloseCurrentWindow();
                    NewWIN.ShowDialog();
                }

            }
            else if (myResponse.Massage != "SUCCESS")
                System.Windows.Forms.MessageBox.Show(myResponse.Massage);
        }

        private void CloseCurrentWindow()
        {

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            window?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
