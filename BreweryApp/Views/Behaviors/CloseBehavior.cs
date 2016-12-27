using System;
using System.Windows;

namespace BreweryApp.Views.Behaviors
{
    public static class CloseBehavior
    {
        public static readonly DependencyProperty ShouldCloseProperty = DependencyProperty.RegisterAttached(
            "ShouldClose", typeof(bool?), typeof(CloseBehavior), new PropertyMetadata(_onShouldCloseChanged));

        private static void _onShouldCloseChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs dpcea)
        {
            Window win = dObj as Window;
            if(win != null)
            {
                win.Close();
            }
        }

        public static bool? GetShouldClose(DependencyObject dObj)
        {
            return (bool?)dObj.GetValue(ShouldCloseProperty);
        }

        public static void SetShouldClose(DependencyObject dObj, bool? value)
        {
            dObj.SetValue(ShouldCloseProperty, value);
        }
    }
}
