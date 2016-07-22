using sconnConnector.POCO.Config.Abstract.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmSystemManagmentService
{
    public interface IUsersConfigurationService : IAlarmSystemConfigurationService<sconnRemoteUser>
    {
    }
}
