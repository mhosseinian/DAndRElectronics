using System.Windows.Controls;

namespace Common.Helpers
{
    public class ListBoxScrollToView : ListBox
    {
        public ListBoxScrollToView() : base()
        {
            SelectionChanged += new SelectionChangedEventHandler(ListBoxScroll_SelectionChanged);
        }

        void ListBoxScroll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollIntoView(SelectedItem);
        }
    }
}