using rudistor.Contents.ResetModal.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace rudistor.Contents.ResetModal.View
{
    /// <summary>
    /// Description for ModifyModalView.
    /// </summary>
    public partial class ResetModalView : Window
    {
        /// <summary>
        /// Initializes a new instance of the ModifyModalView class.
        /// </summary>
        public ResetModalView()
        {
            InitializeComponent();
            //selectA.ItemsSource.ToString();
           
        }

        

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtVolume.Text.Trim() != "" && selectA.SelectedValue !=null)
            {
                this.DialogResult = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请输入方向及仓位！");
            }
            //selectA.SelectedValue.ToString();
            //System.Windows.Forms.MessageBox.Show((this.DataContext as ResetModalView).Stages.Values.ToString());

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }

        private void selectA_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            string direction = cmb.SelectedValue as string;
            //KeyValuePair<int,string> kp = (KeyValuePair<int,string>)cmb.SelectedItem;

            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Direction = direction; }
        }

        
      



   

       
        
    }
}