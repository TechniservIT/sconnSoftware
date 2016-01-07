using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract.IO;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{

    public enum sconnInputType
    {
        NormallyOpen = 0,
        NormallyClosed,
        ParametrizedNC,
        ParametrizedArm,
        ParametrizedSensor
    }

    public enum sconnActivationGroup
    {
        Arm = 0,
        Disarm,
        ActivateOutput,
        ArmedViolation,
        DisarmedViolation,
        ArmedAndDisarmedViolation
    }

    public class sconnInput : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public int NameId { get; set; }
        public int Id { get; set; }
        public sconnInputType Type { get; set; }
        public int Value { get; set; }
        public int Sensitivity { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public sconnActivationGroup ActivationGroup { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnInput()
        {
            Name = "Input";
        }

        public sconnInput(byte[] rawBytes) : this()
        {
                this.Deserialize(rawBytes);
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.mAdrInputMemSize];
                buffer[ipcDefines.mAdrInputType] = (byte)Type;
                buffer[ipcDefines.mAdrInputEnabled] = (byte)(Enabled ? 1 : 0);
                buffer[ipcDefines.mAdrInputVal] = (byte)Value;
                buffer[ipcDefines.mAdrInputNameAddr] = (byte)NameId;
                buffer[ipcDefines.mAdrInputSensitivity] = (byte)(Sensitivity / ipcDefines.InputSensitivityStep);
                buffer[ipcDefines.mAdrInputAG] = (byte)ActivationGroup;
                return buffer;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }

        public void Deserialize(byte[] buffer)
        {
            try
            {
                Type = (sconnInputType)buffer[ipcDefines.mAdrInputType];
                Value = buffer[ipcDefines.mAdrInputVal];
                NameId = buffer[ipcDefines.mAdrInputNameAddr];
                Enabled = buffer[ipcDefines.mAdrInputEnabled] > 0 ? true : false;
                Sensitivity = buffer[ipcDefines.mAdrInputSensitivity] * ipcDefines.InputSensitivityStep;
                ActivationGroup = (sconnActivationGroup)buffer[ipcDefines.mAdrInputAG];

            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
   }

        public void Fake()
        {
            try
            {
                this.Id = 0;
                this.Enabled = true;
                this.Name = Guid.NewGuid().ToString();
                this.NameId = 0;
                this.Value = 1;
                this.Type = sconnInputType.NormallyClosed;
                this.ActivationGroup = sconnActivationGroup.ArmedViolation;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

    }

}
