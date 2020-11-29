using rudistor.Contents.WorkPage.ViewModel;
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
using System.Windows.Shapes;

namespace rudistor.Contents.WorkPage.View
{
    /// <summary>
    /// StrategyWorkView.xaml 的交互逻辑
    /// </summary>
    public partial class StrategyWorkView2 : System.Windows.Controls.UserControl
    {
        public StrategyWorkView2()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.WindowState = System.Windows.WindowState.Maximized;
            window.PreviewKeyDown += WorkView_KeyDown;
            ((WorkerViewModel)this.DataContext).workviewLoaded();

        }
        void WorkView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((WorkerViewModel)this.DataContext).enterEventHandler(sender, e);
        }

    }
}
