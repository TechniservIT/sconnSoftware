using sconnConnector;
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

namespace sconnRem
{
    
    public class CbxInputTypeSelect : ComboBox
    {

        public Dictionary<byte, string> InputTypeValueForName = new Dictionary<byte, string>()      
        {
                {0x00, "NO"},
                {0x01, "NC"},
                {128, "Delayed NO"},
                {129, "Delayed NC"}
        };

        private string[] InputTypeDescriptions = new string[]
        {
            "NO",
            "NC",
            "Delayed NO",
            "Delayed NC"
        };

        public CbxInputTypeSelect(int SelectedIndex)
        {
            foreach (string desc in InputTypeDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (SelectedIndex < this.Items.Count)
            {
                this.SelectedIndex = SelectedIndex;
                this.SelectedItem = this.Items[SelectedIndex];
            }

        }

        public CbxInputTypeSelect(byte configVal)
        {
            string selName;
            bool valok = InputTypeValueForName.TryGetValue(configVal,  out selName);
            int SelectedIndex = 0;
            for (int i = 0; i < InputTypeDescriptions.Count(); i++)
			{
			 ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = InputTypeDescriptions[i];
                this.Items.Add(cbxItem);
                if (valok)
                {
                    if (InputTypeDescriptions[i].Equals(selName))
                    {
                        SelectedIndex = i;
                    }
                }
			}

            this.SelectedIndex = SelectedIndex;
            this.SelectedItem = this.Items[SelectedIndex];
        }

        public void SetSelectedItem(int ItemNo)
        {
            this.SelectedIndex = ItemNo;
            this.SelectedItem = this.Items[ItemNo];
        }

    }

    public class CbxInputAgSelect : ComboBox
    {


         private string[] InputTypeDescriptions = new string[]
        {
            "Arm",
            "Disarm",
            "Activate Output 1",
            "Activate Output 2",
            "Armed Violation",
            "Disarmed Violation",
            "Armed or Disarmed Violation",
        };

        public CbxInputAgSelect(int SelectedIndex)
        {
            foreach (string desc in InputTypeDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (SelectedIndex < this.Items.Count)
            {
                this.SelectedItem = this.Items[SelectedIndex];
            }

        }

        public void SetSelectedItem(int ItemNo)
        {
            this.SelectedIndex = ItemNo;
            this.SelectedItem = this.Items[ItemNo];
        }
    }



    public class CbxZoneSelect : ComboBox
    {


        private string[] ZoneDescriptions = new string[]    //TODO - load from name cfg
        {
            "Zone 1",
            "Zone 2",
            "Zone 3",
            "Zone 4",
            "Zone 5",
            "Zone 6",
            "Zone 7",
            "Zone 8",
            "Zone 9",
            "Zone 10",
            "Zone 11",
            "Zone 12",
            "Zone 13",
            "Zone 14",
            "Zone 15",
            "Zone 16"
        };

        public CbxZoneSelect(int SelectedIndex)
        {
            foreach (string desc in ZoneDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (SelectedIndex < this.Items.Count)
            {
                this.SelectedItem = this.Items[SelectedIndex];
            }

        }

        public void SetSelectedItem(int ItemNo)
        {
            this.SelectedIndex = ItemNo;
            this.SelectedItem = this.Items[ItemNo];
        }
    }




    public class GbxConfigureInputsGroup : GroupBox
    {
           public Dictionary<string, byte> InputAgNameValue = new Dictionary<string, byte>()      
        {
                {"Arm", 0x00},
                {"Disarm", 0x01},
                {"Activate Output 1", 0x02},
                {"Activate Output 2", 0x03},
                {"Armed Violation", 0x04},
                {"Disarmed Violation", 0x05},
                {"Armed or Disarmed Violation", 0x06}
        };

           public Dictionary<string, byte> InputTypeNameValue = new Dictionary<string, byte>()      
        {
                {"NO", 0x00},
                {"NC", 0x01},
                {"Delayed NO", 128},
                {"Delayed NC", 129}
        };


       public Dictionary<string, byte> InputZoneId = new Dictionary<string, byte>()      
        {
                {"Zone 1", 0},
                {"Zone 2", 1},
                {"Zone 3", 2},
                {"Zone 4", 3},
                {"Zone 5", 4},
                {"Zone 6", 5},
                {"Zone 7", 6},
                {"Zone 8", 7},
                {"Zone 9", 8},
                {"Zone 10", 9},
                {"Zone 11", 10},
                {"Zone 12", 11},
                {"Zone 13", 12},
                {"Zone 14", 13},
                {"Zone 15", 14},
                {"Zone 16", 15},
        };

        public int InputsNo { get; set; }
        private Grid GrdInputsConfig;
        private List<Label> LblInputNames;
        private List<CbxInputTypeSelect> CbxInputTypeSelect;
        private List<CbxInputAgSelect> CbxInputAgSelect;
        private List<CbxZoneSelect>     CbxZoneSelect;
        

        private void InitStaticFields()
        {
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            GrdInputsConfig.ColumnDefinitions.Add(colDef1);
            GrdInputsConfig.ColumnDefinitions.Add(colDef2);
            GrdInputsConfig.ColumnDefinitions.Add(colDef3);
            GrdInputsConfig.ColumnDefinitions.Add(colDef4);
        }


        public byte GetInputAgAt(int InputNo)
        {
            int selindex = CbxInputAgSelect[InputNo].SelectedIndex;
             if (selindex >= 0){
                 ComboBoxItem selitem = (ComboBoxItem)CbxInputAgSelect[InputNo].Items[selindex]; 
                return InputAgNameValue[(string)selitem.Content];            
             }

            return 0;
        }

        public byte GetInputTypeAt(int InputNo)
        {
             int selindex = CbxInputTypeSelect[InputNo].SelectedIndex;
             if (selindex >= 0)
             {
                 ComboBoxItem selitem = (ComboBoxItem)CbxInputTypeSelect[InputNo].Items[selindex];
                 return InputTypeNameValue[(string)selitem.Content];
             }
             return 0;
        }

        public GbxConfigureInputsGroup(byte[] InputsConfig, int Inputs) : base()
        {
            GrdInputsConfig = new Grid();
            LblInputNames = new List<Label>();
            CbxInputTypeSelect = new List<CbxInputTypeSelect>();
            CbxInputAgSelect = new List<CbxInputAgSelect>();
            CbxZoneSelect = new List<CbxZoneSelect>();

            this.InputsNo = Inputs;
            InitStaticFields();
            for (int i = 0; i < InputsNo; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                GrdInputsConfig.RowDefinitions.Add(rowDef1);

                //set type select
                CbxInputTypeSelect.Add(new CbxInputTypeSelect(InputsConfig[ipcDefines.mAdrInput + (i * ipcDefines.mAdrInputMemSize) + ipcDefines.mAdrInputType]));
                Grid.SetRow(CbxInputTypeSelect[i], i);
                Grid.SetColumn(CbxInputTypeSelect[i], 1);
                GrdInputsConfig.Children.Add(CbxInputTypeSelect[i]);

                //set AG select
                CbxInputAgSelect.Add(new CbxInputAgSelect(InputsConfig[ipcDefines.mAdrInput + (i * ipcDefines.mAdrInputMemSize) + ipcDefines.mAdrInputAG]));
                //CbxInputAgSelect[i].SelectedIndex = 0;
                Grid.SetRow(CbxInputAgSelect[i], i);
                Grid.SetColumn(CbxInputAgSelect[i], 2);
                GrdInputsConfig.Children.Add(CbxInputAgSelect[i]);

                //set type select
                CbxZoneSelect.Add(new CbxZoneSelect(InputsConfig[ipcDefines.mAdrInput + (i * ipcDefines.mAdrInputMemSize) + ipcDefines.mAdrInputZoneId]));
                Grid.SetRow(CbxZoneSelect[i], i);
                Grid.SetColumn(CbxZoneSelect[i], 3);
                GrdInputsConfig.Children.Add(CbxZoneSelect[i]);

            }
        }

        public GbxConfigureInputsGroup(byte[] InputsConfig, byte[][] InputsNamesConfig, int Inputs)
            : this(InputsConfig, Inputs)
        {
            this.InputsNo = Inputs;
            for (int i = 0; i < InputsNo; i++)
            {
                //set names
                LblInputNames.Add(new Label());
                LblInputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(InputsNamesConfig[i], 0, InputsNamesConfig[i].GetLength(0)); 
                Grid.SetRow(LblInputNames[i], i);
                Grid.SetColumn(LblInputNames[i], 0);
                GrdInputsConfig.Children.Add(LblInputNames[i]);
            }

            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
            this.Header = "Input Config";

            this.Content = GrdInputsConfig;

        }

        public void UpdateData(byte[] InputsConfig, byte[][] InputsNamesConfig, int Inputs)
        {
            this.InputsNo = Inputs;
            for (int i = 0; i < InputsNo; i++)
            {

                LblInputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(InputsNamesConfig[i + ipcDefines.mAddr_NAMES_Inputs_Pos], 0, InputsNamesConfig[i + ipcDefines.mAddr_NAMES_Inputs_Pos].GetLength(0));
                CbxInputTypeSelect[i].SetSelectedItem(InputsConfig[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputType]);
                CbxInputAgSelect[i].SetSelectedItem(InputsConfig[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputAG]);
            
            }
        }


    }

}
