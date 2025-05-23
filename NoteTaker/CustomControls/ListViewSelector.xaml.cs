using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace NoteTaker.CustomControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ListViewSelector : UserControl
    {
        private string _selection;

        public ListViewSelector()
        {
            InitializeComponent();
            this.DataContext = this;

            Title = "title";
            ListItemFontSize = 14;
            ListLabelFontSize = 14;
            List = selectionList;
            _selection = "";

            selectionList.FontSize = ListItemFontSize;
            selectionLabel.FontSize = ListLabelFontSize;

        }

        public string Title { get; set; }
        public int ListItemFontSize { get; set; }
        public int ListLabelFontSize { get; set; }
        public Boolean IsSearch {  get; set; } // If true the list will scroll to the first matcing item
        public Boolean IsDigitsOnly { get; set; } // If true then only digits can be typed in text box
        public ListView List { get; set; }

        // Sets this._selection to the passed argument
        // Sets the text box to the passed argument
        // Invokes a custom event to signal that a selection change has occurred
        public String Selection 
        {
            get { return _selection; }
            set 
            {
                value = ItemToString(value);
                _selection = value;
                selectionTextBox.Text = value;
                selectionTextBox.Select(selectionTextBox.Text.Length, 0);
                ListSelectionChanged?.Invoke(this, EventArgs.Empty);
            } 
        }

        // A custom event that signals when a selection change has occurred
        public event EventHandler? ListSelectionChanged;

        // Runs when an item in the list is clicked
        // Sets this.Selection to the item clicked
        private void SelectionList_Click(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                var item = listView.SelectedItem;
                if (item != null)
                {
                    string selectString = ItemToString(item.ToString());

                    Selection = selectString;
                }
            }
        }

        // When enter is pressed in the selectionTextBox, this.Selection is set to the text in text box only if present in the list
        private void SelectionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (IndexOf(selectionTextBox.Text) >= 0) {
                    Selection = selectionTextBox.Text;
                }
                
            }
        }

        // When selectionTextBox focus is lost, this.Selection is set to the text in text box only if present in the list
        private void SelectionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IndexOf(selectionTextBox.Text) >= 0)
            {
                Selection = selectionTextBox.Text;
            }
        }

        // If IsSearch == true, scrolls to the first list item that starts with the text in selectionTextBox
        // Regardless of IsSearch, if the text in the textbox matches an item in the list that item will be selected
        private void SelectionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsSearch)
            {
                int indexStart = IndexOfStart(selectionTextBox.Text);
                if (indexStart >= 0)
                {
                    selectionList.ScrollIntoView(selectionList.Items[indexStart]);
                }
            }

            SelectFromString(selectionTextBox.Text);
        }

        // Prevents non-digits from being typed if IsDigtisOnly == true
        private void SelectionTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsDigitsOnly)
            {
                Regex regex = new Regex("[^0-9-]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }

        // Selects an item if its string value is equal to the string argument
        public Boolean SelectFromString(string str)
        {
            int index = IndexOf(str);
            if (index >= 0)
            {
                selectionList.SelectedIndex = index;
                Selection = ItemToString(selectionList.SelectedItem.ToString());
                selectionList.ScrollIntoView(selectionList.SelectedItem);
                return true;
            } else
            {
                return false;
            }
        }

        // Font families and typefaces have preceding information present in their string values removed
        public string ItemToString(string item)
        {
            if (item.Contains(":"))
            {
                item = item.Substring(item.LastIndexOf(':') + 1).Trim();
            }
            return item;
        }

        // Searches for first item in the list that starts with the target and returns the index or -1 if not present
        // Not case-sensitive
        private int IndexOfStart(String target)
        {
            int index = 0;
            foreach (var item in selectionList.Items)
            {
                if (item != null && ItemToString(item.ToString()).ToLower().StartsWith(target.ToLower()))
                {
                    return index;
                }

                index++;
            }
            return -1;
        }

        // Searches for an item in the list and returns the index or -1 if not present
        // Not case-sensitive
        private int IndexOf(string target)
        {
            int index = 0;
            foreach (var item in selectionList.Items)
            {
                if (item != null && String.Equals(ItemToString(item.ToString()).ToLower(), target.ToLower()))
                {
                    return index;
                }

                index++;
            }
            return -1;
        }
    }
}
