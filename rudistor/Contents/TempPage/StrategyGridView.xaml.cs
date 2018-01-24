using rudistor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rudistor.Contents.TempPage
{
    /// <summary>
    /// StrategyGrid.xaml 的交互逻辑
    /// </summary>
    public partial class StrategyGridView : UserControl
    {
        public StrategyGridView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Strategy datacontext = (Strategy)this.DataContext;
            //var temp = datacontext.lockNum;
        }
    }
}
