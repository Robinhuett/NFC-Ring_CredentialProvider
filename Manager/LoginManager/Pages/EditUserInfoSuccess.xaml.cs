using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoginManager
{
    /// <summary>
    /// Interaktionslogik für EditUserInfoSuccess.xaml
    /// </summary>
    public partial class EditUserInfoSuccess : UserControl, ISwitchable, INotifyPropertyChanged
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

        public EditUserInfoSuccess()
        {
            InitializeComponent();
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
