using System;
using System.Collections.Generic;
using System.Text;

namespace sconnMobileForms.Service.AlarmSystem.Io
{
    public interface IAlarmIoConfigService
    {
        void Toggle();
        bool Get();
        void Set(bool value);
    
    }

}
