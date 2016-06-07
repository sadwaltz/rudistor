using Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MvvmControlChange;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using rudistor.Model;
using rudistor.Services;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace rudistor.Contents.LoginPage.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginControlViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the LoginControlViewModel class.
        /// </summary>

        //private TcpClient client;
        public TcpConnection tcpConnection;
        //private byte[] byteData;

        // ManualResetEvent instances signal completion.     
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public LoginControlViewModel()
        {
            //CurrentUser = new User() { UserId = "",Passwd="",InvestorId="" };
            CurrentUser = new User();
            LoginCommand = new RelayCommand(() => doLogin());
            _brokers = new ObservableCollection<Broker>();
            foreach (var broker in BrokersRepository.GetInstance().GetBrokers())
            {
                _brokers.Add(broker);
            }
            string lastUsr = ConfigurationManager.AppSettings["loginUsr"];
            string loginCompany = ConfigurationManager.AppSettings["loginCompany"];
            CurrentUser.UserId = lastUsr;
            SelectedBrokerId = loginCompany;
            //client = SimpleIoc.Default.GetInstance<TcpClient>();
            //tcpConnection = TcpConnection.GetInstance();
            
        }
       
        public string Password { private get; set; }
        public String SelectedBrokerId
        {
            get { return _selectedBrokerId; }
            set
            {
                if (value != _selectedBrokerId)
                {

                    _selectedBrokerId = value;
                    RaisePropertyChanged("SelectedBrokerId");
                }
            }
        }
        private String _selectedBrokerId;

        private bool _IsAuthenticated;
        private ObservableCollection<Broker> _brokers;
        public ObservableCollection<Broker> Brokers
        {
            get
            {
                if (_brokers == null)
                {
                    _brokers = new ObservableCollection<Broker>();
                    foreach (var broker in BrokersRepository.GetInstance().GetBrokers())
                    {
                        _brokers.Add(broker);
                    }
                }
                return _brokers;
            }
        }

        private User _CurrentUser;
        public User CurrentUser
        {
            get { return _CurrentUser; }
            set
            {
                if (value != _CurrentUser)
                {

                    _CurrentUser = value;
                    RaisePropertyChanged("CurrentUser");
                }
            }
        }
        
        
        public bool IsAuthenticated
        {
            get { return _IsAuthenticated; }
            set
            {
                if (value != _IsAuthenticated)
                {
                    _IsAuthenticated = value;
                    RaisePropertyChanged("IsAuthenticated");
                    RaisePropertyChanged("IsNotAuthenticated");
                }
            }
        }
        public bool IsNotAuthenticated
        {
            get
            {
                return !IsAuthenticated;
            }
        }
        
        public RelayCommand LoginCommand
        {
            get;
            private set;
        }

        private object doLogin()
        {
            if (Password != null && CurrentUser.UserId != null && SelectedBrokerId != null && !Password.Trim().Equals("") && !CurrentUser.UserId.Trim().Equals(""))
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Password);
                //密码进行Base64加密
                CurrentUser.Passwd = Convert.ToBase64String(plainTextBytes);
                CurrentUser.BrokerId = SelectedBrokerId;

                //System.Windows.MessageBox.Show(CurrentUser.UserId);
                //System.Windows.MessageBox.Show(Password);
                //System.Windows.MessageBox.Show(CurrentUser.InvestorId);
                Broker broker = Brokers.Single(i => i.brokerId == SelectedBrokerId);
                //System.Windows.MessageBox.Show(broker.brokerName);
                Message<User, String> login = new Message<User, String>(CurrentUser, "login", Guid.NewGuid().ToString());
                //client.start(broker.host, broker.port);
                tcpConnection = new TcpConnection();
                connectToServer(broker);
                //client.Connect(broker.host, broker.port);
                //NetworkStream ns = client.GetStream();
                String loginUser = JsonConvert.SerializeObject(login);
                byte[] loginData = Encoding.UTF8.GetBytes(loginUser);
                if (tcpConnection.Connected == true)
                {
                    tcpConnection.SendMessageWithPrefixLengthAsync(loginData);
                    tcpConnection.StartReceive();
                }
                
            }
            else
            {
                System.Windows.MessageBox.Show("用户名、密码、券商都不能为空！");
            }
            return null;
        }

        private bool connectToServer(Broker broker)
        {
            if (tcpConnection.Connected == false)
            {

                return tcpConnection.Start(broker.host, broker.port);
            }
            else
            {
                IPEndPoint connectedEnd = (IPEndPoint)tcpConnection.RemoteEndPoint;
                if (broker.host != connectedEnd.Address.ToString() && broker.port != connectedEnd.Port)
                {
                    tcpConnection.Disconnect();
                    return tcpConnection.Start(broker.host, broker.port);
                }
                return true;
            }
        }

        
        
    }
}