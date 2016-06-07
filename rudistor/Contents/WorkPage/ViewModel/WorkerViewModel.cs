using Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using rudistor.Contents.LoginPage.ViewModel;
using rudistor.Contents.ModifyModal.View;
using rudistor.Contents.ModifyModal.ViewModel;
using rudistor.Contents.ResetModal.View;
using rudistor.Contents.ResetModal.ViewModel;
using rudistor.Model;
using rudistor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace rudistor.Contents.WorkPage.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class WorkerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the WorkerViewModel class.
        /// </summary>
        /// 
        private static Logger logger = LogManager.GetCurrentClassLogger();  

        private User _currentUser;
        private String _currentCompany;
        private Strategy _strategyA;
        private Strategy _strategyB;
        private Strategy _strategyC;
        private Strategy _strategyD;
        private Strategy _strategyE;
        private Strategy _strategyF;
        
        private ObservableCollection<Position> _positions;

        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set
            {
                _positions = value;
                RaisePropertyChanged("Positions");
            }
        }
        private ObservableCollection<Canceled> _canceled;

        public ObservableCollection<Canceled> CanceledOrder
        {
            get { return _canceled; }
            set
            {
                _canceled = value;
                RaisePropertyChanged("Canceled");
            }
        }
        
        private Dictionary<int,string> _instrument;

        public Dictionary<int,string> Instrument
        {
            get { return _instrument; }
            set
            {
                _instrument = value;
                RaisePropertyChanged("Instrument");
            }
        }
        private string _gridAIncre;
        public string GridAIncre
        {
            get { return _gridAIncre; }
            set
            {
                _gridAIncre = value;
                RaisePropertyChanged("GridAIncre");
            }
        }
        private string _gridBIncre;
        public string GridBIncre
        {
            get { return _gridBIncre; }
            set
            {
                _gridBIncre = value;
                RaisePropertyChanged("GridBIncre");
            }
        }
        private string _gridCIncre;
        public string GridCIncre
        {
            get { return _gridCIncre; }
            set
            {
                _gridCIncre = value;
                RaisePropertyChanged("GridCIncre");
            }
        }
        private string _gridDIncre;
        public string GridDIncre
        {
            get { return _gridDIncre; }
            set
            {
                _gridDIncre = value;
                RaisePropertyChanged("GridDIncre");
            }
        }
        private string _gridEIncre;
        public string GridEIncre
        {
            get { return _gridEIncre; }
            set
            {
                _gridEIncre = value;
                RaisePropertyChanged("GridEIncre");
            }
        }
        private string _gridFIncre;
        public string GridFIncre
        {
            get { return _gridFIncre; }
            set
            {
                _gridFIncre = value;
                RaisePropertyChanged("GridFIncre");
            }
        }
        #region combobox init
        private ObservableCollection<ComboboxItem> _T2ComboboxItems;
        private ObservableCollection<ComboboxItem> _ClComboboxItems;
        public ObservableCollection<ComboboxItem> T2ComboboxItems
        {
            get 
            {
                if (_T2ComboboxItems == null)
                {
                    _T2ComboboxItems = new ObservableCollection<ComboboxItem>();
                    _T2ComboboxItems.Add(new ComboboxItem() { index = "0", describe = "0-直接对手价报单" });
                    _T2ComboboxItems.Add(new ComboboxItem() { index = "1", describe = "1-按指定价报单" });
                }
                return _T2ComboboxItems; 
            }
            
        }

        public ObservableCollection<ComboboxItem> ClComboboxItems
        {
            get
            {
                if (_ClComboboxItems == null)
                {
                    _ClComboboxItems = new ObservableCollection<ComboboxItem>();
                    _ClComboboxItems.Add(new ComboboxItem() { index = "0", describe = "0-股指策略" });
                    _ClComboboxItems.Add(new ComboboxItem() { index = "1", describe = "1-mm策略" });
                    _ClComboboxItems.Add(new ComboboxItem() { index = "2", describe = "2-商品策略" });
                    _ClComboboxItems.Add(new ComboboxItem() { index = "3", describe = "3-商品策略2" });
                }
                return _ClComboboxItems;
            }

        }
        #endregion //初始化下单菜单
        //全部生效
        private bool _IsAllActivated;
        public bool IsAllActivated
        {
            get { return _IsAllActivated; }
            set
            {
                if (value != _IsAllActivated)
                {
                    _IsAllActivated = value;
                    RaisePropertyChanged("IsAllActivated");
                    RaisePropertyChanged("IsNotAllActivated");
                }
            }
        }

        public bool IsNotAllActivated
        {
            get
            {
                return !IsAllActivated;
            }
        }
        //comm
        private TcpClient client;
        private TcpConnection tcpConnection;
        private byte[] byteData;

        // ManualResetEvent instances signal completion.     
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public System.Timers.Timer timer;

        public RelayCommand<String> ModifyStage
        {
            get;
            private set;
        }

        public RelayCommand<String> ResetPosition
        {
            get;
            private set;
        }
        public RelayCommand ActivateAll
        {
            get;
            private set;
        }
        public RelayCommand<String> Send
        {
            get;
            private set;
        }
        public RelayCommand<String> SendWithActivate
        {
            get;
            private set;
        }
        public RelayCommand<String> Activate
        {
            get;
            private set;
        }
        

        public Strategy StrategyA
        {
            get { return _strategyA; }
            set
            {
                if (value != _strategyA)
                {

                    _strategyA = value;
                    RaisePropertyChanged("StrategyA");
                }
            }
        }
        public Strategy StrategyB
        {
            get { return _strategyB; }
            set
            {
                if (value != _strategyB)
                {

                    _strategyB = value;
                    RaisePropertyChanged("StrategyB");
                }
            }
        }
        public Strategy StrategyC
        {
            get { return _strategyC; }
            set
            {
                if (value != _strategyC)
                {

                    _strategyC = value;
                    RaisePropertyChanged("StrategyC");
                }
            }
        }
        public Strategy StrategyD
        {
            get { return _strategyD; }
            set
            {
                if (value != _strategyD)
                {

                    _strategyD = value;
                    RaisePropertyChanged("StrategyD");
                }
            }
        }
        public Strategy StrategyE
        {
            get { return _strategyE; }
            set
            {
                if (value != _strategyE)
                {

                    _strategyE = value;
                    RaisePropertyChanged("StrategyE");
                }
            }
        }
        public Strategy StrategyF
        {
            get { return _strategyF; }
            set
            {
                if (value != _strategyF)
                {

                    _strategyF = value;
                    RaisePropertyChanged("StrategyF");
                }
            }
        }
        private List<string> _wantedInstrument;
        public WorkerViewModel()
        {
            
            //生效所有策略
            ActivateAll = new RelayCommand(() => activateAll());
            //生效或者关闭一个策略
            Activate = new RelayCommand<String>((s) => activate(s));
            //发送一个策略参数
            Send = new RelayCommand<String>((s) => send(s));
            //生效并发送一个策略
            SendWithActivate = new RelayCommand<string>((s) => sendWithActivate(s));
            //打开修改策略窗口
            ModifyStage =  new RelayCommand<string>((s) => modifyStage(s));
            //打开重置仓位窗口
            ResetPosition = new RelayCommand<string>((s) => resetPosition(s));
            init();
            

         }

        
        private void init()
        {
            //initComm();
            _wantedInstrument = getInstrumentsFromINI();
            initGUI();


            
        }
        private List<string> getInstrumentsFromINI()
        {
            INIManager ini = new INIManager("config.ini");
            return new List<string>(ini.IniReadValue("InstrumentList", "list").Split(','));
        }
        public void workviewLoaded()
        {

            saveLoginInfo();
            tcpConnection = SimpleIoc.Default.GetInstance<LoginControlViewModel>().tcpConnection;
            //tcpConnection.OnDataReceivedCompleted += tcpConnection_OnDataReceivedCompleted;
            //client = SimpleIoc.Default.GetInstance<TcpClient>();

            tcpConnection.OnDataReceivedCompleted += tcpConnection_OnDataReceivedCompleted;
            tcpConnection.OnDisconnected += tcpConnection_OnDisconnected;
            
            
            queryInstrument();
            
            startTimer();
            
        }

        void tcpConnection_OnDisconnected(TcpConnection obj)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            System.Windows.Forms.MessageBox.Show("通讯连接已断开，请重新登录");
        }

        private void initGUI()
        {
            IsAllActivated = false;
            StrategyA = StrategyRepository.GetInstance().getStrategyByGridName("GridA");
            StrategyB = StrategyRepository.GetInstance().getStrategyByGridName("GridB");
            StrategyC = StrategyRepository.GetInstance().getStrategyByGridName("GridC");
            StrategyD = StrategyRepository.GetInstance().getStrategyByGridName("GridD");
            StrategyE = StrategyRepository.GetInstance().getStrategyByGridName("GridE");
            StrategyF = StrategyRepository.GetInstance().getStrategyByGridName("GridF");
            //StrategyA = new Strategy("GridA",true,"aaaa-bbbb","5","2","1");
            Positions = new ObservableCollection<Position>();
            CanceledOrder = new ObservableCollection<Canceled>();
            Instrument = new Dictionary<int, string>();
            updateGridIncre("GridA", StrategyA.StageId);
            updateGridIncre("GridB", StrategyB.StageId);
            updateGridIncre("GridC", StrategyC.StageId);
            updateGridIncre("GridD", StrategyD.StageId);
            updateGridIncre("GridE", StrategyE.StageId);
            updateGridIncre("GridF", StrategyF.StageId);
        }

        private void saveLoginInfo()
        {
            this._currentUser = SimpleIoc.Default.GetInstance<LoginControlViewModel>().CurrentUser;
            this._currentCompany = SimpleIoc.Default.GetInstance<LoginControlViewModel>().SelectedBrokerId;
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //cfa.AppSettings.Settings.Remove("loginUsr");
            cfa.AppSettings.Settings["loginUsr"].Value = _currentUser.UserId;
            cfa.AppSettings.Settings["loginCompany"].Value = _currentCompany;
            cfa.Save(ConfigurationSaveMode.Modified);
        }

        void tcpConnection_OnDataReceivedCompleted(TcpConnection conn, byte[] receivedData)
        {
            try
            {
                //Console.WriteLine(Encoding.UTF8.GetString(receivedData));
                
                Newtonsoft.Json.Linq.JObject response = JObject.Parse(Encoding.UTF8.GetString(receivedData));
                //Accordingly process the message received
                switch (response["cmd"].ToString())
                {

                    case "ResetPosition":
                        try
                        {
                            if (response["response"]["errorid"].ToString() == "0")
                            {
                                System.Windows.Forms.MessageBox.Show("重置仓位命令发送成功");
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("重置仓位命令发送失败");
                            }
                        }
                        catch (NullReferenceException)
                        {
                            logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
                        }
                        break;
                    case "QueryInstrument":
                        try
                        {
                            //System.Windows.Forms.MessageBox.Show(response["response"]["Payload"][1].ToString());
                            if (response["response"]["errorid"].ToString() == "0" && response["response"]["payload"] != null)
                            {
                                JArray jar = JArray.Parse(response["response"]["payload"].ToString());
                                for (var i = 0; i < jar.Count; i++)
                                {
                                    if (_wantedInstrument.Contains(jar[i].ToString()))
                                    {
                                    Instrument.Add(i, jar[i].ToString());
                                    }
                                }
                            }
                        }
                        catch (NullReferenceException)
                        {
                            logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
                        }

                        break;
                    case "QueryPosition":
                        //System.Windows.Forms.MessageBox.Show(response["response"]["Payload"][1].ToString());
                        try
                        {
                            if (response["response"]["errorid"].ToString() == "0" && response["response"]["payload"] != null)
                            {
                                JArray jar = JArray.Parse(response["response"]["payload"].ToString());

                                DispatcherHelper.CheckBeginInvokeOnUI(
                                    () =>
                                    {
                                        Positions.Clear();
                                        for (var i = 0; i < jar.Count; i++)
                                        {
                                            Position positionA = JsonConvert.DeserializeObject<Position>(jar[i].ToString());
                                            Positions.Add(positionA);

                                        }

                                    });
                            }
                            if (response["response"]["errorid"].ToString() == "0" && response["response"]["payload1"] != null)
                            {
                                JArray jar = JArray.Parse(response["response"]["payload1"].ToString());

                                DispatcherHelper.CheckBeginInvokeOnUI(
                                    () =>
                                    {
                                        CanceledOrder.Clear();
                                        for (var i = 0; i < jar.Count; i++)
                                        {
                                            Canceled cancel = JsonConvert.DeserializeObject<Canceled>(jar[i].ToString());
                                            CanceledOrder.Add(cancel);

                                        }

                                    });
                            }
                        }
                        catch (NullReferenceException )
                        {
                            logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
                        }
                        break;
                    case "CloseAll":
                        try
                        {
                            if (response["response"]["errorid"].ToString() == "0")
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(
                                    () =>
                                    {
                                        //IsAllActivated = StrategyA.IsActivate = StrategyB.IsActivate = StrategyC.IsActivate = StrategyD.IsActivate = StrategyE.IsActivate = StrategyF.IsActivate = !IsAllActivated;
                                        IsAllActivated = !IsAllActivated;
                                        //TODO:关闭所有策略状态
                                        StrategyA.IsActivate = StrategyB.IsActivate = StrategyC.IsActivate = StrategyD.IsActivate = StrategyE.IsActivate = StrategyF.IsActivate = false;
                                    });
                                
                            }
                        }
                        catch (NullReferenceException )
                        {
                            logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
                        }
                        break;
                    case "update":
                        try
                        {
                            if (response["response"]["errorid"].ToString() == "0")
                                DispatcherHelper.CheckBeginInvokeOnUI(
                                       () =>
                                       {
                                           String whichGrid = response["request"]["whichGrid"].ToString();

                                           switch (whichGrid)
                                           {
                                               case "GridA":
                                                   StrategyA = JsonConvert.DeserializeObject<Strategy>(response["request"].ToString());
                                                   StrategyRepository.GetInstance().updateStrategy(StrategyA);
                                                   break;
                                               case "GridB":
                                                   StrategyB = JsonConvert.DeserializeObject<Strategy>(response["request"].ToString());
                                                   StrategyRepository.GetInstance().updateStrategy(StrategyB);
                                                   break;
                                               case "GridC":
                                                   StrategyC = JsonConvert.DeserializeObject<Strategy>(response["request"].ToString());
                                                   StrategyRepository.GetInstance().updateStrategy(StrategyC);
                                                   break;
                                               case "GridD":
                                                   StrategyD = JsonConvert.DeserializeObject<Strategy>(response["request"].ToString());
                                                   StrategyRepository.GetInstance().updateStrategy(StrategyD);
                                                   break;
                                               case "GridE":
                                                   StrategyE = JsonConvert.DeserializeObject<Strategy>(response["request"].ToString());
                                                   StrategyRepository.GetInstance().updateStrategy(StrategyE);
                                                   break;
                                               case "GridF":
                                                   StrategyF = JsonConvert.DeserializeObject<Strategy>(response["request"].ToString());
                                                   StrategyRepository.GetInstance().updateStrategy(StrategyF);
                                                   break;
                                           }

                                       });
                        }
                        catch (NullReferenceException )
                        {
                            logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
                        }
                        break;
                }


            }
            catch (ObjectDisposedException)
            { }
            catch (NullReferenceException)
            {
                logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //receiveDone.Set();
            }
        }
        private object resetPosition(String GridName)
        {
            Strategy strategy = getStrategyByName(GridName);
            //modifid at 10/11/15 把仓位重置改成在策略关闭的情况下也能重置
            //if (strategy != null && strategy.IsActivate)
            if (strategy != null)
            {
                ResetModalViewModel resetModalViewModel = SimpleIoc.Default.GetInstance<ResetModalViewModel>();
                resetModalViewModel.WhichGrid = GridName;
                resetModalViewModel.StageId = strategy.StageId;
                ResetModalView dialog = new ResetModalView();
                dialog.ShowDialog();
                if (dialog.DialogResult == true)
                {
                    //todo
                    //System.Windows.MessageBox.Show((dialog.DataContext as ResetModalViewModel).Direction);
                    ResetInfo resetInfo = new ResetInfo(GridName, (dialog.DataContext as ResetModalViewModel).Direction, (dialog.DataContext as ResetModalViewModel).Volume);
                    Message<ResetInfo, String> resetMessage = new Message<ResetInfo, string>(resetInfo, "ResetPosition", Guid.NewGuid().ToString());
                    String resetString = JsonConvert.SerializeObject(resetMessage);
                    SendMessage(resetString);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("重置仓位前请先确保已生效策略！");
            }
            return null;
        }
        private Strategy getStrategyByName(String gridName)
        {
            switch (gridName)
            {
                case "GridA":

                    return StrategyA;
                    //RaisePropertyChanged("StrategyA");
                    

                case "GridB":
                    return StrategyB;
                    

                case "GridC":
                    return StrategyC;
                    

                case "GridD":
                    return StrategyD;
                    

                case "GridE":
                    return StrategyE;
                    

                case "GridF":
                    return StrategyF;
                default :
                    return null;
            }
        }

        private object modifyStage(String GridName)
        {
            String selectedStage;
            //List<string> stages = new List<string>();
            if (Instrument.Count == 0)
            {
                //查询无结果,测试代码
                Instrument.Add(0,"Rb0000");
                Instrument.Add(1,"ru0001");
                Instrument.Add(2,"ih0002");
                Instrument.Add(3, "zn0003");
                Instrument.Add(4, "au0004");
                Instrument.Add(5, "OI0005");
                Instrument.Add(6, "Y0006");
            }
            ModifyModalViewModel modifyModalViewModel = SimpleIoc.Default.GetInstance<ModifyModalViewModel>();
            modifyModalViewModel.Stages = Instrument;
            ModifyModalView dialog = new ModifyModalView();
            //ModifyModalViewModel modalViewModel = new ModifyModalViewModel(Instrument);
            //dialog.DataContext = modalViewModel;
            //(dialog.DataContext as ModifyModalViewModel).Stages = Instrument;
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
               //todo
                //System.Windows.MessageBox.Show((dialog.DataContext as ModifyModalViewModel).SelectedStageA);
                selectedStage = (dialog.DataContext as ModifyModalViewModel).SelectedStageA + "-" + (dialog.DataContext as ModifyModalViewModel).SelectedStageB;
                updateGridStage(GridName,selectedStage);
                updateGridIncre(GridName, (dialog.DataContext as ModifyModalViewModel).SelectedStageA);
            }
            /*
            var dialogServcie = new ModalDialogService();
            dialogServcie.ShowDialog(dialog, new ModifyModalViewModel(),
                returnedViewModelInstance =>
                {
                    if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                    {
                        selectedStage = returnedViewModelInstance.SelectedStage;
                    }
                }); 
             * */
            return null;
        }
        private void updateGridIncre(string GridName, string Stage)
        {
            switch (GridName)
            {
                case "GridA":
                    GridAIncre = getIncre(Stage);                  
                    break;

                case "GridB":
                    GridBIncre = getIncre(Stage);  
                    break;
                case "GridC":
                    GridCIncre = getIncre(Stage);
                    break;

                case "GridD":
                    GridDIncre = getIncre(Stage);
                    break;

                case "GridE":
                    GridEIncre = getIncre(Stage);
                    break;

                case "GridF":
                    GridFIncre = getIncre(Stage);
                    break;

            }
        }
        private string getIncre(string stage)
        {
            
            string header = stage.Substring(0, 2);
            string index = header + "Incre";
            try
            {
                return ConfigurationManager.AppSettings[index];
            }
            catch (ConfigurationErrorsException)
            {
                
                return ConfigurationManager.AppSettings["StockIncre"];
            }
            
            
            
        }

        private void updateGridStage(string GridName, string selectedStage)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                switch (GridName)
                {
                    case "GridA":

                        StrategyA.StageId = selectedStage;
                        //RaisePropertyChanged("StrategyA");
                        break;

                    case "GridB":
                        StrategyB.StageId = selectedStage;
                        RaisePropertyChanged("StrategyB");
                        break;

                    case "GridC":
                        StrategyC.StageId = selectedStage;
                        RaisePropertyChanged("StrategyC");
                        break;

                    case "GridD":
                        StrategyD.StageId = selectedStage;
                        RaisePropertyChanged("StrategyD");
                        break;

                    case "GridE":
                        StrategyE.StageId = selectedStage;
                        RaisePropertyChanged("StrategyE");
                        break;

                    case "GridF":
                        StrategyF.StageId = selectedStage;
                        RaisePropertyChanged("StrategyF");
                        break;

                }
            });
        }

        private object activateAll()
        {

            if (IsAllActivated==true)
            {
                /*
                List<Strategy> strategies = new List<Strategy>();
                Strategy temp = new Strategy(StrategyA);
                temp.IsActivate = !IsAllActivated;
                strategies.Add(temp);
                temp = new Strategy(StrategyB);
                temp.IsActivate = !IsAllActivated;
                strategies.Add(temp);

                temp = new Strategy(StrategyC);
                temp.IsActivate = !IsAllActivated;
                strategies.Add(temp);

                temp = new Strategy(StrategyD);
                temp.IsActivate = !IsAllActivated;
                strategies.Add(temp);

                temp = new Strategy(StrategyE);
                temp.IsActivate = !IsAllActivated;
                strategies.Add(temp);
                temp = new Strategy(StrategyF);
                temp.IsActivate = !IsAllActivated;
                strategies.Add(temp);
                */
                Response<String> queryRes = new Response<string>();
                Message<String, String> closeAll = new Message<string, string>(null, queryRes, "CloseAll", Guid.NewGuid().ToString());

                //Message<List<Strategy>, String> updataAllMessage = new Message<List<Strategy>, string>(strategies, "updateall", Guid.NewGuid().ToString());
                String closeAllString = JsonConvert.SerializeObject(closeAll);
                SendMessage(closeAllString);
            }
            else
            {
                IsAllActivated = !IsAllActivated;
            }
            //IsAllActivated = !IsAllActivated;
            return null;
        }
        private object activate(String s)
        {
            
            //TODO 补充通知服务器的步骤
            //s 表示从哪个grid发起的更新动作
            //System.Windows.MessageBox.Show(s);
            //System.Windows.MessageBox.Show(StrategyA.t2cl);
            //TEST CODE
            //StrategyA.IsActivate = !StrategyA.IsActivate;
            Strategy temp;
            
            switch (s)
            {
                case "GridA":
                    temp = new Strategy(StrategyA);
                    temp.IsActivate = !temp.IsActivate;
                    sendStrategy(temp);
                    
                    break;

                case "GridB":
                    temp =  new Strategy(StrategyB);
                    temp.IsActivate = !temp.IsActivate;
                    sendStrategy(temp);
                    break;

                case "GridC":
                    temp =  new Strategy(StrategyC);
                    temp.IsActivate = !temp.IsActivate;
                    sendStrategy(temp);
                    break;

                case "GridD":
                    temp =  new Strategy(StrategyD);
                    temp.IsActivate = !temp.IsActivate;
                    sendStrategy(temp);
                    break;

                case "GridE":
                    temp =  new Strategy(StrategyE);
                    temp.IsActivate = !temp.IsActivate;
                    sendStrategy(temp);
                    break;

                case "GridF":
                    temp =  new Strategy(StrategyF);
                    temp.IsActivate = !temp.IsActivate;
                    sendStrategy(temp);
                    break;
                
            }
            return null;
        }
        private object send(String s)
        {
            //todo、
            switch (s)
            {
                case "GridA":

                    sendStrategyWithoutActive(StrategyA);
                    break;

                case "GridB":

                    sendStrategyWithoutActive(StrategyB);
                    break;

                case "GridC":
                    sendStrategyWithoutActive(StrategyC);
                    break;

                case "GridD":
                    sendStrategyWithoutActive(StrategyD);
                    break;

                case "GridE":
                    sendStrategyWithoutActive(StrategyE);
                    break;

                case "GridF":
                    sendStrategyWithoutActive(StrategyF);
                    break;
            }
            return null;
        }

        private object sendWithActivate(String s)
        {
            //todo、
            Strategy temp;
            switch (s)
            {
                case "GridA":
                    temp =  new Strategy(StrategyA);
                    temp.IsActivate = true;
                    sendStrategy(temp);
                    break;

                case "GridB":
                    temp =  new Strategy(StrategyB);
                    temp.IsActivate = true;
                    sendStrategy(temp);
                    break;

                case "GridC":
                    temp =  new Strategy(StrategyC);
                    temp.IsActivate = true;
                    sendStrategy(temp);
                    break;

                case "GridD":
                    temp =  new Strategy(StrategyD);
                    temp.IsActivate = true;
                    sendStrategy(temp);
                    break;

                case "GridE":
                    temp =  new Strategy(StrategyE);
                    temp.IsActivate = true;
                    sendStrategy(temp);
                    break;

                case "GridF":
                    temp =  new Strategy(StrategyF);
                    temp.IsActivate = true;
                    sendStrategy(temp);
                    break;
            }
            return null;
        }
        private void sendStrategyWithoutActive(Strategy strategy)
        {
            if (checkInput(strategy))
            {
                Message<Strategy, String> updateMessage = new Message<Strategy, string>(strategy, "update", Guid.NewGuid().ToString());
                String updateString = JsonConvert.SerializeObject(updateMessage);
                SendMessage(updateString);
            }
            else
            {
                System.Windows.MessageBox.Show(strategy.whichGrid + ":多开、多平或者空开、空平输入有误");
            }
        }
        private void sendStrategy(Strategy strategy)
        {
            if (_IsAllActivated)
            {
                bool sendFlag = false;
                if (!strategy.IsActivate)
                {
                    sendFlag = true;
                }  
                else
                {
                    if (checkInput(strategy))
                    {
                        sendFlag = true;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(strategy.whichGrid + ":多开、多平或者空开、空平输入有误");
                    }
                }
                

                if (sendFlag)
                {
                    Message<Strategy, String> updateMessage = new Message<Strategy, string>(strategy, "update", Guid.NewGuid().ToString());
                    String updateString = JsonConvert.SerializeObject(updateMessage);
                    SendMessage(updateString);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("全部关闭状态下无法更新参数");
            }
        }
        private void SendMessage(String message)
        {
            byte[] updateData = Encoding.UTF8.GetBytes(message);
            tcpConnection.SendMessageWithPrefixLengthAsync(updateData);
            //tcpConnection.StartReceive();
        }
        public void startTimer()
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer(2000);
                timer.Elapsed += new ElapsedEventHandler(queryPosition);
                timer.Start();
            }
            else
            {
                timer.Start();
            }
            //timer.Start();
        }
        private void queryPosition(object sender, ElapsedEventArgs e)
        {
            //todo ,获取已成订单
            //调试代码
            /*Response<String> queryRes = new Response<string>();
            queryRes.ErrorID = "0";
            queryRes.ErrorMsg = null;
            queryRes.Payload = new List<String>();
            queryRes.Payload.Add("xxxx-xxxx");
            queryRes.Payload.Add("yyyy-xxxx");
            Message<String, String> queryMessage = new Message<string, string>(null, queryRes, "QueryInstrument", Guid.NewGuid().ToString());*/
            
            
            
            Response<Position> queryRes = new Response<Position>();
            //queryRes.ErrorID = "0";
            //queryRes.ErrorMsg = null;
            //test code
            //queryRes.Payload = new List<Position>();
            //Position positionA = new Position("xxxx-yyy","1","2","多","12","12","1");
            //Position positionB = new Position("zzzz-yyy", "1", "2", "空", "12", "12", "1");
            //queryRes.Payload.Add(positionA);
            //queryRes.Payload.Add(positionB);
            Message<String, Position> queryMessage = new Message<string, Position>(null, queryRes, "QueryPosition", Guid.NewGuid().ToString());
            String queryStr = JsonConvert.SerializeObject(queryMessage);
            SendMessage(queryStr);
            //System.Windows.MessageBox.Show("Navigate to Page 2!");
            

        }
        public void queryInstrument()
        {
            Response<String> queryRes = new Response<string>();
            Message<String, String> queryMessage = new Message<string, string>(null, queryRes, "QueryInstrument", Guid.NewGuid().ToString());
            String queryStr = JsonConvert.SerializeObject(queryMessage);
            SendMessage(queryStr);
        }

        internal void enterEventHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var pressedKey = (e != null) ? (System.Windows.Input.KeyEventArgs)e : null;

            if (pressedKey.Key == Key.Enter && pressedKey != null)
            {
                UIElement element = VSHelper.GetElementUnderMouse<UIElement>();
                Border border = VSHelper.GetParent<Border>(element, typeof(Border));
                String gridName = border.Name;
                sendWithActivate(gridName);
                e.Handled = true;
            }
             
        }
        private bool  checkInput( Strategy strategy)
        {
            decimal dk = decimal.Parse(strategy.dkjc);
            decimal dp = decimal.Parse(strategy.dp);
            decimal kk = decimal.Parse(strategy.kkjc);
            decimal kp = decimal.Parse(strategy.kp);

            if (dk >= dp)
            {
                return false;
            }

            if (kk <= kp)
            {
                return false;
            }

            if (kk < dp)
            {
                return false;
            }

            if (kp < dk)
            {
                return false;
            }

            return true;
        }
    }
}