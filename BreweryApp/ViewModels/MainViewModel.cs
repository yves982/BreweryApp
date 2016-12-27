using BreweryApp.ViewModels.Utils;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BreweryApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Privates
        private PropertyChangeSupport _pcs;
        private BeerSetViewModel _beerSet;
        private string _title;
        private SearchViewModel _search;
        private ICommand _quitCommand;
        private ICommand _loadCommand;
        private bool? _shouldClose;

        private async Task _load(object ignore)
        {
            _beerSet = new BeerSetViewModel();
            await ((DelegateCommandAsync<object>)_beerSet.LoadCommand).ExecuteAsync(ignore);
            _search.SearchCompleted += _beerSet.OnBeerSetUpdated;
            _search.SearchCancelled += _beerSet.OnReloadRequested;
            _pcs.NotifyChange("BeerSet");
        }

        private void _quit(object ignore)
        {
            ShouldClose = true;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _pcs.Handler += value; }
            remove { _pcs.Handler -= value; }
        }

        #region Properties
        public BeerSetViewModel BeerSet
        {
            get { return _beerSet; }
            set
            {
                _beerSet = value;
                _pcs.NotifyChange();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                _pcs.NotifyChange();
            }
        }

        public SearchViewModel Search
        {
            get { return _search; }
            set
            {
                _search = value;
                _pcs.NotifyChange();
            }
        }

        public ICommand LoadCommand
        {
            get { return _loadCommand; }
            set
            {
                _loadCommand = value;
                _pcs.NotifyChange();
            }
        }

        public ICommand QuitCommand
        {
            get { return _quitCommand; }
            set { _quitCommand = value; }
        }

        public bool? ShouldClose
        {
            get { return _shouldClose; }
            set
            {
                _shouldClose = value;
                _pcs.NotifyChange();
            }
        }
        #endregion

        public MainViewModel()
        {
            _pcs = new PropertyChangeSupport(this);
            _loadCommand = new DelegateCommandAsync<object>(_load);
            Title = "Brewery REST Client";
            _search = new SearchViewModel();
            _quitCommand = new DelegateCommand<object>(_quit);
        }

        ~MainViewModel()
        {
            _search.SearchCompleted -= _beerSet.OnBeerSetUpdated;
            _search.SearchCancelled -= _beerSet.OnReloadRequested;
        }
    }
}
