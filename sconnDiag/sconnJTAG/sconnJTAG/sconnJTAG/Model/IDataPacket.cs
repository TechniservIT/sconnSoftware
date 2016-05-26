using System;
using System.Collections.Generic;
using System.Text;

namespace sconnJTAG.Model
{
    public interface IDataPacket
    {
        byte[] Buffer { get; set; }
        int Length { get; set; }
    }
}
