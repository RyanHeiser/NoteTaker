using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace NoteTaker.CustomDialogs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FontDialog : Window
    {

        public FontDialog()
        {
            InitializeComponent();

            IOrderedEnumerable<FontFamily> fonts = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            foreach (FontFamily f in fonts)
            {
                fontList.List.Items.Add(new ListViewItem { Content = f, FontFamily = f });
                Debug.WriteLine(f.ToString());
            }

            FontFamily font = new FontFamily(fontList.Selection);
            foreach (FamilyTypeface typeface in font.FamilyTypefaces)
            {
                fontStyleList.List.Items.Add(typeface.Style);
            }
            //fontStyleList.List.ItemsSource = font.FamilyTypefaces;

            fontSizeList.List.ItemsSource = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }


        private void FontList_ListSelectionChanged(object sender, EventArgs e)
        {
            FontFamily font = new FontFamily(fontList.Selection);
            fontStyleList.List.Items.Clear();
            foreach (FamilyTypeface typeface in font.FamilyTypefaces)
            {
                fontStyleList.List.Items.Add(typeface.Style);
            }
        }
    }
}
