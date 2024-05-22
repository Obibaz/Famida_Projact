using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.Logging;
using Models;
namespace Client.ViewModel
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private User user;

        private ItemViewModel _selectedItem;
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AddUserCommand { get; private set; }

        public AdminViewModel()
        {

        }

        public AdminViewModel(User use)
        {
            user = use;

            Items = new ObservableCollection<ItemViewModel>();


            foreach (var item in GetALL())
            {
                Items.Add(new ItemViewModel { Name = item.Name, Pass = item.Pass, Id = item.Id, Active = item.Active, Status = item.Status });
            }
            SaveCommand = new RelayCommand(ExecuteSaveCommand, (parameter) => SelectedItem != null);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, (parameter) => SelectedItem != null);
            UpdateCommand = new RelayCommand(ExecuteUpdateCommand);
            AddUserCommand = new RelayCommand(ExecuteAddCommand);

        }





        private void ExecuteAddCommand(object parameter)
        {
            User user = new User();
            var addUserWindow = new Add_User();
            if (addUserWindow.ShowDialog() == true)
            {
                var newUser = addUserWindow.GetUser();
                user = new User { Name = newUser.Name, Pass = newUser.Pass, Id = newUser.Id, Active = newUser.Active, Status = newUser.Status };
            }
            if (user.Name != null && user.Pass != null)
                System.Windows.Forms.MessageBox.Show(Universal_TCP.SERVER_PROSTO(new MyRequst() { User_1 = user, Header = "ADD USER" }).Massage);
            UpdateCommand.Execute(null);

        }
        private void ExecuteDeleteCommand(object parameter)
        {
            if (SelectedItem != null)
            {
                System.Windows.Forms.MessageBox.Show(Universal_TCP.SERVER_PROSTO(new MyRequst()
                {
                    Id = SelectedItem.Id,
                    Header = "DELETE USER"
                }).Massage);
                UpdateCommand.Execute(null);
            }
        }
        private void ExecuteUpdateCommand(object parameter)
        {
            Items.Clear();
            foreach (var item in GetALL())
            {
                Items.Add(new ItemViewModel { Name = item.Name, Pass = item.Pass, Id = item.Id, Active = item.Active, Status = item.Status });
            }
        }

        private void ExecuteSaveCommand(object parameter)
        {
            if (SelectedItem != null)
            {
                User user = new User() { Name = SelectedItem.Name, Pass = SelectedItem.Pass, Status = SelectedItem.Status, Active = SelectedItem.Active };
                System.Windows.Forms.MessageBox.Show(Universal_TCP.SERVER_PROSTO(new MyRequst()
                {
                    Id = SelectedItem.Id,
                    User_1 = user,
                    Header = "SAVE CHENGE"
                }).Massage);
            }
        }


        // Викликайте метод, який оновлює список ListBox
        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));



                }
            }
        }





        private List<User> GetALL()
        {
            var tmp = Universal_TCP.SERVER_PROSTO(new MyRequst() { Header = "GET ALL USERS" });
            return tmp.UsersList;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
