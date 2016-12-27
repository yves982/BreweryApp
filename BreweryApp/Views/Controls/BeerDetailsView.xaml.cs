using System.Windows;
using System.Windows.Controls;

namespace BreweryApp.Views.Controls
{
    /// <summary>
    /// Logique d'interaction pour DetailedBeerView.xaml
    /// </summary>
    public partial class BeerDetailsView : UserControl
    {
        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
            "ImagePath", typeof(string), typeof(BeerDetailsView));

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName", typeof(string), typeof(BeerDetailsView));

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public BeerDetailsView()
        {
            InitializeComponent();
        }
    }
}
