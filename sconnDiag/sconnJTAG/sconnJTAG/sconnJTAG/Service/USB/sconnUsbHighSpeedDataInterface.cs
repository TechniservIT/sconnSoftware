using System;
using System.Collections.Generic;
using System.Text;
using sconnJTAG.Model;
using sconnJTAG.Service.Abstract;

namespace sconnJTAG.Service.USB
{
    public  class sconnUsbHighSpeedDataInterface : IUsbCommandService
    {
        public void Transmit(IDataPacket packet)
        {
            throw new NotImplementedException();
        }

        public IDataPacket Recieve()
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool Connected { get; set; }
    }
}
