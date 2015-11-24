using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

#if WIN32_ENC
using System.IO.Ports;
#endif

namespace sconnConnector
{


    /****  USB connection class *****
     * 
     *  Multiple COM objects require this class has to be instantiated 
     * 
     * */

    #if WIN32_ENC

    public class USB
    {
        /* USB COMMANDS */
        const byte authCMD = 0x15;
        const byte authACK = 0x06;
        const byte mcuEOT = 0x03; //EOT
        const byte mcuWriteReg = 0x17;
        const byte mcuReadReg = 0x16;

        /* AUTH */
        public string username = "techniserv";
        public string password = "Domo11Domo";

        /* BUS Param */
        public string servername = "serv";
        public SerialPort port1;
        public bool connectedMCU = false;

        const int BaudDefaultValue = 9600;

        public byte[] mcuCFG;

        private string FindSconnPort()
        {
            string port = "";
            try
            {
                string[] ports =    SerialPort.GetPortNames();
                // string KeysLocation = "HKLM/SYSTEM/CurrentControlSet/Services/usber/";
                RegistryKey ComDriversL1 = Registry.LocalMachine.OpenSubKey("SYSTEM");
                RegistryKey ComDriversL2 = ComDriversL1.OpenSubKey("CurrentControlSet"); //.OpenSubKey("services").OpenSubKey("usber").OpenSubKey("Enum");
                RegistryKey ComDriversL3 = ComDriversL2.OpenSubKey("services"); //.OpenSubKey("usber").OpenSubKey("Enum");
                RegistryKey ComDriversL4 = ComDriversL3.OpenSubKey("usbser");
                RegistryKey ComDriversL5 = ComDriversL4.OpenSubKey("Enum");
                int drivers = (int) ComDriversL5.GetValue("Count");

                if (drivers > 1)
                {
                    //prompt for Port selection

                }
                else
                {
                    port = ComDriversL5.GetValue("0").ToString();
                }

            }
            catch (Exception)
            {
                
                throw;
            }
           
            return port;
        }

        public USB()
        {
            //default setup for USB connection
            //FindSconnPort()
            int baud = 115200;
            port1 = new SerialPort("COM18", baud); //BaudDefaultValue
            port1.ReadBufferSize = 64;
            port1.WriteBufferSize = 64;
            port1.Handshake = Handshake.XOnXOff;
          
           

        }

        public USB(SerialPort port)
        {
            port1 = port;
        }

        public USB(SerialPort port, string user, string pass)
        {
            username = user;
            password = pass;
            port1 = port;
        }



        public bool TestConnection()
        {
            try
            {
                port1.Open();
                bool txok = writeConsole("testpass");
                string resp = readConsole();
                port1.Close();
                return resp.Length > 0 ? true : false;
            }
            catch (Exception)
            {        
                throw;
            }


        }

        public void TransmitLoop()
        {
            port1.Handshake = Handshake.XOnXOff;
            port1.Open();
          
            while(true)
            {
                try
                {
                        port1.Write("test");
                        System.Threading.Thread.Sleep(100);
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

        }

        private bool writeConsole(string txt)
        {
            try
            {
                if (port1.IsOpen)
                {
                    port1.Write(txt);
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                
                throw;
            }

        }



        public string ReadUsbBlocking()
        {
            int bytes = 0;
            string bufferdata = "" ;
            port1.Handshake = Handshake.None;
            port1.Open();

            while (bytes == 0)
            {
                if (port1.IsOpen)
                {
                    try
                    {
                        bufferdata = port1.ReadExisting();
                        bytes = bufferdata.Length;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            
            }

            return bufferdata;
        }


        private string readConsole()
        {
            if (port1.IsOpen)
            {
                try
                {
                    string bufferdata = port1.ReadExisting();
                    return bufferdata;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            else return new string('E', 1);

        }

        public void usbSendByte(byte tbyte)
        {
            byte[] buff = new byte[1];
            buff[0] = tbyte;
            port1.Write(buff, 0, 1);
        }

        public byte usbReadByte()
        {
            //read 1 byte from USB buffer
            return (byte)port1.ReadByte();
        }


        /*PIC MCU config  functions */

        public bool authMCU(string passwd)
        {
            usbSendByte(authCMD);

            if (usbReadByte() == authACK) //mcu acked conn request
            {
                port1.Write(passwd);
                if (usbReadByte() == authACK)
                {
                    connectedMCU = true;
                    return true;
                }
                else return false;

            }
            else return false;
        }


        public bool sendMCUcfg(byte msector)
        {
            usbSendByte(mcuWriteReg);
            if (usbReadByte() == authACK) //mcu acked register write
            {
                for (int i = 0; i < mcuCFG.Length; i++)
                {
                    usbSendByte(mcuCFG[i]);
                }
                return true;
            }
            else return false;
        }



        public bool readMCUcfg(byte[] buffer)
        {
            usbSendByte(mcuReadReg);

            if (usbReadByte() == authACK) //mcu acked register read
            {
                int buffinc = 0;
                do
                {
                    buffer[buffinc] = (byte)port1.ReadByte();
                    buffinc++;
                }
                while (buffer[buffinc - 1] != mcuEOT);

                if (buffinc > 0) { return true; }
                else return false;
            }
            else { return false; }

        }

    }
#endif


}
