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
using sconnConnector.POCO.Config;
using sconnRem.Controls.SiteConfig.SiteConfigGroups;

namespace sconnRem
{

    public class SiteEditView : StackPanel
    {
        private StackPanel _siteEditPanel;
        private sconnCfgMngr _configManager = new sconnCfgMngr();
        private int _siteId;


        private Dictionary<string, FrameworkElement> _editViewControls = new Dictionary<string, FrameworkElement>();



        public static readonly DependencyProperty SiteId =
       DependencyProperty.RegisterAttached("SiteId", typeof(int), typeof(Extensions), new PropertyMetadata(default(int)));

        public StackPanel SiteEditPanel { get { return _siteEditPanel; } }

        public SiteEditView(int siteId)
        {
            _siteId = siteId;
            _siteEditPanel = new StackPanel();
            sconnSite site = sconnDataShare.getSite(siteId);
            UpdateEdit(ref site);
        }

        private void SaveGlobalConfigClick(object sender, RoutedEventArgs e)
        {
            try
            {
                sconnSite toSave = sconnDataShare.getSite(_siteId);

                GbxConfigureSiteNames gnames = (GbxConfigureSiteNames)_editViewControls["GlobalNames"];
                toSave.siteCfg.GlobalNameConfig = gnames.Serialize();


                if ( _configManager.WriteGlobalNamesCfg(toSave) && _configManager.WriteGlobalCfg(toSave) )  //try uploading changed device
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
                sconnSite toSave = sconnDataShare.getSite(_siteId);

                //load data to save
                for (int i = 0; i < ipcDefines.RAM_SMS_RECP_NO; i++)
                {
                    TextBox inputname = (TextBox)_editViewControls["Recipient" + i];
                    CheckBox chkbx = (CheckBox)_editViewControls["RecipientEn" + i];
                    //TODO  
                    //toSave.siteCfg.gsmRcpts[i].NumberE164 = inputname.Text;
                    //toSave.siteCfg.gsmRcpts[i].Enabled = (bool)chkbx.IsChecked;   
                }

                if (_configManager.WriteSiteGsmCfg(toSave))  //try uploading changed device
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
            sconnSite toSave = sconnDataShare.getSite(_siteId);
            TextBox devicename = (TextBox)_editViewControls["Device" + devId.ToString()];
            string dname = devicename.Text;


            toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(0, dname);

            int inputsNo = toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrInputsNO];
            int outputsNo = toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrOutputsNO];
            int relayNo = toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrRelayNO];
            int schedNo = ipcDefines.RAM_DEV_SCHED_NO;

            GbxConfigureInputsGroup gbxInputConfig = (GbxConfigureInputsGroup)_editViewControls["Input_Cfg" + devId];
            for (int i = 0; i < inputsNo; i++)
            {
                TextBox inputname = (TextBox)_editViewControls["Input" + devId + "." + i];
                string inname = inputname.Text;
                toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(i + ipcDefines.mAddr_NAMES_Inputs_Pos, inname);
                
                toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputType] = gbxInputConfig.GetInputTypeAt(i);
                toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + ipcDefines.mAdrInputAG] = gbxInputConfig.GetInputAgAt(i);
            }

            GbxConfigureOutputsGroup gbxOutputConfig = (GbxConfigureOutputsGroup)_editViewControls["Output_Cfg" + devId];
            for (int i = 0; i < outputsNo; i++)
            {
                TextBox inputname = (TextBox)_editViewControls["Output" + devId + "." + i];
                string inname = inputname.Text;
                toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(i + ipcDefines.mAddr_NAMES_Outputs_Pos, inname);
                toSave.siteCfg.deviceConfigs[devId].memCFG[ipcDefines.mAdrOutput + i * ipcDefines.mAdrOutputMemSize + ipcDefines.mAdrOutputType] = gbxOutputConfig.GetOutputTypeAt(i);
            }

            //GbxConfigureOutputsGroup GbxOutputConfig = (GbxConfigureOutputsGroup)EditViewControls["Relay_Cfg" + devId + "." + i];
            for (int i = 0; i < relayNo; i++)
            {
                TextBox inputname = (TextBox)_editViewControls["Relay" + devId];
                string inname = inputname.Text;
                toSave.siteCfg.deviceConfigs[devId].SetDeviceNameAt(i + ipcDefines.mAddr_NAMES_Relays_Pos, inname);
            }

            //load schedules
            GbxConfigureSchedulesGroup gbxScheduleConfig = (GbxConfigureSchedulesGroup)_editViewControls["Sched_Cfg" + devId];
            for (int i = 0; i < schedNo; i++)
            {
                toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_TYPE_POS] = (byte) gbxScheduleConfig.GetScheduleTypeAt(i);
                toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_ACTION_TYPE_POS] = (byte) gbxScheduleConfig.GetScheduleActionAt(i);
                byte[] frombytes = gbxScheduleConfig.GetDateTimeConfig(DateTimeType.FromDate, (byte)i);
                for (int j = 0; j < ipcDefines.RAM_DEV_SCHED_DATETIME_SIZE; j++)
                {
                    toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_TIME_FROM_POS + j] = frombytes[j];
                }
                byte[] tobytes = gbxScheduleConfig.GetDateTimeConfig(DateTimeType.ToDate, (byte)i);
                for (int k = 0; k < ipcDefines.RAM_DEV_SCHED_DATETIME_SIZE; k++)
                {
                    toSave.siteCfg.deviceConfigs[devId].ScheduleCFG[i][ipcDefines.SCHED_TIME_TO_POS + k] = frombytes[k];
                }
            }

            //load net cfg
            DeviceNetworkConfig networkConfigPanel = (DeviceNetworkConfig)_editViewControls["Net_Cfg" + devId];
            toSave.siteCfg.deviceConfigs[devId].NetworkConfig = networkConfigPanel.GetNetworkConfig();


            //load dev auth cfg
            GbxConfigureAuthDevicesGroup dauthcfg = (GbxConfigureAuthDevicesGroup)_editViewControls["Device authorization " + devId];
            toSave.siteCfg.deviceConfigs[devId].AuthDevicesCFG = dauthcfg.Serialize();


            try
            {
                //TODO upload net cfg only on change
                bool netwrite = _configManager.WriteDeviceNetCfg( toSave, devId);
                bool devwrite = _configManager.WriteDeviceCfgSingle( toSave,devId);       
                bool namewrite =  _configManager.WriteDeviceNamesCfgSingle( toSave, devId);
                bool schedwrite =  _configManager.WriteDeviceSchedulesCfgSingle( toSave, devId);
                bool dauthwrite = _configManager.WriteDeviceDevAuthCfgSingle(toSave, devId);

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
            ChangePasswordWnd passWnd = new ChangePasswordWnd(this._siteId);
            passWnd.Show();

        }

        public void UpdateEditBody()
        {
            sconnSite site = sconnDataShare.getSite(_siteId);
            UpdateEdit(ref site); //reload view for changes
            this.Children.Clear();
            this.Children.Add(_siteEditPanel);
        }

        private void UpdateEdit(ref sconnSite site)
        {
            if (site.siteCfg != null)
            {
                int sites = site.siteCfg.deviceNo;
                this.Children.Clear();
                _siteEditPanel.Children.Clear();
                TabControl siteTabView = new TabControl();

                /************* Global site config *******************/
                if (site.siteCfg.globalConfig.memCFG != null)
                {
                    TabItem globalTabItem = new TabItem();
                    globalTabItem.Header = "Site";

                    StackPanel globalCfgPanel = new StackPanel();
                    globalCfgPanel.Width = SconnView.ViewWidth;
                    globalCfgPanel.Height = SconnView.ViewHeight;
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
                        Label globallabelDesc = new Label();
                        globallabelDesc.Content = configFieldNames[i];
                        Grid.SetRow(globallabelDesc, i);
                        Grid.SetColumn(globallabelDesc, 0);
                        siteCfgGrid.Children.Add(globallabelDesc);


                        TextBox globalTextBox = new TextBox();
                        globalTextBox.Text = site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrGlobalConfig + i].ToString();
                        Grid.SetRow(globalTextBox, i);
                        Grid.SetColumn(globalTextBox, 1);
                        siteCfgGrid.Children.Add(globalTextBox);
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

                    GroupBox siteGsmCfgGroup = new GroupBox();
                    siteGsmCfgGroup.Header = "GSM config";

                    //TODO
                    if (site.siteCfg.GsmConfig != null)
                    {
                        for (int k = 0; k < ipcDefines.RAM_SMS_RECP_NO + 1; k++)
                        {
                            RowDefinition rowDef1 = new RowDefinition();
                            gsmCfgGrid.RowDefinitions.Add(rowDef1);
                        }

                        for (int i = 0; i < ipcDefines.RAM_SMS_RECP_NO; i++)
                        {
                            Label globallabelDesc = new Label();
                            globallabelDesc.Content = "Recipient " + i.ToString();
                            Grid.SetRow(globallabelDesc, i);
                            Grid.SetColumn(globallabelDesc, 0);
                            gsmCfgGrid.Children.Add(globallabelDesc);

                            CheckBox enBox = new CheckBox();
                            enBox.Content = "Enabled";
                            Grid.SetRow(enBox, i);
                            Grid.SetColumn(enBox, 1);
                            //TODO
                            //enBox.IsChecked = site.siteCfg.gsmRcpts[i].Enabled;
                            gsmCfgGrid.Children.Add(enBox);
                            this._editViewControls.Add("RecipientEn" + i, enBox);

                            TextBox globalTextBox = new TextBox();
                            //GlobalTextBox.Text = site.siteCfg.gsmRcpts[i].NumberE164;
                            Grid.SetRow(globalTextBox, i);
                            Grid.SetColumn(globalTextBox, 2);
                            gsmCfgGrid.Children.Add(globalTextBox);
                            this._editViewControls.Add("Recipient" + i, globalTextBox);
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
                    this._editViewControls.Add("GlobalNames", namesGrp);
                    
                    //TODO
                    if (site.siteCfg.GsmConfig != null)
                    {
                        siteGsmCfgGroup.Content = gsmCfgGrid;
                        globalCfgPanel.Children.Add(siteGsmCfgGroup);
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
                    deviceCfgPanel.Width = SconnView.ViewWidth;
                    deviceCfgPanel.Height = SconnView.ViewHeight;
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
                    ColumnDefinition nameCol1 = new ColumnDefinition();
                    ColumnDefinition nameCol2 = new ColumnDefinition();
                    siteNamesCfgGrid.ColumnDefinitions.Add(nameCol1);
                    siteNamesCfgGrid.ColumnDefinitions.Add(nameCol2);


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
                        this._editViewControls.Add("Device" + i.ToString(), fieldDevlabelVal);


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
                            this._editViewControls.Add("Input" + i + "." + o, fieldDevInputlabelVal);
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
                            this._editViewControls.Add("Output" + i + "." + o, fieldDevOutputlabelVal);
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
                            this._editViewControls.Add("Relay" + i + "." + o, fieldDevRelaylabelVal);
                        }
                        deviceNamesCfgGroup.Content = siteNamesCfgGrid;


                    }
                    

                    /********** Output types ************/
                    GbxConfigureOutputsGroup gbxOutputConfig = new GbxConfigureOutputsGroup(site.siteCfg.deviceConfigs[i].memCFG, site.siteCfg.deviceConfigs[i].NamesCFG, outputsNo);
                    this._editViewControls.Add("Output_Cfg" + i, gbxOutputConfig);


                    /********** Input types ************/
                    GbxConfigureInputsGroup gbxInputConfig = new GbxConfigureInputsGroup(site.siteCfg.deviceConfigs[i].memCFG, site.siteCfg.deviceConfigs[i].NamesCFG, inputsNo);
                    this._editViewControls.Add("Input_Cfg" + i, gbxInputConfig);


                    /********** Relay types ************/

                    /********** Schedules  ************/
                    GbxConfigureSchedulesGroup gbxScheduleConfig = new GbxConfigureSchedulesGroup(site.siteCfg.deviceConfigs[i].ScheduleCFG, ipcDefines.RAM_DEV_SCHED_NO);
                    this._editViewControls.Add("Sched_Cfg" + i, gbxScheduleConfig);
                    if (site.siteCfg.deviceConfigs[i].ScheduleCFG != null)
                    {
                        gbxScheduleConfig = new GbxConfigureSchedulesGroup(site.siteCfg.deviceConfigs[i].ScheduleCFG, ipcDefines.RAM_DEV_SCHED_NO);
                        gbxScheduleConfig.ConfigChanged += GbxScheduleConfig_ConfigChanged;

                    }


                    Button saveButton = new Button();
                    saveButton.Content = "Save ";
                    saveButton.SetValue(SiteId, i);
                    saveButton.Click += new RoutedEventHandler((sender, e) => SaveDeviceConfigClick(sender, e, (int)saveButton.GetValue(SiteId)));



                    /**********  Authorized system devices  ************/
                    GbxConfigureAuthDevicesGroup authcfgConfig = new GbxConfigureAuthDevicesGroup();
                    this._editViewControls.Add("Device authorization "+ i, authcfgConfig);



                    /*********** Network config **********/
                    GroupBox deviceNetCfgGroup = new GroupBox();
                    deviceNetCfgGroup.Header = "Network config";
                    Grid siteNetCfgGrid = new Grid();
                    ColumnDefinition netCol = new ColumnDefinition();
                    siteNetCfgGrid.Background = deviceBrush;
                    siteNetCfgGrid.ColumnDefinitions.Add(netCol);
                    RowDefinition netRow = new RowDefinition();
                    siteNetCfgGrid.RowDefinitions.Add(netRow);

                    DeviceNetworkConfig networkConfigPanel = new DeviceNetworkConfig(site.siteCfg.deviceConfigs[i].NetworkConfig);
                    this._editViewControls.Add("Net_Cfg" + i, networkConfigPanel);
                    networkConfigPanel = new DeviceNetworkConfig(site.siteCfg.deviceConfigs[i].NetworkConfig);

                    Grid.SetRow(networkConfigPanel, 0);
                    Grid.SetColumn(networkConfigPanel, 0);
                    siteNetCfgGrid.Children.Add(networkConfigPanel);

                    deviceNetCfgGroup.Content = siteNetCfgGrid;


                    deviceCfgPanel.Children.Add(deviceGlobalCfgGroup);
                    deviceCfgPanel.Children.Add(deviceNamesCfgGroup);
                    deviceCfgPanel.Children.Add(gbxOutputConfig);
                    deviceCfgPanel.Children.Add(gbxInputConfig);
                    deviceCfgPanel.Children.Add(gbxScheduleConfig);
                    deviceCfgPanel.Children.Add(deviceNetCfgGroup);
                    deviceCfgPanel.Children.Add(authcfgConfig);
                    deviceCfgPanel.Children.Add(saveButton);
                    deviceTabItem.Content = deviceCfgPanel;
                    siteTabView.Items.Add(deviceTabItem);
                }

                SolidColorBrush panelBrush2 = new SolidColorBrush(Colors.DeepSkyBlue);
                _siteEditPanel.Background = panelBrush2;

                _siteEditPanel.Width = (double)SconnView.ViewWidth;
                _siteEditPanel.Height = (double)SconnView.ViewHeight;
                _siteEditPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                _siteEditPanel.VerticalAlignment = VerticalAlignment.Stretch;
                _siteEditPanel.Children.Add(siteTabView);

                this.Children.Add(_siteEditPanel);
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
