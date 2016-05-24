using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnNetworkingServices.Abstract
{
    public static class NetworkClientStatusUpdateService //: INetworkClientStatusUpdateService
    {
        public static void OnConnectionStateChanged()
        {
           
        }

        public static event EventHandler ConnectionStateChanged;


        public static void OnConnectionClosed()
        {
          
        }

        public static event EventHandler ConnectionClosed;


        public static void OnConnectionOpened()
        {
           
        }

        public static event EventHandler ConnectionOpened;


        public static void OnConnectionError(string message)
        {
           
        }

        public static event EventHandler ConnectionError;


        public static  NetworkConnectionState State { get; set; }
    }
}
