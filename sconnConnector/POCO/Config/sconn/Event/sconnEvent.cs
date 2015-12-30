using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract.Event;

namespace sconnConnector.POCO.Config.sconn
{

    public enum sconnEventType
    {
         EVENT_TYPE_ARM       =           20,
         EVENT_TYPE_DISARM    =           21,
         EVENT_TYPE_ENTER     =           22,
         EVENT_TYPE_CHANGE_PASSWD   =     23,
         EVENT_TYPE_SET_PASSWD      =     24,
         EVENT_TYPE_DET_VIOLATION   =     25,
         EVENT_TYPE_DET_SABOTAGE    =     26,
         EVENT_TYPE_CONFIG_CHANGE   =     27,
         EVENT_TYPE_ACTIVATION      =     28,
         EVENT_TYPE_HW_FAILURE      =     29,
         EVENT_TYPE_BOOT            =     30,
         EVENT_TYPE_DET_MASKING     =     31,
         EVENT_TYPE_DET_CONN_LOSS   =     32,
         EVENT_TYPE_DET_TEMP_BREACH =     33,
         EVENT_TYPE_DET_HUM_BREACH  =     34,
         EVENT_TYPE_DEVICE_ADDED    =     35,
         EVENT_TYPE_POWER_LOSS      =     36,
         EVENT_TYPE_ADD_USER        =     37,
         EVENT_TYPE_REMOVE_USER     =     38
    }

    public class sconnEvent
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public sconnEventType Type { get; set; }
        public int Domain { get; set; }
        public int DeviceId { get; set; }
        public int UserId { get; set; }

        public sconnEvent()
        {

        }

        public sconnEvent(byte[] EventBytes) : this()
        {
            //decode
            //TODO better buffer decode 
            if (EventBytes.Length >= ipcDefines.EVENT_DB_RECORD_LEN)
            {
                Id = EventBytes[ipcDefines.EVENT_DB_ID_POS + 3];
                Type = (sconnEventType)EventBytes[ipcDefines.EVENT_DB_CODE_POS + 1];
                Domain = EventBytes[ipcDefines.EVENT_DB_DOMAIN_POS + 1];
                DeviceId = EventBytes[ipcDefines.EVENT_DB_DEVICE_POS + 1];
                UserId = EventBytes[ipcDefines.EVENT_DB_USER_ID_POS + 1];

            }
        }
    }

}
