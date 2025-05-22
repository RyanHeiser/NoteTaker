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
using System.Windows.Shapes;

namespace NoteTaker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UnsavedDialog : Window
    {
        public Boolean Save { get; set; }
        public Boolean Cancel { get; set; }

        public UnsavedDialog(string fileName)
        {
            InitializeComponent();
            promptText.Text += " " + fileName + "?";
            this.Save = false;
            this.Cancel = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save = true;
            Cancel = false;
            this.DialogResult = true;
        }
        private void DontSave_Click(object sender, RoutedEventArgs e)
        {
            Save = false;
            Cancel = false;
            this.DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Save = false;
            Cancel = true;
        }
    }
}
