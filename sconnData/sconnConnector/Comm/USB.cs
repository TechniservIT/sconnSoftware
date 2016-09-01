using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using sconnConnector.POCO.Config;
using NLog;
using System.Security.Cryptography;

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

        private byte[] berkeleySendMsg( byte[] msg)
        {
            return UsbComm_Trx_Message(msg, msg.Length);    //UsbComm_Trx_Transaction(msg, msg.Length);
        }

        private byte[] berkeleySendMsg(string server, byte[] msg, int port)
        {
            return UsbComm_Trx_Message(msg, msg.Length);
        }

        private byte[] berkeleySendMsg(byte[] msg, int bytes)
        {
            return UsbComm_Trx_Message(msg,bytes);
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
            UsbComm_Trx_Setup();
           
            // client.Open();

        }

        public USB(SerialPort port)
        {
            client = port;
            UsbComm_Trx_Setup();
         //   client.Open();
        }

        public USB(SerialPort port, string user, string pass)
        {
            username = user;
            password = pass;
            client = port;
            UsbComm_Trx_Setup();
          //  client.Open();
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
                client.Close();
                return false;
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

        static public int USB_CMD_NET_TX_ACK = 0x0108;
        static public int USB_CMD_NET_RX_ACK = 0x0109;

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



        public bool UsbComm_Sample_Trx()
        {
            byte[] trxMsg = new byte[2];
            trxMsg[0]= ipcCMD.GET;
            trxMsg[1] = ipcCMD.getDevNo;
            return UsbComm_Trx_Transaction(trxMsg, trxMsg.Length).Length != 0;
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

        
        public void UsbComm_Trx_Setup()
        {
            client.WriteTimeout = 400;
            client.ReadTimeout = 400;
            client.ReadBufferSize = 2048;
            client.WriteBufferSize = 2048;

        }

        #region USB_TRX

        public byte[] UsbComm_Recieve_Packet_NoAck(int bytes, int timeoutCycles)
        {
            try
            {

                //recieve
                byte[] Rx_Buffer = new byte[2048];
                byte[] Result_Buffer = new byte[2048];
                int rxLeft = bytes;
                int rxCt = 0;
                int timeout = 0;

                while (rxLeft > 0 && !(timeout > timeoutCycles))
                {
                    int bread = 0;
                    bread = client.Read(Rx_Buffer, rxCt, rxLeft);
                    rxCt += bread;
                    rxLeft -= bread;
                    if (rxLeft == 0)
                    {
                        return Rx_Buffer;
                    }
                }

                return Rx_Buffer;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return new byte[0];
            }
        }

        //TX_UPLOAD 
        public bool UsbComm_Transmit_Packet_NoAck(byte[] packet, int len, int timeoutCycles, ushort header)
        {
            try
            {
                byte[] Trx_Response = new byte[2048];
                byte[] Rx_Buffer = new byte[2048];
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];

                //transmit
                int txLeft = len;
                int txPos = 0;
                int timeout = 0;

                while (txLeft > 0 && !(timeout > timeoutCycles))
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
                            txbinc = txLeft;
                        }
                        //append response with header
                        packetBuffer[0] = (byte)(header >> 8);
                        packetBuffer[1] = (byte)header;
                        for (int i = 0; i < txbinc; i++)
                        {
                            packetBuffer[USB_PACKET_DATA_HEADER_SIZE + i] = packet[txPos + i];
                        }
                        client.Write(packetBuffer, 0, txbinc + USB_PACKET_DATA_HEADER_SIZE);
                        txPos += txbinc;
                        txLeft -= txbinc;
                    }
                    timeout++;
                }
                return ((txLeft == 0));
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


        //TX_UPLOAD 
        public bool UsbComm_Transmit_Command_NoAck(ushort cmd)
        {
            try
            {
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];
                packetBuffer[0] = (byte)(cmd >> 8);
                packetBuffer[1] = (byte)cmd;
                client.Write(packetBuffer, 0, 2);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


        //TX_UPLOAD  - Success if ACKed 
        public byte[] UsbComm_Trx_NoAck(byte[] packet, int txLen, int timeoutCycles, int rxLen, ushort cmd)
        {
            try
            {
                bool TxOk;
                if (txLen == 0)  //cmd only
                {
                    TxOk = UsbComm_Transmit_Command_NoAck((ushort)cmd);
                }
                else
                {
                    TxOk = UsbComm_Transmit_Packet_NoAck(packet, txLen, timeoutCycles, cmd);
                }

                if (TxOk)
                {
                    return UsbComm_Recieve_Packet_NoAck(rxLen, 20000);
                }
                else {
                    return new byte[0];
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return new byte[0];
            }

        }


        #endregion

        #region USB_TX

        /*************************  TX *******************************/
        /*
            TX_START
            TX_UPLOAD_ALL
                TX_UPLOAD
	                TX_ACK
            TX_FIN
        */

        //TX_START  - Success if ACKed
        public bool UsbComm_Trx_Upload_Start(int length)
        {
            try
            {
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];
                //send start packet
                packetBuffer[0] = (byte)(USB_CMD_NET_TX_START >> 8);
                packetBuffer[1] = (byte)USB_CMD_NET_TX_START;
                packetBuffer[2] = (byte)(length >> 8);
                packetBuffer[3] = (byte)(length & 0xFF);
                client.Write(packetBuffer, 0, 4);

                //verify response
                return UsbComm_Rx_Acked();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        //TX_UPLOAD  - Success if ACKed 
        public bool UsbComm_Transmit_Packet(byte[] packet, int len, int timeoutCycles)
        {
            try
            {
                byte[] Trx_Response = new byte[2048];
                byte[] Rx_Buffer = new byte[2048];
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];

                //transmit
                int txLeft = len;
                int txPos = 0;
                int timeout = 0;
                
                while (txLeft > 0 && !(timeout > timeoutCycles))
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
                            txbinc = txLeft;
                        }
                        //append response with header
                        packetBuffer[0] = (byte)(USB_CMD_NET_TX_PUSH >> 8);
                        packetBuffer[1] = (byte)USB_CMD_NET_TX_PUSH;
                        for (int i = 0; i < txbinc; i++)
                        {
                            packetBuffer[USB_PACKET_DATA_HEADER_SIZE + i] = packet[txPos + i];
                        }
                        client.Write(packetBuffer, 0, txbinc + USB_PACKET_DATA_HEADER_SIZE);
                        txPos += txbinc;
                        txLeft -= txbinc;
                    }
                    timeout++;
                }
                //read ack
                return ((txLeft == 0) && UsbComm_Rx_Acked());
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


        // TX_ACK?  - Success if ACK rx
        public bool UsbComm_Rx_Acked()
        {
            try
            {
                byte[] resp = UsbComm_Recieve_Packet_NoAck(2, 20000);
                if (resp != null)
                {
                    if (resp.Length >= USB_PACKET_CMD_LEN)
                    {
                        if (WordFromBufferAtPossition(resp, 0) == USB_CMD_NET_TX_ACK)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }


        public bool UsbComm_Transmit_Message(byte[] message, int len)
        {
            return UsbComm_Trx_Upload_Data(message, len);
        }

        //TX_UPLOAD_ALL
        public bool UsbComm_Trx_Upload_Data(byte[] data, int len)
        {
            try
            {
                bool result = false;
                if (UsbComm_Trx_Upload_Start(len))
                {
                    result = true;
                    //transmit 
                    byte[] packetBytes = new byte[USB_EP0_BUFF_SIZE];
                    int packets = len / USB_EP0_BUFF_SIZE;
                    for (int i = 0; i < packets; i++)
                    {
                        for (int j = 0; j < USB_EP0_BUFF_SIZE; j++)
                        {
                            packetBytes[j] = data[i * USB_EP0_BUFF_SIZE + j];
                        }
                        bool txOk = UsbComm_Transmit_Packet(packetBytes, USB_EP0_BUFF_SIZE,10);
                        if (!txOk)
                        {
                            result = false;
                        }
                    }
                    int singleBytes = len % USB_EP0_BUFF_SIZE;
                    if(singleBytes > 0)
                    {
                        for (int j = 0; j < singleBytes; j++)
                        {
                            packetBytes[j] = data[packets * USB_EP0_BUFF_SIZE + j];
                        }
                        bool txOk = UsbComm_Transmit_Packet(packetBytes, singleBytes, 1000);
                        if (!txOk)
                        {
                            result = false;
                        }
                    }
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        //TX_FIN

        #endregion




        #region USB_RX
        /*************************  RX *******************************/
        /*
            RX_START
            RX_DOWNLOAD_ALL
                RX_DOWNLOAD
	                RX_ACK
            RX_FIN
        */

        static int UsbComm_Rx_Data_Size = 0;

        //RX_START
        public bool UsbComm_Trx_Download_Start()
        {
            try
            {
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];
                byte[] StartResp = UsbComm_Trx_NoAck(packetBuffer, 0, 20000, 4, (ushort)USB_CMD_NET_RX_START);
                if (StartResp != null)
                {
                    if(StartResp.Length >= 4)
                    {
                        UsbComm_Rx_Data_Size = WordFromBufferAtPossition(StartResp, 2);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        //RX_DOWNLOAD_ALL
        public byte[] UsbComm_Trx_Download_Message(int timeoutCycles)
        {
            try
            {
                byte[] ResultData = new byte[2048];
                byte[] packetBytes = new byte[USB_EP0_BUFF_SIZE];
                if (UsbComm_Trx_Download_Start())
                {
                    int packets = UsbComm_Rx_Data_Size / USB_EP0_BUFF_SIZE;
                    for (int i = 0; i < packets; i++)
                    {
                        byte[] packetData =  UsbComm_Download_Packet(USB_EP0_BUFF_SIZE, 1000);
                        for (int j = 0; j < USB_EP0_BUFF_SIZE; j++)
                        {
                            ResultData[i * USB_EP0_BUFF_SIZE + j] = packetData[j];
                        }
                    }

                    int singleBytes = UsbComm_Rx_Data_Size % USB_EP0_BUFF_SIZE;
                    if (singleBytes > 0)
                    {
                        byte[] packetData = UsbComm_Download_Packet(singleBytes, 1000);
                        for (int j = 0; j < singleBytes; j++)
                        {
                            ResultData[packets * USB_EP0_BUFF_SIZE + j] = packetData[j];
                        }
                    }
                }

                return ResultData;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;

            }
        }


        //RX_DOWNLOAD

        //download data single packet with ack
        public byte[] UsbComm_Download_Packet(int len, int timeoutCycles)
        {
            try
            {
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];
                return UsbComm_Trx_NoAck(packetBuffer, 0,2000,len,(ushort)USB_CMD_NET_RX_PUSH);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return new byte[0];
            }
        }


        
        //RX_ACK
        public bool UsbComm_Ack_Rx()
        {
            try
            {
                byte[] packetBuffer = new byte[USB_EP0_BUFF_SIZE];
                return UsbComm_Transmit_Packet_NoAck(packetBuffer, 0, 2000, (ushort)USB_CMD_NET_RX_ACK);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


        //RX_FIN

            

        public bool UsbComm_Is_Packet_Success_For_Oper()
        {
            return false;
        }


        #endregion

        


        public bool UsbComm_Trx_Upload_Verify()
        {
            return false;
        }

        

        public bool UsbComm_Trx_Download_Verify()
        {
            return false;
        }

        ~USB()
        {
            client.Close();

        }

        public byte[] UsbComm_Trx_Message(byte[] message, int length)
        {
            try
            {
                client.Open();
                byte[] Result = new byte[2048];
                bool txed = UsbComm_Transmit_Message(message, length);
                if (txed)
                {
                    Result = UsbComm_Trx_Download_Message(20);
                }
                client.Close();
                return Result;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                client.Close();
                return new byte[0];
            }
        }

        public byte[] UsbComm_Trx_Transaction(byte[] message, int length)
        {
            try
            {

                client.WriteTimeout = 400;
                client.ReadTimeout = 400;
                client.ReadBufferSize = 2048;
                client.WriteBufferSize = 2048;

                client.Open();
                byte[] Trx_Response = new byte[2048];
                int TrxRecieved = 0;
                byte[] Rx_Buffer = new byte[2048];
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

                while (txLeft > 0 && !(timeout > 300))
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

               // client.DiscardInBuffer();

                //recieve
                int Rx_Data_Total_Left = 0;
                int rxLeft = 4;
                int rxCt = 0;
                //rx start packet
                //System.Threading.Thread.Sleep(150); //settling

                while (rxLeft > 0 && !(timeout > 300))
                {


                    //read the resp
                    int bread = 0;
                    try
                    {
                        bread = client.Read(Rx_Buffer, rxCt, rxLeft);
                    }
                    catch (Exception e)
                    {

                    }

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
                Rx_Buffer = new byte[2048];
                rxLeft = Rx_Data_Total_Left;    // >= USB_PACKET_DATA_SIZE ? USB_EP0_BUFF_SIZE : (Rx_Data_Total_Left + USB_PACKET_DATA_HEADER_SIZE);
                int btoRead = 0;
                int pbytesLeft = 0;
                client.DiscardInBuffer();

                while (rxLeft > 0 && !(timeout > 300))
                {
                    if (client.IsOpen)
                    {
                        //send read cmd
                        packetBuffer[0] = (byte)(USB_CMD_NET_RX_PUSH >> 8);
                        packetBuffer[1] = (byte)USB_CMD_NET_RX_PUSH;
                        client.Write(packetBuffer, 0, 2);

                        //calc bytes needed to read
                        if(rxLeft >= USB_PACKET_DATA_SIZE)
                        {
                            btoRead = USB_EP0_BUFF_SIZE;
                            pbytesLeft = btoRead;
                        }
                        else
                        {
                            btoRead = rxLeft + USB_PACKET_DATA_HEADER_SIZE;
                            pbytesLeft = btoRead;
                        }


                        while (pbytesLeft > 0 && !(timeout > 100))
                        {
                            int pcktRx = 0;
                            pcktRx = client.Read(Rx_Buffer, rxCt, pbytesLeft);
                            rxCt += pcktRx;
                            pbytesLeft -= pcktRx;
                            if (rxCt == btoRead)
                            {
                                //strip header and copy msg
                                for (int i = USB_PACKET_DATA_HEADER_SIZE; i < (rxCt); i++)
                                {
                                    Trx_Response[TrxRecieved] = Rx_Buffer[i];
                                    TrxRecieved++;
                                    //client.DiscardInBuffer();
                                    // (pcktRx - USB_PACKET_DATA_HEADER_SIZE);
                                    // rxLeft--;
                                }
                                rxLeft -= rxCt;
                                rxCt = 0;
                                pbytesLeft = 0;
                                btoRead = 0;
                            }
                            timeout++;

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


        #region USB_SITE_TRX

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
                byte[] resp = berkeleySendMsg(cmd, 4); //ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
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

                site.SiteStat.StartConnectionTimer();
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
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

                site.SiteStat.StartConnectionTimer();
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
                            return false;
                        }

                        return true;

                    }
                    else
                    {
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
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
                site.SiteStat.StartConnectionTimer();

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
                site.SiteStat.StartConnectionTimer();
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
                    return false;
                }
                site.SiteStat.StopConnectionTimer();
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
                site.SiteStat.StartConnectionTimer();
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
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
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        client.CloseConnection();
                        return false;
                    }
                    client.CloseConnection();
                }
                else
                {
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
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
                site.SiteStat.StartConnectionTimer();
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
                            client.CloseConnection();
                            return false;
                        }
                        client.CloseConnection();
                    }
                    else
                    {
                        client.CloseConnection();
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    client.CloseConnection();
                    _logger.Error(e, e.Message);
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
                    return false;
                }
                site.SiteStat.StopConnectionTimer();
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
            site.SiteStat.StartConnectionTimer();
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
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
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                else
                {
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
                    return false;
                }
            }
            catch (Exception e)
            {
                site.SiteStat.StopConnectionTimer();
                site.SiteStat.FailedConnections++;
                return false;
            }
            site.SiteStat.StopConnectionTimer();
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

                site.SiteStat.StartConnectionTimer();
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {

                    _logger.Error(e, e.Message);
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
                    return false;
                }
                site.SiteStat.StopConnectionTimer();
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

                site.SiteStat.StartConnectionTimer();
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
                    return false;
                }
                site.SiteStat.StopConnectionTimer();
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

                site.SiteStat.StartConnectionTimer();
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                                site.SiteStat.StopConnectionTimer();
                                site.SiteStat.FailedConnections++;
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
                            site.SiteStat.StopConnectionTimer();
                            site.SiteStat.FailedConnections++;
                            return false;
                        }
                    }
                    else
                    {
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
                    return false;
                }
                site.SiteStat.StopConnectionTimer();
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
                    //Debug.WriteLine(e.Message + " | " + e.InnerException.Message);
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

                site.SiteStat.StartConnectionTimer();
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
                    rxBF = berkeleySendMsg(site.serverIP, cmd, site.serverPort);
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
                            rxBF = berkeleySendMsg(site.serverIP, cmd, site.serverPort);
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
                        site.SiteStat.StopConnectionTimer();
                        return (bool)(deviceUploadStat & globalUploadStat);

                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, e.Message);
                        site.SiteStat.StopConnectionTimer();
                        site.SiteStat.FailedConnections++;
                        return (bool)(deviceUploadStat & globalUploadStat);
                        throw;
                    }

                }
                else
                {
                    site.SiteStat.StopConnectionTimer();
                    site.SiteStat.FailedConnections++;
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
            site.SiteStat.StartConnectionTimer();
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
                    rxBF = berkeleySendMsg(cmd, 3);

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
                rxBF = berkeleySendMsg(cmd, 2);

                cmd[1] = ipcCMD.getEvent;
                cmd[2] = (byte)1; //test event id 1
                rxBF = berkeleySendMsg(cmd, 2);

                site.SiteStat.StopConnectionTimer();

            }
            catch (Exception e)
            {
                site.SiteStat.StopConnectionTimer();
                site.SiteStat.FailedConnections++;
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
                return true;
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

        #endregion



    }
#endif






}
