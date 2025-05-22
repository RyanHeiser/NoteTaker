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

            // Add list of font familys, each list item in its own font
            IOrderedEnumerable<FontFamily> fonts = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            foreach (FontFamily f in fonts)
            {
                fontList.List.Items.Add(new ListViewItem { Content = f, FontFamily = f });
                Debug.WriteLine(f.ToString());
            }

            // Set font styles and sizes
            UpdateFontStyles();
            fontSizeList.List.ItemsSource = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            SelectCurrentFont();

            //Font = new FontFamily(fontList.Selection);
            //Debug.WriteLine(fontTypeList.Selection);
            //Typeface = Font.FamilyTypefaces.ElementAt(fontTypeList.List.SelectedIndex);
            //Size = double.Parse(fontSizeList.Selection);

        }

        public FontFamily Font { get; private set; }
        public FamilyTypeface Typeface { get; private set; }
        public double Size { get; private set; }

        private void Font_ListSelectionChanged(object sender, EventArgs e)
        {
            UpdateFontStyles();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            Font = fontList.GetValue<FontFamily>();
            Typeface = fontTypeList.GetValue<FamilyTypeface>();
            Size = fontSizeList.GetValue<double>();
            //Font = new FontFamily(fontList.Selection);
            //Typeface = Font.FamilyTypefaces.ElementAt(fontTypeList.List.SelectedIndex);
            //Size = double.Parse(fontSizeList.Selection);

            NoteTaker.Properties.Settings.Default.Font = Font.ToString(); ;
            NoteTaker.Properties.Settings.Default.TypefaceIndex = Font.FamilyTypefaces.IndexOf(Typeface); ;
            NoteTaker.Properties.Settings.Default.FontSize = Size;

            this.DialogResult = true;
        }

        private void SelectCurrentFont()
        {
            FontFamily f = new FontFamily(Properties.Settings.Default.Font);
            fontList.SetValue(f);
            fontTypeList.SetValue(f.FamilyTypefaces[Properties.Settings.Default.TypefaceIndex]);
            fontSizeList.SetValue(Properties.Settings.Default.FontSize);
            //fontList.SelectFromString(Properties.Settings.Default.Font);
            //FontFamily f = new FontFamily(Properties.Settings.Default.Font);
            //fontTypeList.SelectFromString(fontTypeList.ItemToString(f.FamilyTypefaces[Properties.Settings.Default.TypefaceIndex].ToString()));
            //fontSizeList.SelectFromString( Properties.Settings.Default.FontSize.ToString());
           
        }

        private void UpdateFontStyles()
        {
            fontTypeList.List.Items.Clear();
            FontFamily font = new FontFamily(fontList.Selection);
            foreach (FamilyTypeface typeface in font.FamilyTypefaces)
            {
                string typefaceStr = typeface.Weight.ToString() + " " + typeface.Style.ToString();
                if (typefaceStr == "Normal Normal")
                    typefaceStr = "Normal";

                fontTypeList.List.Items.Add(new ListViewItem { Content = typefaceStr, FontFamily = font, FontStyle = typeface.Style, FontWeight = typeface.Weight, });
            }
        }
    }
}
