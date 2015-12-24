using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDash.Content.Dynamic.Status
{
    public enum RequestStatus
    {
        Success = 1,
        Failure,
        Warning
    }


    public static class StatusResponseGenerator
    {
       
        public static string GetSuccessPanelWithMsgAndStat(string msg, RequestStatus stat)
        {
            string panel = String.Format(" <p class='bg-success'>{0}</p> ", msg);
            return panel;
        }

        public static string GetPanelWithMsgAndStat(string msg, RequestStatus stat)
        {
            string cclass = "";
            if (stat == RequestStatus.Success)
            {
                cclass = "bg-success";
            }
            else if (stat == RequestStatus.Failure)
            {
                cclass = "bg-failure";
            }

            string panel = String.Format(" <p class='" + cclass + "'>{0}</p> ", msg);
            return panel;
        }

        public static string GetAlertPanelWithMsgAndStat(string msg, RequestStatus stat)
        {
            string cclass = "";
            if (stat == RequestStatus.Success)
            {
                cclass = "alert-success";
            }
            else if (stat == RequestStatus.Failure)
            {
                cclass = "alert-danger";
            }

            string panel = String.Format(" <p class='alert " + cclass + "'>{0}</p> ", msg);
            return panel;
        }

        static public string GetDismissablePanelWithMsgAndStat(string msg, RequestStatus stat)
        {
            string cclass = "";
            if (stat == RequestStatus.Success)
            {
                cclass = "alert-success";
            }
            else if (stat == RequestStatus.Failure)
            {
                cclass = "alert-danger";
            }

            string panel = String.Format(" <p class='alert " + cclass + " alert-dismissible'>{0}</p> ", msg);
            return panel;
        }

    }
}
