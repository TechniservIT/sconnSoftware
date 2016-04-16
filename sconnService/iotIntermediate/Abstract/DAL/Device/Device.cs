using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using iodash.Models.Auth.Credential;

namespace iodash.Models.Common
{
    public class Device
    {
        [Key]
        [Required]
        public int DeviceId { get; set; }

        [Required]
        public string DeviceName { get; set; }                       

        [Required]
        public virtual EndpointInfo EndpInfo { get; set; }


        public virtual DeviceCredentials Credentials { get; set; }

        public virtual ICollection<DeviceAction> Actions { get; set; }

        public virtual ICollection<DeviceProperty> Properties { get; set; }

        [Required]
        public virtual Location DeviceLocation { get; set; }

        [Required]
        public virtual DeviceType Type { get; set; }

        public virtual Site Site { get; set; }



        private ICommProtocol GetProtocolDelegateForType(CommProtocolType protocol)
        {
            if (protocol == CommProtocolType.CommSconnProtocol)
            {
                return new CommSconnProtocol(this);
            }
            return null;
        }

        private CommProtocolType GetDeviceQueryProtcol()
        {
            if (this.EndpInfo.SupportsSconnProtocol)
            {
                return CommProtocolType.CommSconnProtocol;
            }
            return 0;
        }


        public  Boolean QueryDeviceActions()
        {
            CommProtocolType protType = GetDeviceQueryProtcol();
            ICommProtocol protocol = GetProtocolDelegateForType(protType);

            if (protocol.ProtocolDeviceQueryAble())
            {
                return protocol.LoadDeviceActions(this);
            }
            return false;
        }
        public Boolean QueryDeviceProperties()
        {
            CommProtocolType protType = GetDeviceQueryProtcol();
            ICommProtocol protocol = GetProtocolDelegateForType(protType);

            if (protocol.ProtocolDeviceQueryAble())
            {
                return protocol.LoadDeviceProperties(this);
            }
            return false;
        }


        public Boolean UpdateDevice()
        {
            CommProtocolType protType = GetDeviceQueryProtcol();
            ICommProtocol protocol = GetProtocolDelegateForType(protType);

            if (protocol.ProtocolDeviceQueryAble())
            {
                return protocol.LoadDeviceProperties(this);
            }
            return false;
        }



    }
}