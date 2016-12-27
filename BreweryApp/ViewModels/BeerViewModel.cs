using BreweryApp.Models;
using BreweryApp.Repositories;
using BreweryApp.ViewModels.Utils;
using ResotelApp.Utils;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BreweryApp.ViewModels
{
    public class BeerViewModel : INotifyPropertyChanged
    {
        #region Privates
        private string _id;
        private string _displayName;
        private int? _year;
        private string _styleName;
        private bool _isOrganic;
        private string _imagePath;
        private ICommand _imageLoadedCommand;
        private ICommand _showDetailsCommand;
        private bool _isLoaded;
        private PropertyChangeSupport _pcs;
        private BeerDetailsViewModel _details;
        private BeerRepository _beerRepository;

        private void _imageLoaded(object ignore)
        {
            IsLoaded = true;
        }

        private async Task _showDetails(object ignore)
        {
            Beer seekedBeer = await _beerRepository.GetBeerAsync(_id);
            BeerDetailsViewModel detailedBeer = new BeerDetailsViewModel(seekedBeer);

            Details = detailedBeer;
            BeerSelected?.Invoke(null, this);
            detailedBeer.MustShow = true;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _pcs.Handler += value; }
            remove { _pcs.Handler -= value; }
        }

        public event EventHandler<BeerViewModel> BeerSelected;

        #region Properties
        /// <summary> Beer Identifier </summary>
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Beer's DisplayName </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Beer Style Name</summary>
        /// <remarks> A Style is a set of technical (physical or chemical) parameters describing a beer's kind</remarks>
        public string StyleName
        {
            get { return _styleName; }
            set
            {
                _styleName = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Beer's vintage year </summary>
        public int? Year
        {
            get { return _year; }
            set
            {
                _year = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> True of 95% of the ingredients were grown naturally (without GMO, pesticides, ...)</summary>
        public bool IsOrganic
        {
            get { return _isOrganic; }
            set
            {
                _isOrganic = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Absolute Uri of the Beer's image </summary>
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                _pcs.NotifyChange();
            } 
        }

        /// <summary> True if a beer's been loaded into memory </summary>
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Beer's advanced properties (such as bitterness, alcohool by volume, ...) </summary>
        /// <remarks>These aren't quite secondary, but they're loaded in a later phase</remarks>
        public BeerDetailsViewModel Details
        {
            get { return _details; }
            set
            {
                _details = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Command to indicate a Beer's image has been loaded </summary>
        public ICommand ImageLoadedCommand
        {
            get { return _imageLoadedCommand; }
            set { _imageLoadedCommand = value; }
        }

        /// <summary> Command to show Beer's advanced properties</summary>
        public ICommand ShowDetailsCommand
        {
            get { return _showDetailsCommand; }
            set { _showDetailsCommand = value; }
        }
        #endregion

        public BeerViewModel(Beer beer)
        {
            _pcs = new PropertyChangeSupport(this);
            _beerRepository = new BeerRepository();

            if(beer == null)
            {
                throw new ArgumentNullException(nameof(beer));
            }

            _id = beer.Id;
            _displayName = beer.DisplayName;
            _year = beer.Year;
            _styleName = beer.Style?.Name;
            _isOrganic = beer.IsOrganic;

            if (beer.Labels?.Medium != null)
            {
                _imagePath = beer.Labels.Medium.AbsoluteUri;
            }
            else
            {
                _imagePath = ConfigurationManager.AppSettings["DefaultBeerImagePath"];
            }

            _isLoaded = false;
            _imageLoadedCommand = new DelegateCommand<object>(_imageLoaded);
            _showDetailsCommand = new DelegateCommandAsync<object>(_showDetails);

            _details = new BeerDetailsViewModel(beer);
        }

        #region Methods
        /// <summary>
        /// Retrieve a Beer out of this BeerViewModel
        /// </summary>
        /// <returns>Converted Beer</returns>
        /// <remarks>This would be usefull when modifying data</remarks>
        public Beer ToBeer()
        {
            Beer beer = null;
            try
            {
                beer = new Beer();
                beer.Id = _id;
                beer.DisplayName = _displayName;
                beer.Style = new Style();
                beer.Style.Name = _styleName;
                beer.Year = _year;
                beer.IsOrganic = _isOrganic;

                if (_details != null)
                {
                    _details.FillBeer(beer);
                }
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return beer;
        }
        #endregion
    }
}
