using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


using NoteTaker.CustomDialogs;

namespace NoteTaker;

/// <summary>
/// The Main Text Editor window. Contains a top menu, a text box, and a bottom status bar.
/// </summary>
public partial class MainWindow : Window
{
    const double MinZoom = 0.05;
    const double ZoomChange = 0.1;

    String FilePath = "";
    String FileName = "Untitled";
    Boolean Saved = false;
    private double zoom = 1.0;

    public MainWindow()
    {
        InitializeComponent();

        this.NonZoomFontSize = textEditor.FontSize;

        zoomBox.ItemsSource = new List<string>() { "20%", "40%", "60%", "80%", "90%", "100%", "110%", "125%", "150%", "175%", "200%"};
    }

    public double NonZoomFontSize { get; set; }



    /* COMMANDS */

    // File Menu Commands

    private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    // Sets textEditor to empty string
    private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (!Saved && !(FilePath == "" && textEditor.Text == ""))
        {
            // Opens UnsavedDilog if text is unsaved
            if (!UnsavedPrompt())
            {
                e.Handled = true;
                return;
            }
        }
        textEditor.Text = "";
        Title = "Untitled - NoteTaker";
        FilePath = "";
        Saved = false;
    }

    private void NewWindowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    // Creates a new window
    private void NewWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MainWindow newWindow = new MainWindow();

        // Sets font settings in new window
        newWindow.textEditor.FontFamily = this.textEditor.FontFamily;
        newWindow.textEditor.FontStyle = this.textEditor.FontStyle;
        newWindow.textEditor.FontWeight = this.textEditor.FontWeight;
        newWindow.NonZoomFontSize = this.NonZoomFontSize;
        newWindow.Zoom(1.0);
        

        newWindow.Show();
        newWindow.textEditor.Focus();
    }
    private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    // Opens OpenFileDialog to load in text from a file
    private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (!Saved && !(FilePath == "" && textEditor.Text == ""))
        {
            if(!UnsavedPrompt())
            {
                e.Handled = true;
                return;
            }
        }


        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text files (*txt)|*txt|All files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == true)
        {
            ReadFromFile(openFileDialog.FileName);
        }
    }

    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Save();
    }

    private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        SaveAs();
    }

    private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    // Format Menu Commands

    private void TextWrappingCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void TextWrappingCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (textEditor.TextWrapping == TextWrapping.Wrap)
        {
            textEditor.TextWrapping = TextWrapping.NoWrap;
        } 
        else
        {
            textEditor.TextWrapping = TextWrapping.Wrap;
        }
    }

    private void FontDialogCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    // Opens a dialog to change font family, style, and size
    private void FontDialogCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        FontDialog fontDialog = new FontDialog();
        fontDialog.Owner = this;
        if (fontDialog.ShowDialog() == true)
        {
            textEditor.FontFamily = fontDialog.Font;
            textEditor.FontStyle = fontDialog.Typeface.Style;
            textEditor.FontWeight = fontDialog.Typeface.Weight;
            NonZoomFontSize = fontDialog.Size;
            Zoom(this.zoom); // Call Zoom() to update textEditor.FontSize to NonZoomFontSize * this.zoom
        }
    }

    // View Menu Commands

    private void ZoomInCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void ZoomInCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Zoom(zoom + ZoomChange);
        zoomBox.Text = (int) (zoom * 100) + "%";
    }

    private void ZoomOutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = this.zoom - ZoomChange > MinZoom;
    }

    private void ZoomOutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Zoom(zoom - ZoomChange);
        zoomBox.Text = (int)(zoom * 100) + "%";
    }

    private void DefaultZoomCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void DefaultZoomCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Zoom(1.0);
        zoomBox.Text = (int)(zoom * 100) + "%";
    }



    /* ZOOMBOX Event Handlers */

    // Prevents non digits or '%' from being typed into zoomBox
    private void ZoomBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9%-]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    // Zooms using zoomBox text value
    private void ZoomBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        Debug.WriteLine("ZoomBox_TextChanged");

        if (zoomBox.Text.Length == 0)
        {
            return;
        }

        // Zoom if the text in the zoomBox without '%' can be parsed to a double
        string percent = zoomBox.Text.Replace("%", "");
        if (double.TryParse(percent, out double zoomPercent))
        {
            Zoom(zoomPercent / 100D);
        }
        // Otherwise set text in zoomBox to current zoom
        else
        {
            zoomBox.Text = (zoom * 100).ToString() + "%";
        }
    }

    // When zoomBox loses focus a '%' is added to the text value if not already present
    private void ZoomBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (!zoomBox.Text.Contains("%"))
        {
            zoomBox.Text += '%';
        }
    }

    // Returns focus to the textEditor when Enter is pressed in the zoomBox
    private void ZoomBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            zoomBox.Text = (zoom * 100).ToString() + "%";
            textEditor.Focus();
        }
    }



    /* OTHER EVENT HANDLERS */

    // Updates StatusBar when changes are made to the text editor
    private void TextEditor_SelectionChanged(object sender, RoutedEventArgs e)
    {
        int row = textEditor.GetLineIndexFromCharacterIndex(textEditor.CaretIndex);
        int col = textEditor.CaretIndex - textEditor.GetCharacterIndexFromLineIndex(row);
        cursorPosition.Text = "Line " + (row + 1) + ", Column " + (col + 1);

        // charCount = char index at beginning of last line + length of last line - (lineCount - 1) * 2
        // subtracting the line count removes new line characters from the character count
        charCount.Text = "Characters: " + (textEditor.GetCharacterIndexFromLineIndex(textEditor.LineCount - 1) 
            + textEditor.GetLineLength(textEditor.LineCount - 1) - (textEditor.LineCount - 1) * 2);
    }

    // Updates window title to represent if changes have been saved
    private void TextEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        Saved = false;
        UpdateTitleSavedIndicator();
    }

    // Handles window closing
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (!Saved && !(FilePath == "" && textEditor.Text == ""))
        {
            if (!UnsavedPrompt())
            {
                e.Cancel = true;
            }
        }
    }
    


    /* UTILITY */

    // Changes font size to simulate zoom
    public void Zoom(double zoom)
    {
        if (zoom > MinZoom)
        {
            textEditor.FontSize = NonZoomFontSize * zoom;
            this.zoom = zoom;
        }
    }

    public void ReadFromFile(string fileName)
    {
        textEditor.Text = File.ReadAllText(fileName);
        FilePath = fileName;
        FileName = FilePath.Substring(FilePath.LastIndexOf('\\') + 1);
        Saved = true;
        Title = FileName + " - NoteTaker";
    }

    // Saves current text in editor to the saved file path. If none then calls SaveAs
    private void Save()
    {
        if (FilePath.Equals(""))
        {
            SaveAs();
        }
        else
        {
            WriteToFile();
        }
    }

    // Opens SaveFileDialog to save current text in editor
    private void SaveAs()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Text file (*.txt)|*txt|All files (*.*)|*.*";
        saveFileDialog.DefaultExt = "txt";
        saveFileDialog.AddExtension = true;

        if (FilePath == "")
        {
            saveFileDialog.FileName = "*.txt";
        }
        else
        {
            saveFileDialog.FileName = this.FileName;
        }

        if (saveFileDialog.ShowDialog() == true)
        {
            FilePath = saveFileDialog.FileName;
            FileName = FilePath.Substring(FilePath.LastIndexOf('\\') + 1);
            WriteToFile();
            Title = FileName + " - NoteTaker";
        }
    }

    // Creates dialog asking if the user wants to save their changes
    // Returns true if the user doesn't cancel
    private Boolean UnsavedPrompt()
    {
        UnsavedDialog unsavedDialog = new UnsavedDialog(FileName);
        unsavedDialog.Owner = this;
        if (unsavedDialog.ShowDialog() == true)
        {
            if (unsavedDialog.Save)
                Save();

            return !unsavedDialog.Cancel;
        }
        return false;
    }

    // Adds asterik if there are unsaved changes, removes asterik if there are no unsaved changes
    private void UpdateTitleSavedIndicator()
    {
        // removes unsaved indicator if file is saved or there is no save location and the text editor is empty
        if (Title[0] == '*' && (Saved || (FilePath == "" && textEditor.Text.Length == 0)))
        {
            Title = Title.Substring(1);
        }
        else if (Title[0] != '*' && !Saved)
        {
            Title = "*" + Title;
        }
    }

    // Saves text in editor to file path
    private void WriteToFile()
    {
        File.WriteAllText(FilePath, textEditor.Text);
        Saved = true;
        UpdateTitleSavedIndicator();
    }

}

