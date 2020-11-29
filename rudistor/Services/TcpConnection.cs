using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MvvmControlChange;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Communication
{
    public struct AsyncUserToken
    {
        public bool IsHead;
        public int BytesExpected;
        public int BytesReceivedDynamic;
        public int BytesReceivedStatic;

        public AsyncUserToken(bool isHead, int bytesExpected, int bytesReceivedDynamic, int bytesReceivedStatic)
        {
            this.IsHead = isHead;
            this.BytesExpected = bytesExpected;
            this.BytesReceivedDynamic = bytesReceivedDynamic;
            this.BytesReceivedStatic = bytesReceivedStatic;
        }
    }

    public class TcpConnection
    {
        /// <summary>
        /// default receive buffer size, 4MB 
        /// </summary>
        /// 

        public static Logger logger = LogManager.GetCurrentClassLogger();

        const int ReceiveBufferSize = 4;

        //TcpClient client_;
        Socket clientSocket;
        byte[] receiveBuffer;
        bool closeGraceful;
        //public Thread heartbeater;

        int onDisconnectedRaised;

        private EndPoint remoteEndPoint;
        private EndPoint localEndPoint;

        public bool Connected
        {
            get { return this.clientSocket != null && this.clientSocket.Connected; }
        }

        public void Disconnect()
        {
            clientSocket.Disconnect(false);
        }
        public EndPoint RemoteEndPoint
        {
            get
            {
                if (remoteEndPoint == null)
                {
                    try { remoteEndPoint = clientSocket.RemoteEndPoint; }
                    catch { }
                }
                return remoteEndPoint;

                //try { return clientSocket.RemoteEndPoint; }
                //catch { }
                //return null;
            }
        }

        public EndPoint LocalEndPoint
        {
            get
            {
                if (localEndPoint == null)
                {
                    try { localEndPoint = clientSocket.LocalEndPoint; }
                    catch { }
                }
                return localEndPoint;

                //try { return clientSocket.LocalEndPoint; }
                //catch { }
                //return null;
            }
        }

        public event Action<TcpConnection, byte[]> OnDataReceivedCompleted, OnDataSendCompleted;
        //public event Action<SocketAsyncEventArgs,TcpConnection> OnConnected, OnDisconnected;
        public event Action<TcpConnection> OnConnected, OnDisconnected;
        public event Action<SocketAsyncEventArgs, AsyncUserToken> ReportReceivedProgress, ReportSendProgress;


        /*
        private static TcpConnection _instance;

        public static TcpConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TcpConnection();
            }
            return _instance;
        }*/
        public TcpConnection()
        {
            try
            {
                this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                this.closeGraceful = false;
                this.onDisconnectedRaised = 0;
                OnConnected += TcpConnection_OnConnected;
                OnDisconnected += TcpConnection_OnDisconnected;
                OnDataReceivedCompleted += TcpConnection_OnDataReceivedCompleted;
            }
            catch (SocketException ex) { throw ex; }
            catch (ArgumentOutOfRangeException ex) { throw ex; }
        }

        void TcpConnection_OnDisconnected(TcpConnection obj)
        {

            logger.Debug("server disconnected!!!");
            var msg = new GoToPageMessage() { PageName = "LoginControl" };
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                Messenger.Default.Send<GoToPageMessage>(msg);
            });
        }

        void TcpConnection_OnDataReceivedCompleted(TcpConnection conn, byte[] receivedData)
        {

            Boolean loginResult = false;
            String mainWorkView = "StrategyWorkView2";

            //Console.WriteLine(Encoding.UTF8.GetString(receivedData));
            logger.Debug("message received : " + Encoding.UTF8.GetString(receivedData));
            try
            {

                JObject response = JObject.Parse(System.Text.Encoding.Default.GetString(receivedData));
                //Accordingly process the message received
                switch (response["cmd"].ToString())
                {
                    case "login":
                        Thread.Sleep(1000);
                        if (null != ConfigurationManager.AppSettings["mainWorkView"])
                        {
                            mainWorkView = "StrategyWorkView";
                        }
                        //正式代码
                        loginResult = (response["response"]["errorid"].ToString() == "0");
                        if (loginResult)
                        {
                            //var msg = new GoToPageMessage() { PageName = "StrategyWorkView" };
                            //old version
                            var msg = new GoToPageMessage() { PageName = mainWorkView };
                            DispatcherHelper.CheckBeginInvokeOnUI(
                            () =>
                            {
                                Messenger.Default.Send<GoToPageMessage>(msg);
                            });
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(response["response"]["errormsg"].ToString(), "登录失败");
                        }

                        //test code

                        /*
                        var msg = new GoToPageMessage() { PageName = "WorkView" };
                        DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            Messenger.Default.Send<GoToPageMessage>(msg);
                        });
                        */

                        break;


                }


            }
            catch (ObjectDisposedException)
            {

            }
            catch (InvalidOperationException)
            {
                logger.Debug("not expected message:" + Encoding.UTF8.GetString(receivedData));
                System.Windows.MessageBox.Show("登录失败");
            }
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

        void TcpConnection_OnConnected(TcpConnection obj)
        {
            //this.Connected = (e.SocketError == SocketError.Success);
            logger.Debug("server connected!!!");

        }

        internal TcpConnection(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            this.closeGraceful = false;
            this.onDisconnectedRaised = 0;
        }

        public TcpConnection(string serverIPAddress, int port)
        {
            try
            {
                this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIPAddress), port);
                this.closeGraceful = false;
                this.onDisconnectedRaised = 0;
            }
            catch (SocketException ex) { throw ex; }
            catch (ArgumentOutOfRangeException ex) { throw ex; }
        }

        ~TcpConnection()
        {
            if (this.clientSocket != null && this.clientSocket.Connected)
            {
                this.CloseConnection();
            }

            this.receiveBuffer = null;

            /*
            if (this.heartbeater != null && this.heartbeater.IsAlive)
            {
                try
                {
                    this.heartbeater.Abort();
                    this.heartbeater.Join(1000);
                }
                catch { }
            }
            */
        }

        public bool Start(string serverIPAddress, int port)
        {
            /*if (remoteEndPoint == null)
            {
                this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIPAddress), port);
            }*/
            this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIPAddress), port);
            try
            {
                this.clientSocket.Connect(remoteEndPoint);
                return true;
            }
            catch (SocketException)
            {
                int tries = 3;
                do
                {
                    try { this.clientSocket.Connect(remoteEndPoint); }
                    catch { }
                } while (--tries > 0);
                if (this.clientSocket.Connected == false)
                {
                    System.Windows.MessageBox.Show("无法连接到指定的服务器");
                    logger.Debug("server cannot connect after 3 retries!");
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex) { throw ex; }
            /*
            try
            {
                this.StartReceive();
            }
            catch (Exception ex) { throw ex; }*/
        }

        public void StartReceive()
        {
            /*
            this.heartbeater = new Thread(new ThreadStart(CheckConnection));
            this.heartbeater.Priority = ThreadPriority.Lowest;
            this.heartbeater.Start();
            */
            this.clientSocket.Blocking = false;
            // set send buffer
            this.clientSocket.SendBufferSize = 0;
            this.clientSocket.NoDelay = true;

            // set receive buffer
            this.receiveBuffer = new byte[ReceiveBufferSize];

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            e.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);

            if (this.clientSocket.Connected)
                this.OnConnected(this);

            e.UserToken = new AsyncUserToken(true, 0, 0, 0);

            if (!this.clientSocket.ReceiveAsync(e))
                this.ProcessReceived(e);
        }

        void CheckConnection()
        {
            byte[] emptyMessage = new byte[0];
            int sleepMilliseconds = 1000;
            while (true)
            {
                try
                {
                    this.clientSocket.Send(emptyMessage);

                    Thread.Sleep(sleepMilliseconds);
                }
                catch (ArgumentNullException) { emptyMessage = new byte[0]; }
                catch (ArgumentOutOfRangeException) { sleepMilliseconds = 1000; }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock &&
                        ex.SocketErrorCode != SocketError.IOPending)
                    {
                        if (ex.SocketErrorCode != SocketError.ConnectionReset &&
                           ex.SocketErrorCode != SocketError.NotConnected)
                        {
                            Console.WriteLine(ex.SocketErrorCode);
                            Console.ReadLine();
                        }
                        break;
                    }
                }
                catch (ObjectDisposedException) { break; }
                catch (ThreadInterruptedException) { break; }
                catch (ThreadAbortException) { break; }
            }

            CloseConnection();
        }

        void e_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Connect:
                        ProcessConnected(e);
                        break;
                    case SocketAsyncOperation.Disconnect:
                        ProcessDisconnected(e);
                        break;
                    case SocketAsyncOperation.Receive:
                        ProcessReceived(e);
                        break;
                    case SocketAsyncOperation.Send:
                        ProcessSend(e);
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }

        void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                token.BytesReceivedDynamic = e.BytesTransferred;
                token.BytesReceivedStatic += e.BytesTransferred;
                // send not completed
                if (e.BytesTransferred < token.BytesExpected)
                {
                    if (ReportSendProgress != null)
                        ReportSendProgress(e, token);

                    e.SetBuffer(e.BytesTransferred, token.BytesExpected - e.BytesTransferred);
                    e.UserToken = token;
                    clientSocket.SendAsync(e);
                }
                else // send has completed
                {
                    if (OnDataSendCompleted == null)
                        return;
                    OnDataSendCompleted(this, e.Buffer);
                }
            }
        }

        void ProcessReceived(SocketAsyncEventArgs e)
        {
            //..
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;

                int offset = 0; // token.BytesReceivedStatic;
                int bytesExpected = 0; // token.BytesExpected;

                token.BytesReceivedDynamic = e.BytesTransferred; // bytes received this time
                token.BytesReceivedStatic += e.BytesTransferred; // bytes accumulated in the buffer

                if (ReportReceivedProgress != null)
                    ReportReceivedProgress(e, token);

                bool hasCompletedMessage = true;
                while (hasCompletedMessage)
                {
                    if (token.IsHead) // is head, read the prefix length
                    {
                        // incomplete prefix bytes
                        if (token.BytesReceivedStatic < 4)
                            break;

                        //token.BytesExpected = BitConverter.ToInt32(e.Buffer, 0);
                        byte[] lengthTemp = new byte[4];
                        Buffer.BlockCopy(e.Buffer, 0, lengthTemp, 0, lengthTemp.Length);
                        String len = Encoding.UTF8.GetString(lengthTemp);
                        //报文长度不包含报文头
                        token.BytesExpected = Int32.Parse(len) + 4;
                        if (token.BytesExpected == 0)
                        {
                            // heartbeat message
                            // remove 4 bytes at head
                            Buffer.BlockCopy(e.Buffer, 4, e.Buffer, 0, token.BytesReceivedStatic - 4);

                            // continue to receive
                            break;
                        }

                        // expand the buffer size when the buffer is not large enough
                        if (token.BytesExpected > receiveBuffer.Length)
                        {
                            byte[] temp = new byte[token.BytesExpected];
                            Buffer.BlockCopy(e.Buffer, 0, temp, 0, e.Buffer.Length);
                            receiveBuffer = temp;
                            e.SetBuffer(temp, 0, temp.Length);
                        }

                        token.IsHead = false;
                    }

                    // if bytes accumulated in the buffer is greater than or equals to bytes expected
                    hasCompletedMessage = token.BytesReceivedStatic >= token.BytesExpected;
                    //
                    if (hasCompletedMessage) // at least one complete message
                    {
                        // deliver the first message in the buffer
                        byte[] receivedData = new byte[token.BytesExpected - 4];
                        Buffer.BlockCopy(receiveBuffer, 4, receivedData, 0, receivedData.Length);
                        OnDataReceivedCompleted(this, receivedData);

                        token.IsHead = true; // reset the head flag

                        // valid bytes left in the buffer
                        int bytesLeftInBuffer = token.BytesReceivedStatic - token.BytesExpected;

                        // shift the incomplete data to the head of the buffer
                        // TODO: 
                        // FIXME: only do shift the last incomplete data
                        Buffer.BlockCopy(e.Buffer, token.BytesExpected, e.Buffer, 0, bytesLeftInBuffer);

                        // reset the bytes of valid data in the buffer
                        token.BytesReceivedStatic = bytesLeftInBuffer;
                    }
                }

                // append the following data to the buffer from offset position
                offset = token.BytesReceivedStatic; // start position of the buffer

                bytesExpected =
                    //// only expected message length bytes
                    //token.BytesExpected - token.BytesReceivedStatic;
                    //// expected bytes as much as the buffer can hold
                    e.Buffer.Length - token.BytesReceivedStatic;

                e.SetBuffer(offset, bytesExpected);
                e.UserToken = token;

                if (!clientSocket.ReceiveAsync(e))
                    ProcessReceived(e);
            }
            else
            {
                CloseConnection();
            }
        }

        void ProcessConnected(SocketAsyncEventArgs e)
        {
            if (this.OnConnected == null)
                return;

            this.OnConnected(this);
        }

        void ProcessDisconnected(SocketAsyncEventArgs e)
        {
            if (this.OnDisconnected == null)
                return;

            this.OnDisconnected(this);
        }

        public bool Shutdown()
        {
            try
            {
                CloseConnection();
                return true;
            }
            catch { }

            return false;
        }

        internal void CloseConnection()
        {
            if (Interlocked.CompareExchange(ref onDisconnectedRaised, 1, 0) > 0)
                return;

            // close the socket associated with the client
            try
            {
                if (!closeGraceful)
                {
                    clientSocket.LingerState = new LingerOption(true, 0);
                }

                //clientSocket.Shutdown(SocketShutdown.Both);
            }
            catch { }

            try
            {
                this.OnDisconnected(this);
                clientSocket.Close(0);
            }
            catch { }


        }

        public void SendMessage(byte[] data)
        {
            SendMessage(data, 1);
        }

        public int SendMessage(byte[] data, int tries)
        {
            int count = 0;
            try
            {
                count = clientSocket.Send(data, 0, data.Length, SocketFlags.None);
            }
            catch (Exception)
            {
                if (tries-- > 0)
                    count = SendMessage(data, tries);
            }
            return count;
        }

        public int SendMessageWithPrefixLength(byte[] data)
        {
            int count = 0;
            try
            {
                byte[] message = new byte[data.Length + 4];
                //byte[] prefixBytes = BitConverter.GetBytes(data.Length + 4);
                String prefix = String.Format("{0:D4}", data.Length);
                byte[] prefixBytes = Encoding.UTF8.GetBytes(prefix);
                Buffer.BlockCopy(prefixBytes, 0, message, 0, prefixBytes.Length);
                Buffer.BlockCopy(data, 0, message, prefixBytes.Length, data.Length);

                count = SendMessage(message, 1);
            }
            catch (Exception ex)
            {
                ex.ToString();
                count = 0;
            }

            return count;
        }

        public bool SendMessageWithPrefixLengthAsync(byte[] data)
        {
            bool succeed = false;
            try
            {
                byte[] message = new byte[data.Length + 4];
                //byte[] prefixBytes = BitConverter.GetBytes(data.Length + 4);
                //报文长度不包括报文头
                //String prefix = String.Format("{0:D4}", data.Length + 4);
                String prefix = String.Format("{0:D4}", data.Length);
                byte[] prefixBytes = Encoding.UTF8.GetBytes(prefix);
                Buffer.BlockCopy(prefixBytes, 0, message, 0, prefixBytes.Length);
                Buffer.BlockCopy(data, 0, message, prefixBytes.Length, data.Length);
                //Console.WriteLine(Encoding.UTF8.GetString(message));
                logger.Debug("message send : " + Encoding.UTF8.GetString(message));
                succeed = SendMessageAsync(message, false);
            }
            catch (Exception ex)
            {
                ex.ToString();
                succeed = false;
            }

            return succeed;
        }

        public bool SendMessageAsync(byte[] data, bool safe)
        {
            bool succeed = false;
            if (this.clientSocket.Connected == false)
            {
                this.OnDisconnected(this);
            }
            try
            {
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
                if (safe)
                {
                    byte[] safeData = new byte[data.Length];
                    Buffer.BlockCopy(data, 0, safeData, 0, data.Length);
                    e.SetBuffer(safeData, 0, safeData.Length);
                }
                else
                {
                    e.SetBuffer(data, 0, data.Length);
                }

                e.UserToken = new AsyncUserToken(true, data.Length, 0, 0);

                clientSocket.SendAsync(e);
                succeed = true;
            }
            catch (Exception)
            {
                succeed = false;
            }

            return succeed;
        }
    }
}
