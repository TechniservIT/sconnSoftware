using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config.Abstract.Auth
{
    public class sconnRemoteUserConfig : IAlarmSystemEntityConfig, IFakeAbleConfiguration
    {
        public List<sconnRemoteUser> Users { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnRemoteUserConfig()
        {
            UUID = Guid.NewGuid().ToString();
            Users = new List<sconnRemoteUser>();
        }

        public int GetEntityCount()
        {
            return Users.Count;
        }

        public void Clear()
        {
            this.Users = new List<sconnRemoteUser>();
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
                Users.Add(new sconnRemoteUser(buffer));
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
                sconnRemoteUser zone = new sconnRemoteUser();
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
