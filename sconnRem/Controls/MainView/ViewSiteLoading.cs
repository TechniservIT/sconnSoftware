using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using sconnConnector;

namespace sconnRem
{
    
    class ViewSiteLoading : StackPanel
    {
        private Label lblConnectionProgress = new Label();
        private ProgressBar pbar = new ProgressBar();
        private int SiteId;
        private sconnCfgMngr ConfMan = new sconnCfgMngr();
        private ETH ethernet = new ETH();

        public delegate void UpdateProgressDel();
        public UpdateProgressDel UpdateDel;
        

        public delegate void ConnectingFinished(object sender, EventArgs e, bool success, int siteId);
        public event ConnectingFinished ConnectedDel;

        public enum ConnectionProgress { Connecting=0, Authenticating, Authenticated, RequestingData, Recieving, Success, ConnectErr, ReadErr };

        private ConnectionProgress _Progress;
        public ConnectionProgress Progress {
            get { return _Progress; }
            set {
                _Progress = value;
                UpdateHandler();
                } 
        }

        private void ProgressBarInit()
        {
            lblConnectionProgress.FontSize = 18.0;
            lblConnectionProgress.Content = StatusToString(ConnectionProgress.Connecting);
            this.Children.Add(lblConnectionProgress);
            pbar.Height = 50.00;
            pbar.Value = 0;
            pbar.Maximum = 5;
            this.Children.Add(pbar);
        }

        public ViewSiteLoading()
        {
            ProgressBarInit();

        }

         public ViewSiteLoading(int id) 
        {
            SiteId = id;
            ProgressBarInit();

        }

        public void TestConnection()
         {
             Thread updateThread = new Thread(() => ConnectToSite() );
             updateThread.Start();

         }

        public bool ConnectToSite()
         {
             try
             {
                 Progress = ConnectionProgress.Connecting;
                 sconnSite site = sconnDataShare.getSite(SiteId);
                 Progress = ConnectionProgress.Authenticating;
                 bool stat = ConfMan.ReadSiteRunningConfig(site);   //update
                 if (stat)
                 {
                     Progress = ConnectionProgress.Success;
                     Dispatcher.BeginInvoke((Action)(() =>
                     {
                         ConnectedDel.Invoke(this, new EventArgs(), true, this.SiteId);
                     }));
                 }
                 else
                 {
                     Progress = ConnectionProgress.ConnectErr;
                 }
                 return stat;
             }
             catch (Exception)
             {
                 Progress = ConnectionProgress.ConnectErr;
                 throw;
             }
        }


        public void UpdateHandler()
        {
            UpdateDel = delegate() { 
                                 lblConnectionProgress.Content = StatusToString(this.Progress);                          
                                   };
            lblConnectionProgress.Dispatcher.Invoke(UpdateDel);
            UpdateDel = delegate()
            {
                pbar.Value = (int)Progress;
            };
            pbar.Dispatcher.Invoke(UpdateDel);

        }



        public void UpdateProgress()
         {           
             lblConnectionProgress.Content = StatusToString(this.Progress);
             pbar.Value = (int)Progress;
         }

        private string StatusToString(ConnectionProgress prog)
        {
            string sitename = sconnDataShare.getSite(SiteId).siteName;
            string resp = Properties.Resources.lblStatus_ConnectError;
            if ( prog == ConnectionProgress.Connecting )
            {
                 resp = Properties.Resources.lblStatus_Connecting + "  " + sitename;
                return resp;
            }
            else if (prog == ConnectionProgress.Authenticating)
            {
                resp = Properties.Resources.lblStatus_Authenticating + "  " + sitename;
                return resp;
            }
            else if (prog == ConnectionProgress.Authenticated)
            {
                resp = Properties.Resources.lblStatus_Authenticated + "  " + sitename;
                return resp;
            }
            else if (prog == ConnectionProgress.RequestingData)
            {
                resp = Properties.Resources.lblStatus_RequestingData + "  " + sitename;
                return resp;
            }
            else if (prog == ConnectionProgress.Recieving)
            {
                resp = Properties.Resources.lblStatus_Recieving + "  " + sitename;
                return resp;
            }
            else if (prog == ConnectionProgress.Success)
            {
                resp = Properties.Resources.lblStatus_Success + "  " + sitename;
                return resp;
            }
            else if (prog == ConnectionProgress.ConnectErr || prog == ConnectionProgress.ReadErr)
            {
                resp = Properties.Resources.lblStatus_ConnectError + "  " + sitename;
                return resp;
            }
           else
                {
                    return resp;
                }

        }
    }


}
