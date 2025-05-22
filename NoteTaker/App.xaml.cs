using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;
using NoteTaker.Properties;

namespace NoteTaker;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow wnd = new MainWindow();

        //App.Current.Properties["FontFamily"] = NoteTaker.Properties.Settings.Default.Font;
        //App.Current.Properties["TypefaceIndex"] = NoteTaker.Properties.Settings.Default.TypefaceIndex;
        //App.Current.Properties["FontSize"] = NoteTaker.Properties.Settings.Default.FontSize;

        String fontFamilySetting = NoteTaker.Properties.Settings.Default.Font;
        int typefaceIndex = NoteTaker.Properties.Settings.Default.TypefaceIndex;
        double fontSize = NoteTaker.Properties.Settings.Default.FontSize;

        FontFamily fontFamily = new FontFamily(fontFamilySetting);
        wnd.textEditor.FontFamily = fontFamily;
        wnd.textEditor.FontStyle = fontFamily.FamilyTypefaces[typefaceIndex].Style;
        wnd.textEditor.FontWeight = fontFamily.FamilyTypefaces[typefaceIndex].Weight;
        wnd.NonZoomFontSize = fontSize;
        wnd.Zoom(1.0);

        wnd.Show();
        wnd.textEditor.Focus();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        //NoteTaker.Properties.Settings.Default.Font = (string)App.Current.Properties["FontFamily"];
        //NoteTaker.Properties.Settings.Default.TypefaceIndex = (int)App.Current.Properties["TypefaceIndex"];
        //NoteTaker.Properties.Settings.Default.FontSize = (double)App.Current.Properties["FontSize"];
        NoteTaker.Properties.Settings.Default.Save();
    }
}

