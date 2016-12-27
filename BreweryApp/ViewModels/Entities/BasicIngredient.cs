using BreweryApp.Models;
using BreweryApp.ViewModels.Utils;
using System;
using System.ComponentModel;

namespace BreweryApp.ViewModels.Entities
{
    public class BasicIngredient : INotifyPropertyChanged
    {
        private PropertyChangeSupport _pcs;
        private int _id;
        private string _name;
        private string _categoryDisplayName;

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

        public string CategoryDisplayName
        {
            get { return _categoryDisplayName; }
            set
            {
                _categoryDisplayName = value;
                _pcs.NotifyChange();
            }
        }

        public BasicIngredient(Ingredient ingredient)
        {
            _pcs = new PropertyChangeSupport(this);

            if(ingredient == null)
            {
                throw new ArgumentNullException(nameof(ingredient));
            }

            _id = ingredient.Id;
            _name = ingredient.Name;
            _categoryDisplayName = ingredient.CategoryDisplay;
        }

        public Ingredient ToIngredient()
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Id = _id;
            ingredient.Name = _name;
            ingredient.CategoryDisplay = _categoryDisplayName;
            return ingredient;
        }
    }
}