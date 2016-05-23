using System;
using AlarmSystemManagmentService;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Device;
using UIKit;

namespace sconnRemMobile.iOS
{
	public partial class ViewController : UIViewController
	{
		int count = 1;

		public ViewController (IntPtr handle) : base (handle)
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


            sconnSite site = new sconnSite();

            sconnCfgMngr mngr = new sconnCfgMngr();
		    site.serverIP = "192.168.1.108";
		    site.serverPort = 9898;
		    site.authPasswd = "testpass";
         
            EndpointInfo info = new EndpointInfo();
		    info.Hostname = site.serverIP;
		    info.Port = site.serverPort;

            DeviceCredentials cred = new DeviceCredentials();
		    cred.Password = site.authPasswd;
		    cred.Username = site.authPasswd;



            AlarmSystemConfigManager man = new AlarmSystemConfigManager(info,cred);

            GlobalConfigService  gs = new GlobalConfigService(man);

		    var dev = gs.Get();

            // Perform any additional setup after loading the view, typically from a nib.
            Button.AccessibilityIdentifier = "myButton";
			Button.TouchUpInside += delegate {
				var title = string.Format ("{0} clicks!", count++);
				Button.SetTitle (title, UIControlState.Normal);
			};

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

