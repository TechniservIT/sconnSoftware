using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sconnMobileForms.View.SiteManagment;
using Xamarin.Forms;

namespace sconnMobileForms
{
	public class App : Application
	{
        public static INavigation Navigation { get; set; }

        public App ()
		{
            var nav = new NavigationPage(new SiteListView())
            {
               // BarBackgroundColor = Color.Aqua,
                BarTextColor = Color.Black,
            };
            MainPage = nav;


            Navigation = nav.Navigation;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
