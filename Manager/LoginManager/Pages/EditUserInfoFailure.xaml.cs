using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoginManager
{
    /// <summary>
    /// Interaktionslogik für EditUserInfoFailure.xaml
    /// </summary>
    public partial class EditUserInfoFailure : UserControl, ISwitchable, INotifyPropertyChanged
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

        public EditUserInfoFailure(string ErrorMessage)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                error = ErrorMessage;
                editUserInfoFailure_Error.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #region Variables

        private string _error;
        public string error
        {
            get { return _error; }
            set
            {
                if (_error == value) return;
                else
                {
                    _error = value;
                    RaisePropertyChanged("error");
                }
            }
        }

        #endregion

        private void comSettings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Switcher.Switch(new Settings());
        }
        private void comSettings_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void comClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private void comClose_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
    }
}
