using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.Logging;
using Models;
namespace Client.ViewModel
{
    public class WorkerViewModel : INotifyPropertyChanged
    {
        private User user;

        private ItemCourtViewModel _selectedItem;

        private ObservableCollection<Decision> _selectedCourtDecisions;
        public ObservableCollection<ItemCourtViewModel> Items { get; private set; }

        private string _newNum;

        public string NewNum
        {
            get { return _newNum; }
            set
            {
                if (_newNum != value)
                {
                    _newNum = value;
                    OnPropertyChanged(nameof(NewNum));
                }
            }
        }

        public ICommand AddUserCommand { get; private set; }
        public ICommand UPDUserCommand { get; private set; }

        //private ItemCourtViewModel _selectedCourtItem;

        public WorkerViewModel()
        {

        }

        public WorkerViewModel(User use)
        {
            user = use;

            Items = new ObservableCollection<ItemCourtViewModel>();

            foreach (var item in user.Courts)
            {
                Items.Add(new ItemCourtViewModel {  Number = item.Number,  Poz = item.Poz,  Id=item.Id,  Vid= item.Vid,  Notes= item.Notes, _Decision = item.Decisions });

            }
            AddUserCommand = new RelayCommand(ExecuteAddCommand);
            UPDUserCommand = new RelayCommand(ExecuteUPDCommand);
        }

        private void ExecuteAddCommand(object parameter)
        {
            System.Windows.Forms.MessageBox.Show(Universal_TCP.SERVER_PROSTO(new MyRequst() { Header = "ADD COURT", User_1 = user , Inf = NewNum}).Massage);
            this.UPDUserCommand.Execute(parameter);
        }

        private void ExecuteUPDCommand(object parameter)
        {
            var tmp1 = Universal_TCP.SERVER_PROSTO(new MyRequst() { Header = "UPD", User_1 = user });
            user = tmp1.Userss;

            Items.Clear();

            foreach (var item in user.Courts)
            {
                Items.Add(new ItemCourtViewModel
                {
                    Number = item.Number,
                    Poz = item.Poz,
                    Id = item.Id,
                    Vid = item.Vid,
                    Notes = item.Notes,
                    _Decision = item.Decisions
                });
            }
        }

        // Викликайте метод, який оновлює список ListBox
        public ItemCourtViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    SelectedCourtDecisions = new ObservableCollection<Decision>(_selectedItem._Decision);
                }
            }
        }

        public ObservableCollection<Decision> SelectedCourtDecisions
        {
            get { return _selectedCourtDecisions; }
            set
            {
                if (_selectedCourtDecisions != value)
                {
                    _selectedCourtDecisions = value;
                    OnPropertyChanged(nameof(SelectedCourtDecisions));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
