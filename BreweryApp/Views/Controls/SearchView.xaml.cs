using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BreweryApp.Views.Controls
{
    /// <summary>
    /// Logique d'interaction pour SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public static readonly DependencyProperty CanEditSearchProperty = DependencyProperty.Register(
            "CanEditSearch", typeof(bool), typeof(SearchView));

        public bool CanEditSearch
        {
            get { return (bool)GetValue(CanEditSearchProperty); }
            set { SetValue(CanEditSearchProperty, value); }
        }


        public SearchView()
        {
            InitializeComponent();
        }
    }
}
