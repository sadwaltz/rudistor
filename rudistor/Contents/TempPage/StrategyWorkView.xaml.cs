﻿using rudistor.Contents.WorkPage.ViewModel;
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

namespace rudistor.Contents.TempPage
{
    /// <summary>
    /// StrategyWorkView.xaml 的交互逻辑
    /// </summary>
    public partial class StrategyWorkView : System.Windows.Controls.UserControl
    {
        public StrategyWorkView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.WindowState = System.Windows.WindowState.Maximized;
            window.PreviewKeyDown += WorkView_KeyDown;
            ((StrategyWorkViewModel)this.DataContext).workviewLoaded();

        }
        void WorkView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((StrategyWorkViewModel)this.DataContext).enterEventHandler(sender, e);
        }

    }
}
