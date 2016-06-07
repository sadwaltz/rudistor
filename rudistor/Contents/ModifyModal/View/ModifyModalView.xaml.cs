using rudistor.Contents.ModifyModal.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace rudistor.Contents.ModifyModal.View
{
    /// <summary>
    /// Description for ModifyModalView.
    /// </summary>
    public partial class ModifyModalView : Window
    {
        /// <summary>
        /// Initializes a new instance of the ModifyModalView class.
        /// </summary>
        public ModifyModalView()
        {
            InitializeComponent();
            //selectA.ItemsSource.ToString();
           
        }

        

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //selectA.SelectedValue.ToString();
            //System.Windows.Forms.MessageBox.Show((this.DataContext as ModifyModalViewModel).Stages.Values.ToString());

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }

        private void selectA_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            KeyValuePair<int,string> kp = (KeyValuePair<int,string>)cmb.SelectedItem;

            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SelectedStageA = kp.Value; }
        }

        private void selectB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            KeyValuePair<int, string> kp = (KeyValuePair<int, string>)cmb.SelectedItem;

            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SelectedStageB = kp.Value; }
        }
      



   

       
        
    }
}