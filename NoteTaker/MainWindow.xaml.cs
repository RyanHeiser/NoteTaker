using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteTaker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    String FilePath = "";
    String FileName = "Untitled";
    Boolean Saved = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    /* COMMANDS */

    // File Menu Commands

    private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (!Saved && !(FilePath == "" && textEditor.Text == ""))
        {
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

    private void NewWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MainWindow newWindow = new MainWindow();
        newWindow.Show();
        newWindow.textEditor.Focus();
    }
    private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

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
            textEditor.Text = File.ReadAllText(openFileDialog.FileName);
            FilePath = openFileDialog.FileName;
            FileName = FilePath.Substring(FilePath.LastIndexOf('\\') + 1);
            Saved = true;
            Title = FileName + " - NoteTaker";
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
            if(!UnsavedPrompt())
            {
                e.Cancel = true;
            }
        }
    }


    /* UTILITY */

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
        if (unsavedDialog.ShowDialog() == true)
        {
            if (unsavedDialog.Save)
                Save();

            return !unsavedDialog.Cancel;
        }
        return false;
    }

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

    private void WriteToFile()
    {
        File.WriteAllText(FilePath, textEditor.Text);
        Saved = true;
        UpdateTitleSavedIndicator();
    }
}

