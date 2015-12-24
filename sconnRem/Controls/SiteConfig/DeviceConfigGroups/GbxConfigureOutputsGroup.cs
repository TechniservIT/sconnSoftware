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
        private string[] OutputTypeDescriptions = new string[]
        {
            "Alarm NA",
            "Alarm NIA",
            "Power",
            "Schedule 1 NA",
            "Schedule 1 NIA",
            "Schedule 2 NA",
            "Schedule 2 NIA",
        };

        public CbxOutputTypeSelect(int SelectedIndex)
        {
            try
            {
                foreach (string desc in OutputTypeDescriptions)
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
            catch (Exception e)
            {
                
             
            }



        }

        public void SetSelectedItem(int ItemNo)
        {
            this.SelectedItem = this.Items[ItemNo];
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
        private Grid GrdOutputsConfig;
        private List<Label> LblOutputNames;
        private List<CbxOutputTypeSelect> CbxOutputTypeSelect;

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

        public byte GetOutputTypeAt(int OutputNo)
        {
            ComboBoxItem selitem = (ComboBoxItem)CbxOutputTypeSelect[OutputNo].SelectedItem;
            return OutputTypeNameValue[(string)selitem.Content];
        }


        private void InitStaticFields()
        {
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            GrdOutputsConfig.ColumnDefinitions.Add(colDef1);
            GrdOutputsConfig.ColumnDefinitions.Add(colDef2);
        }



        public GbxConfigureOutputsGroup(byte[] OutputsConfig, int Outputs)
            : base()
        {
            GrdOutputsConfig = new Grid();
            LblOutputNames = new List<Label>();
            CbxOutputTypeSelect = new List<CbxOutputTypeSelect>();

            this.OutputsNo = Outputs;
            InitStaticFields();
            for (int i = 0; i < OutputsNo; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                GrdOutputsConfig.RowDefinitions.Add(rowDef1);

                //set type select
                CbxOutputTypeSelect.Add(new CbxOutputTypeSelect(OutputsConfig[ipcDefines.mAdrOutput + (i * ipcDefines.mAdrOutputMemSize) + ipcDefines.mAdrOutputType]));
                Grid.SetRow(CbxOutputTypeSelect[i], i);
                Grid.SetColumn(CbxOutputTypeSelect[i], 1);
                GrdOutputsConfig.Children.Add(CbxOutputTypeSelect[i]);
            }
        }

        public GbxConfigureOutputsGroup(byte[] OutputsConfig, byte[][] OutputsNamesConfig, int Outputs)  : this(OutputsConfig, Outputs)
        {
            this.OutputsNo = Outputs;
            for (int i = 0; i < OutputsNo; i++)
            {
                //set names
                LblOutputNames.Add(new Label());
                LblOutputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(OutputsNamesConfig[i], 0, OutputsNamesConfig[i].GetLength(0)); 
                Grid.SetRow(LblOutputNames[i], i);
                Grid.SetColumn(LblOutputNames[i], 0);
                GrdOutputsConfig.Children.Add(LblOutputNames[i]);
            }

            this.Visibility = Visibility.Visible;
            bool visisble = this.IsVisible;
            this.Header = "Output Config";
            
            this.Content = GrdOutputsConfig;

        }

        public void UpdateData(byte[] OutputsConfig, byte[][] OutputsNamesConfig, int Outputs)
        {
            this.OutputsNo = Outputs;
            for (int i = 0; i < OutputsNo; i++)
            {

                LblOutputNames[i].Content = System.Text.Encoding.BigEndianUnicode.GetString(OutputsNamesConfig[i + ipcDefines.mAddr_NAMES_Outputs_Pos], 0, OutputsNamesConfig[i + ipcDefines.mAddr_NAMES_Outputs_Pos].GetLength(0));
                CbxOutputTypeSelect[i].SetSelectedItem(OutputsConfig[ipcDefines.mAdrOutput + i * ipcDefines.mAdrOutputMemSize + ipcDefines.mAdrOutputType]);
            }
        }


    }
}

