using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace sconnMobileForms.View.SiteManagment
{
    public class sconnSitesPage
    {
        public sconnSitesPage()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public bool Done { get; set; }

    }
}
