using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using MahApps.Metro.Controls;
using System.Windows.Input;

namespace LoginManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //Page-Switcher
        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }
        public void Navigate(UserControl nextPage, object state)
        {
            this.Content = nextPage;
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
                s.UtilizeState(state);
            else
                throw new ArgumentException("NextPage is not ISwitchable! "
                  + nextPage.Name.ToString());
        }

        public MainWindow()
        {
            InitializeComponent();

            Switcher.pageSwitcher = this;
            Switcher.Switch(new MainMenu());
        }
    }

    public static class Commands
    {
        //General Commands
        public static readonly RoutedCommand comCancel = new RoutedCommand("comCancel", typeof(UserControl));
        public static readonly RoutedCommand comClose = new RoutedCommand("comClose", typeof(UserControl));

        //pageEditUserFailure
        public static readonly RoutedCommand comSettings = new RoutedCommand("comSettings", typeof(UserControl));

        //pageEditUserInfo
        public static readonly RoutedCommand comSaveCredentials = new RoutedCommand("comSaveCredentials", typeof(UserControl));
    }

    public static class Switcher
    {
        public static MainWindow pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            pageSwitcher.Navigate(newPage);
        }

        public static void Switch(UserControl newPage, object state)
        {
            pageSwitcher.Navigate(newPage, state);
        }
    }
    public interface ISwitchable
    {
        void UtilizeState(object state);
    }
}
