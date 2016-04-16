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

    public class CbxOutputTypeSelect : ComboBox
    {
        private string[] _outputTypeDescriptions = new string[]
        {
            "Alarm NA",
            "Alarm NIA",
            "Power",
            "Schedule 1 NA",
            "Schedule 1 NIA",
            "Schedule 2 NA",
            "Schedule 2 NIA",
        };

        public CbxOutputTypeSelect(int selectedIndex)
        {
            try
            {
                foreach (string desc in _outputTypeDescriptions)
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
            catch (Exception e)
            {
                
             
            }



        }

        public void SetSelectedItem(int itemNo)
        {
            this.SelectedItem = this.Items[itemNo];
        }

    }

    public class GbxCustomGroup : GroupBox
    {

        public GbxCustomGroup( string name) :base()
        {
            this.Header = name;
        }
    }

    public class GbxConfigureOutputsGroup : GroupBox
    {
        public int OutputsNo { get; set; }
        private Grid _grdOutputsConfig;
        private List<Label> _lblOutputNames;
        private List<CbxOutputTypeSelect> _cbxOutputTypeSelect;

        public Dictionary<string, byte> OutputTypeNameValue = new Dictionary<string, byte>()      
        {     
                {"Alarm NA", 0x00},
                {"Alarm NIA", 0x01},
                {"Power", 0x02},
                {"Schedule 1 NA", 0x03},
                {"Schedule 1 NIA", 0x04},
                {"Schedule 2 NA", 0x05},
                {"Schedule 2 NIA", 0x06}
        };


        private delegate byte[] CopyNameBytes();

        public byte GetOutputTypeAt(int outputNo)
        {
            ComboBoxItem selitem = (ComboBoxItem)_cbxOutputTypeSelect[outputNo].SelectedItem;
            return OutputTypeNameValue[(string)selitem.Content];
        }


        private void InitStaticFields()
        {
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            _grdOutputsConfig.ColumnDefinitions.Add(colDef1);
            _grdOutputsConfig.ColumnDefinitions.Add(colDef2);
        }



        public GbxConfigureOutputsGroup(byte[] outputsConfig, int outputs)
            : base()
        {
            _grdOutputsConfig = new Grid();
            _lblOutputNames = new List<Label>();
            _cbxOutputTypeSelect = new List<CbxOutputTypeSelect>();

            this.OutputsNo = outputs;
            InitStaticFields();
            for (int i = 0; i < OutputsNo; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                _grdOutputsConfig.RowDefinitions.Add(rowDef1);

                //set type select
                _cbxOutputTypeSelect.Add(new CbxOutputTypeSelect(outputsConfig[ipcDefines.mAdrOutput + (i * ipcDefines.mAdrOutputMemSize) + ipcDefines.mAdrOutputType]));
                Grid.SetRow(_cbxOutputTypeSelect[i], i);
                Grid.SetColumn(_cbxOutputTypeSelect[i], 1);
                _grdOutputsConfig.Children.Add(_cbxOutputTypeSelect[i]);
            }
        }

        public GbxConfigureOutputsGroup(byte[] outputsConfig, byte[][] outputsNamesConfig, int outputs)  : this(outputsConfig, outputs)
        {
            this.OutputsNo = outputs;
            for (int i = 0; i < OutputsNo; i++)
            {
                //set names
                _lblOutputNames.Add(new Label());
                _lblOutputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(outputsNamesConfig[i], 0, outputsNamesConfig[i].GetLength(0)); 
                Grid.SetRow(_lblOutputNames[i], i);
                Grid.SetColumn(_lblOutputNames[i], 0);
                _grdOutputsConfig.Children.Add(_lblOutputNames[i]);
            }

            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
            this.Header = "Output Config";
            
            this.Content = _grdOutputsConfig;

        }

        public void UpdateData(byte[] outputsConfig, byte[][] outputsNamesConfig, int outputs)
        {
            this.OutputsNo = outputs;
            for (int i = 0; i < OutputsNo; i++)
            {

                _lblOutputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(outputsNamesConfig[i + ipcDefines.mAddr_NAMES_Outputs_Pos], 0, outputsNamesConfig[i + ipcDefines.mAddr_NAMES_Outputs_Pos].GetLength(0));
                _cbxOutputTypeSelect[i].SetSelectedItem(outputsConfig[ipcDefines.mAdrOutput + i * ipcDefines.mAdrOutputMemSize + ipcDefines.mAdrOutputType]);
            }
        }


    }
}

