using System;
using System.ComponentModel;
using System.Windows.Controls;



namespace LoginManager
{
    /// <summary>
    /// Interaktionslogik für EditUserInfoScan.xaml
    /// </summary>
    public partial class EditUserInfoScan : UserControl, ISwitchable, INotifyPropertyChanged
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

        public EditUserInfoScan()
        {
            InitializeComponent();
        }

        #region Variables

        RFID c_RFID;

        private string _comPort;
        public string comPort
        {
            get { return _comPort; }
            set
            {
                if (_comPort == value) return;
                else
                {
                    _comPort = value;
                    RaisePropertyChanged("comPort");
                }
            }
        }

        #endregion

        void c_RFID_IncomingRfid(string id)
        {
            this.Dispatcher.Invoke(new SetActivePageCallback(delegate() { c_RFID.Close(); Switcher.Switch(new EditUserInfo(id)); }));
        }
        void c_RFID_RfidFailure(string reason)
        {
            this.Dispatcher.Invoke(new SetActivePageCallback(delegate() { c_RFID.Close(); Switcher.Switch(new EditUserInfoFailure(reason)); }));
        }

        private void ButtonSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            c_RFID.Close();
            Switcher.Switch(new EditUserInfo("123456789"));
        }

        private void ButtonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            c_RFID.Close();
        }
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                c_RFID = new RFID((string)RegistryAccess.Primary.GetValue("Port"));
                c_RFID.IncomingRfid += new RfidRead(c_RFID_IncomingRfid);
                c_RFID.RfidFailure += new RfidFail(c_RFID_RfidFailure);
            }
            catch
            {
                Switcher.Switch(new EditUserInfoFailure(""));
            }
        }
    }

    public delegate void SetActivePageCallback();
}
