﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Client.ViewModel
{
    public class ItemCourtViewModel : INotifyPropertyChanged
    {
        private int _id;
        private string _number;
        private string _poz;
        private string _vid;
        private string _notes;
        private ICollection<Decision> _decisions;

        //public List<string> _statuses;

        //public ItemViewModel()
        //{
        //    Statuses = new List<string> { "Admin", "Director", "Worker" };
        //}

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
