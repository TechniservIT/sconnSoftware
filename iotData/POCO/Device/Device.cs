using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Runtime.Serialization;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class Device
    {
        [DataMember]
        [Key]
        [Required]
        public int DeviceId { get; set; }

        [DataMember]
        [Required]
        public string DeviceName { get; set; }

        [DataMember]
        [Required]
        public virtual EndpointInfo EndpInfo { get; set; }

        [DataMember]
        public virtual DeviceCredentials Credentials { get; set; }

        [DataMember]
        public virtual List<DeviceAction> Actions { get; set; }

        [DataMember]
        public virtual List<DeviceProperty> Properties { get; set; }

        [DataMember]
        [Required]
        public virtual Location DeviceLocation { get; set; }

        [DataMember]
        [Required]
        public virtual DeviceType Type { get; set; }

        [DataMember]
        public virtual Site Site { get; set; }

        /*
        public bool AddProperty(DeviceProperty prop)
        {
            try
            {
                ApplicationDbContext cont = new ApplicationDbContext();
                Device self = (from dp in cont.Devices
                               where dp.DeviceId == this.DeviceId
                               select dp).First();
                if (self != null && prop != null)
                {
                    prop.Device = self;
                    cont.Properties.Add(prop);
                    cont.SaveChanges();

                    //verify
                    DeviceProperty stored = (from dp in cont.Properties
                                             where prop.PropertyId == dp.PropertyId
                                           select dp).First();
                    if (stored != null)
                    {
                        return true;
                    }
                }

            }
            catch (Exception e)
            {

            }
            return false;
        }

        public bool AddAction(DeviceAction action)
        {
            try
            {
                ApplicationDbContext cont = new ApplicationDbContext();
                Device self = (from dp in cont.Devices
                                     where dp.DeviceId == this.DeviceId
                                     select dp).First();
                if (self != null    && action != null)
                {
                        action.Device = self;
                        cont.Actions.Add(action);
                        cont.SaveChanges();

                        //verify
                        DeviceAction stored = (from dp in cont.Actions
                                                       where action.ActionId == dp.ActionId
                                                       select dp).First();
                        if (stored != null)
                        {
                            return true;
                        }
                }

            }
            catch (Exception e)
            {

            }
            return false;
        }
        */


    }
}