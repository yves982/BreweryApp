using BreweryApp.Models;
using BreweryApp.Repositories;
using BreweryApp.ViewModels.Entities;
using BreweryApp.ViewModels.Utils;
using ResotelApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BreweryApp.ViewModels
{
    public class BeerSetViewModel : IPagedViewModel, INotifyPropertyChanged
    {
        #region Privates
        private PropertyChangeSupport _pcs;
        private BeerSet _beerSet;
        private BeerRepository _beerRepository;
        private ICommand _loadCommand;
        private DelegateCommandAsync<Page> _selectPageCommand;
        private DelegateCommandAsync<string> _sortCommand;
        private bool _beersReloadSwitch;

        private Dictionary<string, ESortOrder> _sortOrders;

        private ObservableCollection<BeerViewModel> _beersCollection;
        private ICollectionView _beersView;
        private int _itemsPerPage;
        private int _pageCount;
        private bool _isLoaded;
        private ObservableCollection<int> _nextPages;
        private ICollectionView _nextPagesView;
        private bool _firstLoad;
        private EOrderField _lastOrderField;
        private bool _hasLastOrderField;

        private async Task _load(object ignore)
        {
            Logger.Log("LOAD: Loading beers");
            _beerRepository = new BeerRepository();
            _beerSet = await _beerRepository.GetBeersAsync(1);
            _beersCollection = new ObservableCollection<BeerViewModel>();
            
            foreach (Beer beer in _beerSet.Beers)
            {
                BeerViewModel beerVM = new BeerViewModel(beer);
                beerVM.BeerSelected += _beer_BeerSelected;
                _beersCollection.Add(beerVM);
            }

            _beersView = CollectionViewSource.GetDefaultView(_beersCollection);
            CurrentPage = 1;
            PageCount = _beerSet.NumberOfPages;

            _fillNextPages();
            _nextPagesView.MoveCurrentToFirst();

            ItemsPerPage = 50;
            IsLoaded = true;
            FirstLoad = false;
            Logger.Log("LOAD: Done");
        }

        private void _fillNextPages()
        {
            _nextPages.Clear();
            int min = Math.Max(1, CurrentPage - 2);
            int max = Math.Min(_beerSet.NumberOfPages, min+4);

            for (int i = min; i <= max; i++)
            {
                _nextPages.Add(i);
            }
        }

        private async Task _selectPage(Page page)
        {
            await _selectPage(page, false);
        }

        private async Task _selectPage(Page page, bool forceExecution = false)
        {
            if( (!_isLoaded || CurrentPage == page.Id) && !forceExecution)
            {
                return;
            }
            Logger.Log($"PAGE: Requesting page {page.Id}");
            IsLoaded = false;
            if (!_hasLastOrderField)
            {
                _beerSet = await _beerRepository.GetBeersAsync(1, page.Id);
            }
            else
            {
                ESortDir lastSortDir = _sortOrders[_lastOrderField.ToString()].ToSortDir();
                _beerSet = await _beerRepository.GetBeersAsync(1, page.Id, _lastOrderField, lastSortDir);
            }
            
            _fillBeersCollection();
            _beersView = CollectionViewSource.GetDefaultView(_beersCollection);

            PageCount = _beerSet.NumberOfPages;
            _fillNextPages();
            _nextPagesView.MoveCurrentTo(CurrentPage);
            IsLoaded = true;
            BeersReloadSwitch = !BeersReloadSwitch;
            Logger.Log($"PAGE: Done page {page.Id}");
        }

        private async void _nextPagesView_CurrentChanged(object sender, EventArgs e)
        {
            int currentPageId = 1;
            if(_nextPagesView.CurrentItem != null)
            {
                currentPageId = int.Parse(_nextPagesView.CurrentItem.ToString());
            }
            Page currentPage = new Page { Id=currentPageId };
            await _selectPage(currentPage);
        }

        private async Task _loadBeersWithLastSortParameters(string fieldName)
        {
            if (_sortOrders[fieldName] == ESortOrder.None)
            {
                _beerSet = await _beerRepository.GetBeersAsync(1, CurrentPage);
                _hasLastOrderField = false;
            }
            else
            {
                _hasLastOrderField = true;
                EOrderField orderField = (EOrderField)Enum.Parse(typeof(EOrderField), fieldName);
                ESortDir sortDir = _sortOrders[fieldName].ToSortDir();
                _lastOrderField = orderField;
                _beerSet = await _beerRepository.GetBeersAsync(1, CurrentPage, orderField, sortDir);
            }
        }

        private void _beer_BeerSelected(object sender, BeerViewModel beerVM)
        {
            _beersView.MoveCurrentTo(beerVM);
        }

        private void _fillBeersCollection()
        {
            _beersCollection.Clear();

            foreach (Beer beer in _beerSet.Beers)
            {
                BeerViewModel beerViewModel = new BeerViewModel(beer);
                beerViewModel.BeerSelected += _beer_BeerSelected;
                _beersCollection.Add(beerViewModel);
            }
        }

        private void _restoreOtherFieldsSortOrder(string fieldName)
        {
            string[] sortFields = new string[_sortOrders.Count];
            _sortOrders.Keys.CopyTo(sortFields, 0);
            for (int i = 0; i < sortFields.Length; i++)
            {
                string sortField = sortFields[i];
                if (!sortField.Equals(fieldName, StringComparison.InvariantCulture))
                {
                    _sortOrders[sortField] = ESortOrder.None;
                    _pcs.NotifyChange(nameof(SortOrders));
                }
            }
        }

        private async Task _sort(string fieldName)
        {
            Logger.Log($"SORT: Sorting field {fieldName} {_sortOrders[fieldName]}");
            IsLoaded = false;
            _beerRepository = new BeerRepository();

            _restoreOtherFieldsSortOrder(fieldName);

            await _loadBeersWithLastSortParameters(fieldName);
            
            _fillBeersCollection();
            _beersView = CollectionViewSource.GetDefaultView(_beersCollection);

            PageCount = _beerSet.NumberOfPages;
            _fillNextPages();
            _nextPagesView.MoveCurrentTo(CurrentPage);

            ItemsPerPage = 50;
            IsLoaded = true;
            Logger.Log($"SORT: Done field {fieldName} {_sortOrders[fieldName]}");
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _pcs.Handler += value; }
            remove { _pcs.Handler -= value; }
        }

        #region Properties

        /// <summary> Sort orders : used to store fields's last requested sort orders</summary>
        /// <remarks>It's here in case we use multi fields sorting, later on
        /// right now there's only one at a time
        /// </remarks>
        public Dictionary<string, ESortOrder> SortOrders
        {
            get { return _sortOrders; }
            set
            {
                _sortOrders = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Initialize this object, retrieves data from repository </summary>
        public ICommand LoadCommand
        {
            get { return _loadCommand; }
            set { _loadCommand = value; }
        }

        /// <summary> Async Command to select a given Page (will send a request to the web service) </summary>
        public DelegateCommandAsync<Page> SelectPageCommand
        {
            get { return _selectPageCommand; }
            set { _selectPageCommand = value; }
        }

        /// <summary> Async Command to retrieve Beers in a given order (will send a request to the web service)</summary>
        public DelegateCommandAsync<string> SortCommand
        {
            get { return _sortCommand; }
            set { _sortCommand = value; }
        }

        /// <summary> True if we're in first load phase, False otherwise</summary>
        public bool FirstLoad
        {
            get { return _firstLoad; }
            set
            {
                _firstLoad = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Identifier of current BeerSet's first Page</summary>
        public int FirstPageId
        {
            get { return 1; }
        }

        /// <summary> Current Page Identifier </summary>
        public int CurrentPage
        {
            get { return _beerSet.CurrentPage; }
            set
            {
                _beerSet.CurrentPage = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Maximum number of items per page (last one may hold less items)</summary>
        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set
            {
                _itemsPerPage = value;
                _pcs.NotifyChange();
                _pcs.NotifyChange(nameof(PageCount));
            }
        }

        /// <summary> Number of Pages in this BeerSet</summary>
        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                _pageCount = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Number of Beers in this BeerSet </summary>
        public int ItemCount
        {
            get { return _beerSet.TotalResults; }
            set
            {
                _beerSet.TotalResults = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> Property to change to indicate a Page has just been requested</summary>
        /// <remarks>Used to reset scrollbar on page selection, through Views.ScrollToTopBehavior</remarks>
        public bool BeersReloadSwitch
        {
            get { return _beersReloadSwitch; }
            set
            {
                _beersReloadSwitch = value;
                _pcs.NotifyChange();
            }
        }

        /// <summary> View of BeerSet's BeerViewModel</summary>
        /// <remarks>Each BeerViewModel represents a detailess Beer, until details gets requested</remarks>
        public ICollectionView BeersView
        {
            get { return _beersView; }
        }

        /// <summary> View of the Pages Collection </summary>
        /// <remarks>Used for navigation, pages are generated according to CurrentPage</remarks>
        public ICollectionView NextPagesView
        {
            get { return _nextPagesView; }
        }

        /// <summary>False if this BeerSet is being loaded, True otherwise</summary>
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                _pcs.NotifyChange();
            }
        }
        #endregion


        public BeerSetViewModel()
        {
            _pcs = new PropertyChangeSupport(this);
            _loadCommand = new DelegateCommandAsync<object>(_load);
            _selectPageCommand = new DelegateCommandAsync<Page>(_selectPage);
            _sortCommand = new DelegateCommandAsync<string>(_sort);
            _nextPages = new ObservableCollection<int>();
            _nextPagesView = CollectionViewSource.GetDefaultView(_nextPages);
            _sortOrders = new Dictionary<string, ESortOrder> {
                { "Name", ESortOrder.None },
                { "IsOrganic", ESortOrder.None }
            };
            _isLoaded = false;
            _beersReloadSwitch = false;
            FirstLoad = true;
            _nextPagesView.CurrentChanged += _nextPagesView_CurrentChanged;
        }

        ~BeerSetViewModel()
        {
            _nextPagesView.CurrentChanged -= _nextPagesView_CurrentChanged;
        }

        #region Methods
        /// <summary>
        /// Synchronize itself with given BeerSet
        /// </summary>
        /// <param name="sender">Unused - EventHandler pattern</param>
        /// <param name="beerSet">BeerSet retrieved by another ViewModel</param>
        public void OnBeerSetUpdated(object sender, BeerSet beerSet)
        {
            try
            {
                IsLoaded = false;
                _beerSet = beerSet;
                _fillBeersCollection();
                _beersView = CollectionViewSource.GetDefaultView(_beersCollection);
 
                CurrentPage = beerSet.CurrentPage;
                PageCount = beerSet.NumberOfPages;

                _fillNextPages();
                _nextPagesView.MoveCurrentToPosition(CurrentPage - 1);

                ItemsPerPage = 50;
                IsLoaded = true;
                BeersReloadSwitch = !BeersReloadSwitch;
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Reload first Page
        /// </summary>
        /// <param name="sender">Unused - EventHandler pattern</param>
        /// <param name="ea">Unused - EventHandler pattern</param>
        /// <remarks>Called when SearchViewModel has completed its job</remarks>
        public async void OnReloadRequested(object sender, EventArgs ea)
        {
            try
            {
                Page firstPage = new Page { Id = 1 };
                await _selectPage(firstPage, true);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
        #endregion
    }
}
