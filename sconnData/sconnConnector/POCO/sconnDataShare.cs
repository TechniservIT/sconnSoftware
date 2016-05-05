using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using sconnConnector.POCO.Config;

#if WIN32_ENC

using System.Windows;
using System.Windows.Input;
using System.Timers;
using sconnConnector.Config;

#endif

namespace sconnConnector
{

    public static class sconnDataShare
    {
        private static sconnDataSrc _dataSrc;

        public static DataSourceType DataSourceType { get; set; }

        public static int siteNo 
        {
            get { return sconnSites.Count; }
        }
        
        public static XmlDocument getConfigXML()
        {
            XmlDocument configDoc = new XmlDocument();
            return configDoc;
        }

        static sconnDataShare()
        {
            _dataSrc = new sconnDataSrc();
            DataSourceType = DataSourceType.xml;
            sconnDataShare.sconnSites = new ObservableCollection<sconnSite>(_dataSrc.GetSites(DataSourceType));
        }

        public static void Reload()
        {
            sconnDataShare.sconnSites = new ObservableCollection<sconnSite>(_dataSrc.GetSites(DataSourceType));
        }

        public static void Save()
        {
            _dataSrc.SaveConfig(DataSourceType);
        }

        //static sconnDataShare()
        //{
        //    _dataSrc = new sconnDataSrc();
        //}
        public static sconnSite getSite(int id)
        {
            if (id < sconnDataShare.sconnSites.Count)
            {
                return sconnDataShare.sconnSites[id];
            }
            else
            {
                return null;
            }
           
        }

        public static bool updateSite(sconnSite site)
        {
            if (site != null)
            {
                sconnSite existing = sconnSites.FirstOrDefault(s => s.Id.Equals(site.Id));
                if (existing != null)
                {
                    existing.CopyFrom(site);
                    Save();
                    return true;
                }
                else
                {
                    sconnSites.Add(site);
                    return true;
                }
            }
            return false;
            //foreach (sconnSite site in sconnSites)
            //{
            //    if (site.Id.Equals(updated.Id))
            //    {
            //        site.CopyFrom(updated);
            //    }
            //}
            //return false;
        }

        public static bool addSite(sconnSite site)
        {
            sconnDataShare.sconnSites.Add(site);
            return true;
        }
        
        public static bool addSite(string hostname, int port, string password, string siteName)
        {
            if (hostname == null ||  password == null)
            {
                return false;
            }
            sconnSite site = new sconnSite(siteName,1000, hostname, port, password );
            sconnSites.Add(site);
            return true;
        }
        
        static private bool _SiteLiveViewEnabled = true;
        public static bool SiteLiveViewEnabled { get { return _SiteLiveViewEnabled; } set { _SiteLiveViewEnabled = value; } }
        
        /********  Handles application data between View, Tasker and Mngr/Src , with R/W and R access  ************/
        static bool updatePending = false; //signal UI threads update is in progress and status should not be read
      //  static public ObservableCollection<sconnSite> sconnSites = new ObservableCollection<sconnSite>();
        static public ObservableCollection<sconnSite> sconnSites { get; set; }

        #region sitestack

        private class siteStack : IEnumerable
        {
            public sconnSite[] sites;
            private int siteInc = 0;
            private int stackMaxSize = 1000;

            public int siteNo
            {
                get { return siteInc; }
            }

            public siteStack()
            {
                sites = new sconnSite[0];
            }

            public void Push(sconnSite site)
            {
                siteInc++;
                if (siteInc >= stackMaxSize)
                {
                    throw new StackOverflowException();
                }
                else
                {
                    sconnSite[] newSites = new sconnSite[siteInc];
                    if (siteInc > 0)
                    {
                        sites.CopyTo(newSites, 0);
                    }
                    sites = newSites;
                    sites[siteInc - 1] = site;

                }
            }

            public sconnSite Pop()
            {
                siteInc--;
                if (siteInc <= 0)
                {
                    throw new InsufficientExecutionStackException();
                }
                else
                {
                    sconnSite poped = sites[siteInc];
                    sconnSite[] newSites = new sconnSite[siteInc];
                    sites.CopyTo(newSites, 0);
                    return poped;
                }
            }

            public bool RemoveAt(int siteIndex)
            {
                if ((siteIndex > siteInc) || (siteIndex < 0))
                {
                    return false; //cannot remove item over stack boundaries
                }
                try
                {
                    for (int i = siteIndex; i < siteInc - 1; i++)
                    {
                        sites[i] = sites[i + 1];
                    }
                    siteInc--;
                    sconnSite[] newSites = new sconnSite[siteInc];
                    for (int i = 0; i < siteInc; i++)
                    {
                        newSites[i] = sites[i];
                    }
                    sites = newSites;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }

            private siteStackEnum GetEnumerator()
            {
                return new siteStackEnum(sites);
            }

        }
        private class siteStackEnum : IEnumerator
        {
            public sconnSite[] sconnSites;
            private int enumPosition = -1;

            public siteStackEnum(sconnSite[] sites)
            {
                sconnSites = sites;
            }

            /******  Enumaration ******/
            public bool MoveNext()
            {
                enumPosition++;
                return (enumPosition < sconnSites.GetLength(0));
            }

            public void Reset()
            { enumPosition = -1; }

            public object Current
            {
                get { return sconnSites[enumPosition]; }
            }

            public IEnumerator GetEnumerator()
            {
                return (IEnumerator)this;
            }
        }
        #endregion

    }
    

        /***********   MEMORY       ***********/

        public struct memPage
        {
            public byte[] memPageData;
        };


        public struct memGlobalCFG
        {
            public byte[] memPage;
        };


        public struct memDeviceCFG
        {
            public byte[] memPage;
        };


        //command type definitions
        public enum ipcCMDcom
        {

            i2cCMD = 1, spiCMD = 2, miwiCMD = 3, usbCMD = 4, ethCMD = 5
        };
    
        public enum ipcROLE
        {
            ipcCore = 0, ipcSens = 1, ipcIO = 2, ipcMotor = 3 //S = sensor, IO = io device,  CD = communication device
        };

        public enum ipcAuthType
        {
            authNone = 0, authPass = 1
        };

        public struct ipcAddr
        {
            public byte domain;        //domain number of address, 255 possible domains in system
            public ipcROLE role;       //role in systen
            public byte address;        //1-byte device no in sequence

        };


        public struct authCred
        {
            public byte[] password;        //domain number of address, 255 possible domains in system
        };



        public struct ipcMsg
        {
            public authCred cred;
            public ipcAddr target;
            public ipcROLE role;
            public ipcCMDcom com;
            public byte[] msg;

        };

        public enum upCfgState
        {
            CFG_AUTH = 0,
            CFG_SEND,
            CFG_DONE
        };




        /*********    Device Info        ***********/


        /* IDs of boards*/
        public enum ipcDevID
        {
            IOv1 = 0, Sensorv2, Communicationv1, Motorv1
        } ;


        /*Revision number*/
        public enum ipcDevRev
        {
            Rev1 = 1, Rev2, Rev3, Rev4
        } ;

        public struct ipcDeviceType
        {
            public ipcDevID boardID;
            public ipcDevRev boardRev;
        };




        /***********  Config  Adresses    ************/

        public struct eeAddr256K
        {
            public char addrHigh;
            public char addrLow;
        };  // memory address for up to 256-kbyte memory

        public struct eeAddr512K
        {
            public char addrHigh;
            public char addrLow;
        };  // memory address for up to 512-kbyte memory

   
    public struct macADDR
    {
        byte VEND1;
        byte VEND2;
        byte VEND3;
        byte DEV1;
        byte DEV2;
        byte DEV3;

        public macADDR(byte VEND1addr, byte VEND2addr, byte VEND3addr, byte DEV1addr, byte DEV2addr, byte DEV3addr)
        {
            VEND1 = VEND1addr;
            VEND2 = VEND2addr;
            VEND3 = VEND3addr;
            DEV1 = DEV1addr;
            DEV2 = DEV2addr;
            DEV3 = DEV3addr;
        }

        public macADDR(byte DEV1addr, byte DEV2addr, byte DEV3addr)
        {
            VEND1 = 0x00;   //microchip
            VEND2 = 0x04;
            VEND3 = 0xA3;
            DEV1 = DEV1addr;
            DEV2 = DEV2addr;
            DEV3 = DEV3addr;
        }


        public static bool operator ==(macADDR m1, macADDR m2)
        {
            if ((m1.DEV1 == m2.DEV1) && (m1.DEV2 == m2.DEV2) && (m1.DEV3 == m2.DEV3))
            { return true; }
            else { return false; }
        }

        public static bool operator !=(macADDR m1, macADDR m2)
        {
            if ((m1.DEV1 != m2.DEV1) || (m1.DEV2 != m2.DEV2) || (m1.DEV3 != m2.DEV3))
            { return true; }
            else { return false; }
        }

        
        public override int GetHashCode()
        {
            int addr = 0;
            addr = addr| DEV1;
            addr  = addr | (DEV2 <<8);
            addr  = addr | (DEV2 <<16);
            return addr;
        }

        public override bool Equals(object obj)
        {
                return base.Equals(obj);
        }

    }




    /*********  Static defines and data types ***********/

    public struct ipcCMD
    {

        //ASCII proprietary CMDs
        public const byte STX = 0x02; //start text
        public const byte ETX = 0x03; //end text
        public const byte EOT = 0x04; // end of transmission

        public const byte SVAL = 0x07; //start of value bytes
        public const byte EVAL = 0x08; //end value

        //Custom CMDs replacing ASCII
        public const byte SET = 0x05; // set following register group value
        public const byte setRegVal = 0x51;
        public const byte setGlobalCfg = 0x52;
        public const byte setDeviceCfg = 0x53;
        public const byte setDeviceNamesCfg = 0x54;
        public const byte setDeviceSchedulesCfg = 0x57;
        public const byte setPasswdCfg = 0x58;
        public const byte setDeviceNetworkCfg = 0x59;
        public const byte setGsmRcptCfg = 0x60;
        public const byte setAuthDevCfg = 0x61;
        public const byte setGlobalNames = 0x62;
        public const byte setZoneName = 0x63;
        public const byte setName = 0x64;

        public const byte GET = 0x06; // get following register group value
        public const byte getRegVal = 0x61; //retrieve single register byte value    Param :   Register Address (2b)
        public const byte getDevNo = 0x62; //get number of devices in system to map memory addresses Param : None
        public const byte getDevCfg = 0x63;
        public const byte getGlobCfg = 0x64;
        public const byte getRunGlobCfg = 0x65;
        public const byte getRunDevCfg = 0x66;
        public const byte getArmStatus = 0x69;
        public const byte getInputState = 0x6A;
        public const byte getOutputState  = 0x6B;
        public const byte getNamesDevCfg = 0x67;
        public const byte getSchedulesDevCfg = 0x6C;
        public const byte getPasswdCfg =  0x6D;
        public const byte getEvents =  0x6E;
        public const byte getEventNo =  0x6F;
        public const byte getEvent  = 0x70;
        public const byte getDevNetCfg = 0x71;
        public const byte getGsmRecpCfg = 0x72;
        public const byte getAuthDevices = 0x73;
        public const byte getGsmModemResponse = 0x74;
        public const byte getDeviceName = 0x75;
        public const byte getGlobalNames = 0x76;
        public const byte getConfigHash = 0x77;
        public const byte getZoneCfg = 0x78;
        public const byte getZoneName = 0x79;
        public const byte getName = 0x80;

        public const byte CFG = 0x11; //register to set, followed by value SVAL <value bytes > EVAL

        //groups of registers to be set
        public const byte IO = 0x10; //output port
        public const byte TM = 0x11; //timer to set

        //groups of get register
        public const byte ADC = 0x12; //directly read input value
        public const byte UI = 0x15; //Universal Input , specific for each slave device, NO/NC digital inputs, returns 1 / 0, 0 = normal state
        public const byte TMP = 0x13; //temperature indicator value, 2 bytes are sent : signed decimal , unsigned 2digit precision
        public const byte HUM = 0x14; //humidity % indicator value, 2 bytes are sent : signed decimal , unsigned 2digit precision
        //Following group code is string with peripherial name in format :  STX <name> ETX


        public const byte SOP = 0x16;  //Start of Password ,following is password string
        public const byte EOP = 0x17;   //password end
        public const byte AUTHOK = 0x18;
        public const byte AUTHFAIL = 0x19;
        public const byte ACK = 0x20;
        public const byte ACKNXT = 0x21;
        public const byte ACKFIN = 0x22;
        public const byte ERRCMD = 0x21;
        public const byte PSH = 0x22;
        public const byte PSHNXT = 0x23;
        public const byte PSHFIN = 0x24;
        public const byte OVF = 0x25;

        /********  Unique Device ID    *********/
        public const byte DevID0 = 0x01;
        public const byte DevID1 = 0x00;

    }

    public struct ipcDefines
    {

        public const int SHA256_DIGEST_SIZE = 32;

        public const byte comI2C = 0x01;
        public const byte comSPI = 0x02;
        public const byte comMiWi = 0x03;
        public const byte comUSB = 0x04;
        public const byte comETH = 0x05;


        public const byte mAdrCfgSingleLen = 0x01;


        //public const int deviceConfigSize = 256;  //256 bytes
        public const int ipcMaxDevices = 8;

        /*
         256 Bytes for each devies ( 1024 devies @ 256Kb mem)
         * 64 bytes device config/info
         * 128 inputs state
         * 64 output state
         */


        /*
         Address locations are added to device start address
         First 1Kbyte is reserved for general config, so each device start addr is increased by 1024
         *
         * 
         */
        public const int ipcAbsMaxDevices = 16;
        public const int RAM_DEVCFG_NO = 16;
        public const int RAM_DEVCFG_SIZE = 512;
        public const int RAM_GCFG_SIZE = 1024;



        public const int mAdrDevID_LEN = 0x0002;

        /********    Global Config      **********/
        public const int ipcGlobalConfigSize = 1024;
        public const int mAdrDevNO = 0x0000; //number of devices

        public const int mAdrArmed =0x0002;
        public const int mAdrViolation= 0x0003;
        public const int mAdrDeamonType = 0x0004;
        public const int mAdrDeamonType_LEN = 0x0001;

        public const int mAdrHostDeviceAddr = (mAdrDeamonType + mAdrDeamonType_LEN);      //system master dev id
        public const int mAdrHostDeviceAddr_LEN = mAdrDevID_LEN;

        public const int mAdrUserNo       =  (mAdrHostDeviceAddr+mAdrHostDeviceAddr_LEN);  // current number of registered users in PASSWD
        public const int mAdrUserNo_LEN  =    0x0002;


        public const int mAdrCurrDevIncPos     =  (mAdrUserNo+mAdrUserNo_LEN);
        public const int mAdrCurrDevInc_LEN  =    mAdrDevID_LEN;

        public const int mAdrHostDeviceSystemId    = (mAdrCurrDevIncPos+mAdrCurrDevInc_LEN);
        public const int mAdrHostDeviceSystemId_LEN = mAdrDevID_LEN;

        public const int mAdrHostDeviceIsSystemConnected= (mAdrHostDeviceSystemId+mAdrHostDeviceSystemId_LEN);
        public const int mAdrHostDeviceIsSystemConnected_LEN =0x01;

        public const int mAdrHostDeviceServerId = (mAdrHostDeviceIsSystemConnected+mAdrHostDeviceIsSystemConnected_LEN);
        public const int mAdrHostDeviceServerId_LEN = 0x02;


        public const int mAdrHostDeviceServerIpAddr = (mAdrHostDeviceServerId+mAdrHostDeviceServerId_LEN);
        public const int mAdrHostDeviceServerIpAddr_LEN =  30;

        public const int mAdrHostDeviceServerPasswd = (mAdrHostDeviceServerId+mAdrHostDeviceServerId_LEN);
        public const int mAdrHostDeviceServerPasswd_LEN = 30;

        public const int mAdrSysFail = (mAdrHostDeviceServerPasswd+mAdrHostDeviceServerPasswd_LEN);
        public const int mAdrSysFail_LEN  = 0x01;
      
        public const int mAdrZoneNo = (mAdrSysFail + mAdrSysFail_LEN);
        public const int mAdrZoneNo_LEN = 0x01;

        public const int mAdrZoneNo_Pos = (0x00);

        public const int mAdrZoneCfgStartAddr = (mAdrZoneNo + mAdrZoneNo_LEN);

        public const int ZONE_CFG_ENABLED_POS = (mAdrZoneNo_LEN);
        public const int ZONE_CFG_ENABLED_LEN = 0x01;

        public const int ZONE_CFG_TYPE_POS = (ZONE_CFG_ENABLED_POS + ZONE_CFG_ENABLED_LEN);
        public const int ZONE_CFG_TYPE_LEN = 0x01;

        public const int ZONE_CFG_NAME_ID_POS = (ZONE_CFG_TYPE_POS + ZONE_CFG_TYPE_LEN);
        public const int ZONE_CFG_NAME_ID_LEN = 0x02;

        public const int ZONE_CFG_LEN = (ZONE_CFG_NAME_ID_POS + ZONE_CFG_NAME_ID_LEN);

        public const int ZONE_CFG_MAX_ZONES = 32;

        public const int ZONE_CFG_TOTAL_LEN = (ZONE_CFG_MAX_ZONES*ZONE_CFG_LEN);
       

        public const int GSM_GCFG_LOG_LVL_START_ADDR = (mAdrZoneCfgStartAddr+ZONE_CFG_MAX_ZONES*ZONE_CFG_LEN);
        public const int GSM_GCFG_LOG_LVL_LEN = 0x04;

        public const int GSM_GCFG_LOG_LVL_EN_ARM_CHANGE_MASK   =   0x01;
        public const int GSM_GCFG_LOG_LVL_EN_VIO_MASK         =    0x02;
        public const int GSM_GCFG_LOG_LVL_EN_PWR_STAT_MASK    =    0x04;
        public const int GSM_GCFG_LOG_LVL_EN_ENTRENCE_MASK    =    0x08;

        public const int mAdrGeo_Pos     =   (GSM_GCFG_LOG_LVL_START_ADDR+GSM_GCFG_LOG_LVL_LEN);
        public const int mAdrLATd_Pos    =       0x00; //degrees
        public const int mAdrLATm_Pos     =      0x01; //minutes
        public const int mAdrLATs_Pos     =      0x02; //seconds
        public const int mAdrLNGd_Pos      =     0x03; //degrees
        public const int mAdrLNGm_Pos      =     0x04; //minutes
        public const int mAdrLNGs_Pos      =     0x05; //seconds
        public const int mAdrGeo_Pos_LEN =  0x06;


        public const int GCFG_HASH_POS  =  (mAdrGeo_Pos+mAdrGeo_Pos_LEN);
        public const int GCFG_HASH_LEN  =  SHA256_DIGEST_SIZE;

        public const int GCFG_DEV_MOD_CTR_START_POS = (GCFG_HASH_POS+GCFG_HASH_LEN);
        public const int GCFG_DEV_MOD_CTR_LEN = 4;  //integer value on config revision
        public const int GCFG_DEV_MOD_CTR_TOTAL_LEN = (GCFG_DEV_MOD_CTR_LEN * ipcAbsMaxDevices);

        public const int GCFG_NAMES_MOD_CTR_POS  =    (GCFG_DEV_MOD_CTR_START_POS+GCFG_DEV_MOD_CTR_TOTAL_LEN);
        public const int GCFG_NAMES_MOD_CTR_LEN = SHA256_DIGEST_SIZE;

        public const int GCFG_END_REG            =    (GCFG_NAMES_MOD_CTR_POS+GCFG_NAMES_MOD_CTR_LEN);


        public const int mAdrDevStart = 0x400; //1024- start address in memory of device configs
        public const int mAdrGlobalConfig = 0x0000;




        public const int mAdrSiteName = 0x0040; //16 char UTF8(4b) site nam
        public const int mAdrSitePasswd = 0x0080; // 32char UTF8 password
        public const int PasswordMaxChars = 32;
        public const int PasswordSize = 128; //32char 128 byte

        public const byte sysVersion = 0x00;

        /********  Device config  ********/
       
        public const byte mAdrDevID = 0x00;    //device unique ID , for I2C it is also bus Address

        public const byte mAdrDomain = 0x02;    //device domain
        public const byte mAdrDevRev = 0x03;   //Revision number   
        public const byte mAdrDevType = 0x04;  //Type of device, IO, motor etc.
        public const byte mAdrInputsNO = 0x05;  //number of inputs on board
        public const byte mAdrOutputsNO = 0x06; //number of outputs on board
        public const byte mAdrRelayNO = 0x07; //number of Relays on board
        public const byte mAdrKeypadMod = 0x08; //devices has KeyPad module
        public const byte mAdrTempMod = 0x09; // device has temperature module ( bool )
        public const byte mAdrHumMod = 0x0A;   //humidity
        public const byte mAdrPresMod = 0xB;  //pressure
        public const byte mAdrCOMi2c = 0xC;  //device has I2C COM , bool
        public const byte mAdrCOMeth = 0xD;  //device has ETH COM
        public const byte mAdrCOMmiwi = 0xE;  //device has MiWi COM
        public const byte mAdrI2CAddr = 0xF; //i2c bus address

        public const byte mAdrSensorBattLvl = 0xF;

        public const byte mAdrDevArmState = (mAdrI2CAddr + mAdrCfgSingleLen);
        public const byte mAdrDevViolationState = (mAdrDevArmState + mAdrCfgSingleLen);
        public const byte mAdrDevFailureState = (mAdrDevViolationState + mAdrCfgSingleLen);
        public const byte mAdrDeviceZone = (mAdrDevFailureState + mAdrCfgSingleLen);
        public const byte mAdrDeviceZone_LEN =  0x02;

        /********  Input state  ********/
        public const int mAdrInput = 0x20;  //128 - start address of input states, format : <input type> <value1> <value2-Analog>
        public const byte mAdrInputMemSize = 0x08;
        public const byte mAdrInputType = 0x00;
        public const byte mAdrInputAG = 0x01;
        public const byte mAdrInputVal = 0x02;
        public const byte mAdrInputSensitivity = 0x03;
        public const byte mAdrInputEnabled = 0x04;
        public const byte mAdrInputNameAddr = 0x05;
        public const byte mAdrInputZoneId = 0x06;
        public const byte mAdrInputTypeParam = 0x07;


        public const byte DeviceMaxInputs = 24;
        public const byte InputSensitivityStep = 50;


        /********  Output state  ********/
        public const int mAdrOutput = (mAdrInput + mAdrInputMemSize * DeviceMaxInputs);  //40 - start address of output states, format : <output type> <value1>
        public const byte mAdrOutputType = 0x00;
        public const byte mAdrOutputVal = 0x01;
        public const byte mAdrOutputEnabled = 0x02;
        public const byte mAdrOutputNameAddr = 0x03;
        public const byte mAdrOutputPar1 = 0x04;

        public const byte mAdrOutputMemSize = 0x05;

        public const byte outputON = 0x01;  //Output active
        public const byte outputOFF = 0x00;  //Output inactive

        public const byte OutputNA = 0x01;
        public const byte OutputNIA = 0x00; // normaly inactive

        public const byte Output1Addr = 0x40;
        public const byte Output2Addr = 0x43;

        public const byte DeviceMaxOutputs = 16;
        public const int OutputsTotalMemSize = (mAdrOutputMemSize * DeviceMaxOutputs);

        /******   Relay state ********/
        public const int mAdrRelay = (mAdrOutput + mAdrOutputMemSize * DeviceMaxOutputs);
        public const byte mAdrRelayType = 0x00;
        public const byte mAdrRelayVal = 0x01;
        public const byte mAdrRelayEnabled = 0x02;
        public const byte mAdrRelayNameAddr = 0x03;
        public const byte mAdrRelayPar1 = 0x04;
        public const byte DeviceMaxRelays = 8;
        public const byte RelayMemSize = 0x05;
        public const int mAdrRelayMemSize = (RelayMemSize * DeviceMaxRelays);

        public const int RelayTotalMemSize = (RelayMemSize*DeviceMaxRelays);

        public const int mAdrSuppVolt_Start_Pos = (mAdrRelay + RelayTotalMemSize);
        public const int mAdrSuppVolt_Start_Len = (4);
        public const int mAdrBackupVolt_Start_Pos = (mAdrSuppVolt_Start_Pos + mAdrSuppVolt_Start_Len);
        public const int mAdrBackupVolt_Start_Len = (4);

       // public const int deviceTotalNames = ();
        public const int deviceConfigSize = 512;   //  mAdrRelay + (RelayMemSize * DeviceMaxRelays);

  

        /******* Device types ***********/

        public const int IPC_DEV_TYPE_GKP =   0x01;
        public const int REV_HW_CFG_GKP_32MX_ETH   =  0x01;
        public const int REV_HW_CFG_GKPNG_32MX      = 0x02;
        public const int REV_HW_CFG_GKP_32MZ_WIFI    =0x11;
        public const int IPC_DEV_TYPE_MB     =0x02;
        public const int REV_HW_CFG_MB_32MX_ETH =     0x01;
        public const int REV_HW_CFG_MB_32MX_ETH_GSM = 0x02;
        public const int REV_HW_CFG_MB_32MZ         = 0x03;
        public const int REV_HW_CFG_MB_32MX_NG      = 0x04;
        public const int IPC_DEV_TYPE_GSM    = 0x03;
        public const int REV_HW_CFG_GSM_32MX250_ETH =     0x01;
        public const int REV_HW_CFG_GSM_32MX270_ETH =     0x02;
        public const int IPC_DEV_TYPE_PIR_SENSOR   =  0x04;
        public const int REV_HW_CFG_SENS_18F46K20   =   0x01;
        public const int IPC_DEV_TYPE_RM   =  0x05;
        public const int REV_HW_CFG_RM_32M795_ETH = 0x01;







        /******  DEAMON  *******/

        /***** DEAMON TYPES  *****/
        public const byte DEA_ALARM      =     0x00;
        public const byte DEA_SCHED     =      0x01;
        public const byte DEA_MAN       =      0x02;
        public const byte DEA_ALARM_SCHED = 0x03;     /* Alarm deamon with scheduled events*/


        /********* ALARM DEAMON ********/

        /*  INPUT  TYPE        */
        public const byte IN_TYPE_NO        =  0x00;
        public const byte IN_TYPE_NC     =     0x01;

        /*  INPUT  ACTIVATION GROUP    */
        public const byte IN_TYPE_ARM      =   0x40; /* Arm System */
        public const byte IN_TYPE_DISARM   =   0x41;
        public const byte IN_TYPE_AO1     =    0x42; /* Activate Output 1*/
        public const byte IN_TYPE_AO2     =    0x43;
        public const byte IN_TYPE_AV      =    0x44; /* Armed Violation  - Alarm input */
        public const byte IN_TYPE_DAV     =    0x45; /* DisArmed Violation -  Alarm on disarmed */
        public const byte IN_TYPE_ADV     =    0x46; /*Arm&Disarmed violation - 24/7 */

        public const byte ACTIVE    =  0x01;
        public const byte INACTIVE   =  0x00;

        /* INPUT VALUE  */
        public const byte IN_HIGH               =0x01;
        public const byte IN_LOW               = 0x00;

        /*  OUTPUT  TYPE        */
        public const byte OUT_TYPE_NA              =   0x00; /* Normaly Active*/
        public const byte OUT_TYPE_NIA            =    0x01; /* Normaly InActive*/

        public const byte OUT_TYPE_ALARM_NA        =   0x00;
        public const byte OUT_TYPE_ALARM_NIA       =   0x01;
        public const byte OUT_TYPE_PWR             =   0x02;
        public const byte OUT_TYPE_SCHED_1_NA      =   0x03;
        public const byte OUT_TYPE_SCHED_1_NIA     =   0x04;
        public const byte OUT_TYPE_SCHED_2_NA      =   0x05;
        public const byte OUT_TYPE_SCHED_2_NIA     =   0x06;

        /*  OUTPUT STATE */
        public const byte OUT_STATE_ACTIVE    =0x01;
        public const byte OUT_STATE_INACTIVE  = 0x00;





        /************  NAMES *****************/

        public const int RAM_NAME_SIZE = 32;  //  16x 2byte unicode
        public const int mAddr_NAMES_StartAddr = (RAM_GCFG_SIZE+(RAM_DEVCFG_SIZE*RAM_DEVCFG_NO));
        
        public const int mAddr_NAMES_Device_StartIndex = 0x0000; 
        public const int RAM_DEVICE_NAMES_SIZE = (RAM_DEV_NAMES_NO*RAM_NAME_SIZE);
        public const int RAM_DEV_NAMES_NO = (DeviceMaxRelays + DeviceMaxOutputs + DeviceMaxInputs + 1); //device name + IOs
        public const int mAddr_NAMES_Board_Pos = 0x00;
        public const int mAddr_NAMES_Inputs_Pos = 0x01;
        public const int mAddr_NAMES_Outputs_Pos = (DeviceMaxInputs + mAddr_NAMES_Inputs_Pos);
        public const int mAddr_NAMES_Relays_Pos = (mAddr_NAMES_Outputs_Pos + DeviceMaxOutputs);

        public const int mAddr_NAMES_Global_StartIndex = (mAddr_NAMES_Device_StartIndex + (RAM_DEV_NAMES_NO* RAM_DEVCFG_NO));
        public const int mAddr_NAMES_Global_StartAddr = mAddr_NAMES_StartAddr + (RAM_DEV_NAMES_NO * RAM_NAME_SIZE);
        public const int mAddr_NAMES_Global_SystemName_Pos = 0;
        public const int RAM_NAMES_Global_Total_Records = (mAddr_NAMES_Global_SystemName_Pos + 1);
        public const int RAM_NAMES_Global_Total_Size = (RAM_NAMES_Global_Total_Records * RAM_NAME_SIZE);

        public const int mAddr_NAMES_Zone_StartIndex = (mAddr_NAMES_Global_StartIndex + RAM_NAMES_Global_Total_Records);
        public const int mAddr_NAMES_Zone_StartAddr = (mAddr_NAMES_Global_StartAddr + RAM_NAMES_Global_Total_Size);
        public const int RAM_ZONE_NAMES_SIZE = (ZONE_CFG_MAX_ZONES * RAM_NAME_SIZE);


        public const int RAM_NAMES_SIZE = ((RAM_DEV_NAMES_NO * RAM_NAME_SIZE) + (RAM_NAMES_Global_Total_Size) + (RAM_ZONE_NAMES_SIZE));
        

        

        /************  SCHEDULE ***************/
        public const int mAddr_SCHED_StartAddr =0x4000; //16384 - after Names CFG
        public const int RAM_SCHED_SIZE  = 8192;         //32 devices * 8sched/dev *  32B sched size
        public const int RAM_DEV_SCHED_SIZE = 256;
      //  public const int RAM_SMS_RECP_MEM_SIZE = 256;
        public const int RAM_DEV_SCHED_NO    =0x08;     //32 schedules
        public const int RAM_DEV_SCHED_MEM_SIZE  =0x20;  //32B  
        public const int RAM_DEV_SCHED_DATETIME_SIZE = 0x08;

        
        /****************    Schedule  *****************/
        //4b date, from 4b date to , 4b hr from, 4b hr to  = 16b/time
        public const int SCHED_MEM_SIZE  =0x2000;


        /*************  Schdule Types ***************/
        public const int SCHED_TYPE_POS =0x00;
        public const int SCHED_TYPE_FROMTO   =0x00;    //activation
        public const int SCHED_TYPE_ONDATE   =0x01;   //single on start activation, ex. sms, mail
        public const int SCHED_TYPE_UNTIL    =0x02;    //Activate always, from now until date
        public const int SCHED_TYPE_FROM     =0x03;    //Activate always, from date to infinity

        public const int SCHED_TIME_HR_POS = 0x03;
        public const int SCHED_TIME_MIN_POS = 0x02;
        public const int SCHED_TIME_SEC_POS = 0x01;
        public const int SCHED_TIME_RSVD_POS = 0x00;
        public const int SCHED_DATE_YR_POS   = 0x03;
        public const int SCHED_DATE_MON_POS   = 0x02;
        public const int SCHED_DATE_MDAY_POS   = 0x01;
        public const int SCHED_DATE_WDAY_POS   = 0x00;

        public const int SCHED_TIMEDATE_SIZE=   0x08;

        /***********    Schedule  Dates  *********/
        public const int SCHED_DATE_START_POS  =0x01;
        public const int SCHED_TIME_FROM_POS   =0x01;
        public const int SCHED_DATE_FROM_POS   =0x05;
        public const int SCHED_TIME_TO_POS     =0x09;
        public const int SCHED_DATE_TO_POS     =0x0E;

        /************  Action Types  *************/
        public const int SCHED_ACTION_TYPE_POS =0x10;
            public const int SCHED_ACTION_TYPE_ACTIV_OUT =0x00;
                public const int SCHED_ACTION_ACTIV_OUTNO_POS =0x11;
                public const int SCHED_ACTION_ACTIV_OUTVAL_POS   =0x12;
                    public const int SCHED_ACTION_ACTIV_OUT_ON =0x01;
                    public const int SCHED_ACTION_ACTIV_OUT_OFF =0x00;
                    public const int SCHED_ACTION_ACTIV_OUT_PULSE = 0x02;
                      public const int SCHED_ACTION_ACTIV_OUT_PULSE_ON_TIME_POS = 0x13;
                      public const int SCHED_ACTION_ACTIV_OUT_PULSE_OFF_TIME_POS = 0x14;
            public const int SCHED_ACTION_TYPE_ACTIV_REL =0x01;
                public const int SCHED_ACTION_ACTIV_RELNO_POS =0x11;
                public const int SCHED_ACTION_ACTIV_REL_VAL_POS    = 0x12;
                    public const int SCHED_ACTION_ACTIV_REL_ON =0x01;
                    public const int SCHED_ACTION_ACTIV_REL_OFF =0x00;
                    public const int SCHED_ACTION_ACTIV_REL_PULSE = 0x02;
                      
            public const int SCHED_ACTION_TYPE_SEND_MAIL     =0x02;
                public const int SCHED_ACTION_MAIL_SERVER_ID_POS =0x11;
                public const int SCHED_ACTION_MAIL_MSG_POS   =0x12;
            public const int SCHED_ACTION_TYPE_SEND_SMS     =0x03;
                public const int SCHED_ACTION_SMS_RECP_ID_POS =0x11;
                public const int SCHED_ACTION_SMS_MSG_ID_POS =0x12;
            public const int SCHED_ACTION_TYPE_DISARM    =0x04;
            public const int SCHED_ACTION_TYPE_ARM       =0x05;

        /* Send Mail/SMS with content and recpients stored in memory config*/
            public const int SCHED_STAT_POS =  31;
            public const int SCHED_STAT_ACTIVE   =0x01;
            public const int SCHED_STAT_INACTIVE =0x00;


        /*****************  Mail ***********************/




        /*****************  SMS ***********************/
        public const int mAddr_SMS_StartAddr     =0x6000;  //24576 - after Schedule
        public const int mAddr_RECP_StartAddr =0x6000;
        public const int mAddr_SMS_MSG_StartAddr =0x6100;

        public const byte RAM_SMS_RECP_ADDR_POS = (byte)0x0000;
        public const byte RAM_SMS_RECP_ADDR_LEN = 15;
        public const byte RAM_SMS_RECP_COUNTRY_CODE_POS = (byte)(RAM_SMS_RECP_ADDR_POS + RAM_SMS_RECP_ADDR_LEN);
        public const byte RAM_SMS_RECP_COUNTRY_CODE_LEN = 0x02;
        public const  byte RAM_SMS_RECP_MESSAGE_LEVEL_POS = (byte)(RAM_SMS_RECP_COUNTRY_CODE_POS + RAM_SMS_RECP_COUNTRY_CODE_LEN);
        public const  byte RAM_SMS_RECP_MESSAGE_LEVEL_LEN = 0x01;
        public const  byte RAM_SMS_RECP_ENABLED_POS = (byte)(RAM_SMS_RECP_MESSAGE_LEVEL_POS + RAM_SMS_RECP_MESSAGE_LEVEL_LEN);
        public const  byte RAM_SMS_RECP_ENABLED_LEN = 0x01;


        public const  byte RAM_SMS_RECP_SIZE = 32;    // E.164 ITU-T max 15 digit 
        public const  byte RAM_SMS_RECP_NO = 16;    //16 recpients

        public const  int RAM_SMS_MSG_SIZE  = 64;    //64 ASCII chars
        public const  int RAM_SMS_MSG_NO    = 16;


        public const int RAM_SMS_RECP_MEM_SIZE = (RAM_SMS_RECP_SIZE*RAM_SMS_RECP_NO);

        /**********************  AUTH *********************/
        public const int AUTH_PASS_SIZE = 30;       // pass  15 x 2b UTF8
        public const int AUTH_RECORD_PASS_LEN_POS = 0x00;
        public const int AUTH_RECORD_PASS_LEN_LEN = 0x01;
        public const int AUTH_RECORD_ENABLED_POS = (AUTH_RECORD_PASS_LEN_POS + AUTH_RECORD_PASS_LEN_LEN);
        public const int AUTH_RECORD_ENABLED_LEN = 0x01;
        public const int AUTH_RECORD_GROUP_POS = (AUTH_RECORD_ENABLED_POS + AUTH_RECORD_ENABLED_LEN);
        public const int AUTH_RECORD_GROUP_LEN = 0x01;
        public const int AUTH_RECORD_PERM_POS = (AUTH_RECORD_GROUP_POS + AUTH_RECORD_GROUP_LEN);
        public const int AUTH_RECORD_PERM_LEN = 0x04;
        public const int AUTH_RECORD_ALLOWED_FROM_POS = (AUTH_RECORD_PERM_POS + AUTH_RECORD_PERM_LEN);
        public const int AUTH_RECORD_ALLOWED_FROM_LEN = 0x04;
        public const int AUTH_RECORD_ALLOWED_UNTIL_POS = (AUTH_RECORD_ALLOWED_FROM_POS + AUTH_RECORD_ALLOWED_FROM_LEN);
        public const int AUTH_RECORD_ALLOWED_UNTIL_LEN = 0x04;
        public const int AUTH_RECORD_LOGIN_POS = (AUTH_RECORD_ALLOWED_UNTIL_POS + AUTH_RECORD_ALLOWED_UNTIL_LEN);
        public const int AUTH_RECORD_LOGIN_LEN = AUTH_PASS_SIZE;
        public const int AUTH_RECORD_PASSWD_POS = (AUTH_RECORD_LOGIN_POS + AUTH_RECORD_LOGIN_LEN);
        public const int AUTH_RECORD_PASSWD_LEN = AUTH_PASS_SIZE;
        public const int AUTH_RECORD_SIZE = (AUTH_RECORD_PASSWD_POS + AUTH_RECORD_PASSWD_LEN);

        public const int AUTH_CRED_SIZE   =   AUTH_RECORD_SIZE; 
        public const int AUTH_MAX_USERS = 16;
        public const int AUTH_RECORDS_SIZE = (AUTH_RECORD_SIZE * AUTH_MAX_USERS);


        /******************   NET   *************/
        public const int NET_MAX_TX_SIZE = 1024;
        public const int NET_MAX_RX_SIZE = 2048;
        public const int NET_MAX_SESSION_IDLE_SEC = 100;
        public const int NET_DATA_PACKET_CONTROL_BYTES = 2;
        public const int NET_CMD_PACKET_LEN = 3;
        public const int NET_UPLOAD_HEADER_BYTES = 4;
        public const int NET_UPLOAD_TAIL_BYTES = 1;
        public const int NET_UPLOAD_PACKET_CONTROL_BYTES = (NET_UPLOAD_TAIL_BYTES+ NET_UPLOAD_HEADER_BYTES);
        public const int NET_UPLOAD_PACKET_DATA_OFFSET = 4;
        public const int NET_MAX_PACKET_DATA = (NET_MAX_TX_SIZE - NET_UPLOAD_PACKET_CONTROL_BYTES);

        public const int MessageHeader_Command_Pos = 0;
        public const int MessageHeader_CommandType_Pos = 1;
        public const int MessageHeader_CommandParam_Pos = 2;
        public const int MessageHeader_Command_Reg_Low_Pos = 2;
        public const int MessageHeader_Command_Reg_High_Pos = 3;
        public const int NET_PACKET_TX_PAYLOAD_SIZE = (NET_MAX_PACKET_DATA);

        public const byte NET_PACKET_TYPE_GCFG = 0x0001;
        public const byte NET_PACKET_TYPE_DEVCFG = 0x0002;
        public const byte NET_PACKET_TYPE_DEVNAMECFG = 0x0003;
        public const byte NET_PACKET_TYPE_SCHEDCFG = 0x0004;
        public const byte NET_PACKET_TYPE_PASSWDCFG = 0x0005;
        public const byte NET_PACKET_TYPE_AUTH = 0x0006;
        public const byte NET_PACKET_TYPE_NETCFG = 0x0007;
        public const byte NET_PACKET_TYPE_GSMRCPTCFG = 0x0008;
        public const byte NET_PACKET_TYPE_DEVAUTHCFG = 0x0009;
        public const byte NET_PACKET_TYPE_GLOBNAMECFG = 0x000A;

        public const int SYS_ALRM_DEV_UUID_POS = 0x00;
        public const int SYS_ALRM_DEV_UUID_LEN = 16;
        public const int SYS_ALRM_DEV_ENABLED_POS = (SYS_ALRM_DEV_UUID_POS + SYS_ALRM_DEV_UUID_LEN);
        public const int SYS_ALRM_DEV_ENABLED_LEN = 0x01;
        public const int SYS_ALRM_DEV_START_DATE_POS = (SYS_ALRM_DEV_ENABLED_POS + SYS_ALRM_DEV_ENABLED_LEN);
        public const int SYS_ALRM_DEV_START_DATE_LEN = 0x04;
        public const int SYS_ALRM_DEV_END_DATE_POS = (SYS_ALRM_DEV_START_DATE_POS + SYS_ALRM_DEV_START_DATE_LEN);
        public const int SYS_ALRM_DEV_END_DATE_LEN = 0x04;

        public const int SYS_ALRM_DEV_AUTH_LEN = (SYS_ALRM_DEV_END_DATE_POS + SYS_ALRM_DEV_END_DATE_LEN);
        public const int SYS_ALRM_UUID_LEN = SYS_ALRM_DEV_AUTH_LEN;

        public const int SYS_ALARM_DEV_AUTH_MAX_RECORDS = 32;
        public const int SYS_ALARM_DEV_AUTH_MEM_SIZE = SYS_ALARM_DEV_AUTH_MAX_RECORDS * SYS_ALRM_UUID_LEN;

        

        /***************    EVENTS      *****************/

        public const byte EVENT_DB_INFO_EVNO_POS =  0x00;
        public const byte EVENT_DB_INFO_EVNO_LEN = 0x04;

        public const byte EVENT_DB_INFO_LEN =  (EVENT_DB_INFO_EVNO_LEN);

        public const byte EVENT_DB_ID_POS= 0x00;
        public const byte EVENT_DB_ID_LEN =0x04;

        public const byte EVENT_DB_CODE_POS =  (byte)(EVENT_DB_ID_POS+EVENT_DB_ID_LEN);
        public const byte EVENT_DB_CODE_LEN =  0x02;

        public const byte EVENT_DB_DOMAIN_POS = (byte)(EVENT_DB_CODE_POS + EVENT_DB_CODE_LEN);
        public const byte EVENT_DB_DOMAIN_LEN= 0x02;

        public const byte EVENT_DB_DEVICE_POS = (byte)(EVENT_DB_DOMAIN_POS + EVENT_DB_DOMAIN_LEN);
        public const byte EVENT_DB_DEVICE_LEN= 0x02;

        public const byte EVENT_DB_USER_ID_POS = (byte)(EVENT_DB_DEVICE_POS + EVENT_DB_DEVICE_LEN);
        public const byte EVENT_DB_USER_ID_LEN  =   0x02;

        public const byte EVENT_DB_TIME_POS = (byte)(EVENT_DB_USER_ID_POS + EVENT_DB_USER_ID_LEN);
        public const byte EVENT_DB_TIME_LEN =  0x04;

        public const byte EVENT_DB_DATE_POS = (byte)(EVENT_DB_TIME_POS + EVENT_DB_TIME_LEN);
        public const byte EVENT_DB_DATE_LEN  = 0x04;

        public const byte EVENT_DB_RECORD_LEN = (byte)(EVENT_DB_CODE_LEN + EVENT_DB_DOMAIN_LEN + EVENT_DB_DEVICE_LEN + EVENT_DB_USER_ID_LEN + EVENT_DB_TIME_LEN + EVENT_DB_DATE_LEN);

    
        /**************  NETWORK **************/
        public const byte NET_UPLOAD_DATA_OFFSET= 0x03;
        public const byte NET_UPLOAD_DATA_END_OFFSET =0x01;
        public const byte NET_CFG_SIZE   = 52;


        /*************** OTHER *****************/
        public const double SUPP_BATT_LEAD_ACID_MAX_VOLTAGE = 15.00;
        public const double SUPP_MAIN_AC_MAX_VOLTAGE = 24.00;

    }

}
