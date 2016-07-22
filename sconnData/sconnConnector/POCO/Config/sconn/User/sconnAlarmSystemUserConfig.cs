using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract.Auth;

namespace sconnConnector.POCO.Config.sconn.User
{

    public class sconnAlarmSystemUserConfig : IAlarmSystemEntityConfig, IFakeAbleConfiguration
    {
        public List<sconnAlarmSystemUser> Users { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnAlarmSystemUserConfig()
        {
            UUID = Guid.NewGuid().ToString();
            Users = new List<sconnAlarmSystemUser>();
        }

        public int GetEntityCount()
        {
            return Users.Count;
        }

        public void Clear()
        {
            this.Users = new List<sconnAlarmSystemUser>();
        }

        public byte[] SerializeEntityWithId(int id)
        {
            try
            {
                return Users[id].Serialize();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }
        public void DeserializeEntityWithId(byte[] buffer)
        {
            try
            {
                Users.Add(new sconnAlarmSystemUser(buffer));
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
                sconnAlarmSystemUser zone = new sconnAlarmSystemUser();
                zone.Fake();
                Users.Add(zone);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

        public string UUID { get; set; }
    }

}
