using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.Logging;
using Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
namespace Client.ViewModel
{
    public class WorkerViewModel : INotifyPropertyChanged
    {
        private User user;

        private ItemCourtViewModel _selectedItem;

        private ObservableCollection<Decision> _selectedCourtDecisions;
        public ObservableCollection<ItemCourtViewModel> Items { get; private set; }

        //private Court _selectedCourt;

        //public Court SelectedCourt
        //{
        //    get { return _selectedCourt; }
        //    set
        //    {
        //        _selectedCourt = value;
        //        OnPropertyChanged(nameof(SelectedCourt));
        //    }
        //}


        private Decision _selectedCourtDecision;

        public Decision SelectedCourtDecision
        {
            get { return _selectedCourtDecision; }
            set
            {
                if (_selectedCourtDecision != value)
                {
                    _selectedCourtDecision = value;
                    OnPropertyChanged(nameof(SelectedCourtDecision));
                }
            }
        }

        private string _newNum;
        private string _newNum1;
        private string _newNum2;
        private string _use;

        public string Use
        {
            get { return _use; }
            set
            {
                if (_use != value)
                {
                    _use = value;
                    OnPropertyChanged(nameof(Use));
                }
            }
        }
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
        public string NewNum1
        {
            get { return _newNum1; }
            set
            {
                if (_newNum1 != value)
                {
                    _newNum1 = value;
                    OnPropertyChanged(nameof(NewNum1));
                }
            }
        }
        public string NewNum2
        {
            get { return _newNum2; }
            set
            {
                if (_newNum2 != value)
                {
                    _newNum2 = value;
                    OnPropertyChanged(nameof(NewNum2));
                }
            }
        }

        public ICommand AddUserCommand { get; private set; }
        public ICommand UPDUserCommand { get; private set; }
        public ICommand OpenDisCommand { get; private set; }
        public ICommand SaveChCommand { get; private set; }
        public ICommand DelOrVidCommand { get; private set; }

        //private ItemCourtViewModel _selectedCourtItem;

        public WorkerViewModel()
        {

        }

        public WorkerViewModel(User use)
        {
            user = use;
            _use = "Акаунт: "+ user.Name;
            Items = new ObservableCollection<ItemCourtViewModel>();
            FilteredItems = new ObservableCollection<ItemCourtViewModel>();

            foreach (var item in user.Courts)
            {
                Items.Add(new ItemCourtViewModel {  Number = item.Number,  Poz = item.Poz,  Id=item.Id,  Vid= item.Vid, Isdel = item.Is_del,  Notes= item.Notes, _Decision = item.Decisions });
                FilteredItems.Add(new ItemCourtViewModel {  Number = item.Number,  Poz = item.Poz,  Id=item.Id,  Vid= item.Vid, Isdel = item.Is_del,  Notes= item.Notes, _Decision = item.Decisions });
            }
            AddUserCommand = new RelayCommand(ExecuteAddCommand, CanExecuteOpenAddCommand);
            UPDUserCommand = new RelayCommand(ExecuteUPDCommand);
            OpenDisCommand = new RelayCommand(ExecuteOPENCommand, CanExecuteOpenDisCommand);
            SaveChCommand = new RelayCommand(ExecuteSAVECommand, CanExecuteOpenDisCommand);
            DelOrVidCommand = new RelayCommand(ExecuteDelOrVidCommand, CanExecuteOpenDisCommand3);
            SelectedFilter = "Всі";
        }

        private async void ExecuteDelOrVidCommand(object parameter)
        {
            await Task.Run(() =>
            {
                Universal_TCP.SERVER_PROSTO(new MyRequst()
                {
                    Header = "DelOrVid",
                    User_1 = user,
                    Id = SelectedItem.Id
                });
            });
            
            this.UPDUserCommand.Execute(parameter);

        }
            
            private async void ExecuteSAVECommand(object parameter)
        {

            await Task.Run(() =>
            {
                Court te = new Court() { Decisions = SelectedCourtDecisions };
                var tmp = Universal_TCP.SERVER_PROSTO(new MyRequst()
                {
                    Header = "SAVE CH",
                    User_1 = user,
                    Id = SelectedItem.Id,
                    Court1 = new Court() { Decisions = SelectedCourtDecisions }

                }).Massage;
                System.Windows.Forms.MessageBox.Show(tmp);
                //this.UPDUserCommand.Execute(parameter);
            });
        }





        private async void ExecuteAddCommand(object parameter)
        {
            string tmp ="Some problem";
            IsLoading = true;
            try
            {
                await Task.Run(() =>
                {
                    tmp = Universal_TCP.SERVER_PROSTO(new MyRequst()
                    {
                        Header = "ADD COURT",
                        User_1 = user,
                        Inf = NewNum,
                        Inf1 = NewNum1,
                        Inf2 = NewNum2
                    }).Massage;



                });

                IsLoading = false;
                System.Windows.Forms.MessageBox.Show(tmp);
                this.UPDUserCommand.Execute(parameter);

                NewNum = "";
                NewNum1 = "";
                NewNum2 = "";

            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
           
            
        }
        private ItemCourtViewModel _selectedItemBeforeUpdate;
        private void ExecuteUPDCommand(object parameter)
        {
            _selectedItemBeforeUpdate = SelectedItem;
            var tmp1 = Universal_TCP.SERVER_PROSTO(new MyRequst() { Header = "UPD", User_1 = user });
            user = tmp1.Userss;

            Items.Clear();
            FilteredItems.Clear();

            foreach (var item in user.Courts)
            {
                Items.Add(new ItemCourtViewModel
                {
                    Number = item.Number,
                    Poz = item.Poz,
                    Id = item.Id,
                    Isdel =item.Is_del,
                    Vid = item.Vid,
                    Notes = item.Notes,
                    _Decision = item.Decisions
                });
                FilteredItems.Add(new ItemCourtViewModel
                {
                    Number = item.Number,
                    Poz = item.Poz,
                    Id = item.Id,
                    Isdel = item.Is_del,
                    Vid = item.Vid,
                    Notes = item.Notes,
                    _Decision = item.Decisions
                });
            }
            SelectedItem = _selectedItemBeforeUpdate;
            FilterItems();
        }


        private void ExecuteOPENCommand(object parameter)
        {

            ///ДОДАТИ ПЕРЕВІРКУ 
         
                var window = new Disig(_selectedCourtDecision.Index.ToString());
                window.Show();

            
        }
        private bool CanExecuteOpenAddCommand(object parameter)
        {
           if(NewNum!= ""&& NewNum1 != "" && NewNum2 != "")
            return true;
           else  return false;
        } 
        private bool CanExecuteOpenDisCommand(object parameter)
        {
            if (SelectedItem != null && SelectedItem._Decision != null && SelectedCourtDecision!=null)
                return true;

            return false;
        }
        private bool CanExecuteOpenDisCommand3(object parameter)
        {
            if (SelectedItem != null /*&& SelectedItem._Decision != null && SelectedCourtDecision!=null*/)
                return true;

            return false;
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
                    if (_selectedItem != null)
                    {
                        SelectedCourtDecisions = new ObservableCollection<Decision>(_selectedItem._Decision);
                        
                    }
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

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterItems(); // Викликаємо функцію фільтрації при зміні тексту пошуку
                }
            }
        }

        private ObservableCollection<ItemCourtViewModel> _filteredItems;
        public ObservableCollection<ItemCourtViewModel> FilteredItems
        {
            get { return _filteredItems; }
            set
            {
                if (_filteredItems != value)
                {
                    _filteredItems = value;
                    OnPropertyChanged(nameof(FilteredItems));
                }
            }
        }

        private string _selectedFilter;

        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    OnPropertyChanged(SelectedFilter);
                    FilterItems();
                }
            }
        }

        private void FilterItems()
        {
            var filtered = Items.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchText))
            {
                filtered = filtered.Where(item =>
                    item.Number.Contains(SearchText) ||
                    (!string.IsNullOrEmpty(item.Poz) && item.Poz.Contains(SearchText)) ||
                    (!string.IsNullOrEmpty(item.Vid) && item.Vid.Contains(SearchText)));
            }

            if (SelectedFilter == "System.Windows.Controls.ComboBoxItem: Актуальні")
            {
                filtered = filtered.Where(item => item.Isdel ==true);
            }
            else if (SelectedFilter == "System.Windows.Controls.ComboBoxItem: Завершені")
            {
                filtered = filtered.Where(item => item.Isdel == false);
            }
            else if (SelectedFilter == "System.Windows.Controls.ComboBoxItem: Всі")
            {
                filtered = filtered.Where(item => item.Isdel == false || item.Isdel==true);
            }
            FilteredItems = new ObservableCollection<ItemCourtViewModel>(filtered);
        }



        /// <summary>
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        /// </summary>


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
