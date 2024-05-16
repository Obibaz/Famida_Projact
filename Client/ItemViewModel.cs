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
        private string _name;
        private string _pass;

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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
    }
}
