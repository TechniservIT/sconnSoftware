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
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Device;
using sconnRem.View.Config;

namespace sconnRem
{

    public class SiteView : StackPanel
    {
        private StackPanel _siteViewPanel;
        private int _siteId;
        private sconnCfgMngr _configManager = new sconnCfgMngr();

        private TextBox _gsmBx;
        private CheckBox _cbxAddLineFeed;
        private CheckBox _cbxAddCarriageReturn;
        private CheckBox _cbxAddSignSub;

        private AlarmSystemConfigManager _manager;


        public bool LiveViewEnabled { get; set; }

        public StackPanel SiteViewPanel { get { return _siteViewPanel; } }

        public SiteView(int siteId)
        {
            _siteId = siteId;
            _siteViewPanel = new StackPanel();
            sconnSite site = sconnDataShare.getSite(_siteId);
            EndpointInfo info = new EndpointInfo();
            info.Hostname = site.serverIP;
            info.Port = site.serverPort;
            DeviceCredentials cred = new DeviceCredentials();
            cred.Password = site.authPasswd;
            cred.Username = "";
            _manager = new AlarmSystemConfigManager(info,cred);
            UpdateView(ref site);
        }

        public void UpdateViewBody()
        {
            sconnSite site = sconnDataShare.getSite(_siteId);
            UpdateView(ref site);
            this.Children.Clear();
            this.Children.Add(_siteViewPanel);
        }

        private void OutputClick(object sender, RoutedEventArgs e, int outNo)
        {
            sconnSite site = sconnDataShare.getSite(_siteId);
            int outputNo = outNo;
            int outputCfgAddr = ipcDefines.mAdrOutput + (outNo * ipcDefines.mAdrOutputMemSize); //output 1 starts at 0
            int toSet = 0;
            int aktState = site.siteCfg.deviceConfigs[0].memCFG[outputCfgAddr + ipcDefines.mAdrOutputVal];
            toSet = aktState == 1 ? 0 : 1;
            site.siteCfg.deviceConfigs[0].memCFG[outputCfgAddr + ipcDefines.mAdrOutputVal] = (byte)toSet;
            _configManager.WriteDeviceCfg( site);
        }

        private void RelayClick(object sender, RoutedEventArgs e, int relays)
        {
            sconnSite site = sconnDataShare.getSite(_siteId);
            int relNo = relays;
            int outputCfgAddr = ipcDefines.mAdrRelay + (relNo * ipcDefines.RelayMemSize);
            int toSet = 0;
            int aktState = site.siteCfg.deviceConfigs[0].memCFG[outputCfgAddr + ipcDefines.mAdrRelayVal];
            toSet = aktState == 1 ? 0 : 1;
            site.siteCfg.deviceConfigs[0].memCFG[outputCfgAddr + ipcDefines.mAdrRelayVal] = (byte)toSet;
            _configManager.WriteDeviceCfg( site);
        }

        private void ArmChangeClick(object sender, RoutedEventArgs e)
        {
            sconnSite site = sconnDataShare.getSite(_siteId);
            int toSet = 0;
            int aktState = site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrArmed];
            toSet = aktState == 1 ? 0 : 1;
            site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrArmed] = (byte)toSet;
            if (toSet == 0) //remove violation on disarm
            {
                site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrViolation] = 0;
            }
            _configManager.WriteGlobalCfg( site);
        }

        public TabControl SiteTabView;
        public TabItem GlobalTabItem;
        public StackPanel GlobalCfgPanel;

        private BitmapImage GetImg()
        {
          return  new BitmapImage(new Uri("pack://application:,,,/images/worldLock.jpg", UriKind.Absolute));
        }

        private System.Windows.Controls.Image CreateImageForName(string name)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,,/images/"+name +".png", UriKind.Absolute));    // bi;

            return img;
        }
           

        private void UpdateView(ref sconnSite site)
        {
            this.Children.Clear();
            _siteViewPanel.Children.Clear();
            this.Children.Add(_siteViewPanel);

            if (site.siteCfg != null && sconnDataShare.SiteLiveViewEnabled == true)
            {

            

                int sites = site.siteCfg.deviceNo;
                this.Children.Clear();
                _siteViewPanel.Children.Clear();
                SiteTabView = new TabControl();

                double outterMargin = 25.0;
                SolidColorBrush onBrush = new SolidColorBrush(Colors.YellowGreen);
                SolidColorBrush offBrush = new SolidColorBrush(Colors.IndianRed);

                /************* Global site config *******************/
                if (site.siteCfg.globalConfig.memCFG != null)
                {
                    GlobalTabItem = new TabItem();
                    GlobalTabItem.Header = Properties.Resources.lblSiteDesc;

                    GlobalCfgPanel = new StackPanel();
                    GlobalCfgPanel.Width = SconnView.ViewWidth;
                    GlobalCfgPanel.Height = SconnView.ViewHeight;
                    GlobalCfgPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    GlobalCfgPanel.VerticalAlignment = VerticalAlignment.Stretch;

                    SolidColorBrush panelBrush = new SolidColorBrush(Colors.LightBlue);
                    SolidColorBrush deviceBrush = new SolidColorBrush(Colors.SkyBlue);

                    GlobalCfgPanel.Background = panelBrush;
                    GroupBox siteGlobalCfgGroup = new GroupBox();
                    siteGlobalCfgGroup.Header = Properties.Resources.lblGlobalConfigDesc;
                    Grid siteCfgGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    siteCfgGrid.ColumnDefinitions.Add(colDef1);
                    siteCfgGrid.ColumnDefinitions.Add(colDef2);
                    siteCfgGrid.Background = deviceBrush;

                    string[] configFieldNames = {
                                                    Properties.Resources.lblDeviceNumberDesc,
                                                   Properties.Resources.lblArmedDesc,
                                                   Properties.Resources.lblViolationDesc,
                                                    Properties.Resources.lblFunctionDesc
                                                };

                    for (int k = 0; k < configFieldNames.GetLength(0); k++)
                    {
                        RowDefinition rowDef1 = new RowDefinition();
                        siteCfgGrid.RowDefinitions.Add(rowDef1);
                    }

                    int cfgPosCounter = 0;
                    for (int i = 0; i < configFieldNames.GetLength(0); i++)
                    {
                        Label globallabelDesc = new Label();
                        globallabelDesc.Content = configFieldNames[i];
                        Grid.SetRow(globallabelDesc, i);
                        Grid.SetColumn(globallabelDesc, 0);
                        siteCfgGrid.Children.Add(globallabelDesc);

                        int cfgLen = 1;
                        if (cfgPosCounter == ipcDefines.mAdrDevID)
                        {
                            cfgLen = ipcDefines.mAdrDevID_LEN;
                        }
                        int cfgval = 0;
                        int shftinc = 0;
                        for (int b = (cfgLen - 1); b >= 0; b--)
                        {
                            cfgval |= (site.siteCfg.globalConfig.memCFG[cfgPosCounter + b]) << (8 * shftinc);
                            shftinc++;
                        }


                        Label globalTextBox = new Label();
                        globalTextBox.Content = cfgval.ToString();
                        Grid.SetRow(globalTextBox, i);
                        Grid.SetColumn(globalTextBox, 1);
                        siteCfgGrid.Children.Add(globalTextBox);

                        cfgPosCounter += cfgLen;
                    }
                    siteGlobalCfgGroup.Content = siteCfgGrid;
                    GlobalCfgPanel.Children.Add(siteGlobalCfgGroup);


                    /************ Deamon Config *************/
                    GroupBox siteDeamonGroup = new GroupBox();
                    siteDeamonGroup.Header = Properties.Resources.lblDeamonDesc;
                    StackPanel deamonPanel = new StackPanel();
                    Grid siteDeamonGrid = new Grid();
                    siteDeamonGrid.Width = deamonPanel.Width;
                    siteDeamonGrid.Margin = new Thickness(outterMargin, outterMargin, outterMargin, outterMargin);

                    int siteDeamonType = site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrDeamonType];
                    if (siteDeamonType == ipcDefines.DEA_ALARM)
                    {
                        RowDefinition armDescRow = new RowDefinition();
                        siteDeamonGrid.RowDefinitions.Add(armDescRow);
                        RowDefinition armStateRow = new RowDefinition();
                        siteDeamonGrid.RowDefinitions.Add(armStateRow);
                        RowDefinition armRow = new RowDefinition();
                        siteDeamonGrid.RowDefinitions.Add(armRow);

                        Label out1Desc = new Label();
                        out1Desc.Content = "Arm State";
                        Grid.SetRow(out1Desc, 0);
                        Grid.SetColumn(out1Desc, 0);
                        siteDeamonGrid.Children.Add(out1Desc);

                        Label out1State = new Label();
                        int outputState = site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrArmed];
                        Grid.SetRow(out1State, 1);
                        Grid.SetColumn(out1State, 0);
                        siteDeamonGrid.Children.Add(out1State);
                        if (outputState == 1)
                        {
                            out1State.Content = Properties.Resources.lblArmedDesc;
                            out1State.Background = onBrush;
                        }
                        else
                        {
                            out1State.Content = Properties.Resources.lblDisarmedDesc;
                            out1State.Background = offBrush;
                        }
                        OutputButton out1Btn = new OutputButton();
                        if (outputState == 1)
                        {
                            out1Btn.Content = Properties.Resources.lblDisarmDesc;
                        }
                        else
                        {
                            out1Btn.Content = Properties.Resources.lblArmDesc;
                        }
                        out1Btn.Click += new RoutedEventHandler((sender, e) => ArmChangeClick(sender, e));
                        Grid.SetRow(out1Btn, 2);
                        Grid.SetColumn(out1Btn, 0);
                        siteDeamonGrid.Children.Add(out1Btn);
                    }
                    else if (siteDeamonType == ipcDefines.DEA_ALARM_SCHED)
                    {

                    }
                    else if (siteDeamonType == ipcDefines.DEA_MAN)
                    {

                    }

                    else if (siteDeamonType == ipcDefines.DEA_SCHED)
                    {

                    }
                    siteDeamonGroup.Content = siteDeamonGrid;
                    GlobalCfgPanel.Children.Add(siteDeamonGroup);


                    GlobalTabItem.Content = GlobalCfgPanel;
                    SiteTabView.Items.Add(GlobalTabItem);
                }


                int totalInputs = 0;
                int totalOutputs = 0;

                /************ Device configs    *****************/
                for (int i = 0; i < sites; i++)
                {
                    TabItem deviceTabItem = new TabItem();
                    deviceTabItem.Header = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(0); // "Dev " + site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrDevID];

                    StackPanel deviceCfgPanel = new StackPanel();
                    deviceCfgPanel.Width = SconnView.ViewWidth;
                    deviceCfgPanel.Height = SconnView.ViewHeight;
                    deviceCfgPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    deviceCfgPanel.VerticalAlignment = VerticalAlignment.Stretch;

                    SolidColorBrush panelBrush = new SolidColorBrush(Colors.LightBlue);
                    SolidColorBrush deviceBrush = new SolidColorBrush(Colors.SkyBlue);
                    deviceCfgPanel.Background = panelBrush;

                    GroupBox deviceGlobalCfgGroup = new GroupBox();

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(CreateImageForName("config1"));
                    TextBlock txtBlock = new TextBlock();
                    txtBlock.Text = Properties.Resources.lblGlobalConfigDesc;
                    sp.Children.Add(txtBlock);

                    deviceGlobalCfgGroup.Header =  Properties.Resources.lblGlobalConfigDesc;    //sp; //


                    Grid siteCfgGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    siteCfgGrid.ColumnDefinitions.Add(colDef1);
                    siteCfgGrid.ColumnDefinitions.Add(colDef2);
                    siteCfgGrid.Background = deviceBrush;

                    string[] deviceConfigFields = {
                                                       Properties.Resources.lblDeviceNumberDesc+": ",
                                                       Properties.Resources.lblDomainDesc +" : ",
                                                       Properties.Resources.lblDeviceRevisionDesc + " :",
                                                       Properties.Resources.lblDeviceTypeDesc + " : ",                                                
                                                       Properties.Resources.lblInputsDesc + " : ",
                                                       Properties.Resources.lblOutputsDesc + " : ",
                                                       Properties.Resources.lblRelaysDesc + " : ",
                                                       Properties.Resources.lblKeypadDesc + " : ",
                                                       Properties.Resources.lblTemperatureDesc + " : ",
                                                       Properties.Resources.lblHumidityDesc + " : ",
                                                       Properties.Resources.lblPresureDesc + " : ",
                                                       Properties.Resources.lblcomBusDesc + " : ",
                                                       Properties.Resources.lblcomETHDesc + " : ",
                                                       Properties.Resources.lblcommiwiDesc + " : ",
                                                       Properties.Resources.lblcomBusAddrDesc + " : ",
                                                       Properties.Resources.lblArmedDesc + " : "
                                                  };

                    for (int k = 0; k < deviceConfigFields.GetLength(0); k++)
                    {
                        RowDefinition rowDef1 = new RowDefinition();
                        siteCfgGrid.RowDefinitions.Add(rowDef1);
                    }

                    int cfgPosCounter = 0;
                    for (int j = 0; j < deviceConfigFields.GetLength(0); j++)
                    {
                        Label fieldlabelDesc = new Label();
                        fieldlabelDesc.Content = deviceConfigFields[j];
                        Grid.SetRow(fieldlabelDesc, j);
                        Grid.SetColumn(fieldlabelDesc, 0);
                        siteCfgGrid.Children.Add(fieldlabelDesc);

                        int cfgLen = 1;
                        if (cfgPosCounter == ipcDefines.mAdrDevID)
                        {
                            cfgLen = ipcDefines.mAdrDevID_LEN;
                        }
                        int cfgval  = 0;
                        int shftinc = 0;
                        for (int b = (cfgLen-1); b >= 0; b--)
                        {
                            cfgval |= (site.siteCfg.deviceConfigs[i].memCFG[cfgPosCounter + b]) << (8 * shftinc);
                            shftinc++;
                        }

                        Label fieldlabelVal = new Label();
                        fieldlabelVal.Content = cfgval.ToString();
                        Grid.SetRow(fieldlabelVal, j);
                        Grid.SetColumn(fieldlabelVal, 1);
                        siteCfgGrid.Children.Add(fieldlabelVal);

                        cfgPosCounter += cfgLen;
                    }

                     deviceGlobalCfgGroup.Content = siteCfgGrid;
                     deviceCfgPanel.Children.Add(deviceGlobalCfgGroup);


                    /************** Sensor status *********/

                    if (
                        site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrDevType] == ipcDefines.IPC_DEV_TYPE_PIR_SENSOR  ||
                        site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrDevType] == ipcDefines.IPC_DEV_TYPE_RM  //TODO fix - sensor bad cfg type ? 
                        )
                    {
                        GroupBox deviceSensorStatus = new GroupBox();
                        StackPanel sensorPanel = new StackPanel();
                        //sensorPanel.Children.Add(CreateImageForName("eye"));
                        TextBlock txtGrpDesc = new TextBlock();
                        txtGrpDesc.Text = Properties.Resources.lblInputsDesc;
                        sensorPanel.Children.Add(txtGrpDesc);
                        deviceSensorStatus.Header = Properties.Resources.lblInputsDesc;


                        StackPanel sensBattStatPanel = new StackPanel();
                        Grid sensBattStatGrid = new Grid();
                        sensBattStatGrid.Width = sensBattStatPanel.Width;
                        sensBattStatGrid.Margin = new Thickness(outterMargin, outterMargin, outterMargin, outterMargin);
                        ColumnDefinition currentCol = new ColumnDefinition();
                        sensBattStatGrid.ColumnDefinitions.Add(currentCol);
                        for (int l = 0; l < 2; l++)
                        {
                            RowDefinition currentRow = new RowDefinition();
                            sensBattStatGrid.RowDefinitions.Add(currentRow);
                        }

                        Label battDesc = new Label();
                        battDesc.Content = "Battery level"; //TODO translation 
                        Grid.SetRow(battDesc, 0);
                        Grid.SetColumn(battDesc, 0);
                        sensBattStatGrid.Children.Add(battDesc);

                        ProgressBar pbbattlvl = new ProgressBar();
                        pbbattlvl.Height = 50;
                        pbbattlvl.Maximum = 100;
                        pbbattlvl.Value = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrSensorBattLvl];

                        TextBlock tbBattPercDesc = new TextBlock();
                        tbBattPercDesc.Text = Properties.Resources.lblInputsDesc;
                        pbbattlvl.Tag = pbbattlvl.Value.ToString();
                       
                        Grid.SetRow(pbbattlvl, 1);
                        Grid.SetColumn(pbbattlvl, 0);
                        sensBattStatGrid.Children.Add(pbbattlvl);

                        sensBattStatPanel.Children.Add(sensBattStatGrid);

                        //TODO battery left days estimation @ sensor

                        deviceSensorStatus.Content = sensBattStatPanel;
                        deviceCfgPanel.Children.Add(deviceSensorStatus);

                    }




                    /********   Input States ********/
                    GroupBox deviceInputGroup = new GroupBox();
                    StackPanel sp2 = new StackPanel();
                    sp2.Children.Add(CreateImageForName("eye"));
                    TextBlock txtBlock2 = new TextBlock();
                    txtBlock2.Text = Properties.Resources.lblInputsDesc;
                    sp2.Children.Add(txtBlock2);
                    deviceInputGroup.Header = Properties.Resources.lblInputsDesc;    //sp;2 

                    StackPanel inputsPanel = new StackPanel();
                    int devInputs = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrInputsNO];
                    totalInputs += devInputs;

                    Grid siteInputsGrid = new Grid();
                    siteInputsGrid.Width = inputsPanel.Width;
                    siteInputsGrid.Margin = new Thickness(outterMargin, outterMargin, outterMargin, outterMargin);
                    
                    //add output columns
                    const int outputsPerRow = 15;
                    const int relaysPerRow = 2;
                    for (int m = 0; m < outputsPerRow; m++)
                    {
                        ColumnDefinition currentCol = new ColumnDefinition();
                        siteInputsGrid.ColumnDefinitions.Add(currentCol);
                    }
                    // Define the Rows
                    int rows = (int)devInputs / outputsPerRow;
                    int addRow = devInputs % outputsPerRow != 0 ? 1 : 0;
                    rows += addRow;
                    for (int l = 0; l < rows; l++)
                    {
                        RowDefinition currentRow = new RowDefinition();
                        siteInputsGrid.RowDefinitions.Add(currentRow);
                    }

                    int colInc = 0;

                    if (site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrInputsNO] <= ipcDefines.DeviceMaxInputs)
                    {
                        
                        for (int k = 0; k < devInputs; k++)
                        {
                            Grid siteInStateGrid = new Grid();
                            ImageBrush outBackBrush = new ImageBrush();
                            outBackBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/worldLock.jpg", UriKind.Absolute));
                            outBackBrush.Stretch = Stretch.Fill;
                            siteInStateGrid.Background = outBackBrush;

                            siteInStateGrid.Width = siteInputsGrid.Width / outputsPerRow;
                            siteInStateGrid.Height = siteInStateGrid.Width; //square grid
                            double fieldMargin = 10.0;
                            siteInStateGrid.Margin = new Thickness(fieldMargin, fieldMargin, fieldMargin, fieldMargin);

                            ColumnDefinition oColDef1 = new ColumnDefinition();
                            // Define the Rows
                            RowDefinition oRowDef1 = new RowDefinition();
                            RowDefinition oRowDef2 = new RowDefinition();
                            RowDefinition oRowDef3 = new RowDefinition();

                            siteInStateGrid.ColumnDefinitions.Add(oColDef1);
                            siteInStateGrid.RowDefinitions.Add(oRowDef1);
                            siteInStateGrid.RowDefinitions.Add(oRowDef2);
                            siteInStateGrid.RowDefinitions.Add(oRowDef3);

                            Label out1Desc = new Label();
                            string name = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(ipcDefines.mAddr_NAMES_Inputs_Pos + k);
                            out1Desc.Content = name; //"Input " + k.ToString() + " : ";

                            Grid.SetRow(out1Desc, 0);
                            Grid.SetColumn(out1Desc, 0);
                            siteInStateGrid.Children.Add(out1Desc);

                            Label out1State = new Label();
                            int outputState = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrInput + (k * ipcDefines.mAdrInputMemSize) + ipcDefines.mAdrInputVal];
                            Grid.SetRow(out1State, 1);
                            Grid.SetColumn(out1State, 0);
                            siteInStateGrid.Children.Add(out1State);
                            if (outputState == 1)
                            {
                                out1State.Content = Properties.Resources.lblOnDesc;
                                out1State.Background = onBrush;
                            }
                            else
                            {
                                out1State.Content = Properties.Resources.lblOffDesc;
                                out1State.Background = offBrush;
                            }

                            //set column/row of field grid
                            Grid.SetRow(siteInStateGrid, (int)k / outputsPerRow);
                            Grid.SetColumn(siteInStateGrid, colInc);
                            colInc++;
                            if (colInc >= outputsPerRow) { colInc = 0; }

                            siteInputsGrid.Children.Add(siteInStateGrid);
                        }
                    }
         
                    inputsPanel.Children.Add(siteInputsGrid);

                   


                    /************  OUTPUTS ***********/
                    GroupBox deviceOutputGroup = new GroupBox();
                    StackPanel sp3 = new StackPanel();
                    sp3.Children.Add(CreateImageForName("eye"));
                    TextBlock txtBlock3 = new TextBlock();
                    txtBlock3.Text = Properties.Resources.lblOutputsDesc;
                    sp3.Children.Add(txtBlock3);
                    deviceOutputGroup.Header = sp3; 
                    StackPanel ouputsPanel = new StackPanel();
                    int devOutputs = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrOutputsNO];
                    totalOutputs += devOutputs;

                    Grid siteOutputsGrid = new Grid();
                    siteOutputsGrid.Width = ouputsPanel.Width;
                    outterMargin = 25.0;
                    siteOutputsGrid.Margin = new Thickness(outterMargin, outterMargin, outterMargin, outterMargin);
                    //add output columns
                    for (int m = 0; m < outputsPerRow; m++)
                    {
                        ColumnDefinition currentCol = new ColumnDefinition();
                        siteOutputsGrid.ColumnDefinitions.Add(currentCol);
                    }
                    // Define the Rows
                    rows = (int)devOutputs / outputsPerRow;
                    addRow = devOutputs % outputsPerRow != 0 ? 1 : 0;
                    rows += addRow;
                    for (int l = 0; l < rows; l++)
                    {
                        RowDefinition currentRow = new RowDefinition();
                        siteOutputsGrid.RowDefinitions.Add(currentRow);
                    }

                    colInc = 0;

                    if (site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrOutputsNO] <= ipcDefines.DeviceMaxOutputs)
                    {
                        for (int k = 0; k < devOutputs; k++)
                        {
                            Grid siteOutStateGrid = new Grid();
                            ImageBrush outBackBrush = new ImageBrush();
                            outBackBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/worldLock.jpg", UriKind.Absolute));
                            outBackBrush.Stretch = Stretch.Fill;
                            siteOutStateGrid.Background = outBackBrush;

                            siteOutStateGrid.Width = siteOutputsGrid.Width / outputsPerRow;
                            siteOutStateGrid.Height = siteOutStateGrid.Width; //square grid
                            double fieldMargin = 10.0;
                            siteOutStateGrid.Margin = new Thickness(fieldMargin, fieldMargin, fieldMargin, fieldMargin);

                            ColumnDefinition oColDef1 = new ColumnDefinition();
                            // Define the Rows
                            RowDefinition oRowDef1 = new RowDefinition();
                            RowDefinition oRowDef2 = new RowDefinition();
                            RowDefinition oRowDef3 = new RowDefinition();

                            siteOutStateGrid.ColumnDefinitions.Add(oColDef1);
                            siteOutStateGrid.RowDefinitions.Add(oRowDef1);
                            siteOutStateGrid.RowDefinitions.Add(oRowDef2);
                            siteOutStateGrid.RowDefinitions.Add(oRowDef3);

                            Label out1Desc = new Label();
                            out1Desc.Content = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(k + ipcDefines.mAddr_NAMES_Outputs_Pos); //"Output " + k.ToString() + " : ";
                            Grid.SetRow(out1Desc, 0);
                            Grid.SetColumn(out1Desc, 0);
                            siteOutStateGrid.Children.Add(out1Desc);

                            Label out1State = new Label();
                            int outputState = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrOutput + (k * ipcDefines.mAdrOutputMemSize) + ipcDefines.mAdrOutputVal];
                            Grid.SetRow(out1State, 1);
                            Grid.SetColumn(out1State, 0);
                            siteOutStateGrid.Children.Add(out1State);
                            if (outputState == 1)
                            {
                                out1State.Content = Properties.Resources.lblOnDesc;
                                out1State.Background = onBrush;
                            }
                            else
                            {
                                out1State.Content = Properties.Resources.lblOffDesc;
                                out1State.Background = offBrush;
                            }

                            OutputButton out1Btn = new OutputButton();
                            out1Btn.OutputNumber = k;
                            if (outputState == 1)
                            {
                                out1Btn.Content = Properties.Resources.lblDeactivateDesc;
                            }
                            else
                            {
                                out1Btn.Content = Properties.Resources.lblActivateDesc;
                            }
                            out1Btn.Click += new RoutedEventHandler((sender, e) => OutputClick(sender, e, out1Btn.OutputNumber));
                            Grid.SetRow(out1Btn, 2);
                            Grid.SetColumn(out1Btn, 0);
                            siteOutStateGrid.Children.Add(out1Btn);
                            //set column/row of field grid
                            Grid.SetRow(siteOutStateGrid, (int)k / outputsPerRow);
                            Grid.SetColumn(siteOutStateGrid, colInc);
                            colInc++;
                            if (colInc >= outputsPerRow) { colInc = 0; }
                            siteOutputsGrid.Children.Add(siteOutStateGrid);
                        } // outputs
                    }

                    
                    ouputsPanel.Children.Add(siteOutputsGrid);


                    /************  RELAYS ***********/
                    GroupBox deviceRelayGroup = new GroupBox();
                    deviceRelayGroup.Header = Properties.Resources.lblRelaysDesc;
                    StackPanel relaysPanel = new StackPanel();
                    int devRelays = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrRelayNO];

                    Grid siteRelaysGrid = new Grid();
                    siteRelaysGrid.Width = relaysPanel.Width;
                    outterMargin = 25.0;
                    siteRelaysGrid.Margin = new Thickness(outterMargin, outterMargin, outterMargin, outterMargin);

                    //add output columns
                    for (int m = 0; m < relaysPerRow; m++)
                    {
                        ColumnDefinition currentCol = new ColumnDefinition();
                        siteRelaysGrid.ColumnDefinitions.Add(currentCol);
                    }
                    // Define the Rows
                    rows = (int)devRelays / relaysPerRow;
                    addRow = devRelays % relaysPerRow != 0 ? 1 : 0;
                    rows += addRow;
                    for (int l = 0; l < rows; l++)
                    {
                        RowDefinition currentRow = new RowDefinition();
                        siteRelaysGrid.RowDefinitions.Add(currentRow);
                    }

                    colInc = 0;

                    if (site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrRelayNO] <= ipcDefines.DeviceMaxRelays)
                    {

                        for (int k = 0; k < devRelays; k++)
                        {
                            Grid siteRelStateGrid = new Grid();
                            ImageBrush relBackBrush = new ImageBrush();
                            relBackBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/worldLock.jpg", UriKind.Absolute));
                            relBackBrush.Stretch = Stretch.Fill;
                            siteRelStateGrid.Background = relBackBrush;

                            siteRelStateGrid.Width = siteRelaysGrid.Width / outputsPerRow;
                            siteRelStateGrid.Height = siteRelStateGrid.Width; //square grid
                            double fieldMargin = 10.0;
                            siteRelStateGrid.Margin = new Thickness(fieldMargin, fieldMargin, fieldMargin, fieldMargin);

                            ColumnDefinition oColDef1 = new ColumnDefinition();
                            // Define the Rows
                            RowDefinition oRowDef1 = new RowDefinition();
                            RowDefinition oRowDef2 = new RowDefinition();
                            RowDefinition oRowDef3 = new RowDefinition();

                            siteRelStateGrid.ColumnDefinitions.Add(oColDef1);
                            siteRelStateGrid.RowDefinitions.Add(oRowDef1);
                            siteRelStateGrid.RowDefinitions.Add(oRowDef2);
                            siteRelStateGrid.RowDefinitions.Add(oRowDef3);

                            Label rel1Desc = new Label();
                            rel1Desc.Content = site.siteCfg.deviceConfigs[i].GetDeviceNameAt(k + ipcDefines.mAddr_NAMES_Relays_Pos); // "Relay " + k.ToString() + " : ";
                            Grid.SetRow(rel1Desc, 0);
                            Grid.SetColumn(rel1Desc, 0);
                            siteRelStateGrid.Children.Add(rel1Desc);

                            Label rel1State = new Label();
                            int relayState = site.siteCfg.deviceConfigs[i].memCFG[ipcDefines.mAdrRelay + (k * ipcDefines.RelayMemSize) + ipcDefines.mAdrRelayVal];
                            Grid.SetRow(rel1State, 1);
                            Grid.SetColumn(rel1State, 0);
                            siteRelStateGrid.Children.Add(rel1State);
                            if (relayState == 1)
                            {
                                rel1State.Content = Properties.Resources.lblOnDesc;
                                rel1State.Background = onBrush;
                            }
                            else
                            {
                                rel1State.Content = Properties.Resources.lblOffDesc;
                                rel1State.Background = offBrush;
                            }

                            OutputButton rel1Btn = new OutputButton();
                            rel1Btn.OutputNumber = k;
                            if (relayState == 1)
                            {
                                rel1Btn.Content = Properties.Resources.lblDeactivateDesc;
                            }
                            else
                            {
                                rel1Btn.Content = Properties.Resources.lblActivateDesc;
                            }
                            rel1Btn.Click += new RoutedEventHandler((sender, e) => RelayClick(sender, e, rel1Btn.OutputNumber));
                            Grid.SetRow(rel1Btn, 2);
                            Grid.SetColumn(rel1Btn, 0);
                            siteRelStateGrid.Children.Add(rel1Btn);
                            //set column/row of field grid
                            Grid.SetRow(siteRelStateGrid, (int)k / relaysPerRow);
                            Grid.SetColumn(siteRelStateGrid, colInc);
                            colInc++;
                            if (colInc >= relaysPerRow) { colInc = 0; }
                            siteRelaysGrid.Children.Add(siteRelStateGrid);
                        } // relays

                    }

                    relaysPanel.Children.Add(siteRelaysGrid);



                    /********   Schedules  ********/
                    GroupBox deviceSchedulesGroup = new GroupBox();
                    deviceSchedulesGroup.Header = Properties.Resources.lblInputsDesc;
                    StackPanel schedulesPanel = new StackPanel();
                    int devSchedules = ipcDefines.RAM_DEV_SCHED_NO;
                    totalInputs += devInputs;

                    Grid siteSchedulesGrid = new Grid();
                    siteSchedulesGrid.Width = inputsPanel.Width;
                    siteSchedulesGrid.Margin = new Thickness(outterMargin, outterMargin, outterMargin, outterMargin);
                    //add output columns
                    const int schedulesPerRow = 8;
                    for (int m = 0; m < schedulesPerRow; m++)
                    {
                        ColumnDefinition currentCol = new ColumnDefinition();
                        siteSchedulesGrid.ColumnDefinitions.Add(currentCol);
                    }
                    // Define the Rows
                    rows = (int)devSchedules / schedulesPerRow;
                    addRow = devSchedules % schedulesPerRow != 0 ? 1 : 0;
                    rows += addRow;
                    for (int l = 0; l < rows; l++)
                    {
                        RowDefinition currentRow = new RowDefinition();
                        siteSchedulesGrid.RowDefinitions.Add(currentRow);
                    }

                     colInc = 0;
                     for (int k = 0; k < devSchedules; k++)
                    {
                        Grid siteSchedStateGrid = new Grid();
                        ImageBrush outBackBrush = new ImageBrush();
                        outBackBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/worldLock.jpg", UriKind.Absolute));
                        outBackBrush.Stretch = Stretch.Fill;
                        siteSchedStateGrid.Background = outBackBrush;

                        siteSchedStateGrid.Width = siteInputsGrid.Width / outputsPerRow;
                        siteSchedStateGrid.Height = siteSchedStateGrid.Width; //square grid
                        double fieldMargin = 10.0;
                        siteSchedStateGrid.Margin = new Thickness(fieldMargin, fieldMargin, fieldMargin, fieldMargin);

                        ColumnDefinition oColDef1 = new ColumnDefinition();
                        // Define the Rows
                        RowDefinition oRowDef1 = new RowDefinition();
                        RowDefinition oRowDef2 = new RowDefinition();
                        RowDefinition oRowDef3 = new RowDefinition();

                        siteSchedStateGrid.ColumnDefinitions.Add(oColDef1);
                        siteSchedStateGrid.RowDefinitions.Add(oRowDef1);
                        siteSchedStateGrid.RowDefinitions.Add(oRowDef2);
                        siteSchedStateGrid.RowDefinitions.Add(oRowDef3);

                        Label out1Desc = new Label();
                        string name = "Schedule " + k;
                        out1Desc.Content = name;

                        Grid.SetRow(out1Desc, 0);
                        Grid.SetColumn(out1Desc, 0);
                        siteSchedStateGrid.Children.Add(out1Desc);

                        Label schedulestateLbl = new Label();
                        int schedstate = site.siteCfg.deviceConfigs[i].ScheduleCFG[k][ipcDefines.SCHED_STAT_POS];
                        Grid.SetRow(schedulestateLbl, 1);
                        Grid.SetColumn(schedulestateLbl, 0);
                        siteSchedStateGrid.Children.Add(schedulestateLbl);
                        if (schedstate == 1)
                        {
                            schedulestateLbl.Content = Properties.Resources.lblOnDesc;
                            schedulestateLbl.Background = onBrush;
                        }
                        else
                        {
                            schedulestateLbl.Content = Properties.Resources.lblOffDesc;
                            schedulestateLbl.Background = offBrush;
                        }

                        //set column/row of field grid
                        Grid.SetRow(siteSchedStateGrid, (int)k / outputsPerRow);
                        Grid.SetColumn(siteSchedStateGrid, colInc);
                        colInc++;
                        if (colInc >= outputsPerRow) { colInc = 0; }

                        siteSchedulesGrid.Children.Add(siteSchedStateGrid);
                    }
                     schedulesPanel.Children.Add(siteSchedulesGrid);

                    /*GSM*/
                     _gsmBx = new TextBox();
                     _gsmBx.Width = 400;

                     Label cbxLabel1 = new Label();
                     cbxLabel1.Content = "Line Feed";
                     _cbxAddLineFeed =  new CheckBox();
                     _cbxAddLineFeed.Content = cbxLabel1;

                     Label cbxLabel2 = new Label();
                     cbxLabel2.Content = "Career Return";
                     _cbxAddCarriageReturn = new CheckBox();
                     _cbxAddCarriageReturn.Content = cbxLabel2;

                     Label cbxLabel3 = new Label();
                     cbxLabel3.Content = "Substitue (CTRL-Z)";
                     _cbxAddSignSub = new CheckBox();
                     _cbxAddSignSub.Content = cbxLabel3;
                    


                     OutputButton gsmBtn = new OutputButton();
                     gsmBtn.Content = "GSM";
                     gsmBtn.Click += gsmBTN_Click;
                        
                    deviceOutputGroup.Content = ouputsPanel;
                    deviceInputGroup.Content = inputsPanel;
                    deviceRelayGroup.Content = relaysPanel;
                   
                    deviceSchedulesGroup.Content = schedulesPanel;

                    deviceCfgPanel.Children.Add(deviceInputGroup);
                    deviceCfgPanel.Children.Add(deviceOutputGroup);
                    deviceCfgPanel.Children.Add(deviceRelayGroup);
                    deviceCfgPanel.Children.Add(deviceSchedulesGroup);

                    deviceCfgPanel.Children.Add(_gsmBx);
                    deviceCfgPanel.Children.Add(gsmBtn);
                    deviceCfgPanel.Children.Add(_cbxAddCarriageReturn);
                    deviceCfgPanel.Children.Add(_cbxAddLineFeed);
                    deviceCfgPanel.Children.Add(_cbxAddSignSub);
                    

                    deviceTabItem.Content = deviceCfgPanel;

                    SiteTabView.Items.Add(deviceTabItem);

                }// devices



                /************* Sites summary config *******************/

                TabItem summaryTabItem = new TabItem();
                summaryTabItem.Header = Properties.Resources.lblSummaryDesc;

                StackPanel summaryCfgPanel = new StackPanel();
                summaryCfgPanel.Width = SconnView.ViewWidth;
                summaryCfgPanel.Height = SconnView.ViewHeight;
                summaryCfgPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                summaryCfgPanel.VerticalAlignment = VerticalAlignment.Stretch;

                /*******  Connection Staistic ******/
                GroupBox siteSummaryStatisticCfgGroup = new GroupBox();
                siteSummaryStatisticCfgGroup.Header = Properties.Resources.lblConnectionDesc;

                Grid siteStatisticGrid = new Grid();
                ColumnDefinition column1 = new ColumnDefinition();
                ColumnDefinition column2 = new ColumnDefinition();
                siteStatisticGrid.ColumnDefinitions.Add(column1);
                siteStatisticGrid.ColumnDefinitions.Add(column2);

                string[] configSummaryFieldNames = {
                                                    Properties.Resources.lblConnectionElapsedDesc,
                                                    Properties.Resources.lblConnectErrorsDesc,
                                                    Properties.Resources.lblAvRespTimeDesc,
                                                };
                string[] configFieldValues = {
                                                   site.siteStat.ConnectionElapsed.ToString(),
                                                   site.siteStat.FailedConnectionsText,
                                                   site.siteStat.AverageResponseTimeMsText,
                                                };

                for (int v = 0; v < configSummaryFieldNames.GetLength(0); v++)
                {
                    RowDefinition rowDef1 = new RowDefinition();
                    siteStatisticGrid.RowDefinitions.Add(rowDef1);
                }

                for (int x = 0; x < configSummaryFieldNames.GetLength(0); x++)
                {
                    Label summarylabelDesc = new Label();
                    summarylabelDesc.Content = configSummaryFieldNames[x];
                    Grid.SetRow(summarylabelDesc, x);
                    Grid.SetColumn(summarylabelDesc, 0);
                    siteStatisticGrid.Children.Add(summarylabelDesc);

                    Label summaryTextBox = new Label();
                    summaryTextBox.Content = configFieldValues[x];
                    Grid.SetRow(summaryTextBox, x);
                    Grid.SetColumn(summaryTextBox, 1);
                    siteStatisticGrid.Children.Add(summaryTextBox);

                }
                siteSummaryStatisticCfgGroup.Content = siteStatisticGrid;
                summaryCfgPanel.Children.Add(siteSummaryStatisticCfgGroup);


                /*******  Site config summary ******/

                GroupBox siteSummaryConfigCfgGroup = new GroupBox();
                siteSummaryStatisticCfgGroup.Header = Properties.Resources.lblConfigDesc;

                Grid siteSummaryConfigGrid = new Grid();
                ColumnDefinition columnC1 = new ColumnDefinition();
                ColumnDefinition columnC2 = new ColumnDefinition();
                siteSummaryConfigGrid.ColumnDefinitions.Add(columnC1);
                siteSummaryConfigGrid.ColumnDefinitions.Add(columnC2);

                string[] fieldNames = {
                                                    Properties.Resources.lblDomainDesc,
                                                    Properties.Resources.lblDevicesDesc,
                                                    Properties.Resources.lblTotalInputsDesc,
                                                    Properties.Resources.lblTotalOutputsDesc
                                                };
                string[] fieldValues = {
                                                   site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrDomain].ToString(),
                                                   site.siteCfg.deviceNo.ToString(),
                                                   totalInputs.ToString(),
                                                   totalOutputs.ToString()
                                                };

                for (int v = 0; v < configSummaryFieldNames.GetLength(0); v++)
                {
                    RowDefinition rowDef1 = new RowDefinition();
                    siteSummaryConfigGrid.RowDefinitions.Add(rowDef1);
                }

                for (int x = 0; x < configSummaryFieldNames.GetLength(0); x++)
                {
                    Label summarylabelDesc = new Label();
                    summarylabelDesc.Content = fieldNames[x];
                    Grid.SetRow(summarylabelDesc, x);
                    Grid.SetColumn(summarylabelDesc, 0);
                    siteSummaryConfigGrid.Children.Add(summarylabelDesc);

                    Label summaryTextBox = new Label();
                    summaryTextBox.Content = fieldValues[x];
                    Grid.SetRow(summaryTextBox, x);
                    Grid.SetColumn(summaryTextBox, 1);
                    siteSummaryConfigGrid.Children.Add(summaryTextBox);

                }
                siteSummaryConfigCfgGroup.Content = siteSummaryConfigGrid;
                summaryCfgPanel.Children.Add(siteSummaryConfigCfgGroup);

                summaryTabItem.Content = summaryCfgPanel;
                SiteTabView.Items.Add(summaryTabItem);

                SiteTabView.SelectionChanged += siteTabView_SelectionChanged;
                SolidColorBrush panelBrush2 = new SolidColorBrush(Colors.DeepSkyBlue);
                _siteViewPanel.Background = panelBrush2;
                _siteViewPanel.Width = (double)SconnView.ViewWidth;
                _siteViewPanel.Height = (double)SconnView.ViewHeight;
                _siteViewPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                _siteViewPanel.VerticalAlignment = VerticalAlignment.Stretch;
                _siteViewPanel.Children.Add(SiteTabView);

                this.Children.Add(_siteViewPanel);
            } //config initialized

        }

        void gsmBTN_Click(object sender, RoutedEventArgs e)
        {
            sconnSite site = sconnDataShare.getSite(_siteId);
            string cmd = _gsmBx.Text;    // "AT\r";
            if (_cbxAddCarriageReturn.IsChecked == true)
            {
                cmd = cmd + '\r';
            }
            if (_cbxAddLineFeed.IsChecked == true)
            {
                cmd = cmd + '\n';
            }
            if (_cbxAddSignSub.IsChecked == true)
            {
                cmd = cmd + Convert.ToChar(26);
            }
            
            string resp = _configManager.SendGsmCommandDirect(site,cmd);
            MessageBoxResult result = MessageBox.Show(resp);
           
            
        }

        void siteTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


    }

}
