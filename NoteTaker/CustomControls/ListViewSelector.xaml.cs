using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public ListView List { get; set; }
        public FamilyTypeface Typeface { get; private set; }
        public String Selection 
        {
            get { return _selection; }
            set 
            {
                _selection = value;
                ListSelectionChanged?.Invoke(this, EventArgs.Empty);
            } 
        }

        public event EventHandler? ListSelectionChanged;

        //protected virtual void OnSelectionChanged(EventArgs e)
        //{
        //    ListSelectionChanged?.Invoke(this, e);
        //}

        private void SelectionList_Click(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                var item = listView.SelectedItem;
                if (item != null)
                {
                    string selectString = item.ToString();
                    if (selectString.Contains(":"))
                    {
                        selectString =selectString.Substring(item.ToString().LastIndexOf(':') + 1).Trim();
                    }
                    selectionTextBox.Text = selectString;
                    Selection = selectString;
                }
            }
        }

        private void SelectionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Selection = selectionTextBox.Text;
            }
        }

        private void SelectionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Selection = selectionTextBox.Text;
        }
    }
}
