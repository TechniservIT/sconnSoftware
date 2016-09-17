using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using NLog;
using sconnConnector;
using sconnConnector.POCO;
using sconnConnector.POCO.Config.Abstract.Event;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.Tools.Extensions;

namespace sconnConnector.POCO.Config.sconn
{

   

    public enum sconnEventType
    {
        EVENT_TYPE_OS             =          0x01,
        EVENT_TYPE_HW             =          0x02,
        EVENT_TYPE_ALRM_SYS       =          0x03,
        EVENT_TYPE_USER_ACTION    =          0x04,
        EVENT_TYPE_IOT            =          0x05
    }


    public enum sconnEvent_AlarmSystem_Type
    {
          EVENT_CODE_ALRM_SYS_ARM                    =         1,
          EVENT_CODE_ALRM_SYS_DISARM                 =         2,

          EVENT_CODE_ALRM_SYS_DET_VIOLATION          =         20,
          EVENT_CODE_ALRM_SYS_DET_SABOTAGE           =         21,
          EVENT_CODE_ALRM_SYS_DET_MASKING            =         22,
          EVENT_CODE_ALRM_SYS_DET_CONN_LOSS          =         23,
          EVENT_CODE_ALRM_SYS_DET_TEMP_BREACH        =         24,
          EVENT_CODE_ALRM_SYS_DET_HUM_BREACH         =         25,

          EVENT_CODE_ALRM_SYS_DEVICE_ADDED           =         70,
          EVENT_CODE_ALRM_SYS_DEVICE_REMOVED         =         71,
          EVENT_CODE_ALRM_SYS_DEVICE_CONNECTED       =         72,
          EVENT_CODE_ALRM_SYS_DEVICE_JOINED          =         73,
          EVENT_CODE_ALRM_SYS_DEVICE_SYNCED          =         74,
          EVENT_CODE_ALRM_SYS_DEVICE_TICKED          =         75,
          EVENT_CODE_ALRM_SYS_DEVICE_TICK_TIMEOUT    =         76,
          EVENT_CODE_ALRM_SYS_MSG_BAD_SES_ID         =         77,
          EVENT_CODE_ALRM_SYS_MSG_BAD_DEV_ID         =         78,
          EVENT_CODE_ALRM_SYS_MSG_BAD_CRC            =         79,
          EVENT_CODE_ALRM_SYS_MSG_BAD_               =         80,

                /**********  SYS CONN ***************/
          EVENT_CODE_ALRM_SYS_DEVICE_ADD_ERR               =   90,
          EVENT_CODE_ALRM_SYS_CLIENT_CONN_FAILED           =   91,
          EVENT_CODE_ALRM_SYS_CLIENT_RECONN_OVF            =   92,
          EVENT_CODE_ALRM_SYS_CLIENT_REQ_FAILED            =   93,
          EVENT_CODE_ALRM_SYS_CLIENT_OPER_REQ_RETRY        =   94,
          EVENT_CODE_ALRM_SYS_CLIENT_OPER_REQ_GIVE_UP      =   95,
          EVENT_CODE_ALRM_SYS_CLIENT_OPER_REQ_FAIL_OVF     =   96,


          EVENT_CODE_ALRM_SYS_CLIENT_HS_RETRY               =  100,
          EVENT_CODE_ALRM_SYS_CLIENT_HS_FAIL_CONN           =  101,
          EVENT_CODE_ALRM_SYS_CLIENT_HS_FAIL_JOIN           =  102,
          EVENT_CODE_ALRM_SYS_CLIENT_HS_FAIL_SYNC           =  103,
          EVENT_CODE_ALRM_SYS_CLIENT_HS_RESET               =  104,
          EVENT_CODE_ALRM_SYS_CLIENT_HS_DISCONN             =  105,
          
                /******** UPDATE *********/
          EVENT_CODE_ALRM_SYS_DEVICE_CFG_UPDATE_ERR        =   120,
          EVENT_CODE_ALRM_SYS_DEVICE_CFG_UPDATE_SUCC       =   101,
          EVENT_CODE_ALRM_SYS_DEVICE_CONN_OVERFLOW          =  120

    }

    public enum sconnEvent_Hardware_Type
    {
          EVENT_CODE_HW_BOOT                          =        1,
          EVENT_CODE_HW_DIAG_START                    =        2,
          EVENT_CODE_HW_DIAG_SUCC                     =        3,
          EVENT_CODE_HW_DIAG_FAIL                     =        4,

          EVENT_CODE_HW_DRVR_INIT_START               =        10,
          EVENT_CODE_HW_DRVR_INIT_SUCC                =        11,
          EVENT_CODE_HW_DRVR_INIT_FAIL                =        12,
          EVENT_CODE_HW_DRVR_FAIL_MEM                 =        13,
          EVENT_CODE_HW_DRVR_FAIL_ETH                 =        14,
          EVENT_CODE_HW_DRVR_FAIL_MIWI                =        15,
          EVENT_CODE_HW_DRVR_FAIL_RS485               =        16,
          EVENT_CODE_HW_DRVR_FAIL_WIFI                =        17,
          EVENT_CODE_HW_DRVR_FAIL_GSM                 =        18,
          EVENT_CODE_HW_DRVR_FAIL_SDCARD              =        19,
          EVENT_CODE_HW_DRVR_FAIL_LCD                 =        20,
          EVENT_CODE_HW_DRVR_FAIL_RFID_125KHZ         =        21,
          
          EVENT_CODE_HW_SUPPLY_MAIN_LOSS              =        40,
          EVENT_CODE_HW_SUPPLY_MAIN_RESTORE           =        41,
          EVENT_CODE_HW_SUPPLY_BACKUP_LOSS            =        42,
          EVENT_CODE_HW_SUPPLY_BACKUP_RESTORE         =        43
    }


    public enum sconnEvent_UserAction_Type
    {
        EVENT_TYPE_ARM = 20,
        EVENT_TYPE_DISARM = 21,
        EVENT_TYPE_ENTER = 22,
        EVENT_TYPE_CHANGE_PASSWD = 23,
        EVENT_TYPE_SET_PASSWD = 24
    }

    public enum sconnEvent_IoT_Type
    {
        EVENT_TYPE_ARM = 20,
        EVENT_TYPE_DISARM = 21,
        EVENT_TYPE_ENTER = 22,
        EVENT_TYPE_CHANGE_PASSWD = 23,
        EVENT_TYPE_SET_PASSWD = 24
    }


    [Serializable]
    public class sconnEvent : IAlarmSystemGenericConfigurationEntity
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        private sconnEventType _type;

        public sconnEventType Type
        {
            get { return _type; }
            set
            {
                _type = value;

                LoadImageTypeUrl();
            }
        }

        public string  Description { get; set; }
        public int Domain { get; set; }
        public int DeviceId { get; set; }
        public int UserId { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public ushort Value { get; set; }

        public string imageIconUri { get; set; }
        public string imageRealUri { get; set; }


        public string GetEventTypeImageUriForDevice(sconnEvent ev)
        {
            if (ev.Type == sconnEventType.EVENT_TYPE_HW)
            {
                return "pack://application:,,,/images/config1000.jpg";
            }
            else if (ev.Type == sconnEventType.EVENT_TYPE_ALRM_SYS)
            {
                return "pack://application:,,,/images/czujka1000x1000.jpg";
            }
            else if (ev.Type == sconnEventType.EVENT_TYPE_IOT)
            {
                return "pack://application:,,,/images/glob1000.jpg";
            }
            else if (ev.Type == sconnEventType.EVENT_TYPE_OS)
            {
                return "pack://application:,,,/images/strefa1000.jpg";
            }
            else if (ev.Type == sconnEventType.EVENT_TYPE_USER_ACTION)
            {
                return "pack://application:,,,/images/person1000.jpg";
            }

            return null;
        }

        public void LoadImageTypeUrl()
        {
            imageIconUri = GetEventTypeImageUriForDevice(this);
        }

        public sconnEvent()
        {
            UUID = Guid.NewGuid().ToString();
        }

        public sconnEvent(byte[] EventBytes) : this()
        {
            this.Deserialize(EventBytes);
        }

        public byte[] Serialize()
        {
            try
            {

                byte[] buffer = new byte[ipcDefines.EVENT_DB_RECORD_LEN];
                buffer[ipcDefines.EVENT_DB_ID_POS + 3] = (byte) Id;
                buffer[ipcDefines.EVENT_DB_TYPE_POS + 1] = (byte) Type;
                buffer[ipcDefines.EVENT_DB_DOMAIN_POS + 1] = (byte) Domain;
                buffer[ipcDefines.EVENT_DB_DEVICE_POS + 1] = (byte) (DeviceId);
                buffer[ipcDefines.EVENT_DB_USER_ID_POS + 1] = (byte) UserId;

                AlarmSystemConfig_Helpers.WriteDateTimeToBufferAtPossition(Time, buffer, ipcDefines.EVENT_DB_TIME_POS);

                return buffer;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public void Deserialize(byte[] buffer)
        {
            try
            {
                //decode
                //TODO better buffer decode 
                if (buffer.Length >= ipcDefines.EVENT_DB_RECORD_LEN)
                {
                    Id = buffer[ipcDefines.EVENT_DB_ID_POS + 3];
                    Type = (sconnEventType) buffer[ipcDefines.EVENT_DB_TYPE_POS + 1];

                    //decode sub-type TODO
                    //if (Type == sconnEventType.EVENT_TYPE_ALRM_SYS)
                    //{
                    //    var sub = (sconnEvent_AlarmSystem_Type) buffer[ipcDefines.EVENT_DB_CODE_POS + 1];
                    //    Description = sub.ToString();
                    //}
                    //else if (Type == sconnEventType.EVENT_TYPE_HW)
                    //{
                    //    var sub = (sconnEvent_Hardware_Type)buffer[ipcDefines.EVENT_DB_CODE_POS + 1];
                    //    Description = sub.ToString();
                    //}


                    Domain = buffer[ipcDefines.EVENT_DB_DOMAIN_POS + 1];
                    DeviceId = buffer[ipcDefines.EVENT_DB_DEVICE_POS + 1];
                    UserId = buffer[ipcDefines.EVENT_DB_USER_ID_POS + 1];
                    Time = AlarmSystemConfig_Helpers.GetDateTimeFromBufferAtPossition(buffer, ipcDefines.EVENT_DB_TIME_POS);
                }
                

            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public void Fake()
        {
            try
            {
                this.Id = 0;
                this.Time = DateTime.Now.Truncate(TimeSpan.FromSeconds(1));
                this.Domain = 0;
                this.UserId = 1;
                this.Type = sconnEventType.EVENT_TYPE_ALRM_SYS;

            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }


        public string UUID { get; set; }
        public byte[] SerializeEntityNames()
        {
            return new byte[0];
        }

        public void DeserializeEntityNames(byte[] buffer)
        {
            
        }

        public void CopyFrom(sconnEvent other)
        {
            this.UUID = other.UUID;
            this.Description = other.Description;
            this.DeviceId = other.DeviceId;
            this.Domain = other.Domain;
            this.Id = other.Id;
            this.Time = other.Time;
            this.Type = other.Type;
            this.UserId = other.UserId;
            this.imageIconUri = other.imageIconUri;
            this.imageRealUri = other.imageRealUri;
        }


        public override bool Equals(object source)
        {
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(this, source);
            return result.AreEqual;
            //sconnEvent other = (sconnEvent) source;
            //return (
            //            this.Id == other.Id &&
            //            this.DeviceId == other.DeviceId &&
            //            this.Domain == other.Domain &&
            //            this.Time.Equals(other.Time) &&
            //            this.Type == other.Type
            //    );
        }


    }

}

