using System;
using System.Collections.Generic;
using System.Text;

namespace sconnJTAG.Model
{
    public class JtagDataPacket : IJtagDataPacket
    {
        public byte[] Buffer { get; set; }
        public int Length { get; set; }
    }
}
