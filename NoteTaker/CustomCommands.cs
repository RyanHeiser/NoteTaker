using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoteTaker
{
    public static class CustomCommands
    {
        // NEW WINDOW Command
        public static readonly RoutedUICommand NewWindow = new RoutedUICommand
           (
               "NewWindow",
               "NewWindow",
               typeof(CustomCommands),
               new InputGestureCollection()
               {
                    new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Shift)
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


        
    }
}
