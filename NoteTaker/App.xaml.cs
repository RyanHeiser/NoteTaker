﻿using System.Configuration;
using System.Data;
using System.Windows;

namespace NoteTaker;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow wnd = new MainWindow();
        wnd.Show();
    }
}

