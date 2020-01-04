using System.Windows;
using rudistor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using MvvmControlChange;
using rudistor.Contents.LoginPage.View;
using rudistor.Contents.TempPage;
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

        private StrategyWorkView _strategyWorkView;
        private StrategyWorkView strategyWorkView
        {
            get
            {
                if (_strategyWorkView == null)
                    _strategyWorkView = new StrategyWorkView();
                return _strategyWorkView;
            }
        }

        private StrategyWorkView2 _strategyWorkView2;
        private StrategyWorkView2 strategyWorkView2
        {
            get
            {
                if (_strategyWorkView2 == null)
                    _strategyWorkView2 = new StrategyWorkView2();
                return _strategyWorkView2;
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
               case "StrategyWorkView":
                    if (this.contentControl1.Content != this.strategyWorkView)
                        this.contentControl1.Content = this.strategyWorkView;
                    break;
               case "StrategyWorkView2":
                    if (this.contentControl1.Content != this.strategyWorkView2)
                        this.contentControl1.Content = this.strategyWorkView2;
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