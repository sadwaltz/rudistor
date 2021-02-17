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
using rudistor.Contents.TempPage;
using rudistor.Model;
using rudistor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace rudistor.Contents.TempPage
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StrategyWorkViewModel : ViewModelBase
    {
        private class StrategyRunningStatus 
        {
            public bool IsActivate { get; set; }
            public string AutoCallType { get; set; }
        }
        /// <summary>
        /// Initializes a new instance of the WorkerViewModel class.
        /// </summary>
        /// 
        #region define vars
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private User _currentUser;
        private String _currentCompany;

        //private List<string> _wantedInstrument;

        private Dictionary<string, StrategyRunningStatus> currentStrategyStatus;

        private SortableObservableCollection<Strategy> _strategies;
        public SortableObservableCollection<Strategy> Strategis
        {
            get { return _strategies; }
            set
            {
                _strategies = value;
                RaisePropertyChanged("Strategis");
            }
        }

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

        private SortableObservableCollection<string> _instrument;

        public SortableObservableCollection<string> Instrument
        {
            get { return _instrument; }
            set
            {
                _instrument = value;
                RaisePropertyChanged("Instrument");
            }
        }

        //public static readonly string IniFilePath = Path.Combine(Environment.CurrentDirectory, "Config", "config.ini");

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
                    _ClComboboxItems.Add(new ComboboxItem() { index = "4", describe = "4-认购+认估" });
                    _ClComboboxItems.Add(new ComboboxItem() { index = "5", describe = "5-认购+认估MM" });
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
        private TcpConnection tcpConnection;

        // ManualResetEvent instances signal completion.     
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public System.Timers.Timer timer;
        #endregion
        #region define command handlers
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

        #endregion
        #region constructor
        public StrategyWorkViewModel()
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
            ModifyStage = new RelayCommand<string>((s) => newModifyStage(s));
            //打开重置仓位窗口
            ResetPosition = new RelayCommand<string>((s) => resetPosition(s));
            init();


        }
        

        private void init()
        {
            initData();
            initGUI();
        }

        private void initData()
        {
            //从文件中读取所有策略
            var temp = StrategyRepository.GetInstance().GetStrategies();
            Strategis = new SortableObservableCollection<Strategy>(StrategyRepository.GetInstance().GetStrategies());
            Strategis.Sort(c => c.whichGrid);

            currentStrategyStatus = new Dictionary<string, StrategyRunningStatus>();
            foreach(Strategy i in Strategis) {
                StrategyRunningStatus st = new StrategyRunningStatus();
                st.IsActivate = i.IsActivate;
                st.AutoCallType = i.autoCall;
                currentStrategyStatus.Add(i.whichGrid, st);
            }

            //initComm();
            //_wantedInstrument = getInstrumentsFromINI();
        }
        private void initGUI()
        {
            IsAllActivated = false;

            //StrategyA = new Strategy("GridA",true,"aaaa-bbbb","5","2","1");
            Positions = new ObservableCollection<Position>();
            CanceledOrder = new ObservableCollection<Canceled>();
            Instrument = new SortableObservableCollection<string>();

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
        #endregion
        #region tcp
        void tcpConnection_OnDisconnected(TcpConnection obj)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            System.Windows.Forms.MessageBox.Show("通讯连接已断开，请重新登录");
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
                                // 清理
                                Instrument.Clear();

                                JArray jar = JArray.Parse(response["response"]["payload"].ToString());
                                for (var i = 0; i < jar.Count; i++)
                                {
                                    // 添加合约
                                    addInstrument(i, jar[i].ToString());
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
                        catch (NullReferenceException)
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
                                        
                                        IsAllActivated = false;
                                        //关闭所有策略状态
                                        closeAllStrategy();
                                        
                                    });

                            }
                        }
                        catch (NullReferenceException)
                        {
                            logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
                        }
                        break;
                    // inform update share handler
                    case "inform":
                    case "update":
                        try
                        {
                            if (response["response"]["errorid"].ToString() == "0")
                            {
                                String whichGrid = response["request"]["whichGrid"].ToString();
                                //Strategy temp = selectStrategyById(whichGrid);
                                //从应答中获取更为安全
                                Strategy resp = JsonConvert.DeserializeObject<Strategy>(response["request"].ToString());
                                DispatcherHelper.CheckBeginInvokeOnUI(
                                       () =>
                                       {

                                           //存在应答过程中用户修改了参数的可能性，考虑是否使用temp更新strategy全集
                                           StrategyRepository.GetInstance().updateStrategy(resp);

                                           // 生成临时VM
                                           SortableObservableCollection<Strategy> tempStrategies = new SortableObservableCollection<Strategy>(StrategyRepository.GetInstance().GetStrategies());

                                           // 更新策略打开关闭状态
                                           currentStrategyStatus[resp.whichGrid].IsActivate = resp.IsActivate;
                                           currentStrategyStatus[resp.whichGrid].AutoCallType = resp.autoCall;
                                           // 复制打开关闭状态
                                           foreach (Strategy st1 in tempStrategies)
                                           {
                                               st1.IsActivate = currentStrategyStatus[st1.whichGrid].IsActivate;
                                               st1.autoCall = currentStrategyStatus[st1.whichGrid].AutoCallType;
                                           }

                                           tempStrategies.Sort(c => c.whichGrid);
                                           this.Strategis = tempStrategies;

                                       });
                            }
                        }
                        catch (NullReferenceException)
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

        #endregion
        #region reset position
        private object resetPosition(String GridName)
        {
            Strategy strategy = selectStrategyById(GridName);
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
        #endregion
        #region modify strategy config
        private object newModifyStage(String GridName)
        {
            //for debug
            if (Instrument.Count == 0)
            {
                //查询无结果,测试代码
                Instrument.Add("Rb0000");
                Instrument.Add("ru0001");
                Instrument.Add("ih0002");
                Instrument.Add("zn0003");
                Instrument.Add("au0004");
                Instrument.Add("OI0005");
                Instrument.Add("Y0006");
            }
            Strategy modified = selectStrategyById(GridName);
            Strategy backup = new Strategy(modified);
            OtherConfigView dialog = new OtherConfigView();
            //将修改窗口定位到主窗口中心位置
            dialog.Owner = System.Windows.Application.Current.MainWindow; // We must also set the owner for this to work.
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dialog.DataContext = modified;
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                logger.Debug(modified.whichGrid + "config changed");
            }
            else
            {
                rollback(backup);
            }
            return null;
        }
        
        
        #endregion
        #region send
        //打开或者关闭所有策略
        private object activateAll()
        {

            if (IsAllActivated == true)
            {
                // 关闭所有策略
                foreach (Strategy temp in Strategis)
                {
                    if (temp.IsActivate)
                    {
                        temp.IsActivate = false;

                        // 关闭界面
                        currentStrategyStatus[temp.whichGrid].IsActivate = false;
                        currentStrategyStatus[temp.whichGrid].AutoCallType = AutoCallStatus.Close.ToString();
                    }

                }

                
                
                Response<String> queryRes = new Response<string>();
                Message<String, String> closeAll = new Message<string, string>(null, queryRes, "CloseAll", Guid.NewGuid().ToString());

                //Message<List<Strategy>, String> updataAllMessage = new Message<List<Strategy>, string>(strategies, "updateall", Guid.NewGuid().ToString());
                String closeAllString = JsonConvert.SerializeObject(closeAll);
                SendMessage(closeAllString);
            }

            IsAllActivated = !IsAllActivated;
            
            return null;
        }
        //打开或者关闭策略(状态取反)
        private object activate(String s)
        {

            //TODO 补充通知服务器的步骤
            //s 表示从哪个grid发起的更新动作
            if (!_IsAllActivated)
            {
                System.Windows.MessageBox.Show("全部关闭状态下无法更新参数");
                return null;
            }

            Strategy temp = selectStrategyById(s);
            temp.IsActivate = !temp.IsActivate;
            sendStrategy(temp);
            return null;
        }

        private object send(String s)
        {
            //直接发送策略
            sendStrategyWithoutActive(selectStrategyById(s));
            return null;
        }
        //打开并发送策略
        private object sendWithActivate(String s)
        {
           
            Strategy temp = selectStrategyById(s);
            temp.IsActivate = true;
            sendStrategy(temp);
            return null;
        }
        private void sendStrategyWithoutActive(Strategy strategy)
        {
            if (checkInput(strategy))
            {
                //Message<Strategy, String> updateMessage = new Message<Strategy, string>(strategy, "inform", Guid.NewGuid().ToString());
                Message<Strategy, String> updateMessage = new Message<Strategy, string>(strategy, "update", Guid.NewGuid().ToString());
                String updateString = JsonConvert.SerializeObject(updateMessage);
                SendMessage(updateString);
            }
            else
            {
                System.Windows.MessageBox.Show(strategy.whichGrid + ":多开、多平或者空开、空平输入有误");
            }
        }
        //发送策略
        private void sendStrategy(Strategy strategy)
        {
            if (!_IsAllActivated)
            {
                System.Windows.MessageBox.Show("全部关闭状态下无法更新参数");
                return;
            }

            if (!checkInput(strategy))
            {
                System.Windows.MessageBox.Show(strategy.whichGrid + ":多开、多平或者空开、空平输入有误");
                return;
            }

            //Message<Strategy, String> updateMessage = new Message<Strategy, string>(strategy, "inform", Guid.NewGuid().ToString());
            Message<Strategy, String> updateMessage = new Message<Strategy, string>(strategy, "update", Guid.NewGuid().ToString());
            String updateString = JsonConvert.SerializeObject(updateMessage);
            SendMessage(updateString);

            return;
                
        }
        private void SendMessage(String message)
        {
            byte[] updateData = Encoding.UTF8.GetBytes(message);
            tcpConnection.SendMessageWithPrefixLengthAsync(updateData);
            //tcpConnection.StartReceive();
        }
        #endregion
        #region querys
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
        #endregion
        #region TODO keyboard event handler
        //TODO 获取回车事件对应的策略，并打开、发送到服务端
        internal void enterEventHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var pressedKey = (e != null) ? (System.Windows.Input.KeyEventArgs)e : null;

            if (pressedKey.Key == Key.Enter && pressedKey != null)
            {
                UIElement element = VSHelper.GetElementUnderMouse<UIElement>();
                StrategyGridView grid = VSHelper.GetParentView<StrategyGridView>(element, typeof(StrategyGridView));
                if (grid != null)
                {
                    String gridName = ((Strategy)grid.DataContext).whichGrid;
                    sendWithActivate(gridName);
                    e.Handled = true;
                }
                else
                {
                    logger.Error("cannot find parent strategy!");
                    System.Windows.Forms.MessageBox.Show("发送策略失败");
                }
            }

        }
        #endregion
        #region helper method
        private void addInstrument(int i, string inst)
        {
            if (Instrument.Contains(inst))
            {
                return;
            }

            //if (_wantedInstrument.Count > 1)
            //{
            //    if (!_wantedInstrument.Contains(inst))
            //    {
            //        return;
            //    }
            //}
            Instrument.Add(inst);
            Instrument.Sort(c => c);
            RaisePropertyChanged("Instrument");
        }
        //保存最后一次成功登录信息
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
        //从配置文件读取关注的品种类型
        //private List<string> getInstrumentsFromINI()
        //{
        //    try
        //    {
        //        INIManager ini = new INIManager(IniFilePath);
        //        return new List<string>(ini.IniReadValue("InstrumentList", "list").Split(','));
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e);
        //        return new List<string>();
        //    }
        //}

        //查询定时器
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
        //根据选择，更新策略中的品种组合
        private void updateGridStage(string GridName, string selectedStage)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                Strategy temp = selectStrategyById(GridName);
                temp.StageId = selectedStage;
            });
        }
        private bool checkInput(Strategy strategy)
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
        private Strategy selectStrategyById(string whichGrid)
        {
            foreach (Strategy temp in Strategis)
            {
                if (whichGrid == temp.whichGrid)
                {
                    return temp;
                }

            }
            logger.Error("cannot find Strategy by id!");
            return null;

        }

        private void closeAllStrategy()
        {
            foreach (Strategy temp in Strategis)
            {
                temp.IsActivate = false;
                temp.autoCall = AutoCallStatus.Close.ToString();
                temp.RefreshRunningStatus();
            }

            foreach (StrategyRunningStatus i in currentStrategyStatus.Values)
            {
                i.IsActivate = false;
                i.AutoCallType = AutoCallStatus.Close.ToString();
            }

            
        }
        private void rollback(Strategy backup)
        {
            for (var i = 0; i < Strategis.Count; i++)
            {
                if (Strategis[i].whichGrid == backup.whichGrid)
                {
                    Strategis[i] = backup;
                    break;
                }
            }
        }
        #endregion
    }
}