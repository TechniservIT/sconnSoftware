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

namespace sconnRem
{

    public class StatusViewPanel : StackPanel
    {
        private string _statusText;
        public string StatusText { get { return _statusText; } }

        public void SetStatusText(string text)
        {
            if (text != null && text.Length > 0)
            {
                _statusText = text;
                GetViewBody(); //update body
            }
        }

        private void GetViewBody()
        {
            this.Children.Clear();
            Label status = new Label();
            status.Content = _statusText;
            status.Height = this.Height * 0.5;
            status.Width = this.Width;
            this.Children.Add(status);
        }

        public StatusViewPanel()
        {
            _statusText = "";
            GetViewBody();
        }

        public StatusViewPanel(string text)
        {
            _statusText = text;
            GetViewBody();
        }
    }

}
