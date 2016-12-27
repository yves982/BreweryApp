using System;
using System.Windows;
using System.Windows.Controls;

namespace BreweryApp.Views.Behaviors
{
    public static class ScrollToTopBehavior
    {
        public static readonly DependencyProperty ScrollToTopProperty =
            DependencyProperty.RegisterAttached("ScrollToTop", typeof(bool?), typeof(ScrollToTopBehavior),
            new PropertyMetadata(_scrollToTopChanged));

        public static bool? GetScrollToTop(DependencyObject dObj)
        {
            return (bool?)dObj.GetValue(ScrollToTopProperty);
        }

        public static void SetScrollToTop(DependencyObject dObj, bool? value)
        {
            dObj.SetValue(ScrollToTopProperty, value);
        }

        private static void _scrollToTopChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs dpcea)
        {
            bool? scrollToTop = GetScrollToTop(dObj);
            ListView listView = (ListView)dObj;

            if(listView.Items.Count > 0)
            {
                listView.ScrollIntoView(listView.Items[0]);
            }
        }
    }
}
