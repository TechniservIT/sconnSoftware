using System;
using System.Collections.Generic;
using System.Text;
using sconnJTAG.Model;

namespace sconnJTAG.Service.Abstract
{
    public interface IUsbCommandService
    {

        void Transmit(IDataPacket packet);
        IDataPacket Recieve();
        bool Connect();

        bool Connected { get; set; }
    }
}
