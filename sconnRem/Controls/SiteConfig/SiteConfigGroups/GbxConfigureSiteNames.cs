using sconnConnector;
using sconnConnector.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sconnRem.Controls.SiteConfig.SiteConfigGroups
{
    public class GbxConfigureSiteNames : GroupBox
    {
        
        private Grid GrdGlobalNameConfig;
        private List<Label> LblNameDesc;
        private List<TextBox> TbxGlobalName;
        private List<string> Names;
        

        private void InitStaticFields()
        {
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            GrdGlobalNameConfig.ColumnDefinitions.Add(colDef1);
            GrdGlobalNameConfig.ColumnDefinitions.Add(colDef2);
        }


        public GbxConfigureSiteNames() : base()
        {
            GrdGlobalNameConfig = new Grid();
            LblNameDesc = new List<Label>();
            TbxGlobalName = new List<TextBox>();
            Names = new List<string>();

            InitStaticFields();

            AddDescLabels();

            this.Header = "Names";

            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                GrdGlobalNameConfig.RowDefinitions.Add(rowDef1);

                //set text 
                TbxGlobalName.Add(new TextBox());
                Grid.SetRow(TbxGlobalName[i], i);
                Grid.SetColumn(TbxGlobalName[i], 1);
                GrdGlobalNameConfig.Children.Add(TbxGlobalName[i]);


            }


            this.Content = GrdGlobalNameConfig;
        }

        private void AddDescLabels()
        {

            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                //set names
                LblNameDesc.Add(new Label());
                //TODO zone/sys name desc
                LblNameDesc[i].Content = "Name " + i.ToString();
              
                Grid.SetRow(LblNameDesc[i], i);
                Grid.SetColumn(LblNameDesc[i], 0);
                GrdGlobalNameConfig.Children.Add(LblNameDesc[i]);
            }
        }

        public byte[] Serialize()
        {
            byte[] mem = new byte[ipcDefines.RAM_NAMES_Global_Total_Size];
            for (int j = 0; j < TbxGlobalName.Count; j++)
			{
                string txt = TbxGlobalName[j].Text;
                byte[] namebuff = System.Text.Encoding.BigEndianUnicode.GetBytes(txt);
                for (int i = 0; i < namebuff.Length; i++)
                {
                    mem[j * ipcDefines.RAM_NAME_SIZE + i] = (byte)namebuff[i];
                }
			}
               
            return mem;
        }

        public GbxConfigureSiteNames(byte[] GlobalNamesConfig)
            : this()
        {
            //TODO load names
            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                byte[] name = new byte[ipcDefines.RAM_NAME_SIZE];
                for (int j = 0; j < ipcDefines.RAM_NAME_SIZE; j++)
                {
                    name[j] = GlobalNamesConfig[i * ipcDefines.RAM_NAME_SIZE + j];
                }
                //string txt = System.Text.Encoding.BigEndianUnicode  .GetString(name, 0, ipcDefines.RAM_NAME_SIZE);
                string txt = System.Text.Encoding.BigEndianUnicode.GetString(name, 0, CfgOper.GetUnicodeArrayStringLen(name)); 
                Names.Add(txt);
            }
            UpdateData();
        }

        public void UpdateData()
        {
            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                TbxGlobalName[i].Text = Names[i];
            }
        }


    }
}
