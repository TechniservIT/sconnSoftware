using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnNetworkingServices.Abstract
{
    public enum NetworkConnectionState
    {
        Connecting,
        Negotiating,
        Authentication,
        Authenticated,
        Connected,
        Transmit,
        Recieve,
        Idle,
        Closing,
        Error

    }

    public  interface INetworkClientStatusUpdateService
    {
        void OnConnectionStateChanged();
        event EventHandler ConnectionStateChanged;

        void OnConnectionClosed();
        event EventHandler ConnectionClosed;

        void OnConnectionOpened();
        event EventHandler ConnectionOpened;

        void OnConnectionError(string message);
        event EventHandler ConnectionError;

        NetworkConnectionState State { get; set; }
    }

}
