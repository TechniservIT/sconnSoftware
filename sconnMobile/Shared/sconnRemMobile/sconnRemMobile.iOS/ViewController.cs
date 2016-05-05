﻿using System;
using sconnConnector;
using sconnConnector.POCO.Config;
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
			// Perform any additional setup after loading the view, typically from a nib.
			Button.AccessibilityIdentifier = "myButton";
			Button.TouchUpInside += delegate {
				var title = string.Format ("{0} clicks!", count++);
				Button.SetTitle (title, UIControlState.Normal);
			};

            sconnSite site = new sconnSite();
            sconnCfgMngr mngr = new sconnCfgMngr();

            SconnClient client = new SconnClient("",0,"");
            client.SiteDiscovered += Client_SiteDiscovered;
            client.SearchForSite();
            


		}

        private void Client_SiteDiscovered(object sender, SiteDiscoveryEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

