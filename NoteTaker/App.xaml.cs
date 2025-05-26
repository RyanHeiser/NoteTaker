using System.IO;
using System.Windows;
using System.Windows.Media;

namespace NoteTaker;

/// <summary>
/// Contains startup and exit processes for the application
/// </summary>
public partial class App : Application
{
    // Sets the font settings on startup
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow wnd = new MainWindow();

        // Sets textEditor to have saved font settings
        String fontFamilySetting = NoteTaker.Properties.Settings.Default.Font;
        int typefaceIndex = NoteTaker.Properties.Settings.Default.TypefaceIndex;
        double fontSize = NoteTaker.Properties.Settings.Default.FontSize;
        FontFamily fontFamily = new FontFamily(fontFamilySetting);
        wnd.textEditor.FontFamily = fontFamily;
        wnd.textEditor.FontStyle = fontFamily.FamilyTypefaces[typefaceIndex].Style;
        wnd.textEditor.FontWeight = fontFamily.FamilyTypefaces[typefaceIndex].Weight;
        wnd.NonZoomFontSize = fontSize;
        wnd.Zoom(1.0);

        // Set text editor to text in file if file is opened with NoteTaker
        if (e.Args.Length == 1)
        {
            try
            {
                wnd.ReadFromFile(e.Args[0]);
            }
            catch (Exception) { }
        }

        wnd.Show();
        wnd.textEditor.Focus();
    }

    // Saves settings on exit
    private void Application_Exit(object sender, ExitEventArgs e)
    {
        NoteTaker.Properties.Settings.Default.Save();
    }
}

