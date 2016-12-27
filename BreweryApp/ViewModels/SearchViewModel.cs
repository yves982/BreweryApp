
using BreweryApp.Models;
using BreweryApp.Repositories;
using BreweryApp.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BreweryApp.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Privates
        private PropertyChangeSupport _pcs;
        private string _query;
        private BeerRepository _beerRepository;
        private ICommand _cancelSearchCommand;
        private ICommand _searchCommand;
        private ObservableCollection<BeerViewModel> _beerCollection;
        private string _searchIconPath;
        private bool _searchCancelled;
        private ValidationHelper _validations;

        private void _cancelSearch(object ignore)
        {
            _query = "";
            _searchCancelled = true;
            
            _pcs.NotifyChange(nameof(Query));
            bool canCancelSearch = _cancelSearchCommand.CanExecute(null);

            if (canCancelSearch)
            {
                ((DelegateCommand<object>)_cancelSearchCommand).ChangeCanExecute();
            }

            DelegateCommandAsync<string> searchCommand = (DelegateCommandAsync<string>)_searchCommand;
            if(searchCommand.CanExecute(null))
            {
                searchCommand.ChangeCanExecute();
            }

            SearchCancelled?.Invoke(this, new EventArgs());
        }

        private async Task _search(string query)
        {
            _beerCollection.Clear();
            BeerSet beerSet = await _beerRepository.SearchBeersAsync(1, query);
            SearchCompleted?.Invoke(this, beerSet);
        }

        private string _validateQuery()
        {
            string errMessage = null;
            
            if (string.IsNullOrEmpty(_query) && !_searchCancelled)
            {
                errMessage = "Le champ de recherche doit être renseigné. Cliquez sur le boutton annuler";
            }
            
            return errMessage;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _pcs.Handler += value; }
            remove { _pcs.Handler -= value; }
        }

        public event EventHandler<BeerSet> SearchCompleted;
        public event EventHandler SearchCancelled;


        #region Properties
        /// <summary>Seeked keyword</summary>
        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                
                bool validatesQuery = _validations[nameof(Query)] == null;
                bool initialCanSearch = _searchCommand.CanExecute(null);
                bool canCancelSearch = _cancelSearchCommand.CanExecute(null);

                if(validatesQuery)
                {
                    _searchCancelled = false;
                }

                if(!canCancelSearch)
                { 
                    ((DelegateCommand<object>)_cancelSearchCommand).ChangeCanExecute();
                }

                if ((validatesQuery != initialCanSearch) && (validatesQuery || initialCanSearch))
                {
                    ((DelegateCommandAsync<string>)_searchCommand).ChangeCanExecute();
                }
                _pcs.NotifyChange();
            }
        }

        /// <summary> Full path (from project's root) to the SearchIcon </summary>
        /// <remarks>This is currently pointing to an Icon tile set (manipulated through Converters/CroppedBitmap)</remarks>
        public string SearchIconPath
        {
            get { return _searchIconPath; }
            set
            {
                _searchIconPath = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Command to cancel an ongoing search (including if it's invalid) </summary>
        public ICommand CancelSearchCommand
        {
            get { return _cancelSearchCommand; }
            set { _cancelSearchCommand = value; }
        }

        /// <summary> Command to start seeking Beers</summary>
        public ICommand SearchCommand
        {
            get { return _searchCommand; }
            set { _searchCommand = value; }
        }
        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get { return _validations.Error; }
        }
        
        public string this[string columnName]
        {
            get { return _validations[columnName]; }
        }
        #endregion

        public SearchViewModel()
        {
            _pcs = new PropertyChangeSupport(this);
            _beerRepository = new BeerRepository();
            _beerCollection = new ObservableCollection<BeerViewModel>();
            _cancelSearchCommand = new DelegateCommand<object>(_cancelSearch, false);
            _searchCommand = new DelegateCommandAsync<string>(_search, false);
            _searchIconPath = ConfigurationManager.AppSettings["IconsPath"];
            Dictionary<string, Func<string>> validations = new Dictionary<string, Func<string>>
            {
                { nameof(Query), _validateQuery }
            };
            _searchCancelled = true;
            _validations = new ValidationHelper(validations);
        }
    }
}
