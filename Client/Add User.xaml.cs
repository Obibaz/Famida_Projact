using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для Add_User.xaml
    /// </summary>
    public partial class Add_User : Window
    {
        public User NewUser { get; private set; }
        public Add_User()
        {
            InitializeComponent();
            StatusTextBox.Items.Add("Admin");
            StatusTextBox.Items.Add("Director");
            StatusTextBox.Items.Add("Worker");

            StatusTextBox.SelectedItem = "Worker";
        }
        public User GetUser()
        {
            return NewUser;
        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.Length > 0 && PassTextBox.Text.Length>0)
            {
                NewUser = new User
                {
                    Name = NameTextBox.Text,
                    Pass = PassTextBox.Text,
                    Active = ActiveCheckBox.IsChecked ?? false,
                    Status = StatusTextBox.Text
                };

                DialogResult = true;
            }
            else System.Windows.Forms.MessageBox.Show("Заповінсть всі рядки");
        }
    }
}
