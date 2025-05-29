using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;


namespace NoteTaker.CustomDialogs
{
    /// <summary>
    /// A dialog that allows the user to change font family, style, and size
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
            }

            // Set font styles and sizes
            UpdateFontTypefaces();
            fontSizeList.List.ItemsSource = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            SelectCurrentFont(); // Selects currently used font settings from lists

        }

        public FontFamily Font { get; private set; }
        public FamilyTypeface Typeface { get; private set; }
        public double Size { get; private set; }

        // Listener to update the list of font styles to those belonging to the selected font
        private void Font_ListSelectionChanged(object sender, EventArgs e)
        {
            UpdateFontTypefaces();
        }

        // Saves the font settings to public fields from the selected font information
        // Also saves settings to Properties.Settings
        // Default button
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            // Sets this.Font to the selected fontList item if not null
            if (fontList.Selection != null)
                Font = new FontFamily(fontList.Selection);

            // Sets this.Typeface to the nth typeface of this.Font where n is the index of the selected typeface item
            if (fontTypeList.List.SelectedIndex >= 0)
                Typeface = Font.FamilyTypefaces.ElementAt(fontTypeList.List.SelectedIndex);

            // Tries to parse the text in the fontSizeList textbox and save to this.Size
            if (double.TryParse(fontSizeList.selectionTextBox.Text, out double result))
            {
                Size = result;
            }
            // If the parse fails and the font size selection isn't null then this.Size is set to the selected font size item
            else if (fontSizeList.Selection != null && fontSizeList.Selection.Length > 0)
            {
                Size = double.Parse(fontSizeList.Selection);
            }


            NoteTaker.Properties.Settings.Default.Font = Font.ToString(); ;
            NoteTaker.Properties.Settings.Default.TypefaceIndex = Font.FamilyTypefaces.IndexOf(Typeface); ;
            NoteTaker.Properties.Settings.Default.FontSize = Size;

            this.DialogResult = true;
        }

        // Sets the public font-related fields to the values saved in Properties.Settings and selects them in the font-related lists
        private void SelectCurrentFont()
        {
            // Select font from list and set Font field
            fontList.SelectFromString(Properties.Settings.Default.Font);
            FontFamily f = new FontFamily(Properties.Settings.Default.Font);
            Font = f;

            // Select font typeface from list and set Typeface field
            FamilyTypeface typeface = f.FamilyTypefaces[Properties.Settings.Default.TypefaceIndex];
            Typeface = typeface;
            string typefaceStr = typeface.Weight.ToString() + " " + typeface.Style.ToString();
            typefaceStr = TypefaceToString(typefaceStr);
            fontTypeList.SelectFromString(typefaceStr);

            // Select font size from list and set Size field
            // If the saved font size is not one of the presets, then the saved font is set in the font size text box
            if(!fontSizeList.SelectFromString(Properties.Settings.Default.FontSize.ToString()))
            {
                fontSizeList.Selection = Properties.Settings.Default.FontSize.ToString();
            }
            Size = Properties.Settings.Default.FontSize;
        }

        // Updates the font typefaces list which is dependent on the selected font family
        private void UpdateFontTypefaces()
        {
            fontTypeList.List.Items.Clear();
            FontFamily font = new FontFamily(fontList.Selection);
            foreach (FamilyTypeface typeface in font.FamilyTypefaces)
            {
                string typefaceStr = typeface.Weight.ToString() + " " + typeface.Style.ToString();
                typefaceStr = TypefaceToString(typefaceStr);

                fontTypeList.List.Items.Add(new ListViewItem { Content = typefaceStr, FontFamily = font, FontStyle = typeface.Style, FontWeight = typeface.Weight, });

                // If this is the first of the new typefaces, this typeface is selected in the list
                if (fontTypeList.List.Items.Count == 1)
                {
                    fontTypeList.SelectFromString(typefaceStr);
                }
            }
        }

        private string TypefaceToString(string typeface)
        {
            if (typeface.Contains("Normal") && typeface.Length > "Normal".Length)
                typeface = typeface.Remove(typeface.IndexOf("Normal"), "Normal".Length).Trim();

            return typeface;
        }
    }
}
