using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;

namespace LoginManager
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : UserControl, ISwitchable, INotifyPropertyChanged
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

        public Settings()
        {
            InitializeComponent();

            getAvailablePorts();
            selectedPort = getPortName();
        }

        #region Variables

        private ArrayList _ports = new ArrayList();
        public ArrayList ports
        {
            get { return _ports; }
            set
            {
                if (_ports == value) return;
                else
                {
                    _ports = value;
                    RaisePropertyChanged("ports");
                }
            }
        }

        private string _selectedPort;
        public string selectedPort
        {
            get { return _selectedPort; }
            set
            {
                if (_selectedPort == value) return;
                else
                {
                    _selectedPort = value;
                    RaisePropertyChanged("selectedPort");
                }
            }
        }

        #endregion

        private string getPortName()
        {
            return (string)RegistryAccess.Primary.GetValue("Port");
        }
        private void setPortName()
        {
            RegistryAccess.Primary.SetValue("Port", selectedPort);
        }
        private void getAvailablePorts()
        {
            ports.Clear();

            string[] tmpPorts = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string _p in tmpPorts)
            {
                ports.Add(_p);
            }
        }

        private void ButtonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }
        private void ButtonSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            setPortName();
            Switcher.Switch(new MainMenu());
        }
    }
}
