using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _pass;
        private bool _active;
        private string _status;
        //public List<string> _statuses;

        //public ItemViewModel()
        //{
        //    Statuses = new List<string> { "Admin", "Director", "Worker" };
        //}

        public List<string> PossibleStatuses { get; } = new List<string> { "Admin", "Director", "Worker" };
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        public string Pass
        {
            get { return _pass; }
            set
            {
                if (_pass != value)
                {
                    _pass = value;
                    OnPropertyChanged(nameof(Pass));
                }
            }
        }
        
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    OnPropertyChanged(nameof(Active));
                }
            }
        }
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }


        //public List<string> Statuses
        //{
        //    get { return _statuses; }
        //    set
        //    {
        //        if (_statuses != value)
        //        {
        //            _statuses = value;
        //            OnPropertyChanged(nameof(Statuses));
        //        }
        //    }
        //}

     

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
    }
}
