using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config
{

    public class sconnInput
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int Value { get; set; }

        public int NameId { get; set; }

        public int ActivationGroup { get; set; }

        public int Sensitivity { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }


        public sconnInput()
        {

        }


        public sconnInput(byte[] config, int seqno)
        {
            int baseaddr = ipcDefines.mAdrInput + ipcDefines.mAdrInputMemSize * seqno;
            Id = seqno;
            Type = config[baseaddr + ipcDefines.mAdrInputType];
            Value = config[baseaddr + ipcDefines.mAdrInputVal];
            NameId = config[baseaddr + ipcDefines.mAdrInputNameAddr];
            Sensitivity = config[baseaddr + ipcDefines.mAdrInputSensitivity] * ipcDefines.InputSensitivityStep;
            ActivationGroup = config[baseaddr + ipcDefines.mAdrInputAG];

        }

        public sconnInput(byte[] config, byte[] namecfg, int seqno)
            : this(config, seqno)
        {
            if (namecfg != null)
            {
                Name = System.Text.Encoding.Unicode.GetString(namecfg, 0, namecfg.GetLength(0));
            }


        }

    }

}
