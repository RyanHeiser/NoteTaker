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
       
    }

    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        
    }

    private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        
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
    private void textEditor_SelectionChanged(object sender, RoutedEventArgs e)
    {
        int row = textEditor.GetLineIndexFromCharacterIndex(textEditor.CaretIndex);
        int col = textEditor.CaretIndex - textEditor.GetCharacterIndexFromLineIndex(row);
        cursorPosition.Text = "Line " + (row + 1) + ", Column " + (col + 1);

        // charCount = char index at beginning of last line + length of last line - (lineCount - 1) * 2
        // subtracting the line count removes new line characters from the character count
        charCount.Text = "Characters: " + (textEditor.GetCharacterIndexFromLineIndex(textEditor.LineCount - 1) 
            + textEditor.GetLineLength(textEditor.LineCount - 1) - (textEditor.LineCount - 1) * 2);
    }

}

