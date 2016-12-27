using BreweryApp.Models;
using BreweryApp.ViewModels.Utils;
using System;
using System.ComponentModel;

namespace BreweryApp.ViewModels.Entities
{
    public class BeerColor : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _colorHex;
        private PropertyChangeSupport _pcs;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _pcs.Handler += value; }
            remove { _pcs.Handler -= value; }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                _pcs.NotifyChange();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _pcs.NotifyChange();
            }
        }

        public string ColorHex
        {
            get { return _colorHex; }
            set
            {
                _colorHex = value;
                _pcs.NotifyChange();
            }
        }

        public BeerColor(Srm srm)
        {
            _pcs = new PropertyChangeSupport(this);

            if(srm == null)
            {
                throw new ArgumentNullException("srm");
            }

            _id = srm.Id;
            _name = srm.Name;
            _colorHex =$"#{srm.Hex}";
        }

        public Srm ToSrm()
        {
            Srm srm = new Srm {
                Id = _id,
                Name = _name,
                Hex = _colorHex
            };
            return srm;
        }
    }
}
