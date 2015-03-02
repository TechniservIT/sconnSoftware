using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{

    [DataContract(IsReference = true)]
    public class sconnConfigMapper
    {
        [Key]
        [Required]
        [DataMember]
        public int MapperId { get; set; }

        [DataMember]
        [Required]
        public int ConfigType { get; set; }


        [DataMember]
        [Required]
        public int SeqNumber { get; set; }

        [DataMember]
        public virtual DeviceParameter Parameter { get; set; }

        [DataMember]
        public virtual ActionParameter ActionParam { get; set; }


        /*
        public bool ExsistsInDatabaseContext(iotContext cont)
        {
           // ApplicationDbContext cont = new ApplicationDbContext();
            try
            {
                sconnConfigMapper self;
                if (cont.SconnMappers.Count() > 0)
                {
                    self = (from dp in cont.SconnMappers
                            where dp.MapperId == this.MapperId
                            select dp).FirstOrDefault();
                    if (self != null)
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
        //create record if required
        public sconnConfigMapper CreateWithContext(iotContext cont)
        {
            try
            {
               // ApplicationDbContext cont = new ApplicationDbContext();
                if (!this.ExsistsInDatabaseContext(cont))
                {
                    cont.SconnMappers.Add(this);
                    cont.SaveChanges();
                }
                sconnConfigMapper self = (from dp in cont.SconnMappers
                                          where dp.MapperId == this.MapperId
                                          select dp).FirstOrDefault();

                return self; 
            }
            catch (Exception e)
            {

            }
             return new sconnConfigMapper();
        }
        */

    }
}