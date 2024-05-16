using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;
using Models;
namespace Client
{
    public class AdminViewModel
    {
        private User user;

        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public AdminViewModel(User use)
        {
            user = use;

            Items = new ObservableCollection<ItemViewModel>();

            // Додайте тестові дані
            foreach (var item in GetALL())
            {
                Items.Add(new ItemViewModel { Name = item.Name, Pass = item.Pass});
            }
           
    



            //LoginCommand = new RelayCommand(Login, CanLogin);
        }


        private List<User> GetALL()
        {
             var tmp = Universal_TCP.SERVER_PROSTO(new MyRequst() { Header = "GET ALL USERS"});
                return tmp.UsersList;
        }

    }
}
