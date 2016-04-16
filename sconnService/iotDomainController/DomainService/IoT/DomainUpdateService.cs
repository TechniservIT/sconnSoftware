
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace iotDomainController.DomainService
{
    /* Update domains data in specified intervals */
    public class DomainUpdateService
    {
        private List<iotDomain> Domains;

        private DateTime LastUpdateTime;

        private int UpdateIntervalSeconds;

        private bool Running;

        private Timer UpdateTimer;


        public DomainUpdateService()
        {
            UpdateTimer = new Timer();
            UpdateTimer.Elapsed += UpdateTimer_Elapsed;
            Domains = new List<iotDomain>();
        }

        void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LastUpdateTime = DateTime.Now;
            Parallel.ForEach(Domains, item => UpdateDomain(item));   
        }

        private void UpdateDomain(iotDomain domain)
        {
            Parallel.ForEach(domain.Sites, item => UpdateSite(item));
        }

        private void UpdateSite(Site site)
        {
            Parallel.ForEach(site.Devices, item => UpdateDevice(item));
        }

        private void UpdateDevice(Device dev)
        {
            try
            {
               
            }
            catch (Exception e)
            {
            }
        }

        public DomainUpdateService(int UpdateIntervalSec) :this()
        {
            this.UpdateIntervalSeconds = UpdateIntervalSec;
            UpdateTimer.Interval = this.UpdateIntervalSeconds;
        }

        public void Start()
        {
            Running = true;
            UpdateTimer.Start();
        }

        public void Stop()
        {
            Running = false;
            UpdateTimer.Stop();
        }

    }

}
