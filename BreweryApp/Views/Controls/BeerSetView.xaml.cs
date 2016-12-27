using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BreweryApp.Views.Controls
{
    /// <summary>
    /// Logique d'interaction pour BeerSetView.xaml
    /// </summary>
    public partial class BeerSetView : UserControl
    {
        private static void _onSortCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public static readonly DependencyProperty SortCommandProperty = DependencyProperty.Register(
            "SortCommand", typeof(ICommand), typeof(BeerSetView), new FrameworkPropertyMetadata(_onSortCommandChanged));

        public ICommand SortCommand
        {
            get { return (ICommand)GetValue(SortCommandProperty); }
            set { SetValue(SortCommandProperty, value); }
        }

        public BeerSetView()
        {
            InitializeComponent();
        }
    }
}
