using sconnConnector;
using sconnRem.Wnd;
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
    public class CbxScheduleTypeSelect : ComboBox
    {
       

        private string[] _scheduleTypeDescriptions = new string[]
        {
            "From <Date> To <Date>",
            "On <Date>",
            "Until <Date>",
            "From <Date>",
        };

        public CbxScheduleTypeSelect(int selectedIndex)
        {
            foreach (string desc in _scheduleTypeDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (selectedIndex < this.Items.Count)
            {
                this.SelectedIndex = selectedIndex;
            }
            else
            {
                this.SelectedIndex = 0; //default selected is first
                
            }
            this.SelectedItem = this.Items[this.SelectedIndex];
        }

        public void SetSelectedItem(int itemNo)
        {
            this.SelectedItem = this.Items[itemNo];     
        }

    }

    public class CbxScheduleActionSelect : ComboBox
    {

        //Must much with in-memory defines @ board
        static public string[] ScheduleActionDescriptions = new string[]
        {
            "Activate Output",
            "Activate Relay",
            "Send Mail",
            "Send SMS",
            "Disarm System",
            "Arm System"
        };

        public 
            CbxScheduleActionSelect(int selectedIndex)
        {
            foreach (string desc in ScheduleActionDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (selectedIndex < this.Items.Count)
            {
                this.SelectedIndex = selectedIndex;
            }
            else
            {
                this.SelectedIndex = 0; //default selected is first

            }
            this.SelectedItem = this.Items[this.SelectedIndex];

        }

        public void SetSelectedItem(int itemNo)
        {
            this.SelectedIndex = itemNo;
            this.SelectedItem = this.Items[itemNo];
        }
    }


    public class GbxCustomGroup : GroupBox
    {

        public GbxCustomGroup(string name)
            : base()
        {
            this.Header = name;
        }
    }

  


    public class GbxConfigureSchedulesGroup : GroupBox
    {
        public event EventHandler ConfigChanged;

        public int SchedulesNo { get; set; }
        private Grid _grdSchedulesConfig;
        private List<Label> _lblScheduleNames;
        private List<CbxScheduleActionSelect> _cbxScheduleActionSelectList;
        private List<CbxScheduleTypeSelect> _cbxScheduleTypeSelectList;
        private List<DateTimePickerPanel> _dtToPickerPanelList;
        private List<DateTimePickerPanel> _dtFromPickerPanelList;

        private byte[][] _schedules;

        public Dictionary<string, byte> ScheduleTypeNameValue = new Dictionary<string, byte>()      
        {     
                {"From <Date> To <Date>", 0x00},
                {"On <Date>", 0x01},
                {"Until <Date>", 0x02},
                {"From <Date>", 0x03},
        };

        public Dictionary<string, byte> ScheduleActionNameValue = new Dictionary<string, byte>()      
        {     
                {"Activate Output", 0x00},
                { "Activate Relay", 0x01},
                {"Send Mail", 0x02},
                {"Send SMS", 0x03},
                {"Disarm System", 0x04},
                {"Arm System", 0x05},
        };

        private delegate byte[] CopyNameBytes();

        public byte GetScheduleTypeAt(int scheduleNo)
        {
            ComboBoxItem selitem = (ComboBoxItem)_cbxScheduleTypeSelectList[scheduleNo].SelectedItem;
            return ScheduleTypeNameValue[(string)selitem.Content];
        }

        public byte GetScheduleActionAt(int scheduleNo)
        {
            ComboBoxItem selitem = (ComboBoxItem)_cbxScheduleActionSelectList[scheduleNo].SelectedItem;
            return ScheduleActionNameValue[(string)selitem.Content];
        }

        public byte[] GetScheduleConfig(int scheduleNo)
        {
            ReadScheduleInput();
            return _schedules[scheduleNo];
        }


        private void ReadScheduleInput()
        {
            //load from controls to arrays
        }


        private void InitStaticFields()
        {
            int columns = 5;
            for (int i = 0; i < columns; i++)
            {
                _grdSchedulesConfig.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        public GbxConfigureSchedulesGroup(byte[][] schedulesConfig, int Schedules)
            : base()
        {
            _schedules = schedulesConfig;
            this.SchedulesNo = Schedules;
            LoadScheduleView();
    

            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
            this.Header = "Schedules Config";
            this.Content = _grdSchedulesConfig;
        }

        void GbxConfigureSchedulesGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CbxScheduleActionSelect schedCbx = (CbxScheduleActionSelect)sender;
                int scheduleid = _cbxScheduleActionSelectList.IndexOf(schedCbx);
                int actionid = schedCbx.SelectedIndex;
                ReadScheduleInput(); //update selection data
                WndEditScheduleAction editwnd = new WndEditScheduleAction(_schedules[scheduleid], actionid, scheduleid);
                editwnd.Closing += editwnd_Closing;
                editwnd.Show();
                //throw new NotImplementedException();
            }
            catch (Exception exc)
            {
                //throw;
            }
        }

        private void LoadScheduleView()
        {
            _grdSchedulesConfig = new Grid();
            _lblScheduleNames = new List<Label>();
            _cbxScheduleTypeSelectList = new List<CbxScheduleTypeSelect>();
            _cbxScheduleActionSelectList = new List<CbxScheduleActionSelect>();
            _dtToPickerPanelList = new List<DateTimePickerPanel>();
            _dtFromPickerPanelList = new List<DateTimePickerPanel>();

            
            InitStaticFields();
            for (int i = 0; i < SchedulesNo; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                _grdSchedulesConfig.RowDefinitions.Add(rowDef1);

                //set names
                _lblScheduleNames.Add(new Label());
                _lblScheduleNames[i].Content = "Schedule " + i.ToString();
                Grid.SetRow(_lblScheduleNames[i], i);
                Grid.SetColumn(_lblScheduleNames[i], 0);
                _grdSchedulesConfig.Children.Add(_lblScheduleNames[i]);

                //set type select
                _cbxScheduleTypeSelectList.Add(new CbxScheduleTypeSelect(_schedules[i][ipcDefines.SCHED_TYPE_POS]));
                Grid.SetRow(_cbxScheduleTypeSelectList[i], i);
                Grid.SetColumn(_cbxScheduleTypeSelectList[i], 1);
                _grdSchedulesConfig.Children.Add(_cbxScheduleTypeSelectList[i]);

                //set from - to dates
                _dtFromPickerPanelList.Add(new DateTimePickerPanel(_schedules[i], (byte)DateTimeType.FromDate));
                Grid.SetRow(_dtFromPickerPanelList[i], i);
                Grid.SetColumn(_dtFromPickerPanelList[i], 2);
                _grdSchedulesConfig.Children.Add(_dtFromPickerPanelList[i]);

                _dtToPickerPanelList.Add(new DateTimePickerPanel(_schedules[i], (byte)DateTimeType.ToDate));
                Grid.SetRow(_dtToPickerPanelList[i], i);
                Grid.SetColumn(_dtToPickerPanelList[i], 3);
                _grdSchedulesConfig.Children.Add(_dtToPickerPanelList[i]);

                //set action select
                _cbxScheduleActionSelectList.Add(new CbxScheduleActionSelect(_schedules[i][ipcDefines.SCHED_ACTION_TYPE_POS]));
                _cbxScheduleActionSelectList[i].SelectionChanged += GbxConfigureSchedulesGroup_SelectionChanged;
                Grid.SetRow(_cbxScheduleActionSelectList[i], i);
                Grid.SetColumn(_cbxScheduleActionSelectList[i], 4);
                _grdSchedulesConfig.Children.Add(_cbxScheduleActionSelectList[i]);

            }
        }

        void editwnd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WndEditScheduleAction editwnd = (WndEditScheduleAction)sender;
            //parse schedule and update view
            _schedules[editwnd.ScheduleId] = editwnd.Schedule;
            LoadScheduleView();
            ConfigChanged.Invoke(this, new EventArgs());  //invoke parent update
        }


        public void UpdateData(byte[][] schedulesConfig, int Schedules)
        {
            this.SchedulesNo = Schedules;
            for (int i = 0; i < SchedulesNo; i++)
            {
                _lblScheduleNames[i].Content = "Schedule " + i.ToString();
                _cbxScheduleTypeSelectList[i].SetSelectedItem(schedulesConfig[i][ipcDefines.SCHED_TYPE_POS]);
                _cbxScheduleActionSelectList[i].SetSelectedItem(schedulesConfig[i][ipcDefines.SCHED_ACTION_TYPE_POS]);
                //date update
            }
        }

        public byte[] GetDateTimeConfig(DateTimeType type, byte schedId)
        {
            if ( type == DateTimeType.FromDate)
            {
                return _dtFromPickerPanelList[schedId].GetDateBinary();
            }
            else if ( type == DateTimeType.ToDate)
            {
                return _dtToPickerPanelList[schedId].GetDateBinary();
            }
            else
            { return new byte[0]; }
        }


    }


}
