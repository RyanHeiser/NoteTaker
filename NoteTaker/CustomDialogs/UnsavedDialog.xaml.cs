using System.Windows;
namespace NoteTaker
{
    /// <summary>
    /// A dialog to ask what the user wants to do when unsaved changes are about to be lost
    /// </summary>
    public partial class UnsavedDialog : Window
    {
        public Boolean Save { get; set; } // True iff the information will be saved
        public Boolean Cancel { get; set; } // True iff the operation removing unsaved changes will be canceled

        public UnsavedDialog(string fileName)
        {
            InitializeComponent();
            promptText.Text += " " + fileName + "?";
            this.Save = false;
            this.Cancel = true;
        }

        // The information will be saved and operation will continue
        // Default button
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save = true;
            Cancel = false;
            this.DialogResult = true;
        }

        // The information will not be saved and the operation will continue
        private void DontSave_Click(object sender, RoutedEventArgs e)
        {
            Save = false;
            Cancel = false;
            this.DialogResult = true;
        }

        // The information will not be saved and the operation will not continue
        // Cancel button
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Save = false;
            Cancel = true;
        }
    }
}
