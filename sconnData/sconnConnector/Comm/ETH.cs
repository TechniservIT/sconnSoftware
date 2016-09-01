using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Net.Security;
using System.Collections;
using System.ComponentModel;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Runtime.CompilerServices;
using NLog;
using sconnConnector.Annotations;
using sconnNetworkingServices.Abstract;


namespace sconnConnector
{


/***** Ethernet ******/

    public class SconnClient
    {

        public Socket clientSocket { get; set; }
        public TcpClient client { get; set; }
        public SslStream EncStream;
        private Logger nlogger = LogManager.GetCurrentClassLogger();

        public SocketPermission clientPermission { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string AuthenticationPassword { get; set; }
        public IPAddress Adress { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public int ConnectionTimeoutMs { get; set; }
        public bool Authenticated = false;
        public bool useSsl { get; set; }

        public INetworkClientStatusUpdateService StatusUpdateService { get; set; }

        private UdpState _globalUdp;
        struct UdpState
        {
            public System.Net.IPEndPoint Ep;
            public System.Net.Sockets.UdpClient UdpClient;
        }


        public delegate void AsyncCallback(IAsyncResult ar);
        public event EventHandler<SiteDiscoveryEventArgs> SiteDiscovered;
        public event EventHandler<ConnectionStateEventArgs> ConnectionStateChanged;


        static bool CertHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
                {
                    // Ignore errors
                    return true;
                }


        public SconnClient(string hostname, int port, string password) 
        {
            Hostname = hostname;
            Port = port;
            AuthenticationPassword = password;
            ConnectionTimeoutMs = 2500;

            //SiteDiscovered = new EventHandler(OnSiteDiscovered());
         
            // StatusUpdateService = statusUpdateService;
            ServicePointManager.ServerCertificateValidationCallback += (s, c, k, e) => true;
        }

        private void InitializeConnection()
        {
            if (useSsl)
            {
                client = new TcpClient(Hostname, Port); 
                client.ReceiveTimeout = this.ConnectionTimeoutMs; 
                client.SendTimeout = this.ConnectionTimeoutMs;
                EncStream = new SslStream(
                    client.GetStream(),
                    false,
                    CertHandler,
                    null
                    );
                try
                {

                    this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Authentication));

                    EncStream.AuthenticateAsClient(Hostname, null,
                        System.Environment.OSVersion.Version.Major > 6 ? SslProtocols.Tls12  : SslProtocols.Ssl3, false);
                    this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Authenticated));

                    this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Connected));
                }
                catch (Exception e)
                {
                    this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Error));
                    NetworkClientStatusUpdateService.OnConnectionError(e.Message);
                   nlogger.ErrorException(e.Message, e);
                   
                }
            }
            else
            {

                Adress = IPAddress.Parse(Hostname);
                EndPoint = new IPEndPoint(Adress, Port);

                // Create one SocketPermission for socket access restrictions 
                clientPermission = new SocketPermission(
                    NetworkAccess.Connect,    // Connection permission 
                    TransportType.Tcp,        // Defines transport types 
                    "",                       // Gets the IP addresses 
                    SocketPermission.AllPorts // All ports 
                    );
                clientPermission.Demand();

                // Create one Socket object to setup Tcp connection 
                clientSocket = new Socket(
                    Adress.AddressFamily,// Specifies the addressing scheme 
                    SocketType.Stream,   // The type of socket  
                    ProtocolType.Tcp     // Specifies the protocols  
                    );
                clientSocket.NoDelay = false;
                clientSocket.ReceiveTimeout = ConnectionTimeoutMs;
            }
        }

        public SconnClient(string hostname, int port, string password, bool useSsl) : this(hostname,port,password)
        {
            this.useSsl = useSsl;
        }

        private bool ConnectAndAuthenticate()
        {
            try
            {
                if (useSsl)
                {
                    if (client == null)
                    {
                        InitializeConnection();
                    }
                }
                else
                {
                    if (clientSocket == null)
                    {
                        InitializeConnection();
                    }
                }
                
            }
            catch (Exception e)
            {
                this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Error));
                NetworkClientStatusUpdateService.OnConnectionError(e.Message);
                nlogger.ErrorException(e.Message, e);
                return false;
            }


            //compose auth msg
            byte[] authmsg = new byte[ipcDefines.AUTH_RECORD_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES];
            try
            {
                authmsg[0] = ipcCMD.SOP;
                for (int i = 0; i < AuthenticationPassword.Length; i++)
                {
                    authmsg[ipcDefines.AUTH_RECORD_PASSWD_POS + i + 1] = (byte)AuthenticationPassword[i];
                }
                authmsg[ipcDefines.AUTH_RECORD_PASS_LEN_POS + 1] = (byte)AuthenticationPassword.Length;
                authmsg[ipcDefines.AUTH_RECORD_SIZE + 1] = ipcCMD.EOP;
            }
            catch (Exception e)
            {
                this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Error));
                NetworkClientStatusUpdateService.OnConnectionError(e.Message);
                nlogger.ErrorException(e.Message, e);
                return false;
            }
            
            int len = authmsg.Length;
            byte[] txBF = new byte[len];
            byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
            for (int i = 0; i < len; i++)
            {
                txBF[i] = authmsg[i];
            }     

            if (useSsl)
            {
                try
                {
                    EncStream.Write(txBF);
                    EncStream.Flush();
                    int bytesRec = EncStream.Read(rxBF, 0, ipcDefines.NET_MAX_RX_SIZE);
                    if (rxBF[0] == ipcCMD.AUTHFAIL)
                    {
                        return false;
                    }
                    else if (rxBF[0] == ipcCMD.AUTHOK)
                    {
                        Authenticated = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    NetworkClientStatusUpdateService.OnConnectionError(e.Message);
                    nlogger.ErrorException(e.Message, e);
                    client.Close();
                    return false;
                }

            }
            else
            {
                if (!ETH.EndpointReachable(Hostname))
                {
                    return false;
                }
                try
                {
                    clientSocket.Connect(EndPoint);
                    //authenticate at remote
                   
                    int bytesSent = clientSocket.Send(txBF);
                    int bytesRec = clientSocket.Receive(rxBF);
                    if (rxBF[0] == ipcCMD.AUTHFAIL)
                    {
                        NetworkClientStatusUpdateService.OnConnectionClosed();
                        return false;
                    }
                    else if (rxBF[0] == ipcCMD.AUTHOK)
                    {
                        Authenticated = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                catch (Exception e)
                {
                    CloseSocket();
                    NetworkClientStatusUpdateService.OnConnectionError(e.Message);
                    nlogger.ErrorException(e.Message, e);
                    return false;
                }
            }
        }

        public void CloseSocket()
        {

            if (clientSocket != null)
            {
                if (clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                }
            }
        }

        public void VerifyConnection()
        {
            this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Authentication));
            if (useSsl)
            {
                if ( client == null)
                {
                    ConnectAndAuthenticate();
                }
                else
                {
                    if (!client.Connected)
                    {
                        ConnectAndAuthenticate();
                    }
                }
            }
            else
            {
                if ( clientSocket == null)
                {
                    ConnectAndAuthenticate();
                }
                if (!clientSocket.Connected)
                {
                    ConnectAndAuthenticate();
                }
            }
        }

        public bool Connect()
        {
            VerifyConnection();
            NetworkClientStatusUpdateService.OnConnectionOpened();
            return this.client.Connected;
        }

        public bool Disconnect()
        {
            if (useSsl)
            {
                if (client != null)
                {
                    EncStream.Close();
                    EncStream.Dispose();
                    client.Close();
                }
            }
            else
            {
                if (clientSocket != null)
                {
                   clientSocket.Close();
                }
            }
            NetworkClientStatusUpdateService.OnConnectionClosed();
            this.client = null;
            this.clientSocket = null;
            return true;
        }

        public byte[] berkeleySendMsg(byte[] sendBF)  //single packet send-recieve
        {
            // Receiving byte array  
            int len = sendBF.Length;
            byte[] txBF = new byte[len];
            byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
            for (int i = 0; i < len; i++)
            {
                txBF[i] = sendBF[i];
            }

            VerifyConnection();
            if (Authenticated)
            {
                if (useSsl)
                {  
                    try
                    {
                        EncStream.Write(txBF);
                        EncStream.Flush();
                        int bytesRec = EncStream.Read(rxBF, 0, ipcDefines.NET_MAX_RX_SIZE);
                        return rxBF;
                    }
                    catch (Exception e)
                    {
                        nlogger.ErrorException(e.Message, e);
                        return new byte[1];
                    }
                }
                else
                {
                    try
                    {
                        int bytesSent = clientSocket.Send(txBF);
                        int bytesRec = clientSocket.Receive(rxBF);
                        return rxBF;
                    }
                    catch (Exception e)
                    {
                        nlogger.Error(e.Message, e);
                        return new byte[1];
                    }
                }
                
            }
            else
            {
                return new byte[0];
            }

        }

        public byte[] berkeleySendMsg(byte[] sendBF, int len)  //single packet send-recieve up to len bytes
        {
             // Receiving byte array  
             byte[] txBF = new byte[len];
             byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
             for (int i = 0; i < len; i++)
             {
                   txBF[i] = sendBF[i];
             }

             VerifyConnection();

            if (Authenticated)
            {
                this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Authentication));
                if (useSsl)
                {
                    try
                    {
                        EncStream.Write(txBF);
                        EncStream.Flush();
                        int bytesRec = EncStream.Read(rxBF, 0, ipcDefines.NET_MAX_RX_SIZE);
                        return rxBF;
                    }
                    catch (Exception e)
                    {
                        this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Error));
                        nlogger.Error(e, e.Message);
                        return new byte[1];
                    }
                }
                else
                {
                    try
                    {
                        int bytesSent = clientSocket.Send(txBF);
                        int bytesRec = clientSocket.Receive(rxBF);
                        return rxBF;
                    }
                    catch (Exception e)
                    {
                        this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Error));
                        nlogger.Error(e, e.Message);
                        return new byte[1];
                    }
                }

            }
            else
            {
                return new byte[1];
            }

        }

        public void CloseConnection()
        {
            if (useSsl) //tcpclient
            {
                if (client == null) { return; }
                if (client.Connected)
                {
                    client.Close();
                }
                
            }
            else
            {
                if (clientSocket == null) { return; }
                if (clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                } //berkeley socket
            }
            this.OnConnectionStateChanged(new ConnectionStateEventArgs(NetworkConnectionState.Closing));
        }

        public void OnConnectionStateChanged(ConnectionStateEventArgs args)
        {
            EventHandler<ConnectionStateEventArgs> handler = ConnectionStateChanged;
            if (args != null)
            {
                handler?.Invoke(this, args);
            }
        }



        ~SconnClient()
        {
            CloseConnection();
        }





        /************** SEARCHING  UDP *************/
       

        public  void OnSiteDiscovered(SiteDiscoveryEventArgs args)
        {
            EventHandler<SiteDiscoveryEventArgs> handler = SiteDiscovered;
            if (args != null)
            {
                handler?.Invoke(this, args);
            }
        }



        public void SearchForSite()
        {
            try
            {
                // Try to send the discovery request message
                byte[] discoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");
                _globalUdp.UdpClient.Send(discoverMsg, discoverMsg.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.1.255"), 30303));
            }
            catch (Exception ex)
            {
                nlogger.Error(ex, ex.Message);
            }
        }

        public void ScanInit()
        {
            try
            {

                _globalUdp.UdpClient = new UdpClient();
                _globalUdp.Ep = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("255.255.255.255"), 30303);
                System.Net.IPEndPoint bindEp = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 30303);
                byte[] discoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");

                // Set the local UDP port to listen on
                _globalUdp.UdpClient.Client.Bind(bindEp);

                // Enable the transmission of broadcast packets without having them be received by ourself
                _globalUdp.UdpClient.EnableBroadcast = true;
                _globalUdp.UdpClient.MulticastLoopback = false;

                // Configure ourself to receive discovery responses
                _globalUdp.UdpClient.BeginReceive(ReceiveCallback, _globalUdp);
            }
            catch (Exception ex)
            {
                nlogger.Error(ex, ex.Message);
            }
        }
        
        public void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpState myUdp = (UdpState)ar.AsyncState;

                // Obtain the UDP message body and convert it to a string, with remote IP address attached as well
                string receiveString = Encoding.ASCII.GetString(myUdp.UdpClient.EndReceive(ar, ref myUdp.Ep));
                receiveString = myUdp.Ep.Address.ToString() + "\n" + receiveString.Replace("\r\n", "\n");

                // Configure the UdpClient class to accept more messages, if they arrive
                myUdp.UdpClient.BeginReceive(ReceiveCallback, myUdp);

                string[] ePinfo = receiveString.Split('\n');
                string remoteIp = ePinfo[0];
                string remoteHostname = ePinfo[1];


                //is not internal address
                if ((from netif in NetworkInterface.GetAllNetworkInterfaces()
                     select netif.GetIPProperties() into prop
                     from item in prop.UnicastAddresses
                     where !(item.Address.IsIPv6LinkLocal || item.Address.IsIPv6SiteLocal)
                     select item)
                     .Any(item => item.Address.ToString().Equals(remoteIp)))
                {
                    return;
                }

             

                this.OnSiteDiscovered(new SiteDiscoveryEventArgs(remoteIp));
            }
            catch (Exception ex)
            {
                nlogger.Error(ex, ex.Message);
            }
          
        }




    }

    public class SiteDiscoveryEventArgs : EventArgs
    {
        public string hostname;

        public SiteDiscoveryEventArgs(string remote)
        {
            hostname = remote;
        }
    }

    public class ConnectionStateEventArgs : EventArgs
    {
        public NetworkConnectionState State;

        public ConnectionStateEventArgs(NetworkConnectionState state)
        {
            State = state;
        }
    }




    public class SiteConnectionStat : INotifyPropertyChanged
    {
        /*
         Connection statistics - session variable
         */
        private bool _Authenticated;

        public bool Authenticated
        {
            get { return _Authenticated; }
            set
            {
                _Authenticated = value;
                OnPropertyChanged();
            }
        }

        public bool Connected { get; set; }

        private int _FailedConnections;
        private int _AverageResponseTimeMs;
        private int _ConnectionElapsed;
        private Stopwatch _ConnectionTimer;
        private Logger nlogger = LogManager.GetCurrentClassLogger();


        private int[] ResponsesTimeMs;
        private int _Responses;
        const int MaxResponsesBuffered = 100;

        public int FailedConnections 
        {
            get
            {
                return _FailedConnections;
            }
            set
            {
                if (value > 0)
                {
                    _FailedConnections = value;
                }
            }
        }
        public string FailedConnectionsText => _FailedConnections.ToString();

        public int AverageResponseTimeMs
        {
            get
            {
                return _AverageResponseTimeMs;
            }
            set
            {
                if (value > 0)
                {
                    _AverageResponseTimeMs = value;
                }
            }
        }
        public string AverageResponseTimeMsText => _AverageResponseTimeMs.ToString();


        public int ConnectionElapsed
        {
            get
            {
                return _ConnectionElapsed;
            }
            set
            {
                if (value > 0)
                {
                    _ConnectionElapsed = value;
                }
            }
        }

        public SiteConnectionStat()
        {
            ResetStatistics();
        }
        
        public void ResetStatistics()
        {
            _FailedConnections = 0;
            _ConnectionElapsed = 0;
            _AverageResponseTimeMs = 0;
            _Responses = 0;
            _ConnectionTimer = new Stopwatch();
            ResponsesTimeMs = new int[MaxResponsesBuffered];
        }

        public void StartConnectionTimer()
        {
            _ConnectionTimer.Start();
        }

        public void StopConnectionTimer()
        {
            _ConnectionTimer.Stop();
            ResponsesTimeMs[_Responses] = _ConnectionTimer.Elapsed.Milliseconds;
            _ConnectionElapsed += _ConnectionTimer.Elapsed.Milliseconds;
            _Responses++;
            CalculateAvgResponseTime();
        }

        private void CalculateAvgResponseTime()
        {
            try
            {
                if (_Responses >= MaxResponsesBuffered)
                {
                    _Responses = 0; //reset response count and overwrite from beggining
                }
                long totalResponsesTime = 0;
                for (int i = 0; i < _Responses; i++)
                {
                    totalResponsesTime += ResponsesTimeMs[i];
                }
                _AverageResponseTimeMs = (int)totalResponsesTime / _Responses;
            }
            catch (Exception e)
            {
                nlogger.Error(e, e.Message);
            }

        }

        public TimeSpan GetTimeSpanElapsed()
        {
            try
            {
                return TimeSpan.FromMilliseconds(_ConnectionElapsed);
            }
            catch (Exception e)
            {
                nlogger.Error(e, e.Message);
                throw;
            }            
        }

        public string GetShortTimeElapsed()
        {
            try
            {
                return TimeSpan.FromMilliseconds(_ConnectionElapsed).ToString("g");
            }
            catch (Exception e)
            {
                nlogger.Error(e, e.Message);
                throw;
            }                   
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class ETH
    {
        /*All  Ethernet traffic is routed over existing interfaces, so this class is static */

        /* AUTH */

        /*  COMMANDS  */

        /*  GLOBAL   */

        //private Logger nlogger = LogManager.GetCurrentClassLogger();


        public ETH()
        {

        }


        public bool SSLTestConnection()
        {
            bool connected = false;

            return connected;
        }

        public ETH(int ConnectionTimeoutMs)
        {
            this.ConnectionTimeoutMs = ConnectionTimeoutMs;
        }


        int ConnectionTimeoutMs = 2500;

        // Incoming data from the client.
        public string soxData = null;


         static public bool EndpointReachable(string hostname)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            options.Ttl = 100;

            string data = "PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 150; 
            PingReply reply = pingSender.Send(hostname, timeout, buffer, options);
              if (reply.Status == IPStatus.Success)
              {
                  return true;
              }
              else
              {
                  return false;
              }
        }

        public string tcpSend(String server, String message, Int32 port)
        {
            try
            {
                TcpClient client = new TcpClient(server, port);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);  // Send the message to the connected TcpServer. 
                data = new Byte[256];  // Buffer to store the response bytes.
                String responseData = String.Empty; // String to store the response ASCII representation.
                Int32 bytes = stream.Read(data, 0, data.Length);   // Read the first batch of the TcpServer response bytes.
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                //nlogger.ErrorException(e.Message, e);
                return "E"; //error
            }
            catch (SocketException e)
            {
                //nlogger.ErrorException(e.Message, e);
                return "E"; //error
            }
        }


        public byte[] berkeleySendMsg(String server, byte[] sendBF, Int32 port)  //single packet send-recieve
        {
            if (!EndpointReachable(server))
            {
                return new byte[1];
            }

            // Receiving byte array  
            byte[] txBF = new byte[sendBF.Length];
            byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
            try
            {
                // Create one SocketPermission for socket access restrictions 
                SocketPermission permission = new SocketPermission(
                    NetworkAccess.Connect,    // Connection permission 
                    TransportType.Tcp,        // Defines transport types 
                    "",                       // Gets the IP addresses 
                    SocketPermission.AllPorts // All ports 
                    );
                permission.Demand();

                IPAddress ipAddr = IPAddress.Parse(server);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

                // Create one Socket object to setup Tcp connection 
                Socket sender = new Socket(
                    ipAddr.AddressFamily,// Specifies the addressing scheme 
                    SocketType.Stream,   // The type of socket  
                    ProtocolType.Tcp     // Specifies the protocols  
                    );
                sender.NoDelay = false;
                sendBF.CopyTo(txBF, 0);
                sender.ReceiveTimeout = ConnectionTimeoutMs;
                sender.Connect(ipEndPoint);
                             
                int bytesSent = sender.Send(txBF);
                int bytesRec = sender.Receive(rxBF);
                
             
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                return rxBF;
            }
            catch (Exception e)
            {
                //nlogger.ErrorException(e.Message, e);
                return new byte[1]; //error
            }

        }


        public byte[] berkeleySendMsg(String server, byte[] sendBF, Int32 port, int len)  //single packet send-recieve
        {
            if (!EndpointReachable(server))
            {
                return new byte[1];
            }

            // Receiving byte array  
            byte[] txBF = new byte[len];
            byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
            try
            {
                // Create one SocketPermission for socket access restrictions 
                SocketPermission permission = new SocketPermission(
                    NetworkAccess.Connect,    // Connection permission 
                    TransportType.Tcp,        // Defines transport types 
                    "",                       // Gets the IP addresses 
                    SocketPermission.AllPorts // All ports 
                    );
                permission.Demand();

                IPAddress ipAddr = IPAddress.Parse(server);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

                // Create one Socket object to setup Tcp connection 
                Socket sender = new Socket(
                    ipAddr.AddressFamily,// Specifies the addressing scheme 
                    SocketType.Stream,   // The type of socket  
                    ProtocolType.Tcp     // Specifies the protocols  
                    );
                sender.NoDelay = false;
              //  sendBF.CopyTo(txBF, 0);
                for (int i = 0; i < len; i++)
                {
                    txBF[i] = sendBF[i];
                }
                sender.ReceiveTimeout = ConnectionTimeoutMs;
                sender.Connect(ipEndPoint);

              //  System.Threading.Thread.Sleep(700);     //wait some before sending for session test
                int bytesSent = sender.Send(txBF);
                int bytesRec = sender.Receive(rxBF);

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                return rxBF;
            }
            catch (Exception e)
            {
                //nlogger.ErrorException(e.Message, e);
                return new byte[1]; //error
            }

        }


        public byte[] berkeleyReadLen(String server, byte[] sendBF, Int32 port, int len)
        {
            // Receiving byte array  
            byte[] txBF = new byte[32];
            byte[] respBF = new byte[len];
            byte[] rxBF = new byte[32];
            try
            {
                // Create one SocketPermission for socket access restrictions 
                SocketPermission permission = new SocketPermission(
                    NetworkAccess.Connect,    // Connection permission 
                    TransportType.Tcp,        // Defines transport types 
                    "",                       // Gets the IP addresses 
                    SocketPermission.AllPorts // All ports 
                    );
                permission.Demand();

                IPAddress ipAddr = IPAddress.Parse(server);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

                // Create one Socket object to setup Tcp connection 
                Socket sender = new Socket(
                    ipAddr.AddressFamily,// Specifies the addressing scheme 
                    SocketType.Stream,   // The type of socket  
                    ProtocolType.Tcp     // Specifies the protocols  
                    );
                sender.SendBufferSize = 32;
                sender.ReceiveBufferSize = 32;
                sender.ReceiveTimeout = ConnectionTimeoutMs;

                int totalRecieved = 0;
                int bytesRec = 0;
                int maxRetryCount = 10;
                int retries = 0;

                sender.NoDelay = false;
                sender.Connect(ipEndPoint);
                sendBF.CopyTo(txBF, 0);
              
                    
                do
                {
                     int bytesSend = sender.Send(txBF);                    
                     bytesRec = sender.Receive(rxBF);
                     if ( rxBF[0] == ipcCMD.SVAL ) //read first packet
                     {
                         retries = 0;
                            for (int i = 0; i < bytesRec; i++)
                            {
                                if (totalRecieved + i < len)    //read only if all data has not been read already
                                {
                                    respBF[totalRecieved + i] = rxBF[i];                                  
                                }
                            }
                            totalRecieved +=bytesRec;
                            if ( totalRecieved >= len) //next packet is last
                            {
                                    txBF[1] = ipcCMD.ACKFIN; //finish download
                            }                           

                    }
                    else
                     {
                         retries++;
                     }
                } while (retries < maxRetryCount);

                    
                    txBF[0] = ipcCMD.ACK;
                    txBF[1] = ipcCMD.ACKNXT;
                    retries = 0;
                    do
                    {
                        sender.Send(txBF); //ack packet
                        bytesRec = sender.Receive(rxBF);                        
                        if (bytesRec > 0) //parse
                        {
                            retries = 0; //packet recieve, reset retry
                            for (int i = 0; i < bytesRec; i++)
                            {
                                if (totalRecieved + i < len)
                                {
                                    respBF[totalRecieved + i] = rxBF[i];
                                }
                            }
                            totalRecieved +=bytesRec;
                            if ( totalRecieved >= len) //next packet is last
                            {
                                    txBF[1] = ipcCMD.ACKFIN; //finish download
                            }    
                            
                        }
                        else
                        {
                            //retry
                            retries++;

                        }
                    } while ( (retries < maxRetryCount) && (totalRecieved < len) );

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                return respBF;
            }
            catch (Exception e)
            {
                //nlogger.ErrorException(e.Message, e);
                return new byte[1]; //error
            }

        }



        private class ethClient
        {
            public macADDR mac;
            public int ip;
            public string paswd;
            public int elapsed;

            public ethClient()
            {
                mac = new macADDR(0x00, 0x00, 0x00);
                ip = BitConverter.ToInt32(IPAddress.Parse("127.0.0.1").GetAddressBytes(), 0);
                paswd = "Domo11Domo";
                elapsed = 3600;
            }


            public ethClient(macADDR macADDR, int ipAddr, string paswdStr, int elapsedTime)
            {
                mac = macADDR;
                ip = ipAddr;
                paswd = paswdStr;
                elapsed = elapsedTime;
            }

            public static bool operator ==(ethClient c1, ethClient c2)
            {
                if ((c1.mac == c2.mac) && (c1.paswd == c2.paswd))
                { return true; }
                else { return false; }
            }

            public static bool operator !=(ethClient c1, ethClient c2)
            {
                if ((c1.mac != c2.mac) || (c1.paswd != c2.paswd))
                { return true; }
                else { return false; }
            }
            public override int GetHashCode()
            {
                return mac.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                    return base.Equals(obj);
            }


        }


    }



}//namespace
