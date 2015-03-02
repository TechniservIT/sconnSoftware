using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceProperty
    {
        [DataMember]
        [Key]
        [Required]
        public int PropertyId { get; set; }

        [DataMember]
        [Required]
        public string PropertyName { get; set; }

        [DataMember]
        public string  PropertyDescription { get; set; }

        [DataMember]
        public string VisualRepresentationURL { get; set; }

        [DataMember]
        public virtual List<DeviceParameter> ResultParameters { get; set; }

        [DataMember]
        public DateTime LastUpdateTime { get; set; }

        [DataMember]
        [Required]
        public virtual Device Device { get; set; }

        /*
        public bool AddResultParamToContext(DeviceParameter param, iotContext cont)
        {
            try
            {
               // ApplicationDbContext cont = new ApplicationDbContext();
                DeviceProperty self = (from dp in cont.Properties
                                       where dp.PropertyId == this.PropertyId
                                       select dp).First();
                if (self != null)
                {
                    param.Property = self;
                    if (param != null)
                    {
                        cont.Parameters.Add(param);
                        cont.SaveChanges();

                        //verify
                        DeviceParameter storedparam = (from dp in cont.Parameters
                                                       where param.ParameterId == dp.ParameterId
                                                       select dp).First();
                        if (storedparam != null)
                        {
                            return true;
                        }
                    }
                }

            }
            catch (Exception e)
            {

            }

            return false;
        }

        public bool ExsistsInDatabaseContext(iotContext cont)
        {
            try
            {
                //ApplicationDbContext cont = new ApplicationDbContext();
                DeviceProperty self = (from dp in cont.Properties
                                       where dp.PropertyId == this.PropertyId
                                       select dp).FirstOrDefault();
                if (self != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                
            }
            return false;
        }

        public DeviceProperty CreateWithContext(iotContext cont)
        {
            try
            {
                //ApplicationDbContext cont = new ApplicationDbContext();
                if (!this.ExsistsInDatabaseContext(cont))
                {
                    cont.Properties.Add(this);
                    cont.SaveChanges();
                }
                DeviceProperty self = (from dp in cont.Properties
                                       where dp.PropertyId == this.PropertyId
                                       select dp).FirstOrDefault();
                return self;
            }
            catch (Exception e)
            {
                return new DeviceProperty();
            }


        }
        */

    }


}