using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config.Abstract.Auth
{
    public class sconnUserConfig : IAlarmSystemEntityConfig, IFakeAbleConfiguration
    {
        public List<sconnUser> Users { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnUserConfig()
        {
            UUID = Guid.NewGuid().ToString();
            Users = new List<sconnUser>();
        }

        public int GetEntityCount()
        {
            return Users.Count;
        }

        public void Clear()
        {
            this.Users = new List<sconnUser>();
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
                Users.Add(new sconnUser(buffer));
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
                sconnUser zone = new sconnUser();
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
