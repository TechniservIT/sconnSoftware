using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sconnRem.Controls
{
    /// <summary>
    /// Interaction logic for StkPnConfigureIoActivation.xaml
    /// </summary>
    public partial class StkPnConfigureIoActivation : UserControl
    {

        public enum IoActivationState
        {
            On = 0, Off, Pulse
        }

        public byte IoActionValue { get; set; }

        public byte IoNo { get; set; }
        public byte PulseOnTime { get; set; }
        public byte PulseOffTime { get; set; }

        static public int SchedActionTimeMsMultiplier = 50;

        public StkPnConfigureIoActivation()
        {    
            InitializeComponent();
            LbxSelectActionIoValue.SelectionChanged += LbxSelectActionIoValue_SelectionChanged;
            grdActionPulseSetup.Visibility = Visibility.Hidden;
            for (int i = 1; i < 254; i++)
            {
                LboxScheduleActionPulseTimeOn.Items.Add(new ListBoxItem().Content = (i * SchedActionTimeMsMultiplier).ToString() );
                LboxScheduleActionPulseTimeOff.Items.Add(new ListBoxItem().Content = (i * SchedActionTimeMsMultiplier).ToString());
            }
        }

        void LbxSelectActionIoValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)((ListBox)sender).SelectedItem;
            if (item.Content.ToString().Equals("Pulse"))
            {
                grdActionPulseSetup.Visibility = Visibility.Visible;
            }
        }

        public void ReadUserInput()
        {
            ListBoxItem item = (ListBoxItem)LbxSelectActionIoValue.SelectedItem;
            if  (item.Content.ToString().Equals("Pulse") )
            {
                PulseOffTime = (byte) (Int32.Parse(((ListBoxItem)LboxScheduleActionPulseTimeOff.SelectedItem).Content.ToString()) / SchedActionTimeMsMultiplier);
                PulseOffTime = (byte) (Int32.Parse(((ListBoxItem)LboxScheduleActionPulseTimeOff.SelectedItem).Content.ToString()) / SchedActionTimeMsMultiplier);
            }
            else
            {
                IoActionValue = (byte) LbxSelectActionIoValue.SelectedIndex;    //value matching memory addresses
            }

        }


    }
}
