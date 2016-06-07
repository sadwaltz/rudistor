﻿using System.Windows;
using rudistor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using MvvmControlChange;
using rudistor.Contents.LoginPage.View;
using rudistor.Contents.WorkPage.View;

namespace rudistor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<GoToPageMessage>
                (
                    this,
                    (action) => ReceiveMessage(action)
                );
            GoToLoginControl();
        }
        private LoginControl _loginControl;

        private LoginControl loginControl
        {
            get
            {
                if (_loginControl == null)
                    _loginControl = new LoginControl();
                return _loginControl;
            }
        }
        private WorkView _workView;
        private WorkView workView
        {
            get
            {
                if (_workView == null)
                    _workView = new WorkView();
                return _workView;
            }
        }
        private object ReceiveMessage(GoToPageMessage action)
        {
            //            this.contentControl1.Content = this.Page2View;
            //this shows what pagename property is!!
            switch (action.PageName)
            {
                case "LoginControl":
                    if (this.contentControl1.Content != this.loginControl)
                        this.contentControl1.Content = this.loginControl;
                    break;
                case "WorkView":
                    if (this.contentControl1.Content != this.workView)
                        this.contentControl1.Content = this.workView;
                    break;
                default:
                    break;
            }

            //            string testII = action.PageName.ToString();
            //           System.Windows.MessageBox.Show("You were successful switching to " + testII + ".");

            return null;
        }
        private object GoToLoginControl()
        {
            var msg = new GoToPageMessage() { PageName = "LoginControl" };
            Messenger.Default.Send<GoToPageMessage>(msg);

            
            return null;
        }
    }
}