using System.Windows.Input;

namespace NoteTaker
{
    /// <summary>
    /// Custom command setup
    /// </summary>
    public static class CustomCommands
    {
        // NEW WINDOW Command
        public static readonly RoutedUICommand NewWindow = new RoutedUICommand
           (
               "New Window",
               "NewWindow",
               typeof(CustomCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Shift)
               }
           );

        // SAVE AS Command
        public static readonly RoutedUICommand SaveAs = new RoutedUICommand
            (
                "Save As",
                "SaveAs",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
                }
            );

        // EXIT Command
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Exit",
                "Exit",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        // TEXT WRAPPING Command
        public static readonly RoutedUICommand TextWrapping = new RoutedUICommand
           (
               "Text Wrapping",
               "TextWrapping",
               typeof(CustomCommands)
           );

        // FONT DIALOG Command
        public static readonly RoutedUICommand FontDialog = new RoutedUICommand
           (
               "Font",
               "FontDialog",
               typeof(CustomCommands)
           );

        // ZOOM IN Command
        public static readonly RoutedUICommand ZoomIn = new RoutedUICommand
           (
               "Zoom In",
               "ZoomIn",
               typeof(CustomCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.OemPlus, ModifierKeys.Control, "Ctrl+Plus")
               }
           );

        // ZOOM OUT Command
        public static readonly RoutedUICommand ZoomOut = new RoutedUICommand
           (
               "Zoom Out",
               "ZoomOut",
               typeof(CustomCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.OemMinus, ModifierKeys.Control, "Ctrl+Minus")
               }
           );

        // DEFAULT ZOOM Command
        public static readonly RoutedUICommand DefaultZoom = new RoutedUICommand
           (
               "Default Zoom",
               "DefaultZoom",
               typeof(CustomCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.D0, ModifierKeys.Control)
               }
           );

    }
}
