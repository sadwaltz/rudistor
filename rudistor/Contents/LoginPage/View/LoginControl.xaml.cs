using rudistor.Model;
using System.Windows;
using System.Windows.Controls;

namespace rudistor.Contents.LoginPage.View
{
    /// <summary>
    /// Description for LoginControl.
    /// </summary>
    public partial class LoginControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the LoginControl class.
        /// </summary>
        public LoginControl()
        {
            InitializeComponent();
            
            
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            Broker selectedCar = (Broker)cmb.SelectedItem;
            
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SelectedBrokerId = selectedCar.brokerId; }
        }
    }
}