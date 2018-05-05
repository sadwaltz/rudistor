using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
// State object for reading client data asynchronously     
public class StateObject
{
    // Client socket.     
    public Socket workSocket = null;
    // Size of receive buffer.     
    public const int BufferSize = 9999;
    // Receive buffer.     
    public byte[] buffer = new byte[BufferSize];
    // Received data string.     
    public StringBuilder sb = new StringBuilder();
}
public class AsynchronousSocketListener
{
    // Thread signal.     
    public static ManualResetEvent allDone = new ManualResetEvent(false);

    //public static bool flag = false;
    public AsynchronousSocketListener()
    {
    }
    public static void StartListening()
    {
        // Data buffer for incoming data.     
        byte[] bytes = new Byte[9999];
        // Establish the local endpoint for the socket.     
        // The DNS name of the computer     
        // running the listener is "host.contoso.com".     
        //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        //IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 61002);
        // Create a TCP/IP socket.     
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // Bind the socket to the local     
        //endpoint and listen for incoming connections.     
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);
            while (true)
            {
                // Set the event to nonsignaled state.     
                allDone.Reset();
                // Start an asynchronous socket to listen for connections.     
                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                // Wait until a connection is made before continuing.     
                allDone.WaitOne();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();
    }
    public static void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.     
        allDone.Set();
        // Get the socket that handles the client request.     
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);
        // Create the state object.     
        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
    }
    public static void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;
        bool flag = false;
        // Retrieve the state object and the handler socket     
        // from the asynchronous state object.     
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;
        // Read data from the client socket.    
        SocketError se;
        int bytesRead = handler.EndReceive(ar, out se);
        if (bytesRead > 0)
        {
            // There might be more data, so store the data received so far.     
            state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
            // Check for end-of-file tag. If it is not there, read     
            // more data.     
            content = state.sb.ToString();
            Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
            if (content.Contains("update"))
            {
                //String temp = "0397{\"traceNo\":\"0b78d2df-f5f8-45f0-ae42-2c4dfe30a9d6\",\"response\":{\"errorid\":0,\"errormsg\":\"\"},\"cmd\":\"update\",\"request\":{\"whichGrid\":\"GridB\",\"IsActivate\":false,\"IsNotActivate\":true,\"StageId\":\"IF1506-IF1505\",\"limit\":\"1\",\"lockNum\":\"0\",\"vol\":\"1\",\"kkjc\":\"-160.0\",\"kp\":\"170.0\",\"dkjc\":\"-201.0\",\"dp\":\"-1000.0\",\"t1cj\":\"2\",\"t1dd\":\"5\",\"t2cj\":\"3\",\"t2dd\":\"4\",\"t2cl\":\"1\",\"t2vol\":\"2\",\"cl\":\"2\",\"IsInDesignMode\":false}}";
                String temp = "0396{\"traceNo\":\"8b11649d-ada1-448b-82a2-b57d3a46ba45\",\"response\":{\"errorid\":0,\"errormsg\":\"\"},\"cmd\":\"update\",\"request\":{\"whichGrid\":\"GridA\",\"IsActivate\":false,\"IsNotActivate\":true,\"StageId\":\"ni1605-ni1601\",\"limit\":\"2\",\"lockNum\":\"0\",\"vol\":\"2\",\"kkjc\":\"1040.00\",\"kp\":\"930.00\",\"dkjc\":\"800.0\",\"dp\":\"850.0\",\"t1cj\":\"0\",\"t1dd\":\"2\",\"t2cj\":\"0\",\"t2dd\":\"0\",\"t2cl\":\"0\",\"t2vol\":\"5\",\"cl\":\"2\",\"IsInDesignMode\":false}}";

                byte[] data = Encoding.UTF8.GetBytes(temp);
                handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            else if (content.Contains("inform"))
            {
                String temp = "0587{\"cmd\":\"inform\",\"traceNo\":\"2ec197a7-392e-4e85-afa6-09ff2aecf95c\",\"request\":{\"whichGrid\":\"Grid01\",\"IsActivate\":false,\"IsNotActivate\":true,\"StageId\":\"Rb0000-ru0001\",\"limit\":\"5\",\"lockNum\":\"3\",\"vol\":\"1\",\"t1\":\"Rb0000\",\"t2\":\"ru0001\",\"incre\":\"1.0\",\"kkjc\":\"-120.0\",\"kp\":\"-170.0\",\"dkjc\":\"-900.0\",\"dp\":\"-201.0\",\"t1cj\":\"0\",\"t1dd\":\"0\",\"t2cj\":\"0\",\"t2dd\":\"0\",\"t2cl\":\"1\",\"t2vol\":\"2\",\"cl\":\"4\",\"autoCall\":\"Close\",\"jjkk\":\"555\",\"jjkp\":\"666\",\"jjdk\":\"777\",\"jjdp\":\"888\",\"t2Weight\":\"1\",\"t2Method\":\"0\",\"zdjc\":\"111\",\"zkjc\":\"222\",\"formatString\":\"F1\",\"IsInDesignMode\":false},\"response\":{\"errorid\":0,\"errormsg\":\"\"}}";

                byte[] data = Encoding.UTF8.GetBytes(temp);
                handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            else if (content.Contains("QueryInstrument"))
            {
                String temp = "0161{\"traceNo\":\"5902a541-4910-4cac-b31b-10d031c8e6f0\",\"response\":{\"errorid\":0,\"errormsg\":\"\",\"payload\":[\"IF1508\",\"IF1509\",\"IF1512\",\"IF1603\"]},\"cmd\":\"QueryInstrument\"}";
                byte[] data = Encoding.UTF8.GetBytes(temp);
                handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            else if (content.Contains("QueryPosition"))
            {

                byte[] data;
                String temp = "0911{\"traceNo\":\"58999ff5-6ac5-473f-a92f-f4d6b84e5f98\",\"response\":{\"errorid\":0,\"errormsg\":\"\",\"payload\":[{\"StageId\":7,\"T1\":\"al1603\",\"T2\":\"al1604\",\"Direction\":\"SELL\",\"RealPrice\":60,\"ExpPrice\":60,\"Volume\":1}],\"payload1\":[{\"name\":\"al1601\",\"cnt\":0},{\"name\":\"al1602\",\"cnt\":0},{\"name\":\"al1603\",\"cnt\":0},{\"name\":\"al1604\",\"cnt\":0},{\"name\":\"al1605\",\"cnt\":0},{\"name\":\"al1606\",\"cnt\":0},{\"name\":\"al1607\",\"cnt\":0},{\"name\":\"al1608\",\"cnt\":0},{\"name\":\"al1609\",\"cnt\":0},{\"name\":\"al1610\",\"cnt\":0},{\"name\":\"al1611\",\"cnt\":0},{\"name\":\"al1612\",\"cnt\":0},{\"name\":\"alefp\",\"cnt\":0},{\"name\":\"ni1601\",\"cnt\":0},{\"name\":\"ni1602\",\"cnt\":0},{\"name\":\"ni1603\",\"cnt\":0},{\"name\":\"ni1604\",\"cnt\":0},{\"name\":\"ni1605\",\"cnt\":0},{\"name\":\"ni1606\",\"cnt\":0},{\"name\":\"ni1607\",\"cnt\":0},{\"name\":\"ni1608\",\"cnt\":0},{\"name\":\"ni1609\",\"cnt\":0},{\"name\":\"ni1610\",\"cnt\":0},{\"name\":\"ni1611\",\"cnt\":0},{\"name\":\"ni1612\",\"cnt\":0},{\"name\":\"niefp\",\"cnt\":0}]},\"cmd\":\"QueryPosition\"}";
                String temp1 = "0811{\"traceNo\":\"58999ff5-6ac5-473f-a92f-f4d6b84e5f98\",\"response\":{\"errorid\":0,\"errormsg\":\"\",\"payload\":[],\"payload1\":[{\"name\":\"al1601\",\"cnt\":0},{\"name\":\"al1602\",\"cnt\":0},{\"name\":\"al1603\",\"cnt\":0},{\"name\":\"al1604\",\"cnt\":0},{\"name\":\"al1605\",\"cnt\":0},{\"name\":\"al1606\",\"cnt\":0},{\"name\":\"al1607\",\"cnt\":0},{\"name\":\"al1608\",\"cnt\":0},{\"name\":\"al1609\",\"cnt\":0},{\"name\":\"al1610\",\"cnt\":0},{\"name\":\"al1611\",\"cnt\":0},{\"name\":\"al1612\",\"cnt\":0},{\"name\":\"alefp\",\"cnt\":0},{\"name\":\"ni1601\",\"cnt\":0},{\"name\":\"ni1602\",\"cnt\":0},{\"name\":\"ni1603\",\"cnt\":0},{\"name\":\"ni1604\",\"cnt\":0},{\"name\":\"ni1605\",\"cnt\":0},{\"name\":\"ni1606\",\"cnt\":0},{\"name\":\"ni1607\",\"cnt\":0},{\"name\":\"ni1608\",\"cnt\":0},{\"name\":\"ni1609\",\"cnt\":0},{\"name\":\"ni1610\",\"cnt\":0},{\"name\":\"ni1611\",\"cnt\":0},{\"name\":\"ni1612\",\"cnt\":0},{\"name\":\"niefp\",\"cnt\":0}]},\"cmd\":\"QueryPosition\"}";
                if (!flag)
                {
                    data = Encoding.UTF8.GetBytes(temp);
                    flag = true;
                }
                else
                {
                    data = Encoding.UTF8.GetBytes(temp1);
                }
                handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            else
            {
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                handler.BeginSend(state.buffer, 0, bytesRead, 0, new AsyncCallback(SendCallback), handler);
            }
            //Send(handler, content);
            // Not all data received. Get more.     


        }
        else
        {
            handler.Close();
            //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
    }
    private static void Send(Socket handler, String data)
    {
        // Convert the string data to byte data using ASCII encoding.     
        byte[] byteData = Encoding.UTF8.GetBytes(data);
        // Begin sending the data to the remote device.     
        handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
    }
    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.     
            Socket handler = (Socket)ar.AsyncState;
            // Complete sending the data to the remote device.     
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}