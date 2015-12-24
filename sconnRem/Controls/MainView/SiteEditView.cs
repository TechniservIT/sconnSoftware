using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using sconnRem.Controls;
using sconnRem.Controls.SiteConfig.NetworkSetup;
using sconnConnector;
using sconnRem.Controls.SiteConfig.SiteConfigGroups;

namespace sconnRem
{

    public class SiteEditView : StackPanel
    {
        private StackPanel siteEditPanel;
        private sconnCfgMngr ConfigManager = new sconnCfgMngr();
        private int _SiteId;


        private Dictionary<string, FrameworkElement> EditViewControls = new Dictionary<string, FrameworkElement>();



        public static readonly DependencyProperty SiteId =
       DependencyProperty.RegisterAttached("SiteId", typeof(int), typeof(Extensions), new PropertyMetadata(default(int)));

        public StackPanel SiteEditPanel { get { return siteEditPanel; } }

        public SiteEditView(int siteID)
        {
            _SiteId = siteID;
            siteEditPanel = new StackPanel();
            sconnSite site = sconnDataShare.getSite(siteID);
            updateEdit(ref site);
        }

        private void SaveGlobalConfigClick(object sender, RoutedEventArgs e)
        {
            try
            {
                sconnSite toSave = sconnDataShare.getSite(_SiteId);

                GbxConfigureSiteNames gnames = (GbxConfigureSiteNames)EditViewControls["GlobalNames"];
                toSave.siteCfg.GlobalNameConfig = gnames.Serialize();


                if ( ConfigManager.WriteGlobalNamesCfg(toSave) && ConfigManager.WriteGlobalCfg(toSave) )  //try uploading changed device
                {
                    sconnDataSrc filesrc = new sconnDataSrc();
                    filesrc.SaveConfig(DataSourceType.xml);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        private void SaveGsmConfigClick(object sender, RoutedEventArgs e)
        {

            try
            {
                sconnSite toSave = sconnDataShare.getSite(_SiteId);

                //load data to save
                for (int i = 0; i < ipcDefines.RAM_SMS_RECP_NO; i++)
                {
                    TextBox inputname = (TextBox)EditViewControls["Recipient" + i];
                    CheckBox chkbx = (CheckBox)EditViewControls["RecipientEn" + i];  
                    toSave.siteCfg.gsmRcpts[i].NumberE164 = inputname.Text;
                    toSave.siteCfg.gsmRcpts[i].Enabled = (bool)chkbx.IsChecked;   
                }

                if (ConfigManager.WriteSiteGsmCfg(toSave))  //try uploading changed device
                {
                    sconnDataSrc filesrc = new sconnDataSrc();
                    filesrc.SaveConfig(DataSourceType.xml);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private StackPanel GetPanelForDeviceId(int devId)
        {
            return new StackPanel();
        }

        private void SaveDeviceConfigClick(object sender, RoutedEventArgs e, int devId)
        {
            //load data to save
            sconnSite toSave = sconnDataShare.getSite(_SiteId);
            TextBox devicename = (TextBox)EditViewControls["Device" + devId.ToString()];
            string dname = devicename.Text;


            toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(0, dname);

            int inputsNo = toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrInputsNO];
            int outputsNo = toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrOutputsNO];
            int relayNo = toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrRelayNO];
            int schedNo = ipcDefines.RAM_DEV_SCHED_NO;

            GbxConfigureInputsGroup GbxInputConfig = (GbxConfigureInputsGroup)EditViewControls["Input_Cfg" + devId];
            for (int i = 0; i < inputsNo; i++)
            {
                TextBox inputname = (TextBox)EditViewControls["Input" + devId + "." + i];
                string inname = inputname.Text;
                toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(i + ipcDefines.mAddr_NAMES_Inputs_Pos, inname);
                
                toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputType] = GbxInputConfig.GetInputTypeAt(i);
                toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputAG] = GbxInputConfig.GetInputAgAt(i);
            }

            GbxConfigureOutputsGroup GbxOutputConfig = (GbxConfigureOutputsGroup)EditViewControls["Output_Cfg" + devId];
            for (int i = 0; i < outputsNo; i++)
            {
                TextBox inputname = (TextBox)EditViewControls["Output" + devId + "." + i];
                string inname = inputname.Text;
                toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(i + ipcDefines.mAddr_NAMES_Outputs_Pos, inname);
                toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrOutput + i * ipcDefines.mAdrOutputMemSize + ipcDefines.mAdrOutputType] = GbxOutputConfig.GetOutputTypeAt(i);
            }

            //GbxConfigureOutputsGroup GbxOutputConfig = (GbxConfigureOutputsGroup)EditViewControls["Relay_Cfg" + devId + "." + i];
            for (int i = 0; i < relayNo; i++)
            {
                TextBox inputname = (TextBox)EditViewControls["Relay" + devId];
                string inname = inputname.Text;
                toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(i + ipcDefines.mAddr_NAMES_Relays_Pos, inname);
            }

            //load schedules
            GbxConfigureSchedulesGroup GbxScheduleConfig = (GbxConfigureSchedulesGroup)EditViewControls["Sched_Cfg" + devId];
            for (int i = 0; i < schedNo; i++)
            {
                toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_TYPE_POS] = (byte) GbxScheduleConfig.GetScheduleTypeAt(i);
                toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_ACTION_TYPE_POS] = (byte) GbxScheduleConfig.GetScheduleActionAt(i);
                byte[] frombytes = GbxScheduleConfig.GetDateTimeConfig(DateTimeType.FromDate, (byte)i);
                for (int j = 0; j < ipcDefines.RAM_DEV_SCHED_DATETIME_SIZE; j++)
                {
                    toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_TIME_FROM_POS + j] = frombytes[j];
                }
                byte[] tobytes = GbxScheduleConfig.GetDateTimeConfig(DateTimeType.ToDate, (byte)i);
                for (int k = 0; k < ipcDefines.RAM_DEV_SCHED_DATETIME_SIZE; k++)
                {
                    toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_TIME_TO_POS + k] = frombytes[k];
                }
            }

            //load net cfg
            DeviceNetworkConfig NetworkConfigPanel = (DeviceNetworkConfig)EditViewControls["Net_Cfg" + devId];
            toSave.siteCfg.deviceConfigs[devId].NetworkConfig = NetworkConfigPanel.GetNetworkConfig();


            //load dev auth cfg
            GbxConfigureAuthDevicesGroup dauthcfg = (GbxConfigureAuthDevicesGroup)EditViewControls["Device authorization " + devId];
            toSave.siteCfg.deviceConfigs[devId].AuthDevicesCFG = dauthcfg.Serialize();


            try
            {
                //TODO upload net cfg only on change
                bool netwrite = ConfigManager.WriteDeviceNetCfg( toSave, devId);
                bool devwrite = ConfigManager.WriteDeviceCfgSingle( toSave,devId);       
                bool namewrite =  ConfigManager.WriteDeviceNamesCfgSingle( toSave, devId);
                bool schedwrite =  ConfigManager.WriteDeviceSchedulesCfgSingle( toSave, devId);
                bool dauthwrite = ConfigManager.WriteDeviceDevAuthCfgSingle(toSave, devId);

                if (devwrite || namewrite || schedwrite || schedwrite)  //try uploading changed device
                {
                    sconnDataSrc filesrc = new sconnDataSrc();
                    filesrc.SaveConfig(DataSourceType.xml);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            changePasswordWnd passWnd = new changePasswordWnd(this._SiteId);
            passWnd.Show();

        }

        public void UpdateEditBody()
        {
            sconnSite site = sconnDataShare.getSite(_SiteId);
            updateEdit(ref site); //reload view for changes
            this.Children.Clear();
            this.Children.Add(siteEditPanel);
        }

        private void updateEdit(ref sconnSite site)
        {
            if (site.siteCfg != null)
            {
                int sites = site.siteCfg.deviceNo;
                this.Children.Clear();
                siteEditPanel.Children.Clear();
                TabControl siteTabView = new TabControl();

                /************* Global site config *******************/
                if (site.siteCfg.globalConfig.memCFG != null)
                {
                    TabItem globalTabItem = new TabItem();
                    globalTabItem.Header = "Site";

                    StackPanel globalCfgPanel = new StackPanel();
                    globalCfgPanel.Width = sconnView.viewWidth;
                    globalCfgPanel.Height = sconnView.viewHeight;
                    globalCfgPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    globalCfgPanel.VerticalAlignment = VerticalAlignment.Stretch;

                    SolidColorBrush panelBrush = new SolidColorBrush(Colors.LightBlue);
                    SolidColorBrush deviceBrush = new SolidColorBrush(Colors.SkyBlue);
                    globalCfgPanel.Background = panelBrush;

                    GroupBox siteGlobalCfgGroup = new GroupBox();
                    siteGlobalCfgGroup.Header = "Global config";

                    Grid siteCfgGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    siteCfgGrid.ColumnDefinitions.Add(colDef1);
                    siteCfgGrid.ColumnDefinitions.Add(colDef2);
                    siteCfgGrid.Background = deviceBrush;

                    string[] configFieldNames = {
                                                    "Device Number",
                                                    "Config 2",
                                                    "Config 3",
                                                    "Config 4",
                                                    "Config 5",
                                                    "Config 6",
                                                    "Config 7",
                                                    "Config 8"
                                                };

                    for (int k = 0; k < configFieldNames.GetLength(0); k++)
                    {
                        RowDefinition rowDef1 = new RowDefinition();
                        siteCfgGrid.RowDefinitions.Add(rowDef1);
                    }

                    for (int i = 0; i < configFieldNames.GetLength(0); i++)
                    {
                        Label GloballabelDesc = new Label();
                        GloballabelDesc.Content = configFieldNames[i];
                        Grid.SetRow(GloballabelDesc, i);
                        Grid.SetColumn(GloballabelDesc, 0);
                        siteCfgGrid.Children.Add(GloballabelDesc);


                        TextBox GlobalTextBox = new TextBox();
                        GlobalTextBox.Text = site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrGlobalConfig + i].ToString();
                        Grid.SetRow(GlobalTextBox, i);
                        Grid.SetColumn(GlobalTextBox, 1);
                        siteCfgGrid.Children.Add(GlobalTextBox);
                    }

       

                    Button changePassButton = new Button();
                    changePassButton.Content = "Change password";
                    Grid.SetRow(changePassButton, 7);
                    Grid.SetColumn(changePassButton, 0);
                    changePassButton.Click += new RoutedEventHandler((sender, e) => ChangePasswordClick(sender, e));
                    siteCfgGrid.Children.Add(changePassButton);


                    Button saveButton = new Button();
                    saveButton.Content = "Save ";
                    Grid.SetRow(saveButton, 7);
                    Grid.SetColumn(saveButton, 1);
                    saveButton.Click += new RoutedEventHandler((sender, e) => SaveGlobalConfigClick(sender, e));
                    siteCfgGrid.Children.Add(saveButton);




                    /******* GSM CONFIG *********/
                    Grid gsmCfgGrid = new Grid();
                    ColumnDefinition gsmcolDef1 = new ColumnDefinition();
                    ColumnDefinition gsmcolDef2 = new ColumnDefinition();
                    ColumnDefinition gsmcolDef3 = new ColumnDefinition();
                    gsmCfgGrid.ColumnDefinitions.Add(gsmcolDef1);
                    gsmCfgGrid.ColumnDefinitions.Add(gsmcolDef2);
                    gsmCfgGrid.ColumnDefinitions.Add(gsmcolDef3);
                    gsmCfgGrid.Background = deviceBrush;

                    GroupBox siteGSMCfgGroup = new GroupBox();
                    siteGSMCfgGroup.Header = "GSM config";


                    if (site.siteCfg.gsmRcpts != null)
                    {
                        for (int k = 0; k < ipcDefines.RAM_SMS_RECP_NO + 1; k++)
                        {
                            RowDefinition rowDef1 = new RowDefinition();
                            gsmCfgGrid.RowDefinitions.Add(rowDef1);
                        }

                        for (int i = 0; i < ipcDefines.RAM_SMS_RECP_NO; i++)
                        {
                            Label GloballabelDesc = new Label();
                            GloballabelDesc.Content = "Recipient " + i.ToString();
                            Grid.SetRow(GloballabelDesc, i);
                            Grid.SetColumn(GloballabelDesc, 0);
                            gsmCfgGrid.Children.Add(GloballabelDesc);

                            CheckBox enBox = new CheckBox();
                            enBox.Content = "Enabled";
                            Grid.SetRow(enBox, i);
                            Grid.SetColumn(enBox, 1);
                            enBox.IsChecked = site.siteCfg.gsmRcpts[i].Enabled;
                            gsmCfgGrid.Children.Add(enBox);
                            this.EditViewControls.Add("RecipientEn" + i, enBox);

                            TextBox GlobalTextBox = new TextBox();
                            GlobalTextBox.Text = site.siteCfg.gsmRcpts[i].NumberE164;
                            Grid.SetRow(GlobalTextBox, i);
                            Grid.SetColumn(GlobalTextBox, 2);
                            gsmCfgGrid.Children.Add(GlobalTextBox);
                            this.EditViewControls.Add("Recipient" + i, GlobalTextBox);
                        }


                        Button saveGsmButton = new Button();
                        saveGsmButton.Content = "Save";
                        Grid.SetRow(saveGsmButton, ipcDefines.RAM_SMS_RECP_NO);
                        Grid.SetColumn(saveGsmButton, 0);
                        saveGsmButton.Click += new RoutedEventHandler((sender, e) => SaveGsmConfigClick(sender, e));
                        gsmCfgGrid.Children.Add(saveGsmButton);


                    }

                    siteGlobalCfgGroup.Content = siteCfgGrid;
                    globalCfgPanel.Children.Add(siteGlobalCfgGroup);


                    GbxConfigureSiteNames namesGrp = new GbxConfigureSiteNames(site.siteCfg.GlobalNameConfig);
                    globalCfgPanel.Children.Add(namesGrp);
                    this.EditViewControls.Add("GlobalNames", namesGrp);


                    //Grid.SetRow(namesGrp, 8);
                    //Grid.SetColumn(namesGrp, 0);
                    //siteCfgGrid.Children.Add(namesGrp);

                    if (site.siteCfg.gsmRcpts != null)
                    {
                        siteGSMCfgGroup.Content = gsmCfgGrid;
                        globalCfgPanel.Children.Add(siteGSMCfgGroup);
                    }

                    globalTabItem.Content = globalCfgPanel;
                    siteTabView.Items.Add(globalTabItem);

                }



                /************ Device  configs    *****************/
                for (int i = 0; i < sites; i++)
                {
                    TabItem deviceTabItem = new TabItem();
                    deviceTabItem.Header = "Dev " + site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrDevID];

                    StackPanel deviceCfgPanel = new StackPanel();
                    deviceCfgPanel.Width = sconnView.viewWidth;
                    deviceCfgPanel.Height = sconnView.viewHeight;
                    deviceCfgPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    deviceCfgPanel.VerticalAlignment = VerticalAlignment.Stretch;

                    SolidColorBrush panelBrush = new SolidColorBrush(Colors.LightBlue);
                    SolidColorBrush deviceBrush = new SolidColorBrush(Colors.SkyBlue);
                    deviceCfgPanel.Background = panelBrush;

                    GroupBox deviceGlobalCfgGroup = new GroupBox();
                    deviceGlobalCfgGroup.Header = "Device global config";

                    Grid siteCfgGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    siteCfgGrid.Background = deviceBrush;
                    siteCfgGrid.ColumnDefinitions.Add(colDef1);
                    siteCfgGrid.ColumnDefinitions.Add(colDef2);

                    string[] deviceConfigFields = {
                                                      "Device ID : ",
                                                      "Device Domain : ",
                                                      "Device Revision :",
                                                      "Device Type : ",                                                
                                                      "Inputs : ",
                                                      "Outputs : ",
                                                      "Relays : ",
                                                      "Keypad : ",
                                                      "Temperature : ",
                                                      "Humidity : ",
                                                      "Pressure : "
                                                  };

                    for (int k = 0; k < deviceConfigFields.GetLength(0); k++)
                    {
                        RowDefinition rowDef1 = new RowDefinition();
                        siteCfgGrid.RowDefinitions.Add(rowDef1);
                    }

                    for (int j = 0; j < deviceConfigFields.GetLength(0); j++)
                    {
                        Label fieldlabelDesc = new Label();
                        fieldlabelDesc.Content = deviceConfigFields[j];
                        Grid.SetRow(fieldlabelDesc, j);
                        Grid.SetColumn(fieldlabelDesc, 0);
                        siteCfgGrid.Children.Add(fieldlabelDesc);

                        Label fieldlabelVal = new Label();
                        fieldlabelVal.Content = site.siteCfg.deviceConfigs[i].memCFG[j].ToString();
                        Grid.SetRow(fieldlabelVal, j);
                        Grid.SetColumn(fieldlabelVal, 1);
                        siteCfgGrid.Children.Add(fieldlabelVal);
                    }
                    deviceGlobalCfgGroup.Content = siteCfgGrid;


                    /******** names **********/
                    GroupBox deviceNamesCfgGroup = new GroupBox();
                    deviceNamesCfgGroup.Header = "Device names config";

                    Grid siteNamesCfgGrid = new Grid();
                    siteNamesCfgGrid.Background = deviceBrush;
                    ColumnDefinition NameCol1 = new ColumnDefinition();
                    ColumnDefinition NameCol2 = new ColumnDefinition();
                    siteNamesCfgGrid.ColumnDefinitions.Add(NameCol1);
                    siteNamesCfgGrid.ColumnDefinitions.Add(NameCol2);


                    int inputsNo = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrInputsNO];
                    int outputsNo = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrOutputsNO];
                    int relayNo = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrRelayNO];


                    string[] deviceNamesConfigFields = {
                                                      "Device Name : ",
                                                      "Input names : ",
                                                      "Output names :",
                                                      "Relay names : ",                                                
                                                  };

                    if (site.siteCfg.deviceConfigs[i].NamesCFG != null)
                    {


                        int totalrows = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrInputsNO] +
                                        site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrOutputsNO] +
                                        site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrRelayNO] +
                                        deviceConfigFields.GetLength(0) +
                                        1; //save;
                        for (int k = 0; k < totalrows; k++)
                        {
                            RowDefinition rowDef1 = new RowDefinition();
                            siteNamesCfgGrid.RowDefinitions.Add(rowDef1);
                        }

                        Label fieldDevlabelDesc = new Label();
                        fieldDevlabelDesc.Content = deviceNamesConfigFields[0];
                        Grid.SetRow(fieldDevlabelDesc, 0);
                        Grid.SetColumn(fieldDevlabelDesc, 0);
                        siteNamesCfgGrid.Children.Add(fieldDevlabelDesc);

                        TextBox fieldDevlabelVal = new TextBox();
                        if (site.siteCfg.deviceConfigs[i].NamesCFG != null)
                        {
                            fieldDevlabelVal.Text = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(0);
                        }

                        fieldDevlabelVal.MaxLength = 16;
                        Grid.SetRow(fieldDevlabelVal, 0);
                        Grid.SetColumn(fieldDevlabelVal, 1);
                        siteNamesCfgGrid.Children.Add(fieldDevlabelVal);
                        this.EditViewControls.Add("Device" + i.ToString(), fieldDevlabelVal);


                        //Inputs
                        Label fieldDevInputlabelDesc = new Label();
                        fieldDevInputlabelDesc.Content = deviceNamesConfigFields[1];
                        Grid.SetRow(fieldDevInputlabelDesc, 1);
                        Grid.SetColumn(fieldDevInputlabelDesc, 0);
                        siteNamesCfgGrid.Children.Add(fieldDevInputlabelDesc);
                        for (int o = 0; o < inputsNo; o++)
                        {
                            TextBox fieldDevInputlabelVal = new TextBox();
                            if (site.siteCfg.deviceConfigs[i].NamesCFG != null)
                            {
                                fieldDevInputlabelVal.Text = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(o + 1);
                            }

                            fieldDevInputlabelVal.MaxLength = 16;
                            Grid.SetRow(fieldDevInputlabelVal, 1 + o);
                            Grid.SetColumn(fieldDevInputlabelVal, 1);
                            siteNamesCfgGrid.Children.Add(fieldDevInputlabelVal);
                            this.EditViewControls.Add("Input" + i + "." + o, fieldDevInputlabelVal);
                        }


                        //outputs
                        Label fieldDevOutputlabelDesc = new Label();
                        fieldDevOutputlabelDesc.Content = deviceNamesConfigFields[2];
                        Grid.SetRow(fieldDevOutputlabelDesc, 2 + inputsNo);
                        Grid.SetColumn(fieldDevOutputlabelDesc, 0);
                        siteNamesCfgGrid.Children.Add(fieldDevOutputlabelDesc);
                        for (int o = 0; o < outputsNo; o++)
                        {
                            TextBox fieldDevOutputlabelVal = new TextBox();
                            if (site.siteCfg.deviceConfigs[i].NamesCFG != null)
                            {
                                fieldDevOutputlabelVal.Text = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(o + ipcDefines.mAddr_NAMES_Outputs_Pos);
                            }

                            fieldDevOutputlabelVal.MaxLength = 16;
                            Grid.SetRow(fieldDevOutputlabelVal, 3 + inputsNo + o);
                            Grid.SetColumn(fieldDevOutputlabelVal, 1);
                            siteNamesCfgGrid.Children.Add(fieldDevOutputlabelVal);
                            this.EditViewControls.Add("Output" + i + "." + o, fieldDevOutputlabelVal);
                        }

                        //relays
                        Label fieldDevRelaylabelDesc = new Label();
                        fieldDevRelaylabelDesc.Content = deviceNamesConfigFields[3];
                        Grid.SetRow(fieldDevRelaylabelDesc, 3 + inputsNo + outputsNo);
                        Grid.SetColumn(fieldDevRelaylabelDesc, 0);
                        siteNamesCfgGrid.Children.Add(fieldDevRelaylabelDesc);
                        for (int o = 0; o < relayNo; o++)
                        {
                            TextBox fieldDevRelaylabelVal = new TextBox();
                            if (site.siteCfg.deviceConfigs[i].NamesCFG != null)
                            {
                                fieldDevRelaylabelVal.Text = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(o + ipcDefines.mAddr_NAMES_Relays_Pos);
                            }

                            fieldDevRelaylabelVal.MaxLength = 16;
                            Grid.SetRow(fieldDevRelaylabelVal, 4 + inputsNo + outputsNo + o);
                            Grid.SetColumn(fieldDevRelaylabelVal, 1);
                            siteNamesCfgGrid.Children.Add(fieldDevRelaylabelVal);
                            this.EditViewControls.Add("Relay" + i + "." + o, fieldDevRelaylabelVal);
                        }
                        deviceNamesCfgGroup.Content = siteNamesCfgGrid;


                    }
                    

                    /********** Output types ************/
                    GbxConfigureOutputsGroup GbxOutputConfig = new GbxConfigureOutputsGroup(site.siteCfg.deviceConfigs[i].memCFG, site.siteCfg.deviceConfigs[i].NamesCFG, outputsNo);
                    this.EditViewControls.Add("Output_Cfg" + i, GbxOutputConfig);


                    /********** Input types ************/
                    GbxConfigureInputsGroup GbxInputConfig = new GbxConfigureInputsGroup(site.siteCfg.deviceConfigs[i].memCFG, site.siteCfg.deviceConfigs[i].NamesCFG, inputsNo);
                    this.EditViewControls.Add("Input_Cfg" + i, GbxInputConfig);


                    /********** Relay types ************/

                    /********** Schedules  ************/
                    GbxConfigureSchedulesGroup GbxScheduleConfig = new GbxConfigureSchedulesGroup(site.siteCfg.deviceConfigs[i].ScheduleCFG, ipcDefines.RAM_DEV_SCHED_NO);
                    this.EditViewControls.Add("Sched_Cfg" + i, GbxScheduleConfig);
                    if (site.siteCfg.deviceConfigs[i].ScheduleCFG != null)
                    {
                        GbxScheduleConfig = new GbxConfigureSchedulesGroup(site.siteCfg.deviceConfigs[i].ScheduleCFG, ipcDefines.RAM_DEV_SCHED_NO);
                        GbxScheduleConfig.ConfigChanged += GbxScheduleConfig_ConfigChanged;

                    }


                    Button saveButton = new Button();
                    saveButton.Content = "Save ";
                    saveButton.SetValue(SiteId, i);
                    saveButton.Click += new RoutedEventHandler((sender, e) => SaveDeviceConfigClick(sender, e, (int)saveButton.GetValue(SiteId)));



                    /**********  Authorized system devices  ************/
                    GbxConfigureAuthDevicesGroup authcfgConfig = new GbxConfigureAuthDevicesGroup();
                    this.EditViewControls.Add("Device authorization "+ i, authcfgConfig);



                    /*********** Network config **********/
                    GroupBox deviceNetCfgGroup = new GroupBox();
                    deviceNetCfgGroup.Header = "Network config";
                    Grid siteNetCfgGrid = new Grid();
                    ColumnDefinition netCol = new ColumnDefinition();
                    siteNetCfgGrid.Background = deviceBrush;
                    siteNetCfgGrid.ColumnDefinitions.Add(netCol);
                    RowDefinition netRow = new RowDefinition();
                    siteNetCfgGrid.RowDefinitions.Add(netRow);

                    DeviceNetworkConfig NetworkConfigPanel = new DeviceNetworkConfig(site.siteCfg.deviceConfigs[i].NetworkConfig);
                    this.EditViewControls.Add("Net_Cfg" + i, NetworkConfigPanel);
                    NetworkConfigPanel = new DeviceNetworkConfig(site.siteCfg.deviceConfigs[i].NetworkConfig);

                    Grid.SetRow(NetworkConfigPanel, 0);
                    Grid.SetColumn(NetworkConfigPanel, 0);
                    siteNetCfgGrid.Children.Add(NetworkConfigPanel);

                    deviceNetCfgGroup.Content = siteNetCfgGrid;


                    deviceCfgPanel.Children.Add(deviceGlobalCfgGroup);
                    deviceCfgPanel.Children.Add(deviceNamesCfgGroup);
                    deviceCfgPanel.Children.Add(GbxOutputConfig);
                    deviceCfgPanel.Children.Add(GbxInputConfig);
                    deviceCfgPanel.Children.Add(GbxScheduleConfig);
                    deviceCfgPanel.Children.Add(deviceNetCfgGroup);
                    deviceCfgPanel.Children.Add(authcfgConfig);
                    deviceCfgPanel.Children.Add(saveButton);
                    deviceTabItem.Content = deviceCfgPanel;
                    siteTabView.Items.Add(deviceTabItem);
                }

                SolidColorBrush panelBrush2 = new SolidColorBrush(Colors.DeepSkyBlue);
                siteEditPanel.Background = panelBrush2;

                siteEditPanel.Width = (double)sconnView.viewWidth;
                siteEditPanel.Height = (double)sconnView.viewHeight;
                siteEditPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                siteEditPanel.VerticalAlignment = VerticalAlignment.Stretch;
                siteEditPanel.Children.Add(siteTabView);

                this.Children.Add(siteEditPanel);
            } //config is init
            else
            {

            }

        }

        void GbxScheduleConfig_ConfigChanged(object sender, EventArgs e)
        {
            //reload view after change


            //throw new NotImplementedException();
        }

    }

}
