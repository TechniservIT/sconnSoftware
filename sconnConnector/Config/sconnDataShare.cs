using System;
using System.Windows;
using System.Windows.Input;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;

namespace sconnConnector
{

    static public class sconnDataShare
    {
        static public int siteNo 
        {
            get { return sconnSites.siteNo; }
        }

        static public int GetLastItemID()
        {
            if (sconnSites.siteNo > 0)
            {
                return sconnSites.sites[siteNo - 1].siteID;
            }
            else
            { return 0; }
        }

        static public XmlDocument getConfigXML()
        {
            XmlDocument configDoc = new XmlDocument();

            return configDoc;
        }



        static public sconnSite getSite(int siteNo)
        {
            foreach (sconnSite site in sconnSites)
            {
                if (site.siteID == siteNo)
                {
                    return  site;
                }
            }
            return new sconnSite();
        }


        static public sconnSite[] getSites()
        {
            return sconnSites.sites;
        }

        static public bool addSite(string hostname, int port, string password, string siteName)
        {
                if (hostname == null || port == 0 || password == null)
            {
                return false;
            }
            sconnSite site = new sconnSite(siteName,1000, hostname, port, password );
            sconnSites.Push(site);
            return true;
        }

        static public bool addSite(sconnSite addsite)
        {
            if (addsite.serverIP == null || addsite.serverPort == 0 || addsite.authPasswd == null)
            {
                return false;
            }
            addsite.siteID = GetLastItemID()+1; //set ID as last device, site was not created by DataShare
            sconnSites.Push(addsite);
            return true;
        }

        static public void removeSites()
        {
            for (int i = 0; i < sconnSites.siteNo; i++)
            {
                removeSite(sconnSites.sites[i].siteID);
            }
        }

        static public bool addSites(sconnSite[] sites)
        {
            if (sites == null) { return false; }
            for (int i = 0; i < sites.GetLength(0); i++)
            {
                if (sites[i] != null)
                {
                    sites[i].siteID = sconnDataShare.GetLastItemID() + 1; //set ID for new items
                    sconnSites.Push(sites[i]);
                }
            }
            return true;
        }

        static public void removeSite(int siteID)
        {
            for (int i = 0; i < sconnSites.siteNo; i++)
            {
                if (sconnSites.sites[i].siteID == siteID)
                {
                     sconnSites.RemoveAt(i);
                }
            }
        }

        static private bool _SiteLiveViewEnabled = true;
        static public bool SiteLiveViewEnabled { get { return _SiteLiveViewEnabled; } set { _SiteLiveViewEnabled = value; } }


        /********  Handles application data between View, Tasker and Mngr/Src , with R/W and R access  ************/

        static bool updatePending = false; //signal UI threads update is in progress and status should not be read
        static private siteStack sconnSites = new siteStack();
        //static private sconnSite[] sites = new sconnSite[0];


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
                sites[siteInc-1] = site;

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
                    for (int i = siteIndex; i < siteInc-1; i++)
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


        private class siteStackEnum :  IEnumerator
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

    }

    public class sconnSite
    {
        private int _statusCheckInterval;
        private string _authPasswd;
        private string _serverIP;
        private int _serverPort;
        private bool _ViewEnabled;
        private int _siteID;
        private string _siteName;
        private int SelectedTabId = 0;
        public SiteConnectionStat siteStat;

        public ipcSiteConfig siteCfg;     

        public string siteName         
        {
            get { return _siteName; }
            set { _siteName = value; }
        }
        public string serverIP 
        {
            get { return _serverIP; }
            set {  _serverIP = value; }
        }

        public int serverPort
        {
            get { return _serverPort; }
            set { _serverPort = value; }
        }

        public bool ViewEnable
        {
            get { return _ViewEnabled; }
            set { _ViewEnabled = value; }
        }

        public int statusCheckInterval
        {
            get { return _statusCheckInterval; }
            set { _statusCheckInterval = value; }
        }

        public string authPasswd
        {
            get { return _authPasswd; }
            set
            {
                if (passwordFormatComplies(value))
                {
                    _authPasswd = value;
                }

            }
        }

        private DateTime lastUpdate;
        private DateTime lastWrite;

        public DateTime LastUpdate
        { 
            get { return lastUpdate; }
            set
            {
                if (value != null)
                {
                    lastUpdate = (DateTime)value;
                }
            }
        }
        public DateTime LastWrite { get { return lastWrite; } }


        private bool passwordFormatComplies(string password)
        {
            if (password != null)
            {
                if (    (password.Length <= ipcDefines.AUTH_PASSWD_SIZE)  &&  
                        (password.Length > 0)
                    )
                //additional password restrictions
                { return true; }
                
            }
            return false;
        }

        public int siteID
        {
            get { return _siteID; }
            set
            {
                if (value > 0)
                {
                    _siteID = value;
                }
            }
        }

        public sconnSite()
        {
            statusCheckInterval = 5000; //5s interval
            lastUpdate = DateTime.Now;
            serverPort = 37222;
            _siteID = sconnDataShare.GetLastItemID()+1;
            _siteName = "DefaultSite";
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
        }

        public sconnSite(ipcSiteConfig config, int intervalMs, string siteName)
        {
            statusCheckInterval = intervalMs;
            siteCfg = config;
            _siteID = sconnDataShare.GetLastItemID() + 1;
            _siteName = siteName;
            lastUpdate = DateTime.Now;
            serverPort = 37222;
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
        }


        public sconnSite(int intervalMs, string server, int port, string siteName)
        {
            statusCheckInterval = intervalMs;
            serverIP = server;
            serverPort = port;
            _siteID = sconnDataShare.GetLastItemID() + 1;
            _siteName = siteName;
            lastUpdate = DateTime.Now;
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
        }

        public sconnSite(string siteName, int intervalMs, string server, int port, string password)
            : this(intervalMs, server, port, siteName)
        {
            _authPasswd = password;
        }

    }

    public class ipcSiteConfig
    {
        private int _deviceNo = 0;
        private int _evNo = 0;
        private ipcDataType.ipcDeviceConfig[] _deviceConfigs;
        private ipcDataType.ipcGlobalConfig _globalConfig;
        private ipcDataType.ipcEvent[] events;


        public void AddEvent(ipcDataType.ipcEvent ipcEv)
        {
            if (ipcEv != null)
            {
                events[_evNo] = ipcEv;
                _evNo++;
            }
        }

        public int deviceNo
        {
            get
            {
                return _deviceNo;
            }
            //set
            //{
            //    if (value > 0) { _deviceNo = value; }
            //}
        }

        public ipcDataType.ipcDeviceConfig[] deviceConfigs
        {
            get
            {
                return _deviceConfigs;
            }
            set
            {
                _deviceConfigs = value;
            }
        }

        public ipcDataType.ipcGlobalConfig globalConfig
        {
            get
            {
                return _globalConfig;
            }
            set
            {
                    _globalConfig = value;
            }
        }

        public ipcSiteConfig()
        {
            _deviceNo = 0;
            _deviceConfigs = new ipcDataType.ipcDeviceConfig[deviceNo];
            _globalConfig = new ipcDataType.ipcGlobalConfig();
        }

        public ipcSiteConfig(int devices)
        {
            _deviceNo = devices;
            _deviceConfigs = new ipcDataType.ipcDeviceConfig[deviceNo];
            for (int i = 0; i < deviceNo; i++)
            {
                _deviceConfigs[i] = new ipcDataType.ipcDeviceConfig();
            }
            _globalConfig = new ipcDataType.ipcGlobalConfig();
        }

        public void fillSampleCfg()
        {
            _deviceNo = 1;
            _globalConfig = new ipcDataType.ipcGlobalConfig();
            _deviceConfigs = new ipcDataType.ipcDeviceConfig[deviceNo];
            for (int i = 0; i < deviceNo; i++)
            {
                _deviceConfigs[i] = new ipcDataType.ipcDeviceConfig();
            }
            
            //fill device config
            for (int i = 0; i < ipcDefines.deviceConfigSize; i++)
            {
                _deviceConfigs[0].memCFG[i] = (byte)i;

            }
            //set output Number
            _deviceConfigs[0].memCFG[ipcDefines.mAdrOutputsNO] = 10;

            //fill  gconfig
            for (int i = 0; i < 250; i++)
            {
                _globalConfig.memCFG[i] = (byte)i;
            }


            _globalConfig.memCFG[ipcDefines.mAdrDevNO] = (byte)deviceNo;

        }

        public void addDeviceCfg()
        {
            _deviceNo++;
            ipcDataType.ipcDeviceConfig[] nDeviceConfigs = new ipcDataType.ipcDeviceConfig[deviceNo];
            if (_deviceConfigs.GetLength(0) != 0) //copy only if config is not empty
            {
                for (int i = 0; i < deviceNo - 1; i++)
                {
                    nDeviceConfigs[i] = deviceConfigs[i];
                }
            }          
            nDeviceConfigs[deviceNo - 1] = new ipcDataType.ipcDeviceConfig();
            _deviceConfigs = nDeviceConfigs;
            globalConfig.memCFG[ipcDefines.mAdrDevNO] = (byte)deviceNo;
        }


    }

    public class ipcDataType
    {

        /******    Entire config object   ********/

        //public struct ipcDeviceConfig
        //{
        //    public byte[] memCFG;  //1 page
        //};
        //public struct ipcGlobalConfig
        //{
        //    public byte[] memCFG; //4 pages
        //};

        public class ipcEvent
        {
            public short EventNo { get; set; }

            public ipcEvent()
            {

            }

            public ipcEvent(byte[] EventBytes) :this()
            {
                //decode
            }

        }


        public class ipcDeviceConfig
        {
            private byte[] _memCFG;
            private byte[][] _NamesCFG;
            private byte[][] _ScheduleCFG;
            private byte[] _NetworkConfig;

            public byte[] NetworkConfig
            {
                get { return _NetworkConfig; }
                set { if (value != null) { _NetworkConfig = value; } }
            }

            public byte[] memCFG
            {
                get { return _memCFG; }
                set { if (value != null) { _memCFG = value; } }
            }

            public byte[][] NamesCFG
            {
                get { return _NamesCFG; }
               // set { if (value != null) { _NamesCFG = value; } }
            
            }

            public byte[][] ScheduleCFG
            {
                get { return _ScheduleCFG; }
            }



            public string GetDeviceNameAt(int NameNo)
            {
                if (NameNo < ipcDefines.RAM_DEV_NAMES_NO)
                {
                    return System.Text.Encoding.Unicode.GetString(_NamesCFG[NameNo], 0, _NamesCFG[NameNo].GetLength(0));
                }
                else { return new string('E',1); }
            }

            public void SetDeviceNameAt(int NameNo, string Name)
            {
               byte[] namebuff = System.Text.Encoding.Unicode.GetBytes(Name);
               SetDeviceNameAt(NameNo, namebuff);
            }

            public void SetDeviceNameAt(int NameNo, byte[] Name)
            {
                if (Name.GetLength(0) < 32)
                {
                    byte[] resized = new byte[32];
                    Name.CopyTo(resized, 0);
                    _NamesCFG[NameNo] = resized;
                }
                else
                {
                    _NamesCFG[NameNo] = Name;
                }

                //replaces nulls by spaces
                for (int i = 0; i < _NamesCFG[NameNo].GetLength(0); i+=2)
                {
                    if ( (_NamesCFG[NameNo][i] == (byte)0x00) &&  ( _NamesCFG[NameNo][i + 1] == (byte)0x00 ))
                    {
                        _NamesCFG[NameNo][i] = (byte)0x20; //space
                    }
                }
            }

            public ipcDeviceConfig()
            {
                _memCFG = new byte[ipcDefines.deviceConfigSize];
                _NamesCFG = new byte[ipcDefines.RAM_DEV_NAMES_NO][];
                _NetworkConfig = new byte[ipcDefines.NET_CFG_SIZE];
                for (int i = 0; i < ipcDefines.RAM_DEV_NAMES_NO; i++)
                {
                    _NamesCFG[i] = new byte[ipcDefines.RAM_NAME_SIZE];
                }
                _ScheduleCFG = new byte[ipcDefines.RAM_DEV_SCHED_NO][];
                for (int i = 0; i < ipcDefines.RAM_DEV_SCHED_NO; i++)
                {
                    _ScheduleCFG[i] = new byte[ipcDefines.RAM_DEV_SCHED_MEM_SIZE];
                }
            }
        }

        public class ipcGlobalConfig
        {
            private byte[] _memCFG;
            public byte[] memCFG 
            { 
                get { return _memCFG; }
                set { if (value != null) { _memCFG = value; } }
            }
            public ipcGlobalConfig()
            {
                _memCFG = new byte[ipcDefines.ipcGlobalConfigSize];
            }
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

        public static byte ipcRoleSensor = 0x01;
        public static byte ipcRoleIO = 0x02;
        public static byte ipcRoleMotor = 0x03;
        public static byte ipcRoleCore = 0x00;

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


    }

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
        public static byte STX = 0x02; //start text
        public static byte ETX = 0x03; //end text
        public static byte EOT = 0x04; // end of transmission

        public static byte SVAL = 0x07; //start of value bytes
        public static byte EVAL = 0x08; //end value

        //Custom CMDs replacing ASCII
        public static byte SET = 0x05; // set following register group value
        public static byte setRegVal = 0x51;
        public static byte setGlobalCfg = 0x52;
        public static byte setDeviceCfg = 0x53;
        public static byte setDeviceNamesCfg = 0x54;
        public static byte setDeviceSchedulesCfg = 0x57;
        public static byte setPasswdCfg = 0x58;
        public static byte setDeviceNetworkCfg = 0x59;

        public static byte GET = 0x06; // get following register group value
        public static byte getRegVal = 0x61; //retrieve single register byte value    Param :   Register Address (2b)
        public static byte getDevNo = 0x62; //get number of devices in system to map memory addresses Param : None
        public static byte getDevCfg = 0x63;
        public static byte getGlobCfg = 0x64;
        public static byte getRunGlobCfg = 0x65;
        public static byte getRunDevCfg = 0x66;
        public static byte getArmStatus = 0x69;
        public static byte getInputState = 0x6A;
        public static byte getOutputState  = 0x6B;
        public static byte getNamesDevCfg = 0x67;
        public static byte getSchedulesDevCfg = 0x6C;
        public static byte getPasswdCfg =  0x6D;
        public static byte getEvents =  0x6E;
        public static byte getEventNo =  0x6F;
        public static byte getEvent  = 0x70;
        public static byte getDevNetCfg = 0x71;

        public static byte CFG = 0x11; //register to set, followed by value SVAL <value bytes > EVAL

        //groups of registers to be set
        public static byte IO = 0x10; //output port
        public static byte TM = 0x11; //timer to set

        //groups of get register
        public static byte ADC = 0x12; //directly read input value
        public static byte UI = 0x15; //Universal Input , specific for each slave device, NO/NC digital inputs, returns 1 / 0, 0 = normal state
        public static byte TMP = 0x13; //temperature indicator value, 2 bytes are sent : signed decimal , unsigned 2digit precision
        public static byte HUM = 0x14; //humidity % indicator value, 2 bytes are sent : signed decimal , unsigned 2digit precision
        //Following group code is string with peripherial name in format :  STX <name> ETX


        public static byte SOP = 0x16;  //Start of Password ,following is password string
        public static byte EOP = 0x17;   //password end
        public static byte AUTHOK = 0x18;
        public static byte AUTHFAIL = 0x19;
        public static byte ACK = 0x20;
        public static byte ACKNXT = 0x21;
        public static byte ACKFIN = 0x22;

        public static byte ERRCMD = 0x21;

        public static byte PSH = 0x22;
        public static byte PSHNXT = 0x23;
        public static byte PSHFIN = 0x24;

        public static byte OVF = 0x25;

        /********  Unique Device ID    *********/
        public static byte DevID0 = 0x01;
        public static byte DevID1 = 0x00;

    }

    public struct ipcDefines
    {

        public static byte comI2C = 0x01;
        public static byte comSPI = 0x02;
        public static byte comMiWi = 0x03;
        public static byte comUSB = 0x04;
        public static byte comETH = 0x05;

        public static int deviceConfigSize = 256;  //256 bytes
        public static int ipcMaxDevices = 256;

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


        public static int mAdrDevID_LEN = 0x0002;

        /********    Global Config      **********/
        public static int ipcGlobalConfigSize = 1024;
        public static int mAdrDevNO = 0x0000; //number of devices

        public static int mAdrArmed =0x0002;
        public static int mAdrViolation= 0x0003;
        public static int mAdrDeamonType = 0x0004;
        public static int mAdrDeamonType_LEN = 0x0001;

        public static int mAdrHostDeviceAddr = (mAdrDeamonType + mAdrDeamonType_LEN);      //system master dev id
        public static int mAdrHostDeviceAddr_LEN = mAdrDevID_LEN;

        public static int mAdrUserNo       =  (mAdrHostDeviceAddr+mAdrHostDeviceAddr_LEN);  // current number of registered users in PASSWD
        public static int mAdrUserNo_LEN  =    0x0002;


        public static int mAdrCurrDevIncPos     =  (mAdrUserNo+mAdrUserNo_LEN);
        public static int mAdrCurrDevInc_LEN  =    mAdrDevID_LEN;

        public static int mAdrHostDeviceSystemId    = (mAdrCurrDevIncPos+mAdrCurrDevInc_LEN);
        public static int mAdrHostDeviceSystemId_LEN = mAdrDevID_LEN;

        public static int mAdrHostDeviceIsSystemConnected= (mAdrHostDeviceSystemId+mAdrHostDeviceSystemId_LEN);
        public static int mAdrHostDeviceIsSystemConnected_LEN =0x01;


        public static int mAdrDevStart = 0x400; //1024- start address in memory of device configs
        public static int mAdrGlobalConfig = 0x0000;




        public static int mAdrSiteName = 0x0040; //16 char UTF8(4b) site nam
        public static int mAdrSitePasswd = 0x0080; // 32char UTF8 password
        public static int PasswordMaxChars = 32;
        public static int PasswordSize = 128; //32char 128 byte

        public static byte sysVersion = 0x00;

        /********  Device config  ********/

        public static byte mAdrDevID = 0x00;    //device unique ID , for I2C it is also bus Address

        public static byte mAdrDomain = 0x02;    //device domain
        public static byte mAdrDevRev = 0x03;   //Revision number   
        public static byte mAdrDevType = 0x04;  //Type of device, IO, motor etc.
        public static byte mAdrInputsNO = 0x05;  //number of inputs on board
        public static byte mAdrOutputsNO = 0x06; //number of outputs on board
        public static byte mAdrRelayNO = 0x07; //number of Relays on board
        public static byte mAdrKeypadMod = 0x08; //devices has KeyPad module
        public static byte mAdrTempMod = 0x09; // device has temperature module ( bool )
        public static byte mAdrHumMod = 0x0A;   //humidity
        public static byte mAdrPresMod = 0xB;  //pressure
        public static byte mAdrCOMi2c = 0xC;  //device has I2C COM , bool
        public static byte mAdrCOMeth = 0xD;  //device has ETH COM
        public static byte mAdrCOMmiwi = 0xE;  //device has MiWi COM
        public static byte mAdrI2CAddr = 0xF; //i2c bus address


        /********  Input state  ********/
        public static byte mAdrInput = 0x20;  //128 - start address of input states, format : <input type> <value1> <value2-Analog>
        public static byte mAdrInputMemSize = 0x04;
        public static byte mAdrInputType = 0x00;
        public static byte mAdrInputAG = 0x01;
        public static byte mAdrInputVal = 0x02;
        public static byte mAdrInputNameAddr = 0x03;

        /********  Output state  ********/
        public static byte mAdrOutput = 0x60;  //40 - start address of output states, format : <output type> <value1>
        public static byte mAdrOutputType = 0x00;
        public static byte mAdrOutputVal = 0x01;
        public static byte mAdrOutputNameAddr = 0x02;
        public static byte mAdrOutputPar4 = 0x03;
        public static byte mAdrOutputMemSize = 0x04;

        public static byte outputON = 0x01;  //Output active
        public static byte outputOFF = 0x00;  //Output inactive

        public static byte OutputNA = 0x01;
        public static byte OutputNIA = 0x00; // normaly inactive

        public static byte Output1Addr = 0x40;
        public static byte Output2Addr = 0x43;


        /******   Relay state ********/
        public static byte mAdrRelay = 0x80;
        public static byte mAdrRelayType = 0x00;
        public static byte mAdrRelayVal = 0x01;
        public static byte mAdrRelayNameAddr = 0x02;
        public static byte mAdrRelayPar4 = 0x03;
        public static byte RelayMemSize = 0x04;

  
        /******  DEAMON  *******/

        /***** DEAMON TYPES  *****/
        public static byte DEA_ALARM      =     0x00;
        public static byte DEA_SCHED     =      0x01;
        public static byte DEA_MAN       =      0x02;
        public static byte DEA_ALARM_SCHED = 0x03;     /* Alarm deamon with scheduled events*/


        /********* ALARM DEAMON ********/

        /*  INPUT  TYPE        */
        public static byte IN_TYPE_NO        =  0x00;
        public static byte IN_TYPE_NC     =     0x01;

        /*  INPUT  ACTIVATION GROUP    */
        public static byte IN_TYPE_ARM      =   0x40; /* Arm System */
        public static byte IN_TYPE_DISARM   =   0x41;
        public static byte IN_TYPE_AO1     =    0x42; /* Activate Output 1*/
        public static byte IN_TYPE_AO2     =    0x43;
        public static byte IN_TYPE_AV      =    0x44; /* Armed Violation  - Alarm input */
        public static byte IN_TYPE_DAV     =    0x45; /* DisArmed Violation -  Alarm on disarmed */
        public static byte IN_TYPE_ADV     =    0x46; /*Arm&Disarmed violation - 24/7 */

        public static byte ACTIVE    =  0x01;
        public static byte INACTIVE   =  0x00;

        /* INPUT VALUE  */
        public static byte IN_HIGH               =0x01;
        public static byte IN_LOW               = 0x00;

        /*  OUTPUT  TYPE        */
        public static byte OUT_TYPE_NA              =   0x00; /* Normaly Active*/
        public static byte OUT_TYPE_NIA            =    0x01; /* Normaly InActive*/

        public static byte OUT_TYPE_ALARM_NA        =   0x00;
        public static byte OUT_TYPE_ALARM_NIA       =   0x01;
        public static byte OUT_TYPE_PWR             =   0x02;
        public static byte OUT_TYPE_SCHED_1_NA      =   0x03;
        public static byte OUT_TYPE_SCHED_1_NIA     =   0x04;
        public static byte OUT_TYPE_SCHED_2_NA      =   0x05;
        public static byte OUT_TYPE_SCHED_2_NIA     =   0x06;

        /*  OUTPUT STATE */
        public static byte OUT_STATE_ACTIVE    =0x01;
        public static byte OUT_STATE_INACTIVE  = 0x00;
    
        /************  NAMES *****************/
        public static int mAddr_NAMES_StartAddr= 0x0000;
        public static int mAddr_NAMES_Board =0x0000;    //first name is device name
        public static int mAddr_NAMES_Inputs = 0x20; // 16 inputs
        public static int mAddr_NAMES_Outputs = 0x220;  // 9 outputs
        public static int mAddr_NAMES_Relays = 0x340;  // 2 relays

        public static int RAM_NAMES_SIZE  =16384;
        public static int RAM_DEVICE_NAMES_SIZE= 1024;
        public static int RAM_NAME_SIZE=  32;       //  16x 2byte unicode
        public static int RAM_TOTAL_NAMES_NO  = 512;
        public static int RAM_DEV_NAMES_NO=   32;

        public static int mAddr_NAMES_Board_Pos = 0x00;
        public static int mAddr_NAMES_Inputs_Pos = 0x01;
        public static int mAddr_NAMES_Outputs_Pos = 0x11;
        public static int mAddr_NAMES_Relays_Pos = 0x1A;


        /*****  *****/
        public static int Names_StartAddr= 0x0000;
        public static int Names_No       = 0x200;
        public static int Name_Size     =  0x10;
        public static int Name_Char_Size = 0x02;//4byte unicode char



        /************  SCHEDULE ***************/
        public static int mAddr_SCHED_StartAddr =0x4000; //16384 - after Names CFG
        public static int RAM_SCHED_SIZE  = 8192;         //32 devices * 8sched/dev *  32B sched size
        public static int RAM_DEV_SCHED_SIZE = 256;
        public static int RAM_DEV_SCHED_NO    =0x08;     //32 schedules
        public static int RAM_DEV_SCHED_MEM_SIZE  =0x20;  //32B  
        public static int RAM_DEV_SCHED_DATETIME_SIZE = 0x08;

        
        /****************    Schedule  *****************/
        //4b date, from 4b date to , 4b hr from, 4b hr to  = 16b/time
        static public int SCHED_MEM_SIZE  =0x2000;


        /*************  Schdule Types ***************/
        static public int SCHED_TYPE_POS =0x00;
        static public int SCHED_TYPE_FROMTO   =0x00;    //activation
        static public int SCHED_TYPE_ONDATE   =0x01;   //single on start activation, ex. sms, mail
        static public int SCHED_TYPE_UNTIL    =0x02;    //Activate always, from now until date
        static public int SCHED_TYPE_FROM     =0x03;    //Activate always, from date to infinity

        static public int SCHED_TIME_HR_POS = 0x03;
        static public int SCHED_TIME_MIN_POS = 0x02;
        static public int SCHED_TIME_SEC_POS = 0x01;
        static public int SCHED_TIME_RSVD_POS = 0x00;
        static public int SCHED_DATE_YR_POS   = 0x03;
        static public int SCHED_DATE_MON_POS   = 0x02;
        static public int SCHED_DATE_MDAY_POS   = 0x01;
        static public int SCHED_DATE_WDAY_POS   = 0x00;

        static public int SCHED_TIMEDATE_SIZE=   0x08;

        /***********    Schedule  Dates  *********/
        static public int SCHED_DATE_START_POS  =0x01;
        static public int SCHED_TIME_FROM_POS   =0x01;
        static public int SCHED_DATE_FROM_POS   =0x05;
        static public int SCHED_TIME_TO_POS     =0x09;
        static public int SCHED_DATE_TO_POS     =0x0E;

        /************  Action Types  *************/
        static public int SCHED_ACTION_TYPE_POS =0x10;
            static public int SCHED_ACTION_TYPE_ACTIV_OUT =0x00;
                static public int SCHED_ACTION_ACTIV_OUTNO_POS =0x11;
                static public int SCHED_ACTION_ACTIV_OUTVAL_POS   =0x12;
                    static public int SCHED_ACTION_ACTIV_OUT_ON =0x01;
                    static public int SCHED_ACTION_ACTIV_OUT_OFF =0x00;
                    static public int SCHED_ACTION_ACTIV_OUT_PULSE = 0x02;
                      static public int SCHED_ACTION_ACTIV_OUT_PULSE_ON_TIME_POS = 0x13;
                      static public int SCHED_ACTION_ACTIV_OUT_PULSE_OFF_TIME_POS = 0x14;
            static public int SCHED_ACTION_TYPE_ACTIV_REL =0x01;
                static public int SCHED_ACTION_ACTIV_RELNO_POS =0x11;
                static public int SCHED_ACTION_ACTIV_REL_VAL_POS    = 0x12;
                    static public int SCHED_ACTION_ACTIV_REL_ON =0x01;
                    static public int SCHED_ACTION_ACTIV_REL_OFF =0x00;
                    static public int SCHED_ACTION_ACTIV_REL_PULSE = 0x02;
                      
            static public int SCHED_ACTION_TYPE_SEND_MAIL     =0x02;
                static public int SCHED_ACTION_MAIL_SERVER_ID_POS =0x11;
                static public int SCHED_ACTION_MAIL_MSG_POS   =0x12;
            static public int SCHED_ACTION_TYPE_SEND_SMS     =0x03;
                static public int SCHED_ACTION_SMS_RECP_ID_POS =0x11;
                static public int SCHED_ACTION_SMS_MSG_ID_POS =0x12;
            static public int SCHED_ACTION_TYPE_DISARM    =0x04;
            static public int SCHED_ACTION_TYPE_ARM       =0x05;

        /* Send Mail/SMS with content and recpients stored in memory config*/
            static public int SCHED_STAT_POS =  31;
            static public int SCHED_STAT_ACTIVE   =0x01;
            static public int SCHED_STAT_INACTIVE =0x00;


        /*****************  Mail ***********************/




        /*****************  SMS ***********************/
        static public int mAddr_SMS_StartAddr     =0x6000;  //24576 - after Schedule
        static public int mAddr_RECP_StartAddr =0x6000;
        static public int mAddr_SMS_MSG_StartAddr =0x6100;
        static public int RAM_RECP_SIZE     = 0x100; //256B RECPs
        static public int RAM_SMS_RECP_SIZE = 16; // ITU-T max 15 digit
        static public int RAM_SMS_RECP_NO   = 16;
        static public int RAM_SMS_MSG_SIZE  = 64;    //64 ASCII chars
        static public int RAM_SMS_MSG_NO    = 16;
        static public int RAM_SMS_SIZE      = 0x400; //1024B MSGs


        /**********************  AUTH *********************/
        static public int AUTH_PASSWD_SIZE = 32;
        static public int AUTH_PASSWD_LEN = 30;
        static public int AUTH_GRP_POS = 30;
        static public int AUTH_PERM_POS = 31; //user permission level
        static public int AUTH_CBYTE_OFFSET = 1;


        /******************   SCONN BERKELEY  *************/
        static public int NET_MAX_TX_SIZE = 280;
        static public int NET_MAX_RX_SIZE = 1100;
        static public int NET_MAX_SESSION_IDLE_SEC = 100;
        static public int NET_DATA_PACKET_CONTROL_BYTES = 2;
        static public int NET_CMD_PACKET_LEN = 3;
        static public int NET_UPLOAD_PACKET_CONTROL_BYTES = 4;
        static public int NET_UPLOAD_PACKET_DATA_OFFSET = 3;

        static public byte NET_PACKET_TYPE_GCFG = 0x0001;
        static public byte NET_PACKET_TYPE_DEVCFG = 0x0002;
        static public byte NET_PACKET_TYPE_DEVNAMECFG = 0x0003;
        static public byte NET_PACKET_TYPE_SCHEDCFG = 0x0004;
        static public byte NET_PACKET_TYPE_PASSWDCFG = 0x0005;
        static public byte NET_PACKET_TYPE_AUTH = 0x0006;
        static public byte NET_PACKET_TYPE_NETCFG = 0x0007;


        /***************    EVENTS      *****************/

        static public byte EVENT_DB_INFO_EVNO_POS =  0x00;
        static public byte EVENT_DB_INFO_EVNO_LEN = 0x04;

        static public byte EVENT_DB_INFO_LEN =  (EVENT_DB_INFO_EVNO_LEN);

        static public byte EVENT_DB_ID_POS= 0x00;
        static public byte EVENT_DB_ID_LEN =0x04;

        static public byte EVENT_DB_CODE_POS =  (byte)(EVENT_DB_ID_POS+EVENT_DB_ID_LEN);
        static public byte EVENT_DB_CODE_LEN =  0x02;

        static public byte EVENT_DB_DOMAIN_POS = (byte)(EVENT_DB_CODE_POS + EVENT_DB_CODE_LEN);
        static public byte EVENT_DB_DOMAIN_LEN= 0x02;

        static public byte EVENT_DB_DEVICE_POS = (byte)(EVENT_DB_DOMAIN_POS + EVENT_DB_DOMAIN_LEN);
        static public byte EVENT_DB_DEVICE_LEN= 0x02;

        static public byte EVENT_DB_USER_ID_POS = (byte)(EVENT_DB_DEVICE_POS + EVENT_DB_DEVICE_LEN);
        static public byte EVENT_DB_USER_ID_LEN  =   0x02;

        static public byte EVENT_DB_TIME_POS = (byte)(EVENT_DB_USER_ID_POS + EVENT_DB_USER_ID_LEN);
        static public byte EVENT_DB_TIME_LEN =  0x04;

        static public byte EVENT_DB_DATE_POS = (byte)(EVENT_DB_TIME_POS + EVENT_DB_TIME_LEN);
        static public byte EVENT_DB_DATE_LEN  = 0x04;

        static public byte EVENT_DB_RECORD_LEN = (byte)(EVENT_DB_CODE_LEN + EVENT_DB_DOMAIN_LEN + EVENT_DB_DEVICE_LEN + EVENT_DB_USER_ID_LEN + EVENT_DB_TIME_LEN + EVENT_DB_DATE_LEN);



        /**************  NETWORK **************/
        static public byte NET_UPLOAD_DATA_OFFSET= 0x03;
        static public byte NET_UPLOAD_DATA_END_OFFSET =0x01;
        static public byte NET_CFG_SIZE   = 52;


    }

}
