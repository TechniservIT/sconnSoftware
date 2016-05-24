 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

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
    

    public class DeviceType
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        [DisplayName("Name")]
        public string TypeName { get; set; }

         
        [DisplayName("Description")]
        public string TypeDescription { get; set; }

         
        [DisplayName("Image URL")]
        public string VisualRepresentationURL { get; set; }

         
        public virtual List<Device> Devices { get; set; }

          
         [DisplayName("Category")]
        public DeviceCategory Category { get; set; }
        

        [Required]
         
        public virtual iotDomain Domain { get; set; }

        public DeviceType()
        {
            Devices = new List<Device>();
        }

        public void Load(DeviceType type)
        {
            this.Id = type.Id;
            this.Category = type.Category;
            this.TypeDescription = type.TypeDescription;
            this.TypeName = type.TypeName;
            this.VisualRepresentationURL = type.VisualRepresentationURL;
        }

    }
}