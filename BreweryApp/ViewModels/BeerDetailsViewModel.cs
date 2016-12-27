using BreweryApp.Models;
using BreweryApp.ViewModels.Entities;
using BreweryApp.ViewModels.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace BreweryApp.ViewModels
{
    /// <summary>
    /// ViewModel for Beer
    /// </summary>
    public class BeerDetailsViewModel : INotifyPropertyChanged
    {
        #region Privates
        private PropertyChangeSupport _pcs;
        private ObservableCollection<BasicIngredient> _ingredients;
        private ObservableCollection<string> _foodPairings;
        private float _alcohoolPercent;
        private float _bitterness;
        private string _description;
        private BeerColor _beerColor;
        private bool _mustShow;
        private ICommand _closeCommand;

        private void _close(object ignore)
        {
            MustShow = false;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _pcs.Handler += value; }
            remove { _pcs.Handler -= value; }
        }

        #region Properties
        /// <summary> Beer's Ingredients </summary>
        public ObservableCollection<BasicIngredient> Ingredients
        {
            get { return _ingredients; }
            set
            {
                _ingredients = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Recommended food to go along this Beer </summary>
        public ObservableCollection<string> FoodPairings
        {
            get { return _foodPairings; }
            set
            {
                _foodPairings = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Alcohool by volume </summary>
        public float AlcohoolPercent
        {
            get { return _alcohoolPercent; }
            set
            {
                _alcohoolPercent = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> IBU International Biterness Unit </summary>
        public float Bitterness
        {
            get { return _bitterness; }
            set
            {
                _bitterness = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Beer description (no particular format is expected) </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Beer color : retrieved from Srm (Standard reference method) </summary>
        public BeerColor Color
        {
            get { return _beerColor; }
            set
            {
                _beerColor = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> True if Beer details have been retrieved, False otherwise </summary>
        public bool MustShow
        {
            get { return _mustShow; }
            set
            {
                _mustShow = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Command to hide details from user </summary>
        public ICommand CloseCommand
        {
            get { return _closeCommand; }
            set { _closeCommand = value; }
        }

        #endregion

        /// <summary> Constructor </summary>
        /// <param name="beer">The Beer instance retrieved from repository</param>
        public BeerDetailsViewModel(Beer beer)
        {
            _pcs = new PropertyChangeSupport(this);
            _mustShow = false;
            _ingredients = new ObservableCollection<BasicIngredient>();
            _foodPairings = new ObservableCollection<string>();

            _closeCommand = new DelegateCommand<object>(_close);

            if (beer.Ingredients != null)
            {
                foreach (Ingredient ingredient in beer.Ingredients)
                {
                    BasicIngredient basicIngredient = new BasicIngredient(ingredient);
                    _ingredients.Add(basicIngredient);
                }
            }

            string[] foodPairings = beer.FoodPairing?.Split(',');
            if (foodPairings != null)
            {
                foreach (string food in foodPairings)
                {
                    _foodPairings.Add(food.Trim());
                }
            }
            if (beer.Abv != null)
            {
                _alcohoolPercent = float.Parse(beer.Abv, CultureInfo.CreateSpecificCulture("en-US"));
            }

            if (System.Math.Abs(beer.Ibu) > 0.1f)
            {
                _bitterness = beer.Ibu;
            }
            _description = beer.Description;

            if (beer.Srm != null)
            {
                _beerColor = new BeerColor(beer.Srm);
            }
        }

        #region Methods
        /// <summary> Sets BeerDetails </summary>
        public void FillBeer(Beer beer)
        {
            beer.Ingredients = new List<Ingredient>();
            foreach (BasicIngredient basicIngredient in _ingredients)
            {
                Ingredient ingredient = basicIngredient.ToIngredient();
                beer.Ingredients.Add(ingredient);
            }

            beer.FoodPairing = string.Join(",", _foodPairings);
            beer.Abv = $"{beer.Abv:#.##} %";
            beer.Ibu = _bitterness;
            beer.Description = _description;
            beer.Srm = _beerColor.ToSrm();
        }
        #endregion
    }
}
