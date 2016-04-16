using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.Models
{
    public interface IAsyncStatusModel
    {
        string Result { get; set; }
    }

    public abstract class AsyncResultStatusModel
    {
        public string Result { get; set; }
    }
}