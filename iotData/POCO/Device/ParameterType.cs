using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class ParameterType
    {
        [DataMember]
        [Key]
        [Required]
        public int ParameterId { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string DocumentationURL { get; set; }

        [DataMember]
        public virtual List<DeviceParameter> Parameters { get; set; }

        /*
        public string ParseTypeFromString(string typeval)
        {
            string parsed = "";
            return parsed;
        }

        static public bool TypeExistsForNameInContext(string Name, iotDeviceContext cont)
        {
           // ApplicationDbContext cont = new ApplicationDbContext();
            if (cont.ParamTypes.Count() <= 0)
            {
                return false;
            }
            try
            {
                ParameterType type = (from t in cont.ParamTypes
                                      where t.Name == Name
                                      select t).First();
                if (type != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
            }
            return false;
        }

        static public ParameterType TypeForNameAtContext(string Name, ApplicationDbContext cont)
        {
            try
            {
                ParameterType type;
                if (!TypeExistsForNameInContext(Name, cont))
                {
                    type = new ParameterType();
                    type.Name = Name;
                    cont.ParamTypes.Add(type);
                    cont.SaveChanges();
                }
                type = (from t in cont.ParamTypes
                        where t.Name == Name
                        select t).First();
                return type;
            }
            catch (Exception e)
            {
                
            }
            return new ParameterType();
        }

         */

    }
}