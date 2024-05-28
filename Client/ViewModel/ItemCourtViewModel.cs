using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.ViewModel
{
    public class ItemCourtViewModel : INotifyPropertyChanged
    {
        private int _id;
        private string _number;
        private string _poz;
        private string _vid;
        private string _notes;
        private string _date;
        private string _dlain;

        private ICollection<Decision> _decisions;

        private string _newNum;

        //public List<string> _statuses;

        //public ItemViewModel()
        //{
        //    Statuses = new List<string> { "Admin", "Director", "Worker" };
        //}

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


        public string Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    OnPropertyChanged(nameof(Number));
                }
            }
        }
        //public string Dates
        //{
        //    get { return _date; }
        //    set
        //    {
        //        if (_date != value)
        //        {
        //            _date = value;
        //            OnPropertyChanged(nameof(Dates));
        //        }
        //    }
        //}
        //public string Dlain
        //{
        //    get { return _dlain; }
        //    set
        //    {
        //        if (_dlain != value)
        //        {
        //            _dlain = value;
        //            OnPropertyChanged(nameof(Dlain));
        //        }
        //    }
        //}


        public string Poz
        {
            get { return _poz; }
            set
            {
                if (_poz != value)
                {
                    _poz = value;
                    OnPropertyChanged(nameof(Poz));
                }
            }
        }

        public string Vid
        {
            get { return _vid; }
            set
            {
                if (_vid != value)
                {
                    _vid = value;
                    OnPropertyChanged(nameof(Vid));
                }
            }
        }
        public string Notes
        {
            get { return _notes; }
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged(nameof(Notes));
                }
            }
        }


        public ICollection<Decision> _Decision
        {
            get { return _decisions; }
            set
            {
                if (_decisions != value)
                {
                    _decisions = value;
                    OnPropertyChanged(nameof(_Decision));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
