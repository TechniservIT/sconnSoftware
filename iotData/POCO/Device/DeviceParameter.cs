using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
using iotDatabaseConnector.DAL.POCO.Device.Notify;


namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceParameter : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        [DataMember]
        [Key]
        [Required]
        public int ParameterId { get; set; }


        private string _Value;

        [DataMember]
        [Required]
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value != _Value)
                {
                    _Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        [DataMember]
        [Required]
        public virtual ParameterType Type { get; set; }

        [DataMember]
        public string ParamDescription { get; set; }

        [DataMember]
        public string VisualRepresentationUrl { get; set; }

        [DataMember]
        public virtual DeviceAction Action { get; set; }

        [DataMember]
        public virtual DeviceProperty Property { get; set; }

        [DataMember]
        public virtual List<sconnConfigMapper> sconnMappers { get; set; }

        [DataMember]
        public virtual List<ParameterChangeHistory> Changes { get; set; }

        public bool AddSconnMapper(sconnConfigMapper mapper)
        {
            return false;
        }


        /*
        public bool ExsistsInDatabaseContext(ApplicationDbContext cont)
        {
           // ApplicationDbContext cont = new ApplicationDbContext();
            try
            {
                DeviceParameter self;
                if (cont.Parameters.Count() > 0)
                {
                    self = (from dp in cont.Parameters
                            where dp.ParameterId == this.ParameterId
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

        public DeviceParameter CreateWithContext(ApplicationDbContext cont)
        {
            try
            {
            //    ApplicationDbContext cont = new ApplicationDbContext();
                if (!this.ExsistsInDatabaseContext(cont))
                {
                    cont.Parameters.Add(this);
                    cont.SaveChanges();

                }
                DeviceParameter self = (from dp in cont.Parameters
                                        where dp.ParameterId == this.ParameterId
                                        select dp).FirstOrDefault();
                return self;
            }
            catch (Exception e)
            {
                return new DeviceParameter();
            }


        }
         
         */

    }
}