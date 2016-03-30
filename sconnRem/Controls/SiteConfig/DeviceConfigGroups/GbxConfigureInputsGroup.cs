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

        private string[] _inputTypeDescriptions = new string[]
        {
            "NO",
            "NC",
            "Delayed NO",
            "Delayed NC"
        };

        public CbxInputTypeSelect(int selectedIndex)
        {
            foreach (string desc in _inputTypeDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (selectedIndex < this.Items.Count)
            {
                this.SelectedIndex = selectedIndex;
                this.SelectedItem = this.Items[selectedIndex];
            }

        }

        public CbxInputTypeSelect(byte configVal)
        {
            string selName;
            bool valok = InputTypeValueForName.TryGetValue(configVal,  out selName);
            int selectedIndex = 0;
            for (int i = 0; i < _inputTypeDescriptions.Count(); i++)
			{
			 ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = _inputTypeDescriptions[i];
                this.Items.Add(cbxItem);
                if (valok)
                {
                    if (_inputTypeDescriptions[i].Equals(selName))
                    {
                        selectedIndex = i;
                    }
                }
			}

            this.SelectedIndex = selectedIndex;
            this.SelectedItem = this.Items[selectedIndex];
        }

        public void SetSelectedItem(int itemNo)
        {
            this.SelectedIndex = itemNo;
            this.SelectedItem = this.Items[itemNo];
        }

    }

    public class CbxInputAgSelect : ComboBox
    {


         private string[] _inputTypeDescriptions = new string[]
        {
            "Arm",
            "Disarm",
            "Activate Output 1",
            "Activate Output 2",
            "Armed Violation",
            "Disarmed Violation",
            "Armed or Disarmed Violation",
        };

        public CbxInputAgSelect(int selectedIndex)
        {
            foreach (string desc in _inputTypeDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (selectedIndex < this.Items.Count)
            {
                this.SelectedItem = this.Items[selectedIndex];
            }

        }

        public void SetSelectedItem(int itemNo)
        {
            this.SelectedIndex = itemNo;
            this.SelectedItem = this.Items[itemNo];
        }
    }



    public class CbxZoneSelect : ComboBox
    {


        private string[] _zoneDescriptions = new string[]    //TODO - load from name cfg
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

        public CbxZoneSelect(int selectedIndex)
        {
            foreach (string desc in _zoneDescriptions)
            {
                ComboBoxItem cbxItem = new ComboBoxItem();
                cbxItem.Content = desc;
                this.Items.Add(cbxItem);
            }
            if (selectedIndex < this.Items.Count)
            {
                this.SelectedItem = this.Items[selectedIndex];
            }

        }

        public void SetSelectedItem(int itemNo)
        {
            this.SelectedIndex = itemNo;
            this.SelectedItem = this.Items[itemNo];
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
        private Grid _grdInputsConfig;
        private List<Label> _lblInputNames;
        private List<CbxInputTypeSelect> _cbxInputTypeSelect;
        private List<CbxInputAgSelect> _cbxInputAgSelect;
        private List<CbxZoneSelect>     _cbxZoneSelect;
        

        private void InitStaticFields()
        {
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            _grdInputsConfig.ColumnDefinitions.Add(colDef1);
            _grdInputsConfig.ColumnDefinitions.Add(colDef2);
            _grdInputsConfig.ColumnDefinitions.Add(colDef3);
            _grdInputsConfig.ColumnDefinitions.Add(colDef4);
        }


        public byte GetInputAgAt(int inputNo)
        {
            int selindex = _cbxInputAgSelect[inputNo].SelectedIndex;
             if (selindex >= 0){
                 ComboBoxItem selitem = (ComboBoxItem)_cbxInputAgSelect[inputNo].Items[selindex]; 
                return InputAgNameValue[(string)selitem.Content];            
             }

            return 0;
        }

        public byte GetInputTypeAt(int inputNo)
        {
             int selindex = _cbxInputTypeSelect[inputNo].SelectedIndex;
             if (selindex >= 0)
             {
                 ComboBoxItem selitem = (ComboBoxItem)_cbxInputTypeSelect[inputNo].Items[selindex];
                 return InputTypeNameValue[(string)selitem.Content];
             }
             return 0;
        }

        public GbxConfigureInputsGroup(byte[] inputsConfig, int inputs) : base()
        {
            _grdInputsConfig = new Grid();
            _lblInputNames = new List<Label>();
            _cbxInputTypeSelect = new List<CbxInputTypeSelect>();
            _cbxInputAgSelect = new List<CbxInputAgSelect>();
            _cbxZoneSelect = new List<CbxZoneSelect>();

            this.InputsNo = inputs;
            InitStaticFields();
            for (int i = 0; i < InputsNo; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                _grdInputsConfig.RowDefinitions.Add(rowDef1);

                //set type select
                _cbxInputTypeSelect.Add(new CbxInputTypeSelect(inputsConfig[ipcDefines.mAdrInput + (i * ipcDefines.mAdrInputMemSize) + ipcDefines.mAdrInputType]));
                Grid.SetRow(_cbxInputTypeSelect[i], i);
                Grid.SetColumn(_cbxInputTypeSelect[i], 1);
                _grdInputsConfig.Children.Add(_cbxInputTypeSelect[i]);

                //set AG select
                _cbxInputAgSelect.Add(new CbxInputAgSelect(inputsConfig[ipcDefines.mAdrInput + (i * ipcDefines.mAdrInputMemSize) + ipcDefines.mAdrInputAG]));
                //CbxInputAgSelect[i].SelectedIndex = 0;
                Grid.SetRow(_cbxInputAgSelect[i], i);
                Grid.SetColumn(_cbxInputAgSelect[i], 2);
                _grdInputsConfig.Children.Add(_cbxInputAgSelect[i]);

                //set type select
                _cbxZoneSelect.Add(new CbxZoneSelect(inputsConfig[ipcDefines.mAdrInput + (i * ipcDefines.mAdrInputMemSize) + ipcDefines.mAdrInputZoneId]));
                Grid.SetRow(_cbxZoneSelect[i], i);
                Grid.SetColumn(_cbxZoneSelect[i], 3);
                _grdInputsConfig.Children.Add(_cbxZoneSelect[i]);

            }
        }

        public GbxConfigureInputsGroup(byte[] inputsConfig, byte[][] inputsNamesConfig, int inputs)
            : this(inputsConfig, inputs)
        {
            this.InputsNo = inputs;
            for (int i = 0; i < InputsNo; i++)
            {
                //set names
                _lblInputNames.Add(new Label());
                _lblInputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(inputsNamesConfig[i], 0, inputsNamesConfig[i].GetLength(0)); 
                Grid.SetRow(_lblInputNames[i], i);
                Grid.SetColumn(_lblInputNames[i], 0);
                _grdInputsConfig.Children.Add(_lblInputNames[i]);
            }

            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
            this.Header = "Input Config";

            this.Content = _grdInputsConfig;

        }

        public void UpdateData(byte[] inputsConfig, byte[][] inputsNamesConfig, int inputs)
        {
            this.InputsNo = inputs;
            for (int i = 0; i < InputsNo; i++)
            {

                _lblInputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(inputsNamesConfig[i + ipcDefines.mAddr_NAMES_Inputs_Pos], 0, inputsNamesConfig[i + ipcDefines.mAddr_NAMES_Inputs_Pos].GetLength(0));
                _cbxInputTypeSelect[i].SetSelectedItem(inputsConfig[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputType]);
                _cbxInputAgSelect[i].SetSelectedItem(inputsConfig[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputAG]);
            
            }
        }


    }

}
