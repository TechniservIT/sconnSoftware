using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;


namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceAction 
    {

        [DataMember]
        [Key]
        [Required]
        public int ActionId { get; set; }

        [DataMember]
        [Required]
        public string ActionName { get; set; }

        [DataMember]
        public string ActionDescription { get; set; }

        [DataMember]
        [Required]
        public virtual List<ActionParameter> RequiredParameters { get; set; }

        [DataMember]
        public string VisualRepresentationURL { get; set; }



        [DataMember]
        [Required]
        public virtual List<DeviceParameter> ResultParameters { get; set; }


        [DataMember]
        public DateTime LastActivationTime { get; set; }

        [DataMember]
        [Required]
        public virtual Device Device { get; set; }


        /*
        public DeviceAction()
        {
            RequiredParameters = new List<ActionParameter>();
            ResultParameters = new List<DeviceParameter>();
        }

        public bool AddReqParam(DeviceParameter param)
        {
            try
            {
                ApplicationDbContext cont = new ApplicationDbContext();
                DeviceAction self = (from dp in cont.Actions
                                       where dp.ActionId == this.ActionId
                                       select dp).First();
                if (self != null)
                {
                    param.Action = self;
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

        public bool AddResultParam(DeviceParameter param)
        {
            try
            {
                ApplicationDbContext cont = new ApplicationDbContext();
                DeviceAction self = (from dp in cont.Actions
                                       where dp.ActionId == this.ActionId
                                       select dp).First();
                if (self != null)
                {
                    param.Action = self;
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


        public bool ExsistsInDatabaseContext(ApplicationDbContext cont)
        {
            try
            {
                DeviceAction self = (from dp in cont.Actions
                                     where dp.ActionId == this.ActionId
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

        public DeviceAction  CreateWithContext(ApplicationDbContext cont)
        {
            try
            {
                if (!this.ExsistsInDatabaseContext(cont))
                {
                    cont.Actions.Add(this);
                    cont.SaveChanges();
                }
                DeviceAction self = (from dp in cont.Actions
                                       where dp.ActionId == this.ActionId
                                       select dp).FirstOrDefault();
                return self;
            }
            catch (Exception e)
            {
                return new DeviceAction();
            }


        }


        public Boolean Perform()
        {
            try
            {
                CommProtocolType protType = this.Device.GetDeviceQueryProtcol();
                ICommProtocol protocol = this.Device.GetProtocolDelegateForType(protType);
                if (protocol.ProtocolDeviceQueryAble())
                {
                    bool stat =  protocol.PerformActionAsync(this);
                    return stat;

                    if (stat)   //reload result
                    {
                        return this.Device.QueryDeviceActions();
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