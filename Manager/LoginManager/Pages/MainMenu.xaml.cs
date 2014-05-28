using System;
using System.ComponentModel;
using System.Windows.Controls;


namespace LoginManager
{
    /// <summary>
    /// Interaktionslogik für MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl, ISwitchable, INotifyPropertyChanged
    {
        //Page-Switcher
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        //Properties
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainMenu()
        {
            InitializeComponent();
        }

        private void TileSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new Settings());
        }

        private void TileCredentials_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new EditUserInfoScan());
        }
    }
}
