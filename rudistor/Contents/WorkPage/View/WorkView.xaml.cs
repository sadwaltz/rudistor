using rudistor.Contents.WorkPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;


namespace rudistor.Contents.WorkPage.View
{
    /// <summary>
    /// Description for WorkView.
    /// </summary>
    public partial class WorkView : System.Windows.Controls.UserControl 
    {
        /// <summary>
        /// Initializes a new instance of the WorkView class.
        /// </summary>
        public WorkView()
        {
            InitializeComponent();
            Rect rc = SystemParameters.WorkArea;
            
            Console.WriteLine(rc.Height + ":" + rc.Width);
            //double colWidth = (rc.Width -2*6)/ 3-4;
            String displayWidth = ConfigurationManager.AppSettings["DisplayWidth"];
            String displayHeight = ConfigurationManager.AppSettings["DisplayHeight"];
            double width = displayWidth==null ? 1366 : Double.Parse(displayWidth);
            double height = displayHeight == null ? 768 : Double.Parse(displayHeight);
            //double colWidth = rc.Width / 3;
            double colWidth = width / 3;
            //ColA.Width = new GridLength(colWidth);
            //ColB.Width = new GridLength(colWidth);
            //ColC.Width = new GridLength(colWidth);
            //double rowHeight = (rc.Height - 200 - 2 * 4) / 2 - 4;
            //double rowHeight = (height - 200 - 2 * 4) / 2 - 4;
            //RowA.Height = new GridLength(rowHeight);
            //RowB.Height = new GridLength(rowHeight);
            //GridA_up.Height = GridB_up.Height = GridC_up.Height = GridD_up.Height = GridE_up.Height = GridF_up.Height = rowHeight / 9 * 5;
            //GridA_down.Height = GridB_down.Height = GridC_down.Height = GridD_down.Height = GridE_down.Height = GridF_down.Height = rowHeight / 9 * 4;
            //GridA_down_rowA.Height = GridA_down_rowB.Height = GridA_down_rowC.Height = GridA_down_rowD.Height = new GridLength(GridA_down.Height / 4);
            //GridA_down_colA.Width = GridA_down_colB.Width = GridA_down_colC.Width = GridA_down_colD.Width = new GridLength(colWidth / 4);
            //GridA_up_rowA.Height = GridA_up_rowB.Height = GridA_up_rowC.Height = GridA_up_rowD.Height = new GridLength(GridA_up.Height / 4);
            //GridA_up_colA.Width = GridA_up_colB.Width = GridA_up_colC.Width = GridA_up_colD.Width = GridA_up_colE.Width = GridA_up_colF.Width = new GridLength(colWidth / 6);

            //GridB_down_rowA.Height = GridB_down_rowB.Height = GridB_down_rowC.Height = GridB_down_rowD.Height = new GridLength(GridB_down.Height / 4);
            //GridB_down_colA.Width = GridB_down_colB.Width = GridB_down_colC.Width = GridB_down_colD.Width = new GridLength(colWidth / 4);
            //GridB_up_rowA.Height = GridB_up_rowB.Height = GridB_up_rowC.Height = GridB_up_rowD.Height = new GridLength(GridB_up.Height / 4);
            //GridB_up_colA.Width = GridB_up_colB.Width = GridB_up_colC.Width = GridB_up_colD.Width = GridB_up_colE.Width = GridB_up_colF.Width = new GridLength(colWidth / 6);
            //this.KeyDown += WorkView_KeyDown;
            
        }

        void WorkView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((WorkerViewModel)this.DataContext).enterEventHandler(sender, e);
        }

        private void xc_A_ValueChanged_1(object sender, EventArgs e)
        {
            NumericUpDown xc_A = (NumericUpDown)sender;
            Border boder = VSHelper.GetParent<Border>(GridA_up, typeof(Border));

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.WindowState = System.Windows.WindowState.Maximized;
            //window.KeyDown += WorkView_KeyDown;
            window.PreviewKeyDown += WorkView_KeyDown;
            ((WorkerViewModel)this.DataContext).IsAllActivated = false;
            ((WorkerViewModel)this.DataContext).StrategyA.IsActivate = false;
            ((WorkerViewModel)this.DataContext).StrategyB.IsActivate = false;
            ((WorkerViewModel)this.DataContext).StrategyC.IsActivate = false;
            ((WorkerViewModel)this.DataContext).StrategyD.IsActivate = false;
            ((WorkerViewModel)this.DataContext).StrategyE.IsActivate = false;
            ((WorkerViewModel)this.DataContext).StrategyF.IsActivate = false;
            ((WorkerViewModel)this.DataContext).workviewLoaded();
            
        }

        
    }
}