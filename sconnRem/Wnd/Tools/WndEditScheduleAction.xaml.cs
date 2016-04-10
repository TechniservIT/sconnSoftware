using sconnConnector;
using sconnRem.Controls;
using sconnRem.Controls.ScheduleAction;
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
using System.Windows.Shapes;


namespace sconnRem.Wnd
{
    /// <summary>
    /// Interaction logic for WndEditScheduleAction.xaml
    /// </summary>
    /// 
    
    /*
     * 
#define SCHED_ACTION_TYPE_POS 0x10
     * 
    #define SCHED_ACTION_TYPE_ACTIV_OUT 0x00
        #define SCHED_ACTION_ACTIV_OUTNO_POS 0x11
        #define SCHED_ACTION_ACTIV_OUTVAL_POS   0x12
            #define SCHED_ACTION_ACTIV_OUT_ON 0x01
            #define SCHED_ACTION_ACTIV_OUT_OFF 0x00
    #define SCHED_ACTION_TYPE_ACTIV_REL 0x01
        #define SCHED_ACTION_ACTIV_RELNO_POS 0x11
            #define SCHED_ACTION_ACTIV_REL_ON 0x01
            #define SCHED_ACTION_ACTIV_REL_OFF 0x00
    #define SCHED_ACTION_TYPE_SEND_MAIL     0x02
        #define SCHED_ACTION_MAIL_SERVER_ID_POS 0x11
        #define SCHED_ACTION_MAIL_MSG_POS   0x12
    #define SCHED_ACTION_TYPE_SEND_SMS     0x03
        #define SCHED_ACTION_SMS_RECP_ID_POS 0x11
        #define SCHED_ACTION_SMS_MSG_ID_POS 0x12
    #define SCHED_ACTION_TYPE_DISARM    0x04
    #define SCHED_ACTION_TYPE_ARM       0x05
     * 
     * #define SCHED_STAT_POS  31
    #define SCHED_STAT_ACTIVE   0x01
    #define SCHED_STAT_INACTIVE 0x0
    #define mAddr_SCHED_StartAddr 0x4000 //16384 - after Names CFG
    #define RAM_SCHED_SIZE  8192         //32 devices * 8sched/dev *  32B sched size
    #define RAM_DEV_SCHED_SIZE 256
    #define RAM_DEV_SCHED_NO    0x08     //32 schedules
    #define RAM_DEV_SCHED_MEM_SIZE 0x20  //32B
     * */


  

    public partial class WndEditScheduleAction : Window
    {

        public byte[] Schedule { get; set; }

        private int _selectedAction = 0;

        public int ScheduleId { get; set; }

        private CbxScheduleActionSelect _actionSelect;

        StkPnConfigureIoActivation _ioSelect;
        StckPanConfigureArmChangeActivation _armSelect;
        StckPanConfigureMailActivation _mailSelect;
        StckPanConfigureSmsActivation _smsSelect;

        public WndEditScheduleAction()
        {
            Schedule = new byte[ipcDefines.RAM_DEV_SCHED_MEM_SIZE];

            InitializeComponent();
            _ioSelect = new StkPnConfigureIoActivation();
            _armSelect = new StckPanConfigureArmChangeActivation();
            _mailSelect = new StckPanConfigureMailActivation();
            _smsSelect = new StckPanConfigureSmsActivation();

            _actionSelect = new CbxScheduleActionSelect(_selectedAction);
            _actionSelect.SelectionChanged += ActionSelect_SelectionChanged;

            GrdScheduleActionSelect.Children.Add(_actionSelect);

            LoadActionConfigurationView(_selectedAction);
        }


        void LoadActionConfigurationView(int actionid)
        {
            SckPanConfigureAction.Children.Clear();
            if (actionid == ipcDefines.SCHED_ACTION_TYPE_ACTIV_OUT || actionid == ipcDefines.SCHED_ACTION_TYPE_ACTIV_REL)
            {
                SckPanConfigureAction.Children.Add(_ioSelect);
            }
            else if (actionid == ipcDefines.SCHED_ACTION_TYPE_SEND_MAIL)
            {
                SckPanConfigureAction.Children.Add(_mailSelect);
            }
            else if (actionid == ipcDefines.SCHED_ACTION_TYPE_SEND_SMS)
            {
                SckPanConfigureAction.Children.Add(_smsSelect);
            }
            else if ((actionid == ipcDefines.SCHED_ACTION_TYPE_ARM) || (actionid == ipcDefines.SCHED_ACTION_TYPE_DISARM))
            {
                SckPanConfigureAction.Children.Add(_armSelect);
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        void ActionSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //load correct action config view       
            _selectedAction = _actionSelect.SelectedIndex;
            LoadActionConfigurationView(_selectedAction);
        }

        void LoadUserInputToSchedule()
        {

            Schedule[ipcDefines.SCHED_ACTION_TYPE_POS] = (byte)_selectedAction;
            if (_selectedAction == ipcDefines.SCHED_ACTION_TYPE_ACTIV_OUT || _selectedAction == ipcDefines.SCHED_ACTION_TYPE_ACTIV_REL)
            {
                Schedule[ipcDefines.SCHED_ACTION_ACTIV_OUTNO_POS] = _ioSelect.IoNo;
                Schedule[ipcDefines.SCHED_ACTION_ACTIV_OUTVAL_POS] = _ioSelect.IoActionValue;
                if (_ioSelect.IoActionValue == ipcDefines.SCHED_ACTION_ACTIV_OUT_PULSE)
                {
                    Schedule[ipcDefines.SCHED_ACTION_ACTIV_OUT_PULSE_ON_TIME_POS] = _ioSelect.PulseOnTime;
                    Schedule[ipcDefines.SCHED_ACTION_ACTIV_OUT_PULSE_OFF_TIME_POS] = _ioSelect.PulseOffTime;
                }

            }
            else if (_selectedAction == ipcDefines.SCHED_ACTION_TYPE_SEND_MAIL)
            {
            }
            else if (_selectedAction == ipcDefines.SCHED_ACTION_TYPE_SEND_SMS)
            {
            }
            else if ((_selectedAction == ipcDefines.SCHED_ACTION_TYPE_ARM) || (_selectedAction == ipcDefines.SCHED_ACTION_TYPE_DISARM))
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        public WndEditScheduleAction(byte[] sched) : this()
        {
            Schedule = sched;
        }

        public WndEditScheduleAction(byte[] sched, int actionid)
            : this(sched)
        {
            _selectedAction = actionid;
        }

        public WndEditScheduleAction(byte[] sched, int actionid, int scheduleid)
            : this(sched)
        {
            _selectedAction = actionid;
            ScheduleId = scheduleid;

        }
        private void btnSaveSchedActionEdit_Click(object sender, RoutedEventArgs e)
        {
            //invoke parent update with data
            LoadUserInputToSchedule();

            this.Close(); //on closed event is invoke to subscriped caller window
        }


    }
}
