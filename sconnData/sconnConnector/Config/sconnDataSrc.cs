using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using sconnConnector.POCO.Config;
#if WIN32_ENC

using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

using System.IO;
#endif


namespace sconnConnector
{
    public enum DataSourceType { sql, xml, server, registry };
    public enum XmlType { XDoc, XmlDoc };

    public class sconnDataSrc
    {

        private string _FilePathXML;
        private ipcSql SqlSource;
        private ipcCfgFile XmlSource;


        public sconnDataSrc()
        {
            XmlSource = new ipcCfgFile();
            SqlSource = new ipcSql();
        }


        private void CreateRegistryData()
        {
            #if WIN32_ENC

                        RegistryKey appKey = Registry.CurrentUser.CreateSubKey("sconnRem");
                        GlobalSettings defsettings = new GlobalSettings();
                        if ( appKey != null )
                        {
                            appKey.SetValue("ConfigFilePath", defsettings.ConfigFilePath);
                            appKey.SetValue("CultureName", defsettings.CultureName);
                            appKey.SetValue("DefaultReloadInterval", defsettings.DefaultReloadInterval);
                        }
            #endif



        }

        public GlobalSettings LoadRegistryData()
        {
            #if WIN32_ENC
            GlobalSettings settings = new GlobalSettings();
            RegistryKey appKey = Registry.CurrentUser.OpenSubKey("sconnRem");
            if ( appKey == null)
            {
                CreateRegistryData();
            }
            settings.ConfigFilePath = (string)appKey.GetValue("ConfigFilePath");
            settings.CultureName = (string)appKey.GetValue("CultureName");
            settings.DefaultReloadInterval = (int)appKey.GetValue("DefaultReloadInterval");
            return settings;
            #else
            return null;
            #endif
        }

        public void SaveSettingsToRegistry(GlobalSettings settings)
        {
            #if WIN32_ENC
            RegistryKey appKey = Registry.CurrentUser.CreateSubKey("sconnRem");
            if (appKey == null)
            {
                CreateRegistryData();
            }
            appKey.SetValue("ConfigFilePath", settings.ConfigFilePath);
            appKey.SetValue("CultureName", settings.CultureName);
            appKey.SetValue("DefaultReloadInterval", settings.DefaultReloadInterval );
            #endif

        }

        private class ipcSql
        {
            public ipcSql()
            {

            }
        }

        public bool SaveSites(DataSourceType stype, List<sconnSite> sites )
        {
            switch (stype)
            {
                case DataSourceType.sql:
                    break;
                case DataSourceType.xml:
                    return XmlSource.saveConfig();
                case DataSourceType.server:
                    break;
                case DataSourceType.registry:
                    break;
                default:
                    break;
            }
            return false;
        }


        public bool SaveConfig(DataSourceType stype)
        {
            switch (stype)
            {
                case DataSourceType.sql:
                    break;
                case DataSourceType.xml:
                   return  XmlSource.saveConfig();
                case DataSourceType.server:
                    break;
                case DataSourceType.registry:
                    break;
                default:
                    break;
            }
            return false;
        }

        public XmlDocument GetConfigFileXml()
        {
                    return XmlSource.GetConfigXmlDocument();

        }
        
        public XDocument GetConfigFileXmlLinq()
        {
                    return XmlSource.GetConfigXDocument();
        }

        

        public List<sconnSite> GetSites(DataSourceType stype)
        {
            switch (stype)
            {
                case DataSourceType.sql:
                    break;
                case DataSourceType.xml:
                    return XmlSource.GetSites();
                case DataSourceType.server:
                    break;
                case DataSourceType.registry:
                    break;
                default:
                    break;
            }
            return null;
        }

        public bool LoadConfig(DataSourceType stype)
        {
            switch (stype)
            {
                case DataSourceType.sql:
                    break;
                case DataSourceType.xml:
                    return XmlSource.loadConfig();
                case DataSourceType.server:
                    break;
                case DataSourceType.registry:
                    break;
                default:
                    break;
            }
            return false;
        }

        public bool SetXmlPath(string path)
        {
            _FilePathXML = path;
            XmlSource.FileFullPath = path + "/" + XmlSource.FileName;
            return true;
        }

        public bool SetXmlFileName(string name)
        {
            XmlSource.FileName = name;
            return true;
        }

        public void SetSqlConnection()
        {

        }

        /********   Encrypted File data source XML    ********/
        private class ipcCfgFile
        {
             private string configFileName = "sconnRem.xml";
             private string configFieldName = "sconnConfig";
             private string configFilePath = "sconnRem.xml";

            public string FileName { get { return configFileName; } set { configFileName = value; } }
            public string FileFullPath { get { return configFilePath; } set { configFilePath = value; } }
            public string ConfigFieldName { get { return configFieldName; } set { configFieldName = value; } }


            /**********  Max key is 256 bits **************/
            static private byte[] cryptoKey = { 0x01, 0x02, 0x03, 0x09, 0x01, 0x14, 0x41, 0x12, 0x2F, 0x1C,  0x01, 0x02, 0x03, 0x09, 0x01, 0x14, 0x41, 0x12, 0x2F, 0x1C,  0x01, 0x02, 0x03, 0x09, 0x01, 0x14, 0x41, 0x12, 0x2F, 0x1C, 0x2F, 0x1C,};
            static private byte[] cryptoVector = { 0x02, 0x06, 0x03, 0x09, 0x01, 0x14, 0x41, 0x12, 0x2F, 0x1C, 0x01, 0x02, 0x03, 0x09, 0x03, 0x09 };


            #if WIN32_ENC

            private void encryptXmlConfig(XmlDocument doc)
            {
                RijndaelManaged crypto = new RijndaelManaged();
                crypto.Key = cryptoKey;
                crypto.IV = cryptoVector;
                EncryptXmlElement(doc, configFieldName, crypto); //encrypt entire file
            }

            private void encryptXmlConfig(XDocument doc)
            {
                RijndaelManaged crypto = new RijndaelManaged();
                crypto.Key = cryptoKey;
                crypto.IV = cryptoVector;
                EncryptXmlElement(doc, configFieldName, crypto); //encrypt entire file
            }

            private void decryptXmlConfig(XmlDocument doc)
            {
                RijndaelManaged crypto = new RijndaelManaged();
                crypto.Key = cryptoKey;
                crypto.IV = cryptoVector;
                DecryptXmlElement(doc, crypto); 
            }

            #endif


            private string ToBase64(byte[] data)
            {
                return Convert.ToBase64String(data);
            }



            private XmlNode siteToXML(sconnSite site, ref XmlDocument doc)
            {

                XmlNode siteNode = doc.CreateNode("element", "site"+site.siteID.ToString(), "");

                XmlNode statusCheckIntervalNode = doc.CreateNode("element", "statusCheckInterval", "");
                statusCheckIntervalNode.InnerText = site.statusCheckInterval.ToString();
                siteNode.AppendChild(statusCheckIntervalNode);

                XmlNode siteId = doc.CreateNode("element", "Id", "");
                siteId.InnerText = site.Id;
                siteNode.AppendChild(siteId);

                XmlNode siteName = doc.CreateNode("element", "siteName", "");
                siteName.InnerText = site.siteName;
                siteNode.AppendChild(siteName);

                XmlNode authPasswd = doc.CreateNode("element", "authPasswd", "");
                authPasswd.InnerText = site.authPasswd;
                siteNode.AppendChild(authPasswd);

                XmlNode serverIP = doc.CreateNode("element", "serverIP", "");
                serverIP.InnerText = site.serverIP;
                siteNode.AppendChild(serverIP);

                XmlNode serverPort = doc.CreateNode("element", "serverPort", "");
                serverPort.InnerText = site.serverPort.ToString();
                siteNode.AppendChild(serverPort);

                XmlNode ViewEnable = doc.CreateNode("element", "ViewEnable", "");
                ViewEnable.InnerText = site.ViewEnable.ToString();
                siteNode.AppendChild(ViewEnable);

                XmlNode siteID = doc.CreateNode("element", "siteID", "");
                siteID.InnerText = site.siteID.ToString();
                siteNode.AppendChild(siteID);

                XmlNode deviceNoNode = doc.CreateNode("element", "deviceNo", "");
                deviceNoNode.InnerText = site.siteCfg.deviceNo.ToString();
                siteNode.AppendChild(deviceNoNode);

                XmlNode UsbComNode = doc.CreateNode("element", "UsbCom", "");
                UsbComNode.InnerText = site.UsbCom.ToString();
                siteNode.AppendChild(UsbComNode);

                XmlNode deviceConfigs = doc.CreateNode("element", "deviceConfigs", "");
                string deviceData = "";
                for (int i = 0; i < site.siteCfg.deviceNo; i++)
                {
                    deviceData += Convert.ToBase64String(site.siteCfg.deviceConfigs[i].memCFG);
                }
                deviceConfigs.InnerText = deviceData;
                siteNode.AppendChild(deviceConfigs);

                XmlNode globalConfig = doc.CreateNode("element", "globalConfig", "");
                globalConfig.InnerText = Convert.ToBase64String(site.siteCfg.globalConfig.memCFG);
                siteNode.AppendChild(globalConfig);

                return siteNode;
            }

            private XElement siteToXMLlinq(sconnSite site)
            {
                XElement siteNode = new XElement("site" + site.siteID.ToString());

                XElement statusCheckIntervalNode = new XElement("statusCheckInterval");
                statusCheckIntervalNode.Value = site.statusCheckInterval.ToString();
                siteNode.Add(statusCheckIntervalNode);

                XElement siteName = new XElement("siteName");
                siteName.Value = site.siteName;
                siteNode.Add(siteName);

                XElement authPasswd = new XElement("authPasswd");
                authPasswd.Value = site.authPasswd;
                siteNode.Add(authPasswd);

                XElement serverIP = new XElement("serverIP");
                serverIP.Value = site.serverIP;
                siteNode.Add(serverIP);

                XElement serverPort = new XElement("serverPort");
                serverPort.Value = site.serverPort.ToString();
                siteNode.Add(serverPort);

                XElement ViewEnable = new XElement("ViewEnable");
                ViewEnable.Value = site.ViewEnable.ToString();
                siteNode.Add(ViewEnable);

                XElement siteID = new XElement("siteID");
                siteID.Value = site.siteID.ToString();
                siteNode.Add(siteID);

                XElement deviceNoNode = new XElement("element", "deviceNo", "");
                deviceNoNode.Value = site.siteCfg.deviceNo.ToString();
                siteNode.Add(deviceNoNode);

                XElement deviceConfigs = new XElement("element", "deviceConfigs", "");
                string deviceData = "";
                for (int i = 0; i < site.siteCfg.deviceNo; i++)
                {
                    deviceData += Convert.ToBase64String(site.siteCfg.deviceConfigs[i].memCFG);
                }
                deviceConfigs.Value = deviceData;
                siteNode.Add(deviceConfigs);

                XElement globalConfig = new XElement("element", "globalConfig", "");
                globalConfig.Value = Convert.ToBase64String(site.siteCfg.globalConfig.memCFG);
                siteNode.Add(globalConfig);

                return siteNode;
            }

            private sconnSite siteFromXML(XmlNode siteNode)
            {
                sconnSite site = new sconnSite();
                XmlNodeList nodes = siteNode.ChildNodes;
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes.Item(i);
                    if (node.Name == "statusCheckInterval")
                    {
                        site.statusCheckInterval = int.Parse(node.InnerText);
                    }
                    else if ( node.Name == "siteName")
                    {
                        site.siteName = node.InnerText;
                    }
                    else if (node.Name == "Id")
                    {
                        site.Id = node.InnerText;
                    }
                    else if (node.Name == "authPasswd")
                    {
                        site.authPasswd = node.InnerText;
                    }
                    else if (node.Name == "serverIP")
                    {
                        site.serverIP = node.InnerText;
                    }
                    else if (node.Name == "serverPort")
                    {
                        site.serverPort = int.Parse(node.InnerText);
                    }
                    else if (node.Name == "ViewEnable")
                    {
                        site.ViewEnable = bool.Parse(node.InnerText);
                    }
                    else if (node.Name == "deviceNo")
                    {
                        int deviceNo = int.Parse(node.InnerText);

                    }
                    else if (node.Name == "UsbCom")
                    {
                        site.UsbCom = bool.Parse(node.InnerText);

                    }
                    else if (node.Name == "deviceConfigs")
                    {
                        //decode base64 first
                        byte[] allDeviceCfg = Convert.FromBase64String(node.InnerText);
                        if (allDeviceCfg.GetLength(0) > 0)
                        {
                            int deviceNo = allDeviceCfg.GetLength(0) / ipcDefines.deviceConfigSize;
                            for (int j = 0; j < deviceNo; j++)
                            {
                                byte[] devConfig = new byte[ipcDefines.deviceConfigSize];
                                for (int k = 0; k < ipcDefines.deviceConfigSize; k++)
                                {
                                    devConfig[k] = allDeviceCfg[j * ipcDefines.deviceConfigSize + k];
                                }
                                site.siteCfg.addDeviceCfg();
                                site.siteCfg.deviceConfigs[j].memCFG = devConfig;
                            }
                        }
                    }
                    else if (node.Name == "globalConfig")
                    {
                        site.siteCfg.globalConfig.memCFG = Convert.FromBase64String(node.InnerText);
                    }
                }

                return site;
            }

            private XmlNode configToXML(ref XmlDocument doc)
            {
                try
                {
                    XmlNode config = doc.CreateNode("element", configFieldName, "");
                    sconnSite[] sites = sconnDataShare.sconnSites.ToArray();
                    foreach (sconnSite site in sites)
                    {
                        XmlNode siteNode = siteToXML(site, ref doc);
                        config.AppendChild(siteNode);
                    }
                    return config;
                }
                catch (Exception e)
                {
                    
                    throw;
                }
            }

            private XElement configToXMLlinq()
            {
                try
                {
                    XElement config = new XElement(configFieldName);
                    sconnSite[] sites = sconnDataShare.sconnSites.ToArray();
                    foreach (sconnSite site in sites)
                    {
                        XElement siteNode = siteToXMLlinq(site);
                        config.Add(siteNode);
                    }
                    return config;
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            private sconnSite[] configFromXML(ref XmlDocument doc)
            {
                try
                {
                    XmlNode config = doc.GetElementsByTagName(configFieldName)[0] as XmlNode;
                    XmlNodeList siteElements = config.ChildNodes;
                    sconnSite[] sites = new sconnSite[siteElements.Count];
                    for (int i = 0; i < siteElements.Count; i++)
                    {
                        sites[i] = siteFromXML(siteElements.Item(i));
                    }
                    return sites;
                }
                catch (Exception e)
                {

                    return new sconnSite[0];
                }

            }

            public bool saveConfig()
            {
                try
                {
                        XmlDocument doc = new XmlDocument();
                        XmlNode configData = configToXML(ref doc);
                        doc.AppendChild(configData);
                    #if WIN32_ENC
                        encryptXmlConfig(doc);
                    #endif
                        doc.Save(configFilePath);
                        return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }

            private bool validateConfigFile(XmlDocument doc)
            {
                //check xml fields
                if (doc.GetElementsByTagName(configFieldName)[0] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }



            public XDocument GetConfigXDocument()
            {
                 try
                 {
                     XDocument doc = new XDocument();
                     XElement configData = configToXMLlinq();
                     doc.Add(configData);
                     return doc;
                 }
                 catch (Exception e)
                 {
                     return new XDocument();
                 }
            }

             public XmlDocument GetConfigXmlDocument()
             {
                 try
                 {
                     XmlDocument doc = new XmlDocument();
                     XmlNode configData = configToXML(ref doc);
                     doc.AppendChild(configData);
                     #if WIN32_ENC   
                     encryptXmlConfig(doc);
                    #endif
                     doc.Save(configFilePath);
                     return doc;
                 }
                 catch (Exception e)
                 {
                     return new XmlDocument();
                 }
            }

            public bool loadConfig()
            {

                
                    #if WIN32_ENC
                    if (File.Exists(configFilePath))
                    {
                    #else
                    if(true)
                    {
                    #endif

                    XmlDocument doc = new XmlDocument();
                    doc.Load(configFilePath);
                    #if WIN32_ENC
                    decryptXmlConfig(doc);
                    #endif
                    if (validateConfigFile(doc))
                    {
                        //sconnDataShare.removeSites();
                        //sconnDataShare.addSites(configFromXML(ref doc));

                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }



            public List<sconnSite> GetSites()
            {


                    #if WIN32_ENC
                                    if (File.Exists(configFilePath))
                                    {
                    #else
                                        if(true)
                                        {
                    #endif

                                        XmlDocument doc = new XmlDocument();
                                        doc.Load(configFilePath);
                    #if WIN32_ENC
                                        decryptXmlConfig(doc);
                    #endif
                    if (validateConfigFile(doc))
                    {
                        return configFromXML(ref doc).ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }


#if WIN32_ENC


            private static void EncryptXmlElement(XmlDocument Doc, string ElementName, SymmetricAlgorithm Key)
            {
                if (Doc == null || ElementName == null || Key == null)
                    throw new ArgumentNullException("ElementNull");           
                XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementName)[0] as XmlElement;
                if (elementToEncrypt == null)
                {
                    throw new XmlException("The specified element was not found");
                }

                EncryptedXml eXml = new EncryptedXml();
                byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, Key, false);
                EncryptedData edElement = new EncryptedData();
                edElement.Type = EncryptedXml.XmlEncElementUrl;
                string encryptionMethod = null;

                if (Key is TripleDES)
                {
                    encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
                }
                else if (Key is DES)
                {
                    encryptionMethod = EncryptedXml.XmlEncDESUrl;
                }
                if (Key is Rijndael)
                {
                    switch (Key.KeySize)
                    {
                        case 128:
                            encryptionMethod = EncryptedXml.XmlEncAES128Url;
                            break;
                        case 192:
                            encryptionMethod = EncryptedXml.XmlEncAES192Url;
                            break;
                        case 256:
                            encryptionMethod = EncryptedXml.XmlEncAES256Url;
                            break;
                    }
                }
                else
                {
                    // Throw an exception if the transform is not in the previous categories 
                    throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
                }
                edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);
                edElement.CipherData.CipherValue = encryptedElement;
                EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
            }

            private static void EncryptXmlElement(XDocument Doc, string ElementName, SymmetricAlgorithm Key)
            {
              
            }



            private static void DecryptXmlElement(XmlDocument Doc, SymmetricAlgorithm Alg)
            {  
                if (Doc == null || Alg == null)
                    throw new ArgumentNullException("NullArg");
                XmlElement encryptedElement = Doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;
                if (encryptedElement == null)
                {
                    throw new XmlException("The EncryptedData element was not found.");
                }
                EncryptedData edElement = new EncryptedData();
                edElement.LoadXml(encryptedElement);
                EncryptedXml exml = new EncryptedXml();
                byte[] rgbOutput = exml.DecryptData(edElement, Alg);
                exml.ReplaceData(encryptedElement, rgbOutput);

            }

            #endif

       }


        /********   Config data sources    ********/

        private class ipcDataSource
        {
            enum sourceType
            {
                ipcCfgFile = 0,
                ipcSql = 1
            }
            sourceType dataSource;

            public ipcDataSource()
            {
                dataSource = sourceType.ipcSql;
            }

            public void writeData()
            {
                if (dataSource == sourceType.ipcCfgFile)
                {

                }
                else if (dataSource == sourceType.ipcSql)
                {

                }
            }


            public void readData()
            {
                if (dataSource == sourceType.ipcCfgFile)
                {

                }
                else if (dataSource == sourceType.ipcSql)
                {

                }
            }
        }

        


    }//sconnDataSrc



}
