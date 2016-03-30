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



        public int ScheduleId { get; set; }



        public WndEditScheduleAction()
        {

            InitializeComponent();



        }


        void LoadActionConfigurationView(int actionid)
        {
            SckPanConfigureAction.Children.Clear();
            if (actionid == ipcDefines.SCHED_ACTION_TYPE_ACTIV_OUT || actionid == ipcDefines.SCHED_ACTION_TYPE_ACTIV_REL)
            {
            }
            else if (actionid == ipcDefines.SCHED_ACTION_TYPE_SEND_MAIL)
            {
            }
            else if (actionid == ipcDefines.SCHED_ACTION_TYPE_SEND_SMS)
            {
            }
            else if ((actionid == ipcDefines.SCHED_ACTION_TYPE_ARM) || (actionid == ipcDefines.SCHED_ACTION_TYPE_DISARM))
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        void ActionSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //load correct action config view       
        }

        void LoadUserInputToSchedule()
        {

            {
                {
                }

            }
            {
            }
            {
            }
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        public WndEditScheduleAction(byte[] sched) : this()
        {
        }

        public WndEditScheduleAction(byte[] sched, int actionid)
            : this(sched)
        {
        }

        public WndEditScheduleAction(byte[] sched, int actionid, int scheduleid)
            : this(sched)
        {
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
