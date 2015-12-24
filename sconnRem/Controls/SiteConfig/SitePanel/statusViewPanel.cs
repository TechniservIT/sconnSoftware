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

    public class statusViewPanel : StackPanel
    {
        private string _statusText;
        public string statusText { get { return _statusText; } }

        public void setStatusText(string text)
        {
            if (text != null && text.Length > 0)
            {
                _statusText = text;
                getViewBody(); //update body
            }
        }

        private void getViewBody()
        {
            this.Children.Clear();
            Label status = new Label();
            status.Content = _statusText;
            status.Height = this.Height * 0.5;
            status.Width = this.Width;
            this.Children.Add(status);
        }

        public statusViewPanel()
        {
            _statusText = "";
            getViewBody();
        }

        public statusViewPanel(string text)
        {
            _statusText = text;
            getViewBody();
        }
    }

}
