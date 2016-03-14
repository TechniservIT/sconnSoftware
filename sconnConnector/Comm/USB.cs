using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using sconnConnector.POCO.Config;
using NLog;

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
        public SerialPort client;
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
            catch (Exception e)
            {
                
                throw;
            }
           
            return port;
        }

        private byte[] berkeleySendMsg(byte[] msg, int bytes)
        {
            return UsbComm_Trx_Transaction(msg,bytes);
        }

        public USB()
        {
            //default setup for USB connection
            //FindSconnPort()

            int baud = 9600;
            client = new SerialPort("COM4", baud); //BaudDefaultValue
            client.ReadBufferSize = 32;
            client.WriteBufferSize = 32;
            client.Handshake = Handshake.XOnXOff;
            client.ReadTimeout = 400;
            client.WriteTimeout = 400;
           

        }

        public USB(SerialPort port)
        {
            client = port;
        }

        public USB(SerialPort port, string user, string pass)
        {
            username = user;
            password = pass;
            client = port;
        }



        public bool TestConnection()
        {
            try
            {
                byte[] testArr = new byte[1100];
                testArr[0] = (byte)(0x9900 >> 8);
                testArr[1] = (byte) (0x9900 & 0x00FF );

                client.Open();
                bool txok = Console_Write_Bytes(testArr,4);
                byte[] resp = Usb_Read_Bytes();

                client.Close();
                return resp.Length > 0 ? true : false;
            }
            catch (Exception e)
            {        
                throw;
            }


        }

        static int RxBytesPending;
        static int CurrentMessageLen;
        static byte[] Usb_MessageBuffer = new byte[1100];
        static byte[] USB_Out_Buffer = new byte[1100];


        static public int USB_CMD_NET_TX_START = 0x0100;
        static public int USB_CMD_NET_TX_PUSH = 0x0101;
        static public int USB_CMD_NET_TX_FIN = 0x0102;
        static public int USB_CMD_NET_TX_HALT = 0x0103;
        static public int USB_CMD_NET_TX_END = 0x0104;

        static public int USB_CMD_NET_RX_START = 0x0110;
        static public int USB_CMD_NET_RX_PUSH = 0x0111;
        static public int USB_CMD_NET_RX_FIN = 0x0112;
        static public int USB_CMD_NET_RX_HALT = 0x0113;
        static public int USB_CMD_NET_RX_END = 0x0114;

        static public int USB_CMD_BOOT_TX_START = 0x0200;
        static public int USB_CMD_BOOT_RX_START = 0x0210;
        static public int USB_CMD_DIAG_RX_START = 0x0300;
        static public int USB_CMD_COM_TEST = 0x9900;
        
        static public int USB_PACKET_CMD_POS = 0;  
        static public int USB_PACKET_CMD_LEN = 2;
        
        static public int USB_PACKET_CMD_TX_START_LEN_POS = (USB_PACKET_CMD_POS + USB_PACKET_CMD_LEN);
        static public int USB_PACKET_CMD_TX_START_LEN_SIZE = 0x0002;
        static public int USB_PACKET_CMD_TX_START_LEN = (USB_PACKET_CMD_TX_START_LEN_POS + USB_PACKET_CMD_TX_START_LEN_SIZE);

        static public int USB_PACKET_CMD_TX_PUSH_DATA_POS = (USB_PACKET_CMD_POS + USB_PACKET_CMD_LEN);
        static public int USB_PACKET_MIN_LEN = USB_PACKET_CMD_TX_START_LEN;

        static public int USB_PACKET_DATA_HEADER_SIZE = 2;
        static public int USB_EP0_BUFF_SIZE = 32;

        static public int USB_PACKET_DATA_SIZE = (USB_EP0_BUFF_SIZE- USB_PACKET_DATA_HEADER_SIZE);


        public short WordFromBufferAtPossition(byte[] buffer, int pos)
        {
            short val = 0;
            val = buffer[pos+1];
            val |= (short)(buffer[pos] << 8);
            return val;
        }

        public void UsbComm_Clear_MessageBuffers()
        {
            Usb_MessageBuffer = new byte[1100];
            USB_Out_Buffer = new byte[1100];
        }

        public void UsbComm_Transmit_Message(byte[] message, int len)
        {

        }

        public bool UsbComm_Sample_Trx()
        {
            byte[] trxMsg = new byte[2];
            trxMsg[0]= ipcCMD.GET;
            trxMsg[1] = ipcCMD.getDevNo;
            return UsbComm_Trx_Transaction(trxMsg).Length != 0;
        }

        public void UsbComm_Test_Trx()
        {
            client.Open();
            byte[] trxMsg = new byte[2];
            byte[] trxResp = new byte[254];
            int trxRespInc = 0;
            int pcktRx = 0;

            for (int i = 0; i < 254; i++)
            {
                trxMsg[0] = (byte)i;
                client.Write(trxMsg, 0, 1);
                do
                {
                    pcktRx = client.Read(trxResp, trxRespInc, 1);
                } while (pcktRx == 0);
                trxRespInc++;
                pcktRx = 0;
            }

            client.Close();
        }

        public byte[] UsbComm_Trx_Transaction(byte[] message, int length)
        {
            try
            {

                client.WriteTimeout = 500;
                client.ReadTimeout = 120000;
                client.ReadBufferSize = 2048;
                client.WriteBufferSize = 2048;

                client.Open();
                byte[] Trx_Response = new byte[1100];
                int TrxRecieved = 0;
                byte[] Rx_Buffer = new byte[1100];
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];
                int len = length;   // message.Length;


                //transmit
                int txLeft = len;
                int txPos = 0;
                int timeout = 0;

                //send start packet
                packetBuffer[0] = (byte)(USB_CMD_NET_TX_START >> 8);
                packetBuffer[1] = (byte)USB_CMD_NET_TX_START;
                packetBuffer[2] = (byte)(len >> 8);
                packetBuffer[3] = (byte)(len & 0xFF);
                client.Write(packetBuffer, 0, 4);

                while (txLeft > 0 && !(timeout > 100))
                {

                    if (client.IsOpen)
                    {
                        int txbinc = 0;
                        if (txLeft > USB_EP0_BUFF_SIZE)
                        {
                            txbinc = USB_EP0_BUFF_SIZE - USB_PACKET_DATA_HEADER_SIZE;
                        }
                        else
                        {
                            txbinc = txLeft;    // - USB_PACKET_DATA_HEADER_SIZE;
                        }
                        //append response with header
                        packetBuffer[0] = (byte)(USB_CMD_NET_TX_PUSH >> 8);
                        packetBuffer[1] = (byte)USB_CMD_NET_TX_PUSH;
                        //  memcpy(&packetBuffer[USB_PACKET_DATA_HEADER_SIZE], &message[txPos], txbinc);
                        for (int i = 0; i < txbinc; i++)
                        {
                            packetBuffer[USB_PACKET_DATA_HEADER_SIZE + i] = message[txPos + i];
                        }
                        client.Write(packetBuffer, 0, txbinc + USB_PACKET_DATA_HEADER_SIZE);
                        txPos += txbinc;
                        txLeft -= txbinc;
                    }
                    timeout++;
                }

                client.DiscardInBuffer();

                //recieve
                int Rx_Data_Total_Left = 0;
                int rxLeft = 4;
                int rxCt = 0;
                //rx start packet
                //System.Threading.Thread.Sleep(150); //settling

                while (rxLeft > 0 && !(timeout > 100))
                {


                    //read the resp
                    int bread = client.Read(Rx_Buffer, rxCt, rxLeft);
                    rxCt += bread;
                    if (rxCt == 4)
                    {
                        short cmd = WordFromBufferAtPossition(Rx_Buffer, 0);
                        if (cmd == USB_CMD_NET_RX_START)
                        {
                            Rx_Data_Total_Left =  WordFromBufferAtPossition(Rx_Buffer, 2);
                            client.DiscardInBuffer();
                            rxLeft = 0;
                        }
                    }
                    else
                    {
                        rxLeft -= bread;
                    }
                }

                int ReadOffset = rxCt;
                rxCt = 0;
                Rx_Buffer = new byte[1100];
                rxLeft = Rx_Data_Total_Left >= USB_PACKET_DATA_SIZE ? USB_EP0_BUFF_SIZE : (Rx_Data_Total_Left + USB_PACKET_DATA_HEADER_SIZE);
                int btoRead = rxLeft;
                client.DiscardInBuffer();

                while (rxLeft > 0 && !(timeout > 100))
                {
                    if (client.IsOpen)
                    {
                        //send read cmd
                        packetBuffer[0] = (byte)(USB_CMD_NET_RX_PUSH >> 8);
                        packetBuffer[1] = (byte)USB_CMD_NET_RX_PUSH;
                        client.Write(packetBuffer, 0, 2);

                        int pcktRx = client.Read(Rx_Buffer, rxCt, rxLeft);
                        rxCt += pcktRx;
                        if (rxCt == btoRead)
                        {
                            //strip header and copy msg
                            for (int i = USB_PACKET_DATA_HEADER_SIZE; i < (pcktRx); i++)
                            {
                                Trx_Response[TrxRecieved] = Rx_Buffer[i];
                                TrxRecieved++;  // (pcktRx - USB_PACKET_DATA_HEADER_SIZE);
                               // rxLeft--;
                            }
                        }
                        else
                        {
                            rxLeft -= pcktRx;
                        }
                    }
                    timeout++;
                }

                client.Close();
                return Trx_Response;
            }
            catch (Exception e)
            {
                client.Close();
                return new byte[0];
            }
      
        }



        public void UsbComm_Handle_Message(byte[] message)
        {
            int numBytesRead = message.Length;  // getsUSBUSART(USB_Out_Buffer, USB_EP0_BUFF_SIZE);
            if (numBytesRead >= USB_PACKET_CMD_LEN)
            {
                short currCmd = WordFromBufferAtPossition(USB_Out_Buffer, USB_PACKET_CMD_POS);
                if (currCmd == USB_CMD_NET_TX_START)
                {
                    //read bytes that will be sent
                    RxBytesPending = WordFromBufferAtPossition(USB_Out_Buffer, USB_PACKET_CMD_TX_START_LEN_POS);
                }
                else if (currCmd == USB_CMD_NET_TX_PUSH)
                {
                    short binc = 0;
                    for (binc = 0; binc < numBytesRead - USB_PACKET_CMD_LEN; binc++)
                    {
                        if (RxBytesPending > 0)
                        {
                            Usb_MessageBuffer[CurrentMessageLen] = USB_Out_Buffer[USB_PACKET_CMD_TX_PUSH_DATA_POS + binc];
                            CurrentMessageLen++;
                            RxBytesPending--;
                        }
                    }

                    if (RxBytesPending == 0)
                    {
                        //message recieved
                        //  int btx = TcpEncServer_Handle_Usb_Packet(Usb_MessageBuffer, CurrentMessageLen);  //pass message to net handler
                        //  UsbComm_Transmit_Message(Usb_MessageBuffer, btx);
                        UsbComm_Clear_MessageBuffers();
                    }
                }
                else if (currCmd == USB_CMD_NET_TX_FIN)
                {
                    UsbComm_Clear_MessageBuffers();
                }
                else if (currCmd == USB_CMD_NET_TX_HALT)
                {
                    UsbComm_Clear_MessageBuffers();
                }
                else if (currCmd == USB_CMD_NET_TX_END)
                {
                    UsbComm_Clear_MessageBuffers();
                }
                else if (currCmd == USB_CMD_COM_TEST)
                {
                    Usb_MessageBuffer[0] = 0xFF;
                    Usb_MessageBuffer[1] = 0x11;
                    UsbComm_Transmit_Message(Usb_MessageBuffer, 2);
                }
                else {
                    Usb_MessageBuffer[0] = 0xFF;
                    Usb_MessageBuffer[1] = 0xFF;
                    UsbComm_Transmit_Message(Usb_MessageBuffer, 2);
                }

            }
        }

        public void TransmitLoop()
        {
            client.Handshake = Handshake.XOnXOff;
            client.Open();
          
            while(true)
            {
                try
                {
                        client.Write("test");
                        System.Threading.Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    
                    throw;
                }
            }

        }

        private bool Console_Write_Bytes(byte[] bytes)
        {
            try
            {
                if (client.IsOpen)
                {
                    client.Write(bytes,0,bytes.Length);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private bool Console_Write_Bytes(byte[] bytes, int len)
        {
            try
            {
                if (client.IsOpen)
                {
                    client.Write(bytes, 0, len);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private bool writeConsole(string txt)
        {
            try
            {
                if (client.IsOpen)
                {
                    client.Write(txt);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                
                throw;
            }

        }



        public string ReadUsbBlocking()
        {
            int bytes = 0;
            string bufferdata = "" ;
            client.Handshake = Handshake.None;
            client.Open();

            while (bytes == 0)
            {
                if (client.IsOpen)
                {
                    try
                    {
                        bufferdata = client.ReadExisting();
                        bytes = bufferdata.Length;
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                }
            
            }

            return bufferdata;
        }

        private byte[] Usb_Read_Bytes()
        {
            if (client.IsOpen)
            {
                try
                {
                    byte[] bufferdata = new byte[32];
                    int bread = client.Read(bufferdata,0, client.BytesToRead % 32);
                    return bufferdata;
                }
                catch (Exception e)
                {
                    throw;
                }

            }
            else
            {
                return new byte[0];
            }
        }

        private string readConsole()
        {
            if (client.IsOpen)
            {
                try
                {
                    string bufferdata = client.ReadExisting();
                    return bufferdata;
                }
                catch (Exception e)
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
            client.Write(buff, 0, 1);
        }

        public byte usbReadByte()
        {
            //read 1 byte from USB buffer
            return (byte)client.ReadByte();
        }


        /*PIC MCU config  functions */

        public bool authMCU(string passwd)
        {
            usbSendByte(authCMD);

            if (usbReadByte() == authACK) //mcu acked conn request
            {
                client.Write(passwd);
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
                    buffer[buffinc] = (byte)client.ReadByte();
                    buffinc++;
                }
                while (buffer[buffinc - 1] != mcuEOT);

                if (buffinc > 0) { return true; }
                else return false;
            }
            else { return false; }

        }



        /************** TRX *******************************************/

        private bool Trx_Com_Usb;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /********   Program config    ********/
        /********                      *******/
        /***     Handles Data save/read     ***/
        private sconnDataSrc ConfigSource = new sconnDataSrc();

        public bool authMCU(String server, Int32 port, String cred)
        {

            try
            {
                byte[] authmsg = new byte[ipcDefines.AUTH_RECORD_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES];
                authmsg[0] = ipcCMD.SOP;
                for (int i = 0; i < cred.Length; i++)
                {
                    authmsg[ipcDefines.AUTH_RECORD_PASSWD_POS + i + 1] = (byte)cred[i];
                }
                authmsg[ipcDefines.AUTH_RECORD_PASS_LEN_POS + 1] = (byte)cred.Length;
                authmsg[ipcDefines.AUTH_RECORD_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES - 1] = ipcCMD.EOP;
                byte[] status = berkeleySendMsg(server, authmsg, port);

                if (status[0] == ipcCMD.AUTHFAIL)
                {
                    return false;
                }
                else if (status[0] == ipcCMD.AUTHOK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }


        }

        public void saveConfig()
        {
            ConfigSource.SaveConfig(DataSourceType.xml);
        }

        public void loadConfig()
        {
            ConfigSource.LoadConfig(DataSourceType.xml);
        }

        public int getSiteDeviceNo(sconnSite site)
        {
            try
            {
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
                int sites = 0;
                byte[] cmd = new byte[ipcDefines.NET_CMD_PACKET_LEN];
                cmd[0] = ipcCMD.GET;
                cmd[1] = ipcCMD.getDevNo;
                byte[] resp = berkeleySendMsg(cmd); //ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
                if (resp[0] == ipcCMD.SVAL)
                {
                    sites = (int)resp[2]; // second byte is data,  <SVAL> <DATA> <EVAL>                  
                }
                client.CloseConnection();
                return sites;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return 0;
            }

        }


        public bool WriteGlobalNamesCfg(sconnSite site)
        {
            try
            {
                if (site.siteCfg == null) { return false; }
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

                site.siteStat.StartConnectionTimer();
                try
                {
                    int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                    int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES;
                    int bytesToSend = ipcDefines.RAM_NAMES_Global_Total_Size;

                    // Receiving byte array  
                    byte[] txBF = new byte[bfSize];
                    byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];

                    txBF[0] = ipcCMD.SET;
                    txBF[1] = ipcCMD.setGlobalNames;
                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.RAM_NAMES_Global_Total_Size / packetData;
                        int signleBytes = (int)ipcDefines.RAM_NAMES_Global_Total_Size % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;
                        int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : bytesToSend + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;
                        for (int j = 0; j < fullTxNo; j++)
                        {
                            int startAddr = j * packetData;
                            for (int k = 0; k < packetData; k++)
                            {
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.GlobalNameConfig[(startAddr + k)];
                            }

                            rxBF = berkeleySendMsg(txBF, bfSize);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //last packet
                        if (signleBytes > 0)
                        {
                            for (int l = 0; l < signleBytes; l++)
                            {
                                txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.GlobalNameConfig[(fullTxNo * packetData) + l];
                            }
                            rxBF = berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //signal finish
                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHFIN;
                        txBF[2] = ipcDefines.NET_PACKET_TYPE_GLOBNAMECFG;
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                        if (rxBF[0] != ipcCMD.ACKFIN)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public async Task<bool> WriteGlobalCfgAsync(sconnSite site)
        {
            return true;
        }


        public bool WriteGlobalCfg(sconnSite site)
        {

            try
            {

                if (site.siteCfg == null) { return false; }
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

                site.siteStat.StartConnectionTimer();
                try
                {
                    int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                    int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES;
                    int bytesToSend = ipcDefines.ipcGlobalConfigSize;

                    // Receiving byte array  
                    byte[] txBF = new byte[bfSize];
                    byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];

                    txBF[0] = ipcCMD.SET;
                    txBF[1] = ipcCMD.setGlobalCfg;
                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.ipcGlobalConfigSize / packetData;
                        int signleBytes = (int)ipcDefines.ipcGlobalConfigSize % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;
                        int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : bytesToSend + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;
                        for (int j = 0; j < fullTxNo; j++)
                        {
                            int startAddr = j * packetData;
                            for (int k = 0; k < packetData; k++)
                            {
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.globalConfig.memCFG[(startAddr + k)];
                            }

                            rxBF = berkeleySendMsg(txBF, bfSize);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //last packet
                        if (signleBytes > 0)
                        {
                            for (int l = 0; l < signleBytes; l++)
                            {
                                txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.globalConfig.memCFG[(fullTxNo * packetData) + l];
                            }
                            rxBF = berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
                            //ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.deviceConfigSize + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);

                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //signal finish
                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHFIN;
                        txBF[2] = ipcDefines.NET_PACKET_TYPE_GCFG;
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                        if (rxBF[0] != ipcCMD.ACKFIN)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }

                        return true;

                    }
                    else
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


        public string SendGsmCommandDirect(sconnSite site, string command)
        {
            try
            {
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
                client.ConnectionTimeoutMs = 4000; //long timeout
                site.siteStat.StartConnectionTimer();

                int bfSize = ipcDefines.NET_MAX_TX_SIZE;

                // Receiving byte array  
                byte[] txBF = new byte[bfSize];
                byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                txBF[0] = ipcCMD.GET;
                txBF[1] = ipcCMD.getGsmModemResponse;

                //copy command
                byte[] strb = Encoding.ASCII.GetBytes(command);
                for (int i = 0; i < strb.Length; i++)
                {
                    txBF[i + 2] = strb[i];
                }
                txBF[strb.Length + 2] = 0x0D;

                rxBF = berkeleySendMsg(txBF, strb.Length + 3);

                string resp = Encoding.ASCII.GetString(rxBF);
                return resp;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }


        }

        public bool WriteAuthorizedDevicesCfg(sconnSite site)
        {
            return WriteDeviceDevAuthCfgSingle(site, 0x00);
        }

        public bool WriteUserCfg(sconnSite site)
        {

            try
            {
                if (site.siteCfg == null)
                {
                    return false;
                }
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
                site.siteStat.StartConnectionTimer();
                try
                {
                    int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                    int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
                    int bytesToSend = ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE;

                    // Receiving byte array  
                    byte[] txBF = new byte[bfSize];
                    byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                    txBF[0] = ipcCMD.SET;
                    txBF[1] = ipcCMD.setPasswdCfg;

                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE / packetData;
                        int singleBytes = (int)ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;

                        int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;

                        for (int j = 0; j < fullTxNo; j++)
                        {
                            for (int k = 0; k < packetData; k++)
                            {
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.AuthDevices[j * packetData + k];
                            }

                            rxBF = berkeleySendMsg(txBF, bfSize);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //last packet

                        if (singleBytes > 0)
                        {
                            for (int l = 0; l < singleBytes; l++)
                            {
                                txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[0].AuthDevicesCFG[fullTxNo * packetData + l];
                            }
                            rxBF = berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //signal finish
                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHFIN;
                        txBF[2] = ipcDefines.NET_PACKET_TYPE_DEVAUTHCFG;
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                        if (rxBF[0] != ipcCMD.ACKFIN)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }
                site.siteStat.StopConnectionTimer();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool WriteDeviceCfg(sconnSite site)
        {
            if (site.siteCfg == null)
            {
                return false;
            }
            int devices = site.siteCfg.deviceNo;
            for (int i = 0; i < devices; i++)
            {
                if (!WriteDeviceCfgSingle(site, i))
                    return false;
            }//each device
            return true;
        }


        public bool WriteSiteGsmCfg(sconnSite site)
        {
            try
            {
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
                site.siteStat.StartConnectionTimer();
                if (site.siteCfg == null)
                {
                    return false;
                }

                int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
                int bytesToSend = ipcDefines.RAM_SMS_RECP_NO * ipcDefines.RAM_SMS_RECP_SIZE;

                // Receiving byte array  
                byte[] txBF = new byte[bfSize];
                byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                txBF[0] = ipcCMD.SET;
                txBF[1] = ipcCMD.setGsmRcptCfg;

                rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

                if (rxBF[0] == ipcCMD.ACK)
                {
                    int fullTxNo = (int)bytesToSend / packetData;
                    int signleBytes = (int)bytesToSend % packetData;

                    txBF[0] = ipcCMD.PSH;
                    txBF[1] = ipcCMD.PSHNXT;
                    txBF[2] = ipcCMD.SVAL;

                    int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : bytesToSend + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                    txBF[packetLastByteIndex] = ipcCMD.EVAL;
                    for (int j = 0; j < fullTxNo; j++)
                    {
                        int startAddr = j * packetData;
                        for (int k = 0; k < packetData; k++)
                        {
                            txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.GsmConfig[(startAddr + k)];
                        }

                        rxBF = berkeleySendMsg(txBF, bfSize);
                        if (rxBF[0] != ipcCMD.ACKNXT)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    //last packet
                    if (signleBytes > 0)
                    {
                        for (int l = 0; l < signleBytes; l++)
                        {
                            txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.GsmConfig[(fullTxNo * packetData) + l];
                        }
                        rxBF = berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
                        if (rxBF[0] != ipcCMD.ACKNXT)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    //signal finish
                    txBF[0] = ipcCMD.PSH;
                    txBF[1] = ipcCMD.PSHFIN;
                    txBF[2] = ipcDefines.NET_PACKET_TYPE_GSMRCPTCFG;
                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                    if (rxBF[0] != ipcCMD.ACKFIN)
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        client.CloseConnection();
                        return false;
                    }
                    client.CloseConnection();
                }
                else
                {
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    client.CloseConnection();
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }


        public bool WriteDeviceCfgSingle(sconnSite site, int DevId)
        {
            try
            {
                if (site.siteCfg == null)
                {
                    return false;
                }

                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
                site.siteStat.StartConnectionTimer();
                ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
                try
                {
                    int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                    int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
                    int bytesToSend = ipcDefines.deviceConfigSize;

                    // Receiving byte array  
                    byte[] txBF = new byte[bfSize];
                    byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                    txBF[0] = ipcCMD.SET;
                    txBF[1] = ipcCMD.setDeviceCfg;
                    txBF[2] = (byte)DevId;

                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN); // ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.deviceConfigSize / packetData;
                        int signleBytes = (int)ipcDefines.deviceConfigSize % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;
                        int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.deviceConfigSize + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;
                        for (int j = 0; j < fullTxNo; j++)
                        {
                            int startAddr = j * packetData;
                            for (int k = 0; k < packetData; k++)
                            {
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].memCFG[(startAddr + k)];
                            }

                            rxBF = berkeleySendMsg(txBF, bfSize);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //last packet
                        if (signleBytes > 0)
                        {
                            for (int l = 0; l < signleBytes; l++)
                            {
                                txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].memCFG[(fullTxNo * packetData) + l];
                            }
                            rxBF = berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //signal finish
                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHFIN;
                        txBF[2] = ipcDefines.NET_PACKET_TYPE_DEVCFG;
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                        if (rxBF[0] != ipcCMD.ACKFIN)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            client.CloseConnection();
                            return false;
                        }
                        client.CloseConnection();
                    }
                    else
                    {
                        client.CloseConnection();
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    client.CloseConnection();
                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }
                site.siteStat.StopConnectionTimer();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


        public bool WriteDeviceNetCfg(sconnSite site, int DevId)
        {
            try
            {

            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
            if (site.siteCfg == null)
            {
                return false;
            }

            SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
            site.siteStat.StartConnectionTimer();
            try
            {
                int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
                int bytesToSend = ipcDefines.NET_CFG_SIZE;

                // Receiving byte array  
                byte[] txBF = new byte[bfSize];
                byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                txBF[0] = ipcCMD.SET;
                txBF[1] = ipcCMD.setDeviceNetworkCfg;
                txBF[2] = (byte)DevId;

                rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN); // ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.NET_CMD_PACKET_LEN);

                if (rxBF[0] == ipcCMD.ACK)
                {
                    int fullTxNo = (int)ipcDefines.NET_CFG_SIZE / packetData;
                    int signleBytes = (int)ipcDefines.NET_CFG_SIZE % packetData;

                    txBF[0] = ipcCMD.PSH;
                    txBF[1] = ipcCMD.PSHNXT;
                    txBF[2] = ipcCMD.SVAL;
                    int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.NET_CFG_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                    txBF[packetLastByteIndex] = ipcCMD.EVAL;
                    for (int j = 0; j < fullTxNo; j++)
                    {
                        int startAddr = j * packetData;
                        for (int k = 0; k < packetData; k++)
                        {
                            txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NetworkConfig[(startAddr + k)];
                        }

                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CFG_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
                        // ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.deviceConfigSize + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
                        if (rxBF[0] != ipcCMD.ACKNXT)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    //last packet
                    if (signleBytes > 0)
                    {
                        for (int l = 0; l < signleBytes; l++)
                        {
                            txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NetworkConfig[(fullTxNo * packetData) + l];
                        }
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CFG_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
                        //ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.deviceConfigSize + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);

                        if (rxBF[0] != ipcCMD.ACKNXT)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    //signal finish
                    txBF[0] = ipcCMD.PSH;
                    txBF[1] = ipcCMD.PSHFIN;
                    txBF[2] = ipcDefines.NET_PACKET_TYPE_NETCFG;
                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                    if (rxBF[0] != ipcCMD.ACKFIN)
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                else
                {
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }
            }
            catch (Exception e)
            {
                site.siteStat.StopConnectionTimer();
                site.siteStat.FailedConnections++;
                return false;
            }
            site.siteStat.StopConnectionTimer();
            return true;
        }


        public bool WriteDeviceNamesCfgSingle(sconnSite site, int DevId)
        {
            try
            {
                if (site.siteCfg == null)
                {
                    return false;
                }
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

                site.siteStat.StartConnectionTimer();
                ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
                try
                {
                    int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                    int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
                    int bytesToSend = ipcDefines.RAM_DEVICE_NAMES_SIZE;

                    // Receiving byte array  
                    byte[] txBF = new byte[bfSize];
                    byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                    txBF[0] = ipcCMD.SET;
                    txBF[1] = ipcCMD.setDeviceNamesCfg;
                    txBF[2] = (byte)DevId;

                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.RAM_DEVICE_NAMES_SIZE / packetData;
                        int singleBytes = (int)ipcDefines.RAM_DEVICE_NAMES_SIZE % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;
                        int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.RAM_DEVICE_NAMES_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;

                        int nameinc = 0;
                        int namecharinc = 0;
                        for (int j = 0; j < fullTxNo; j++)
                        {
                            for (int k = 0; k < packetData; k++)
                            {
                                if (namecharinc >= ipcDefines.RAM_NAME_SIZE)
                                { nameinc++; namecharinc = 0; }
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NamesCFG[nameinc][namecharinc]; //site.siteCfg.deviceConfigs[DevId].memCFG[(startAddr + k)];
                                namecharinc++;
                            }

                            rxBF = berkeleySendMsg(txBF, bfSize);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //last packet

                        if (singleBytes > 0)
                        {
                            for (int l = 0; l < singleBytes; l++)
                            {
                                if (namecharinc >= ipcDefines.RAM_NAME_SIZE)
                                { nameinc++; namecharinc = 0; }
                                txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NamesCFG[nameinc][namecharinc];
                                namecharinc++;
                            }
                            rxBF = berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //signal finish
                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHFIN;
                        txBF[2] = ipcDefines.NET_PACKET_TYPE_DEVNAMECFG;
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                        if (rxBF[0] != ipcCMD.ACKFIN)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {

                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }
                site.siteStat.StopConnectionTimer();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


        public bool WriteDeviceSchedulesCfgSingle(sconnSite site, int DevId)
        {
            try
            {
                if (site.siteCfg == null)
                {
                    return false;
                }
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

                site.siteStat.StartConnectionTimer();
                ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
                try
                {
                    int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                    int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
                    int bytesToSend = ipcDefines.RAM_DEV_SCHED_SIZE;

                    // Receiving byte array  
                    byte[] txBF = new byte[bfSize];
                    byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                    txBF[0] = ipcCMD.SET;
                    txBF[1] = ipcCMD.setDeviceSchedulesCfg;
                    txBF[2] = (byte)DevId;

                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.RAM_DEV_SCHED_SIZE / packetData;
                        int singleBytes = (int)ipcDefines.RAM_DEV_SCHED_SIZE % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;

                        int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.RAM_DEV_SCHED_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;

                        int scheduleinc = 0;
                        int schedulebyteinc = 0;
                        for (int j = 0; j < fullTxNo; j++)
                        {
                            for (int k = 0; k < packetData; k++)
                            {
                                if (schedulebyteinc >= ipcDefines.RAM_DEV_SCHED_MEM_SIZE)
                                { scheduleinc++; schedulebyteinc = 0; }
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].ScheduleCFG[scheduleinc][schedulebyteinc];
                                schedulebyteinc++;
                            }

                            rxBF = berkeleySendMsg(txBF, bfSize);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //last packet

                        if (singleBytes > 0)
                        {
                            for (int l = 0; l < singleBytes; l++)
                            {
                                if (schedulebyteinc >= ipcDefines.RAM_DEV_SCHED_MEM_SIZE)
                                { scheduleinc++; schedulebyteinc = 0; }
                                txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].ScheduleCFG[scheduleinc][schedulebyteinc];
                                schedulebyteinc++;
                            }
                            rxBF = berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //signal finish
                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHFIN;
                        txBF[2] = ipcDefines.NET_PACKET_TYPE_SCHEDCFG;
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                        if (rxBF[0] != ipcCMD.ACKFIN)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }
                site.siteStat.StopConnectionTimer();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }


        }




        public bool WriteDeviceDevAuthCfgSingle(sconnSite site, int DevId)
        {
            try
            {

                if (site.siteCfg == null)
                {
                    return false;
                }
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

                site.siteStat.StartConnectionTimer();
                ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
                try
                {
                    int bfSize = ipcDefines.NET_MAX_TX_SIZE;
                    int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
                    int bytesToSend = ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE;

                    // Receiving byte array  
                    byte[] txBF = new byte[bfSize];
                    byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
                    txBF[0] = ipcCMD.SET;
                    txBF[1] = ipcCMD.setAuthDevCfg;
                    txBF[2] = (byte)DevId;

                    rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE / packetData;
                        int singleBytes = (int)ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;

                        int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;

                        for (int j = 0; j < fullTxNo; j++)
                        {
                            for (int k = 0; k < packetData; k++)
                            {
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.AuthDevices[j * packetData + k];
                            }

                            rxBF = berkeleySendMsg(txBF, bfSize);
                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //last packet

                        if (singleBytes > 0)
                        {
                            for (int l = 0; l < singleBytes; l++)
                            {
                                txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].AuthDevicesCFG[fullTxNo * packetData + l];
                            }
                            rxBF = berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

                            if (rxBF[0] != ipcCMD.ACKNXT)
                            {
                                site.siteStat.StopConnectionTimer();
                                site.siteStat.FailedConnections++;
                                return false;
                            }
                        }
                        //signal finish
                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHFIN;
                        txBF[2] = ipcDefines.NET_PACKET_TYPE_DEVAUTHCFG;
                        rxBF = berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
                        if (rxBF[0] != ipcCMD.ACKFIN)
                        {
                            site.siteStat.StopConnectionTimer();
                            site.siteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    return false;
                }
                site.siteStat.StopConnectionTimer();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }




        public bool updateSiteStatus(sconnSite site) //read I/O values
        {
            try
            {
                int devices = 0;
                if (site.siteCfg != null)
                {
                    devices = site.siteCfg.deviceNo;
                }
                else
                {
                    devices = getSiteDeviceNo(site);
                    site.siteCfg = new ipcSiteConfig(devices); //init device configs 
                }

                ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
                try
                {
                    for (int i = 0; i < devices; i++)
                    {
                        siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (i * ipcDefines.deviceConfigSize)); //device start addresss
                        byte[] deviceCFG = new byte[ipcDefines.deviceConfigSize];
                        if (deviceCFG.GetLength(0) == ipcDefines.deviceConfigSize)
                        {
                            site.siteCfg.deviceConfigs[i].memCFG = deviceCFG;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    Debug.WriteLine(e.Message + " | " + e.InnerException.Message);
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        public bool ReadSiteStartupConfig(sconnSite site)
        {
            try
            {

                site.siteStat.StartConnectionTimer();
                bool globalUploadStat = false;
                bool deviceUploadStat = false;
                int devices = 0;
                byte[] cmd = new byte[32];

                if (authMCU(site.serverIP, site.serverPort, site.authPasswd))  //update only if authed to site
                {

                    if (site.siteCfg != null)
                    {
                        devices = site.siteCfg.deviceNo;
                    }
                    else
                    {
                        devices = getSiteDeviceNo(site);
                        site.siteCfg = new ipcSiteConfig(devices); //init device configs 
                    }

                    /**********  Read global config  ***********/


                    ushort siteMemAddr = (ushort)(ipcDefines.mAdrGlobalConfig);
                    byte[] rxBF = new byte[ipcDefines.ipcGlobalConfigSize + 2];

                    cmd[0] = ipcCMD.GET;
                    cmd[1] = ipcCMD.getGlobCfg;
                    rxBF = ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
                    //rxBF = ethernet.berkeleyReadLen(site.serverIP, cmd, site.serverPort, ipcDefines.ipcGlobalConfigSize + 2);

                    if (rxBF[0] == ipcCMD.SVAL)
                    {
                        for (int j = 0; j < ipcDefines.ipcGlobalConfigSize; j++)
                        {
                            site.siteCfg.globalConfig.memCFG[j] = rxBF[j + 1];
                        }
                        globalUploadStat = true;
                    }




                    /**********  Get device configs **********/

                    cmd = new byte[32];
                    rxBF = new byte[ipcDefines.deviceConfigSize + 2];
                    siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
                    try
                    {
                        for (int i = 0; i < devices; i++)
                        {
                            cmd[0] = ipcCMD.GET;
                            cmd[1] = ipcCMD.getDevCfg;
                            cmd[2] = (byte)i; //device number
                            rxBF = ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
                            //rxBF = ethernet.berkeleyReadLen(site.serverIP, cmd, site.serverPort, ipcDefines.deviceConfigSize + 2);
                            deviceUploadStat = true;
                            if (rxBF[0] == ipcCMD.SVAL)
                            {
                                for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
                                {
                                    site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + 1];
                                }
                            }
                            else
                            {
                                deviceUploadStat = false;
                            }
                        }
                        site.siteStat.StopConnectionTimer();
                        return (bool)(deviceUploadStat & globalUploadStat);

                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, e.Message);
                        site.siteStat.StopConnectionTimer();
                        site.siteStat.FailedConnections++;
                        return (bool)(deviceUploadStat & globalUploadStat);
                        throw;
                    }

                }
                else
                {
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    //MessageBoxResult result = MessageBox.Show("MCU Auth Fail");
                    return (bool)(deviceUploadStat & globalUploadStat);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }


        }


        public bool ReadSiteRunningDeviceConfigs(sconnSite site)
        {
            /**********  Get device configs **********/
            site.siteStat.StartConnectionTimer();
            bool globalUploadStat = false;
            bool deviceUploadStat = false;
            int devices = 0;
            byte[] cmd = new byte[32];
            ushort siteMemAddr = (ushort)(ipcDefines.mAdrGlobalConfig);
            byte[] rxBF = new byte[ipcDefines.ipcGlobalConfigSize + 2];

            SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

            cmd = new byte[32];
            rxBF = new byte[ipcDefines.deviceConfigSize + 2];
            byte[,] narrNamesBF = new byte[ipcDefines.RAM_DEV_NAMES_NO, ipcDefines.RAM_NAME_SIZE];
            byte[] SchedBF = new byte[ipcDefines.RAM_DEV_SCHED_SIZE];
            byte[] gsmRcpBF = new byte[ipcDefines.RAM_SMS_RECP_MEM_SIZE];
            byte[] devAuthBF = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
            siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
            int MsgByteOffset = 1;

            try
            {
                for (int i = 0; i < devices; i++)
                {
                    /*******  Get Device Config  ****/
                    cmd[0] = ipcCMD.GET;
                    cmd[1] = ipcCMD.getRunDevCfg;
                    cmd[2] = (byte)i; //devi ce number
                    rxBF = berkeleySendMsg(cmd);

                    if (rxBF[0] == ipcCMD.SVAL)
                    {
                        deviceUploadStat = true;

                        //read device config
                        for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
                        {
                            site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + MsgByteOffset];
                        }

                    }
                    else
                    {
                        deviceUploadStat = false;
                    }
                }

                //get events
                cmd[0] = ipcCMD.GET;
                cmd[1] = ipcCMD.getEventNo;
                rxBF = berkeleySendMsg(cmd);

                cmd[1] = ipcCMD.getEvent;
                cmd[2] = (byte)1; //test event id 1
                rxBF = berkeleySendMsg(cmd);

                site.siteStat.StopConnectionTimer();

            }
            catch (Exception e)
            {
                site.siteStat.StopConnectionTimer();
                site.siteStat.FailedConnections++;
                client.CloseConnection();
                return (bool)(deviceUploadStat & globalUploadStat);
                throw;
            }
            client.CloseConnection();
            return (bool)(deviceUploadStat & globalUploadStat);

        }


        private bool HashMatch()
        {
            return false;
        }



        /*********** Device config update only **************/

        public bool ReadSiteRunningConfigIO(sconnSite site)
        {
            return ReadSiteRunningConfigMin(site, false);
        }



        /***********  Minimum config update **********/
        public bool ReadSiteRunningConfigMin(sconnSite site, bool ReadSpecialRegs)
        {
            try
            {
                SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

                bool globalUploadStat = false;
                bool deviceUploadStat = false;
                int devices = 0;
                byte[] cmd = new byte[32];
                byte[] rxBF = new byte[2048];
                byte[] gcfgRx = new byte[ipcDefines.ipcGlobalConfigSize + 2];
                bool configChanged = true;
                int rxOffset = 1;
                ushort siteMemAddr = (ushort)(ipcDefines.mAdrGlobalConfig);

                try
                {
                    site.siteStat.StartConnectionTimer();

                    //Get config hash first to verify if config has changed at all
                    cmd[0] = ipcCMD.GET;
                    cmd[1] = ipcCMD.getConfigHash;
                    rxBF = berkeleySendMsg(cmd);
                    if (rxBF[0] == ipcCMD.SVAL)
                    {
                        byte[] hashrx = new byte[ipcDefines.SHA256_DIGEST_SIZE];

                        SHA256 shaHash = SHA256.Create();
                        byte[] clientHash = shaHash.ComputeHash((site.siteCfg.globalConfig.memCFG));
                        site.siteCfg.Hash = clientHash; //update local hash based on what we actually have

                        for (int i = 0; i < ipcDefines.SHA256_DIGEST_SIZE; i++)
                        {
                            hashrx[i] = rxBF[i + rxOffset];
                        }
                        configChanged = !(hashrx.SequenceEqual(site.siteCfg.Hash));
                    }
                    else
                    {
                        return false;
                    }
                    bool DeviceChanged;
                    bool NamesChanged;


                    configChanged = true; //for debug

                    /**********  Read global config  ***********/
                    if (configChanged)
                    {
                        //update hash
                        for (int i = 0; i < ipcDefines.SHA256_DIGEST_SIZE; i++)
                        {
                            site.siteCfg.Hash[i] = rxBF[i + rxOffset];
                        }

                        cmd[0] = ipcCMD.GET;
                        cmd[1] = ipcCMD.getRunGlobCfg;
                        gcfgRx = berkeleySendMsg(cmd);     //    ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
                        bool GcfgOk = (gcfgRx[0] == ipcCMD.SVAL);

                        /**********  Get device configs **********/

                        cmd = new byte[32];
                        rxBF = new byte[ipcDefines.deviceConfigSize + 2];
                        byte[,] narrNamesBF = new byte[ipcDefines.RAM_DEV_NAMES_NO, ipcDefines.RAM_NAME_SIZE];
                        byte[] SchedBF = new byte[ipcDefines.RAM_DEV_SCHED_SIZE];
                        byte[] gsmRcpBF = new byte[ipcDefines.RAM_SMS_RECP_MEM_SIZE];
                        byte[] devAuthBF = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
                        siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
                        int MsgByteOffset = 1;

                        if (GcfgOk)
                        {

                            devices = gcfgRx[2];
                            if (site.siteCfg.deviceNo != devices)
                            {
                                site.siteCfg = new ipcSiteConfig(devices);
                            }
                            //site.siteCfg.globalConfig.memCFG = gcfgRx;
                            for (int i = 0; i < ipcDefines.ipcGlobalConfigSize; i++)
                            {
                                site.siteCfg.globalConfig.memCFG[i] = gcfgRx[i + ipcDefines.NET_UPLOAD_DATA_END_OFFSET];
                            }


                            for (int i = 0; i < devices; i++)
                            {

                                //find out which configs changed by gcfg registers
                                byte[] devhashrx = new byte[ipcDefines.SHA256_DIGEST_SIZE];
                                for (int j = 0; j < ipcDefines.SHA256_DIGEST_SIZE; j++)
                                {
                                    devhashrx[j] = gcfgRx[ipcDefines.GCFG_DEV_MOD_CTR_START_POS + (i * ipcDefines.GCFG_DEV_MOD_CTR_LEN) + j + rxOffset];
                                }
                                DeviceChanged = !(devhashrx.SequenceEqual(site.siteCfg.deviceConfigs[i].Hash));

                                if (DeviceChanged)
                                {
                                    for (int h = 0; h < ipcDefines.SHA256_DIGEST_SIZE; h++)
                                    {
                                        site.siteCfg.deviceConfigs[i].Hash[h] = gcfgRx[ipcDefines.GCFG_DEV_MOD_CTR_START_POS + (i * ipcDefines.GCFG_DEV_MOD_CTR_LEN) + h + rxOffset];
                                    }
                                    /*******  Get Device Config  ****/

                                    cmd[0] = ipcCMD.GET;
                                    cmd[1] = ipcCMD.getRunDevCfg;
                                    cmd[2] = (byte)i; //device number
                                    rxBF = berkeleySendMsg(cmd);

                                    if (rxBF[0] == ipcCMD.SVAL)
                                    {

                                        //read device config
                                        for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
                                        {
                                            site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + MsgByteOffset];
                                        }

                                    }    //dev rx ok
                                }

                                ReadSpecialRegs = true;

                                if (ReadSpecialRegs)
                                {
                                    /*****  Get Device AUTH CFG ******/

                                    cmd[1] = ipcCMD.getAuthDevices;
                                    devAuthBF = berkeleySendMsg(cmd);
                                    if (devAuthBF[0] == ipcCMD.SVAL)
                                    {
                                        site.siteCfg.AuthDevices = new byte[ipcDefines.AUTH_RECORDS_SIZE];
                                        for (int j = 0; j < ipcDefines.AUTH_RECORDS_SIZE; j++)
                                        {
                                            site.siteCfg.AuthDevices[j] = devAuthBF[j + 1];
                                        }
                                    }


                                    /*****  Get GSM RCPT ******/

                                    cmd[1] = ipcCMD.getGsmRecpCfg;
                                    gsmRcpBF = berkeleySendMsg(cmd);

                                    deviceUploadStat = true;


                                    //read  schedule config
                                    if (SchedBF[0] == ipcCMD.SVAL)
                                    {
                                        for (int schedule = 0; schedule < ipcDefines.RAM_DEV_SCHED_NO; schedule++)
                                        {
                                            for (int schedbyte = 0; schedbyte < ipcDefines.RAM_DEV_SCHED_MEM_SIZE; schedbyte++)
                                            {
                                                site.siteCfg.deviceConfigs[i].ScheduleCFG[schedule][schedbyte] = SchedBF[(schedule * ipcDefines.RAM_DEV_SCHED_MEM_SIZE) + schedbyte + MsgByteOffset]; //1 byte buffer offset
                                            }
                                        }
                                    }

                                    //read GSM config
                                    if (gsmRcpBF[0] == ipcCMD.SVAL)
                                    {
                                        try
                                        {
                                            site.siteCfg.GsmConfig = new byte[ipcDefines.RAM_SMS_RECP_MEM_SIZE];
                                            for (int j = 0; j < ipcDefines.RAM_SMS_RECP_MEM_SIZE; j++)
                                            {
                                                site.siteCfg.GsmConfig[j] = gsmRcpBF[j + 1];
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            //TODO - buffer overflow
                                        }

                                    }
                                }


                                //cfgIncBefore = CfgOper.GetLongFromBufferAtPos(site.siteCfg.globalConfig.memCFG,ipcDefines.GCFG_NAMES_MOD_CTR_POS);
                                //cfgIncNow = CfgOper.GetLongFromBufferAtPos(gcfgRx, ipcDefines.GCFG_NAMES_MOD_CTR_POS + rxOffset);
                                //NamesChanged = cfgIncBefore < cfgIncNow;

                                if (ReadSpecialRegs)
                                {
                                    byte[] nameshashrx = new byte[ipcDefines.SHA256_DIGEST_SIZE];
                                    for (int j = 0; j < ipcDefines.SHA256_DIGEST_SIZE; j++)
                                    {
                                        nameshashrx[j] = gcfgRx[ipcDefines.GCFG_NAMES_MOD_CTR_POS + j + rxOffset];
                                    }
                                    NamesChanged = !(nameshashrx.SequenceEqual(site.siteCfg.NamesHash));


                                    if (NamesChanged)
                                    {
                                        for (int h = 0; h < ipcDefines.SHA256_DIGEST_SIZE; h++)
                                        {
                                            site.siteCfg.NamesHash[h] = gcfgRx[ipcDefines.GCFG_NAMES_MOD_CTR_POS + h + rxOffset];
                                        }
                                        ///*****  Get Device Names ******/
                                        cmd[1] = ipcCMD.getDeviceName;
                                        for (int n = 0; n < ipcDefines.RAM_DEV_NAMES_NO; n++)
                                        {

                                            cmd[2] = (byte)n;
                                            cmd[3] = (byte)rxBF[ipcDefines.mAdrDevID + 1]; //TODO 2 byte addressing
                                            byte[] narr = berkeleySendMsg(cmd);
                                            if (narr[0] == ipcCMD.SVAL)
                                            {
                                                for (int txtbyte = MsgByteOffset; txtbyte < ipcDefines.RAM_NAME_SIZE + MsgByteOffset; txtbyte++)
                                                {
                                                    site.siteCfg.deviceConfigs[i].NamesCFG[n][txtbyte - MsgByteOffset] = narr[txtbyte]; //1 byte buffer offset
                                                }
                                            }
                                        }

                                        ///*****  Get Global Names ******/
                                        cmd[1] = ipcCMD.getGlobalNames;
                                        byte[] nresp = berkeleySendMsg(cmd);
                                        if (nresp[0] == ipcCMD.SVAL)
                                        {
                                            for (int n = 0; n < ipcDefines.RAM_NAMES_Global_Total_Size; n++)
                                            {
                                                site.siteCfg.GlobalNameConfig[n] = nresp[n + MsgByteOffset];
                                            }
                                        }

                                        //Get Zone Names
                                        cmd[1] = ipcCMD.getZoneName;
                                        site.siteCfg.ZoneNames = new byte[ipcDefines.ZONE_CFG_MAX_ZONES][];
                                        for (int n = 0; n < ipcDefines.ZONE_CFG_MAX_ZONES; n++)
                                        {
                                            try
                                            {
                                                site.siteCfg.ZoneNames[n] = new byte[ipcDefines.RAM_NAME_SIZE];
                                                cmd[2] = (byte)n;
                                                byte[] narr = berkeleySendMsg(cmd);
                                                if (narr[0] == ipcCMD.SVAL)
                                                {
                                                    for (int txtbyte = MsgByteOffset; txtbyte < ipcDefines.RAM_NAME_SIZE + MsgByteOffset; txtbyte++)
                                                    {
                                                        site.siteCfg.ZoneNames[n][txtbyte - MsgByteOffset] = narr[txtbyte];
                                                    }
                                                }
                                            }
                                            catch (Exception e)
                                            {

                                            }

                                        }

                                    }

                                    //get events
                                    cmd[0] = ipcCMD.GET;
                                    cmd[1] = ipcCMD.getEventNo;
                                    rxBF = berkeleySendMsg(cmd);
                                    int events = (((int)rxBF[1] << 8) | (int)rxBF[2]);
                                    site.siteCfg.events = new ipcEvent[events];
                                    //TODO events track change
                                    for (int j = 0; j < events % 100; j++) //get events but not more then 100
                                    {
                                        cmd[1] = ipcCMD.getEvent;
                                        cmd[2] = (byte)j;
                                        rxBF = berkeleySendMsg(cmd);
                                        byte[] evBF = new byte[ipcDefines.EVENT_DB_RECORD_LEN];
                                        for (int k = 0; k < ipcDefines.EVENT_DB_RECORD_LEN; k++)
                                        {
                                            evBF[k] = rxBF[k + 1];
                                        }
                                        site.siteCfg.events[j] = new ipcEvent(evBF);
                                    }

                                    //Auth cfg
                                    cmd[0] = ipcCMD.GET;
                                    cmd[1] = ipcCMD.getPasswdCfg;
                                    rxBF = berkeleySendMsg(cmd);
                                    byte[] AuthBf = new byte[ipcDefines.AUTH_MAX_USERS * ipcDefines.AUTH_RECORD_SIZE];
                                    for (int j = 0; j < ipcDefines.AUTH_MAX_USERS * ipcDefines.AUTH_RECORD_SIZE; j++)
                                    {
                                        AuthBf[j] = rxBF[j + 1];
                                    }
                                    site.siteCfg.UserConfig = AuthBf;
                                }
                            }

                            deviceUploadStat = true;
                            globalUploadStat = true;

                            site.siteStat.StopConnectionTimer();

                            for (int j = 0; j < ipcDefines.ipcGlobalConfigSize; j++)
                            {
                                site.siteCfg.globalConfig.memCFG[j] = gcfgRx[j + rxOffset];
                            }
                        }    //gcfg ok
                    }  //config changed

                    client.CloseConnection();
                    return true;
                }
                catch (Exception e)
                {

                    _logger.Error(e, e.Message);
                    site.siteStat.StopConnectionTimer();
                    site.siteStat.FailedConnections++;
                    client.CloseConnection();
                    return false;
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }



        }


        public bool ReadSiteRunningConfig(sconnSite site)
        {
            //if (site.UsbCom)
            //{
            //    Trx_Com_Usb = true;
            //}
            //bool succ = ReadSiteRunningConfigMin(site, true);
            //Trx_Com_Usb = false;
            //return succ;
            return ReadSiteRunningConfigMin(site, true);
        }


        public bool updateSiteConfig(sconnSite site) //read entire running config
        {
            return ReadSiteRunningConfigMin(site, true);
        }



    }
#endif






}
