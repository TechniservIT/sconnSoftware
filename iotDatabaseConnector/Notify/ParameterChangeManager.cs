using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDatabaseConnector.DAL.POCO.Device.Notify
{
    static public class ParameterChangeManager
    {


        static public void StoreParamChange(DeviceParameter param)
        {
            try
            {
                //add history only if param already in DB
                iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                DeviceParameter stparam = repo.GetById(param.ParameterId);
                if (stparam != null)
                {
                    ParameterChangeHistory hist = new ParameterChangeHistory();
                    hist.Date = DateTime.Now;
                    hist.Property = param;
                    hist.Value = param.Value;
                    iotRepository<ParameterChangeHistory> histrepo = new iotRepository<ParameterChangeHistory>();
                    histrepo.Add(hist);
                }
           } 
           catch (Exception e)
            {
              
            }
        }

    }

}
