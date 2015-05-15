using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace sconnConnector
{
    public class sconnCfgMngr
    {
        ETH ethernet;

        /********   Program config    ********/
        /********                      *******/
        /***     Handles Data save/read     ***/
        private  sconnDataSrc ConfigSource = new sconnDataSrc();

        public sconnCfgMngr()
        {
            ethernet = new ETH();
        }

        public bool authMCU(String server, Int32 port, String cred)
            {
                byte[] authmsg = new byte[ipcDefines.AUTH_PASSWD_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES];
                authmsg[0] = ipcCMD.SOP;
                for (int i = 0; i < cred.Length; i++)    
                {
                    authmsg[i + 1] = (byte)cred[i];
                }
                authmsg[ipcDefines.AUTH_GRP_POS + ipcDefines.AUTH_CBYTE_OFFSET] = 0x01;
                authmsg[ipcDefines.AUTH_PERM_POS + ipcDefines.AUTH_CBYTE_OFFSET] = 0x01;
                authmsg[ipcDefines.AUTH_PASSWD_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES - 1] = ipcCMD.EOP;
                byte[] status = ethernet.berkeleySendMsg(server, authmsg, port);

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
     
        public  void saveConfig()
            {
                ConfigSource.SaveConfig(DataSourceType.xml);
            }
    
        public  void loadConfig()
            {
                ConfigSource.LoadConfig(DataSourceType.xml);
            }

        public  int getSiteDeviceNo( sconnSite site)
           {
               SconnClient client = new SconnClient(site.serverIP, site.serverPort, "testpass", true);
                    int sites = 0;
                    byte[] cmd = new byte[ipcDefines.NET_CMD_PACKET_LEN];
                    cmd[0] = ipcCMD.GET;
                    cmd[1] = ipcCMD.getDevNo;
                    byte[] resp = client.berkeleySendMsg(cmd); //ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
                    if (resp[0] == ipcCMD.SVAL)
                    {
                        sites = (int)resp[2]; // second byte is data,  <SVAL> <DATA> <EVAL>                  
                    }
                    client.CloseConnection();
                    return sites;
            }

        public bool WriteGlobalCfg(sconnSite site)
        {
            if (site.siteCfg == null){ return false;}
            SconnClient client = new SconnClient(site.serverIP, site.serverPort, "testpass", true);

                    site.siteStat.StartConnectionTimer();
                    short siteMemAddr = (short)(ipcDefines.mAdrGlobalConfig);
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
                            rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

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

                                    rxBF = client.berkeleySendMsg(txBF, bfSize);
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
                                    rxBF = client.berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
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
                                rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
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

                    return false;
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


        public bool WriteDeviceCfgSingle(sconnSite site, int DevId)
        {
            if (site.siteCfg == null)
            {
                return false;
            }

            SconnClient client = new SconnClient(site.serverIP, site.serverPort, "testpass", true);
            site.siteStat.StartConnectionTimer();
            short siteMemAddr = (short)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
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

                    rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN); // ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.NET_CMD_PACKET_LEN);

                    if (rxBF[0] == ipcCMD.ACK)
                    {
                        int fullTxNo = (int)ipcDefines.deviceConfigSize / packetData;
                        int signleBytes = (int)ipcDefines.deviceConfigSize % packetData;

                        txBF[0] = ipcCMD.PSH;
                        txBF[1] = ipcCMD.PSHNXT;
                        txBF[2] = ipcCMD.SVAL;
                        int packetLastByteIndex = bytesToSend > packetData ? bfSize-1 : ipcDefines.deviceConfigSize + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
                        txBF[packetLastByteIndex] = ipcCMD.EVAL;
                        for (int j = 0; j < fullTxNo; j++)
                        {
                            int startAddr = j * packetData;
                            for (int k = 0; k < packetData; k++)
                            {
                                txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].memCFG[(startAddr + k)];
                            }

                            rxBF = client.berkeleySendMsg(txBF, bfSize); 
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
                            rxBF = client.berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
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
                        rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
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


        public bool WriteDeviceNetCfg(sconnSite site, int DevId)
        {
            if (site.siteCfg == null)
            {
                return false;
            }

            SconnClient client = new SconnClient(site.serverIP, site.serverPort, "testpass", true);
            site.siteStat.StartConnectionTimer();
            //short siteMemAddr = (short)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
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

                rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN); // ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.NET_CMD_PACKET_LEN);

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

                        rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CFG_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
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
                        rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CFG_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
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
                    rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
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
            if (site.siteCfg == null)
            {
                return false;
            }
            SconnClient client = new SconnClient(site.serverIP, site.serverPort, "testpass", true);

            site.siteStat.StartConnectionTimer();
            short siteMemAddr = (short)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
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

                rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

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
                            { nameinc++; namecharinc=0;}
                            txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NamesCFG[nameinc][namecharinc]; //site.siteCfg.deviceConfigs[DevId].memCFG[(startAddr + k)];
                            namecharinc++;
                        }

                        rxBF = client.berkeleySendMsg(txBF, bfSize);
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
                            { nameinc++;  namecharinc=0;}
                            txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NamesCFG[nameinc][namecharinc];
                            namecharinc++;
                        }
                        rxBF = client.berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

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
                    rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
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


        public bool WriteDeviceSchedulesCfgSingle(sconnSite site, int DevId)
        {
            if (site.siteCfg == null)
            {
                return false;
            }
            SconnClient client = new SconnClient(site.serverIP, site.serverPort, "testpass", true);

            site.siteStat.StartConnectionTimer();
            short siteMemAddr = (short)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
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

                rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

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

                        rxBF = client.berkeleySendMsg(txBF, bfSize);
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
                        rxBF = client.berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

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
                    rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
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

       public  bool updateSiteStatus(sconnSite site) //read I/O values
            {
                int devices = 0;
                if (site.siteCfg != null)
                {
                    devices = site.siteCfg.deviceNo;
                }
                else
                {
                    devices = getSiteDeviceNo(  site);
                    site.siteCfg = new ipcSiteConfig(devices); //init device configs 
                }

                short siteMemAddr = (short)(ipcDefines.mAdrDevStart);
                try
                {
                    for (int i = 0; i < devices; i++)
                    {
                        siteMemAddr = (short)(ipcDefines.mAdrDevStart + (i * ipcDefines.deviceConfigSize)); //device start addresss
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
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

       public bool ReadSiteStartupConfig(sconnSite site)
        {
            site.siteStat.StartConnectionTimer();
            bool globalUploadStat = false;
            bool deviceUploadStat = false;
            int devices = 0;
            byte[] cmd = new byte[32];

            if (authMCU(site.serverIP, site.serverPort,  site.authPasswd))  //update only if authed to site
            {

                if (site.siteCfg != null)
                {
                    devices = site.siteCfg.deviceNo;
                }
                else
                {
                    devices = getSiteDeviceNo(  site);
                    site.siteCfg = new ipcSiteConfig(devices); //init device configs 
                }

                /**********  Read global config  ***********/


                short siteMemAddr = (short)(ipcDefines.mAdrGlobalConfig);
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
                siteMemAddr = (short)(ipcDefines.mAdrDevStart);
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
                catch (Exception)
                {
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

       public bool ReadSiteRunningConfig(sconnSite site)
       {
           site.siteStat.StartConnectionTimer();
           bool globalUploadStat = false;
           bool deviceUploadStat = false;
           int devices = 0;
           byte[] cmd = new byte[32];
           SconnClient client = new SconnClient(site.serverIP, site.serverPort, "testpass",true);

               if (site.siteCfg != null)
               {
                   devices = getSiteDeviceNo(site);
               }
               else
               {
                   devices = getSiteDeviceNo(  site);
                   site.siteCfg = new ipcSiteConfig(devices); //init device configs 
               }

               /**********  Read global config  ***********/

               short siteMemAddr = (short)(ipcDefines.mAdrGlobalConfig);
               byte[] rxBF = new byte[ipcDefines.ipcGlobalConfigSize + 2];

               cmd[0] = ipcCMD.GET;
               cmd[1] = ipcCMD.getRunGlobCfg;
               rxBF = client.berkeleySendMsg(cmd);     //    ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);

               if (rxBF[0] == ipcCMD.SVAL)
               {
                   for (int j = 0; j < ipcDefines.ipcGlobalConfigSize; j++)
                   {
                       site.siteCfg.globalConfig.memCFG[j] = rxBF[j + 1];
                   }
                   globalUploadStat = true;

                   devices = rxBF[2];
               }

               /**********  Get device configs **********/

               cmd = new byte[32];
               rxBF = new byte[ipcDefines.deviceConfigSize + 2];
               byte[] NamesBF = new byte[ipcDefines.RAM_DEVICE_NAMES_SIZE];
               byte[] SchedBF = new byte[ipcDefines.RAM_DEV_SCHED_SIZE];
               siteMemAddr = (short)(ipcDefines.mAdrDevStart);
               int MsgByteOffset = 1;

               try
               {
                   for (int i = 0; i < devices; i++)
                   {
                       /*******  Get Device Config  ****/
                       cmd[0] = ipcCMD.GET;
                       cmd[1] = ipcCMD.getRunDevCfg;
                       cmd[2] = (byte)i; //device number
                       rxBF = client.berkeleySendMsg(cmd);

                       /*******  Get Device Names  ****/
                       cmd[1] = ipcCMD.getNamesDevCfg;
                       NamesBF = client.berkeleySendMsg(cmd);

                       /*****  Get Device Schedules ******/
                       cmd[1] = ipcCMD.getSchedulesDevCfg;
                       SchedBF = client.berkeleySendMsg(cmd);

                       if (rxBF[0] == ipcCMD.SVAL)
                       {
                           deviceUploadStat = true;
                                
                           //read device config
                           for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
                           {
                               site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + MsgByteOffset];
                           }

                           //read names config
                           if (NamesBF[0] == ipcCMD.SVAL)
                           {
                               for (int name = 0; name < ipcDefines.RAM_DEV_NAMES_NO; name++)
                               {
                                   for (int txtbyte = 0; txtbyte < ipcDefines.RAM_NAME_SIZE; txtbyte++)
                                   {
                                       if (NamesBF[(name * ipcDefines.RAM_NAME_SIZE) + txtbyte] == (byte)32) //decode space sign
                                        {
                                            NamesBF[(name * ipcDefines.RAM_NAME_SIZE) + txtbyte] = 0; 
                                        }
                                       site.siteCfg.deviceConfigs[i].NamesCFG[name][txtbyte] = NamesBF[(name * ipcDefines.RAM_NAME_SIZE) + txtbyte + MsgByteOffset]; //1 byte buffer offset
                                   }
                               }
                           }
                           
                           //read  schedule config
                           if (SchedBF[0] == ipcCMD.SVAL)
                           {
                               for (int schedule = 0; schedule < ipcDefines.RAM_DEV_SCHED_NO; schedule++)
                               {
                                   for (int schedbyte = 0; schedbyte < ipcDefines.RAM_DEV_SCHED_MEM_SIZE; schedbyte++)
                                   {
                                       site.siteCfg.deviceConfigs[i].ScheduleCFG[schedule][schedbyte] = NamesBF[(schedule * ipcDefines.RAM_DEV_SCHED_MEM_SIZE) + schedbyte + MsgByteOffset]; //1 byte buffer offset
                                   }
                               } 
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
                   rxBF = client.berkeleySendMsg(cmd);
                                    
                   cmd[1] = ipcCMD.getEvent;
                   cmd[2] = (byte)1; //test event id 1
                   rxBF = client.berkeleySendMsg(cmd);

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

        public bool  updateSiteConfig( sconnSite site) //read entire running config
            {
                   return ReadSiteRunningConfig( site);      
            }    
    }
}
