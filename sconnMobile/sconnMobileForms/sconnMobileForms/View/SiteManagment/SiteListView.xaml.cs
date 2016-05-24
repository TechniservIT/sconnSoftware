using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{
    public partial class SiteListView : ContentPage
    {

        public ISiteRepository Repository { get; set; }
        public List<sconnSite> Sites { get; set; }

        public ListView List { get; set; }

        private void LoadList()
        {
            Sites = Repository.GetAll().ToList();
        }

        public SiteListView()
        {

            Repository = new SiteSqlRepository();
            Sites = new List<sconnSite>();
            List = new ListView();
            List.ItemsSource = Sites;

            InitializeComponent();

        }
    }
}
