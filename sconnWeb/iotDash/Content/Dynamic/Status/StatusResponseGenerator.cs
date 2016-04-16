using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static string ContainerStart = "<div class='col-md-3'>";
        private static string ContainerEnd = "  </div>";

        public static string GetSuccessPanelWithMsgAndStat(string msg, RequestStatus stat)
        {
            string panel = String.Format(ContainerStart + " <p class='bg-success'>{0}</p> " + ContainerEnd, msg);
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

            string panel = String.Format(ContainerStart + " <p class='" + cclass + "'>{0}</p> " + ContainerEnd, msg);
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

            string panel = String.Format(ContainerStart + " <p class='alert " + cclass + "'>{0}</p> " + ContainerEnd, msg);
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

            string panel = String.Format(ContainerStart + " <p class='alert " + cclass + " alert-dismissible'>{0}</p> " + ContainerEnd, msg);
            return panel;
        }

        static public string GetStatusResponseResultForReturnParam(object result)
        {
            try
            {
                TypeCode code = Convert.GetTypeCode(result);
                bool isObj = code == TypeCode.Object;  
                if (isObj)
                {
                    if (result != null)
                    {
                        return GetRequestResponseSuccess();
                    }
                    return GetRequestResponseFailure();
                }
                else
                {
                    if (code == TypeCode.Boolean)   //check the response value for boolean
                    {
                            return (bool)result ? GetRequestResponseSuccess() : GetRequestResponseFailure();
                    }
                    return GetRequestResponseSuccess();
                }
            }
            catch (Exception)
            {
                return GetRequestResponseFailure();
            }
        }


        static public string GetStatusResponseResultForMethod(object result, Delegate method)
        {
            var methodInfo = RuntimeReflectionExtensions.GetMethodInfo(method);
            Type ret = methodInfo.ReturnType;
            TypeCode returnType = Convert.GetTypeCode(ret);
            try
            {
                bool isObj = returnType == TypeCode.Object;    //Convert.GetTypeCode(returnType)
                if (isObj)
                {
                    if (result != null && Convert.GetTypeCode(result) == returnType)
                    {
                        return GetRequestResponseSuccess();
                    }
                    return GetRequestResponseFailure();
                }
                else
                {
                    if (Convert.GetTypeCode(result) == returnType)
                    {
                        if (returnType == TypeCode.Boolean)   //check the response value for boolean
                        {
                            return (bool)result ? GetRequestResponseSuccess() : GetRequestResponseFailure();
                        }
                        return GetRequestResponseSuccess();
                    }
                    return GetRequestResponseFailure();
                }
            }
            catch (Exception)
            {
                return GetRequestResponseFailure();
            }
        }

        static public string GetStatusResponseResultForReturnParamType(object result, TypeCode returnType)
        {
            try
            {
                bool isObj = returnType  == TypeCode.Object;    //Convert.GetTypeCode(returnType)
                if (isObj)
                {
                    if (result != null && Convert.GetTypeCode(result) == returnType)
                    {
                        return GetRequestResponseSuccess();
                    }
                    return GetRequestResponseFailure();
                }
                else
                {
                    if (Convert.GetTypeCode(result) == returnType)
                    {
                        if (returnType == TypeCode.Boolean)   //check the response value for boolean
                        {
                            return (bool)result ? GetRequestResponseSuccess() : GetRequestResponseFailure();
                        }
                        return GetRequestResponseSuccess();
                    }
                    return GetRequestResponseFailure();
                }
            }
            catch (Exception)
            {
                return GetRequestResponseFailure();
            }
        }

        static public string GetRequestResponseCriticalError()
        {
            return GetAlertPanelWithMsgAndStat("Internal server error.", RequestStatus.Failure);
        }

        static public string GetRequestResponseFailure()
        {
            return GetAlertPanelWithMsgAndStat("Request failed.", RequestStatus.Warning);
        }

        static public string GetRequestResponseSuccess()
        {
            return GetAlertPanelWithMsgAndStat("Success.", RequestStatus.Success);
        }
    }
}
