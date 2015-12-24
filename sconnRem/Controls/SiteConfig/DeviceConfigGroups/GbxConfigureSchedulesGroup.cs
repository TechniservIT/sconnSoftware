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
       

        private string[] ScheduleTypeDescriptions = new string[]
        {
            "From <Date> To <Date>",
            "On <Date>",
            "Until <Date>",
            "From <Date>",
        };

        public CbxScheduleTypeSelect(int SelectedIndex)
        {
            foreach (string desc in ScheduleTypeDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (SelectedIndex < this.Items.Count)
            {
                this.SelectedIndex = SelectedIndex;
            }
            else
            {
                this.SelectedIndex = 0; //default selected is first
                
            }
            this.SelectedItem = this.Items[this.SelectedIndex];
        }

        public void SetSelectedItem(int ItemNo)
        {
            this.SelectedItem = this.Items[ItemNo];     
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
            CbxScheduleActionSelect(int SelectedIndex)
        {
            foreach (string desc in ScheduleActionDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (SelectedIndex < this.Items.Count)
            {
                this.SelectedIndex = SelectedIndex;
            }
            else
            {
                this.SelectedIndex = 0; //default selected is first

            }
            this.SelectedItem = this.Items[this.SelectedIndex];

        }

        public void SetSelectedItem(int ItemNo)
        {
            this.SelectedIndex = ItemNo;
            this.SelectedItem = this.Items[ItemNo];
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
        private Grid GrdSchedulesConfig;
        private List<Label> LblScheduleNames;
        private List<CbxScheduleActionSelect> CbxScheduleActionSelectList;
        private List<CbxScheduleTypeSelect> CbxScheduleTypeSelectList;
        private List<DateTimePickerPanel> DtToPickerPanelList;
        private List<DateTimePickerPanel> DtFromPickerPanelList;

        private byte[][] schedules;

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

        public byte GetScheduleTypeAt(int ScheduleNo)
        {
            ComboBoxItem selitem = (ComboBoxItem)CbxScheduleTypeSelectList[ScheduleNo].SelectedItem;
            return ScheduleTypeNameValue[(string)selitem.Content];
        }

        public byte GetScheduleActionAt(int ScheduleNo)
        {
            ComboBoxItem selitem = (ComboBoxItem)CbxScheduleActionSelectList[ScheduleNo].SelectedItem;
            return ScheduleActionNameValue[(string)selitem.Content];
        }

        public byte[] GetScheduleConfig(int ScheduleNo)
        {
            ReadScheduleInput();
            return schedules[ScheduleNo];
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
                GrdSchedulesConfig.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        public GbxConfigureSchedulesGroup(byte[][] SchedulesConfig, int Schedules)
            : base()
        {
            schedules = SchedulesConfig;
            this.SchedulesNo = Schedules;
            LoadScheduleView();
    

            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
            this.Header = "Schedules Config";
            this.Content = GrdSchedulesConfig;
        }

        void GbxConfigureSchedulesGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CbxScheduleActionSelect schedCbx = (CbxScheduleActionSelect)sender;
                int scheduleid = CbxScheduleActionSelectList.IndexOf(schedCbx);
                int actionid = schedCbx.SelectedIndex;
                ReadScheduleInput(); //update selection data
                WndEditScheduleAction editwnd = new WndEditScheduleAction(schedules[scheduleid], actionid, scheduleid);
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
            GrdSchedulesConfig = new Grid();
            LblScheduleNames = new List<Label>();
            CbxScheduleTypeSelectList = new List<CbxScheduleTypeSelect>();
            CbxScheduleActionSelectList = new List<CbxScheduleActionSelect>();
            DtToPickerPanelList = new List<DateTimePickerPanel>();
            DtFromPickerPanelList = new List<DateTimePickerPanel>();

            
            InitStaticFields();
            for (int i = 0; i < SchedulesNo; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                GrdSchedulesConfig.RowDefinitions.Add(rowDef1);

                //set names
                LblScheduleNames.Add(new Label());
                LblScheduleNames[i].Content = "Schedule " + i.ToString();
                Grid.SetRow(LblScheduleNames[i], i);
                Grid.SetColumn(LblScheduleNames[i], 0);
                GrdSchedulesConfig.Children.Add(LblScheduleNames[i]);

                //set type select
                CbxScheduleTypeSelectList.Add(new CbxScheduleTypeSelect(schedules[i][ipcDefines.SCHED_TYPE_POS]));
                Grid.SetRow(CbxScheduleTypeSelectList[i], i);
                Grid.SetColumn(CbxScheduleTypeSelectList[i], 1);
                GrdSchedulesConfig.Children.Add(CbxScheduleTypeSelectList[i]);

                //set from - to dates
                DtFromPickerPanelList.Add(new DateTimePickerPanel(schedules[i], (byte)DateTimeType.FromDate));
                Grid.SetRow(DtFromPickerPanelList[i], i);
                Grid.SetColumn(DtFromPickerPanelList[i], 2);
                GrdSchedulesConfig.Children.Add(DtFromPickerPanelList[i]);

                DtToPickerPanelList.Add(new DateTimePickerPanel(schedules[i], (byte)DateTimeType.ToDate));
                Grid.SetRow(DtToPickerPanelList[i], i);
                Grid.SetColumn(DtToPickerPanelList[i], 3);
                GrdSchedulesConfig.Children.Add(DtToPickerPanelList[i]);

                //set action select
                CbxScheduleActionSelectList.Add(new CbxScheduleActionSelect(schedules[i][ipcDefines.SCHED_ACTION_TYPE_POS]));
                CbxScheduleActionSelectList[i].SelectionChanged += GbxConfigureSchedulesGroup_SelectionChanged;
                Grid.SetRow(CbxScheduleActionSelectList[i], i);
                Grid.SetColumn(CbxScheduleActionSelectList[i], 4);
                GrdSchedulesConfig.Children.Add(CbxScheduleActionSelectList[i]);

            }
        }

        void editwnd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WndEditScheduleAction editwnd = (WndEditScheduleAction)sender;
            //parse schedule and update view
            schedules[editwnd.ScheduleId] = editwnd.schedule;
            LoadScheduleView();
            ConfigChanged.Invoke(this, new EventArgs());  //invoke parent update
        }


        public void UpdateData(byte[][] SchedulesConfig, int Schedules)
        {
            this.SchedulesNo = Schedules;
            for (int i = 0; i < SchedulesNo; i++)
            {
                LblScheduleNames[i].Content = "Schedule " + i.ToString();
                CbxScheduleTypeSelectList[i].SetSelectedItem(SchedulesConfig[i][ipcDefines.SCHED_TYPE_POS]);
                CbxScheduleActionSelectList[i].SetSelectedItem(SchedulesConfig[i][ipcDefines.SCHED_ACTION_TYPE_POS]);
                //date update
            }
        }

        public byte[] GetDateTimeConfig(DateTimeType type, byte SchedId)
        {
            if ( type == DateTimeType.FromDate)
            {
                return DtFromPickerPanelList[SchedId].GetDateBinary();
            }
            else if ( type == DateTimeType.ToDate)
            {
                return DtToPickerPanelList[SchedId].GetDateBinary();
            }
            else
            { return new byte[0]; }
        }


    }


}
