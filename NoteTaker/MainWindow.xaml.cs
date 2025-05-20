using Microsoft.Win32;
using System.IO;
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
    String SavedText = "";
    String FileName = "*.txt";

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
        textEditor.Text = "";
    }

    private void NewWindowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void NewWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MainWindow newWindow = new MainWindow();
        newWindow.Show();
    }
    private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
       OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            textEditor.Text = File.ReadAllText(openFileDialog.FileName);
            FilePath = openFileDialog.FileName;
            FileName = FilePath.Substring(FilePath.LastIndexOf('\\') + 1);
            SavedText = textEditor.Text;
            Title = FileName + " - NoteTaker";
        }
    }

    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (FilePath.Equals(""))
        {
            SaveAsCommand_Executed(sender, e);
        }
        else
        {
            WriteToFile(FilePath);
        }
    }

    private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Text file (*.txt)|*txt|All files (*.*)|*.*";
        saveFileDialog.DefaultExt = "txt";
        saveFileDialog.AddExtension = true;
        saveFileDialog.FileName = this.FileName;
        if (saveFileDialog.ShowDialog() == true)
        {
            FilePath = saveFileDialog.FileName;
            FileName = FilePath.Substring(FilePath.LastIndexOf('\\') + 1);
            WriteToFile(FilePath);
            Title = FileName + " - NoteTaker";
        }
    }

    private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        this.Close();
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
    // Also updates window title to represent if changes have been saved
    private void TextEditor_SelectionChanged(object sender, RoutedEventArgs e)
    {
        int row = textEditor.GetLineIndexFromCharacterIndex(textEditor.CaretIndex);
        int col = textEditor.CaretIndex - textEditor.GetCharacterIndexFromLineIndex(row);
        cursorPosition.Text = "Line " + (row + 1) + ", Column " + (col + 1);

        // charCount = char index at beginning of last line + length of last line - (lineCount - 1) * 2
        // subtracting the line count removes new line characters from the character count
        charCount.Text = "Characters: " + (textEditor.GetCharacterIndexFromLineIndex(textEditor.LineCount - 1) 
            + textEditor.GetLineLength(textEditor.LineCount - 1) - (textEditor.LineCount - 1) * 2);

        UpdateTitleSavedIndicator();
    }


    /* UTILITY */


    private void UpdateTitleSavedIndicator()
    {
        if (Title[0] == '*' && textEditor.Text == SavedText)
        {
            Title = Title.Substring(1);
        }
        else if (Title[0] != '*' && textEditor.Text != SavedText)
        {
            Title = "*" + Title;
        }
    }

    private void WriteToFile(string filePath)
    {
        File.WriteAllText(filePath, textEditor.Text);
        SavedText = textEditor.Text;
        UpdateTitleSavedIndicator();
    }
}

