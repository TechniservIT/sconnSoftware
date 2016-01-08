 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{

    public enum DeviceCategory
    {
        GenericInputOutput = 1,
        AlarmSystem,
        IpCamera,
        Vehicle,
        Thermostat,
        Sensor,
        CommunicationModule,
        SpectrumAnalyser,
        Oscilloscope,
        Speaker,
        Servo,
        ThreePhaseMotor,
        Actuator,
        GpsTracked
    }


    public enum DeviceIoCategory
    {
        CmosInputs = 1,
        CmosOutput,
        Temperature,
        Humidity,
        PowerOutput,
        AnalogInput,
        Relay
    }

    [DataContract(IsReference = true)]
    public class DeviceType
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Name")]
        public string TypeName { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string TypeDescription { get; set; }

        [DataMember]
        [DisplayName("Image URL")]
        public string VisualRepresentationURL { get; set; }

        [DataMember]
        public virtual List<Device> Devices { get; set; }

         [DataMember]
         [DisplayName("Category")]
        public DeviceCategory Category { get; set; }
        

        [Required]
        [DataMember]
        public virtual iotDomain Domain { get; set; }

        public DeviceType()
        {
            Devices = new List<Device>();
        }
    }
}