using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;


namespace LoginManager
{
    /// <summary>
    /// Interaktionslogik für EditUserInfo.xaml
    /// </summary>
    public partial class EditUserInfo : UserControl, ISwitchable, INotifyPropertyChanged
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

        public EditUserInfo(string id)
        {
            InitializeComponent();

            regAccess = new RegistryAccess(id);

            username = regAccess.Username;
            password = regAccess.Password;
            domain = regAccess.Domain;
        }

        #region Variables

        private RegistryAccess regAccess;

        private string _username;
        public string username
        {
            get { return _username; }
            set
            {
                if (_username == value) return;
                else
                {
                    _username = value;
                    RaisePropertyChanged("username");
                }
            }
        }

        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                else
                {
                    _password = value;
                    RaisePropertyChanged("password");
                }
            }
        }

        private string _domain;
        public string domain
        {
            get { return _domain; }
            set
            {
                if (_domain == value) return;
                else
                {
                    _domain = value;
                    RaisePropertyChanged("domain");
                }
            }
        }

        #endregion

        private void comSaveCredentials_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            regAccess.Username = username;
            regAccess.Password = password;
            regAccess.Domain = domain;

            Switcher.Switch(new EditUserInfoSuccess());
        }
        private void comSaveCredentials_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(domain))
            {
                e.CanExecute = true;
            }
        }

        private void comCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }
        private void comCancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
    }
}
