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


    static public class StatusResponseGenerator
    {


        static public string GetSuccessPanelWithMsgAndStat(string msg, RequestStatus stat)
        {
            string panel = String.Format(" <p class='bg-success'>{0}</p> ", msg);
            return panel;
        }

    }
}
