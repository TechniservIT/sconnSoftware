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
        
        private Grid _grdGlobalNameConfig;
        private List<Label> _lblNameDesc;
        private List<TextBox> _tbxGlobalName;
        private List<string> _names;
        

        private void InitStaticFields()
        {
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            _grdGlobalNameConfig.ColumnDefinitions.Add(colDef1);
            _grdGlobalNameConfig.ColumnDefinitions.Add(colDef2);
        }


        public GbxConfigureSiteNames() : base()
        {
            _grdGlobalNameConfig = new Grid();
            _lblNameDesc = new List<Label>();
            _tbxGlobalName = new List<TextBox>();
            _names = new List<string>();

            InitStaticFields();

            AddDescLabels();

            this.Header = "Names";

            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                //create required rows 
                RowDefinition rowDef1 = new RowDefinition();
                _grdGlobalNameConfig.RowDefinitions.Add(rowDef1);

                //set text 
                _tbxGlobalName.Add(new TextBox());
                Grid.SetRow(_tbxGlobalName[i], i);
                Grid.SetColumn(_tbxGlobalName[i], 1);
                _grdGlobalNameConfig.Children.Add(_tbxGlobalName[i]);


            }


            this.Content = _grdGlobalNameConfig;
        }

        private void AddDescLabels()
        {

            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                //set names
                _lblNameDesc.Add(new Label());
                //TODO zone/sys name desc
                _lblNameDesc[i].Content = "Name " + i.ToString();
              
                Grid.SetRow(_lblNameDesc[i], i);
                Grid.SetColumn(_lblNameDesc[i], 0);
                _grdGlobalNameConfig.Children.Add(_lblNameDesc[i]);
            }
        }

        public byte[] Serialize()
        {
            byte[] mem = new byte[ipcDefines.RAM_NAMES_Global_Total_Size];
            for (int j = 0; j < _tbxGlobalName.Count; j++)
			{
                string txt = _tbxGlobalName[j].Text;
                byte[] namebuff = System.Text.Encoding.BigEndianUnicode.GetBytes(txt);
                for (int i = 0; i < namebuff.Length; i++)
                {
                    mem[j * ipcDefines.RAM_NAME_SIZE + i] = (byte)namebuff[i];
                }
			}
               
            return mem;
        }

        public GbxConfigureSiteNames(byte[] globalNamesConfig)
            : this()
        {
            //TODO load names
            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                byte[] name = new byte[ipcDefines.RAM_NAME_SIZE];
                for (int j = 0; j < ipcDefines.RAM_NAME_SIZE; j++)
                {
                    name[j] = globalNamesConfig[i * ipcDefines.RAM_NAME_SIZE + j];
                }
                //string txt = System.Text.Encoding.BigEndianUnicode  .GetString(name, 0, ipcDefines.RAM_NAME_SIZE);
                string txt = System.Text.Encoding.BigEndianUnicode.GetString(name, 0, CfgOper.GetUnicodeArrayStringLen(name)); 
                _names.Add(txt);
            }
            UpdateData();
        }

        public void UpdateData()
        {
            for (int i = 0; i < ipcDefines.RAM_NAMES_Global_Total_Records; i++)
            {
                _tbxGlobalName[i].Text = _names[i];
            }
        }


    }
}
