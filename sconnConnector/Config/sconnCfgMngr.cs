using sconnConnector.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
#if WIN32_ENC

using System.Windows;
using System.Windows.Input;

#endif



namespace sconnConnector
{
	public class sconnCfgMngr
	{
		ETH ethernet;

		/********   Program config    ********/
		/********                      *******/
		/***     Handles Data save/read     ***/
		private  sconnDataSrc ConfigSource = new sconnDataSrc();

		public sconnCfgMngr()
		{
			ethernet = new ETH();
		}

		public bool authMCU(String server, Int32 port, String cred)
			{
				byte[] authmsg = new byte[ipcDefines.AUTH_RECORD_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES];
				authmsg[0] = ipcCMD.SOP;
				for (int i = 0; i < cred.Length; i++)    
				{
					authmsg[ipcDefines.AUTH_RECORD_PASSWD_POS+i + 1] = (byte)cred[i];
				}
		        authmsg[ipcDefines.AUTH_RECORD_PASS_LEN_POS + 1] = (byte)cred.Length;
                authmsg[ipcDefines.AUTH_RECORD_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES - 1] = ipcCMD.EOP;
				byte[] status = ethernet.berkeleySendMsg(server, authmsg, port);

				if (status[0] == ipcCMD.AUTHFAIL)
				{
					return false;
				}
				else if (status[0] == ipcCMD.AUTHOK)
				{
					return true;
				}
				else
				{
					return false;
				}

			}
	 
		public  void saveConfig()
			{
				ConfigSource.SaveConfig(DataSourceType.xml);
			}
	
		public  void loadConfig()
			{
				ConfigSource.LoadConfig(DataSourceType.xml);
			}

		public  int getSiteDeviceNo( sconnSite site)
		   {
			   SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
					int sites = 0;
					byte[] cmd = new byte[ipcDefines.NET_CMD_PACKET_LEN];
					cmd[0] = ipcCMD.GET;
					cmd[1] = ipcCMD.getDevNo;
					byte[] resp = client.berkeleySendMsg(cmd); //ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
					if (resp[0] == ipcCMD.SVAL)
					{
						sites = (int)resp[2]; // second byte is data,  <SVAL> <DATA> <EVAL>                  
					}
					client.CloseConnection();
					return sites;
			}


		public bool WriteGlobalNamesCfg(sconnSite site)
		{
			if (site.siteCfg == null) { return false; }
			SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

			site.siteStat.StartConnectionTimer();
			try
			{
				int bfSize = ipcDefines.NET_MAX_TX_SIZE;
				int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES;
				int bytesToSend = ipcDefines.RAM_NAMES_Global_Total_Size;

				// Receiving byte array  
				byte[] txBF = new byte[bfSize];
				byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];

				txBF[0] = ipcCMD.SET;
				txBF[1] = ipcCMD.setGlobalNames;
				rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

				if (rxBF[0] == ipcCMD.ACK)
				{
					int fullTxNo = (int)ipcDefines.RAM_NAMES_Global_Total_Size / packetData;
					int signleBytes = (int)ipcDefines.RAM_NAMES_Global_Total_Size % packetData;

					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHNXT;
					txBF[2] = ipcCMD.SVAL;
					int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : bytesToSend + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
					txBF[packetLastByteIndex] = ipcCMD.EVAL;
					for (int j = 0; j < fullTxNo; j++)
					{
						int startAddr = j * packetData;
						for (int k = 0; k < packetData; k++)
						{
							txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.GlobalNameConfig[(startAddr + k)];
						}

						rxBF = client.berkeleySendMsg(txBF, bfSize);
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//last packet
					if (signleBytes > 0)
					{
						for (int l = 0; l < signleBytes; l++)
						{
							txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.GlobalNameConfig[(fullTxNo * packetData) + l];
						}
						rxBF = client.berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
					 
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//signal finish
					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHFIN;
					txBF[2] = ipcDefines.NET_PACKET_TYPE_GLOBNAMECFG;
					rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
					if (rxBF[0] != ipcCMD.ACKFIN)
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;
						return false;
					}
				}
				else
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return false;
				}
			}
			catch (Exception e)
			{
				site.siteStat.StopConnectionTimer();
				site.siteStat.FailedConnections++;
				return false;
			}

			return false;
		}

	    public async Task<bool> WriteGlobalCfgAsync(sconnSite site)
	    {
	        return true;
	    }


	    public bool WriteGlobalCfg(sconnSite site)
		{
			if (site.siteCfg == null){ return false;}
			SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

					site.siteStat.StartConnectionTimer();
					try
					{
						int bfSize = ipcDefines.NET_MAX_TX_SIZE;
						int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES;
						int bytesToSend = ipcDefines.ipcGlobalConfigSize;

						// Receiving byte array  
						byte[] txBF = new byte[bfSize];
						byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];

							txBF[0] = ipcCMD.SET;
							txBF[1] = ipcCMD.setGlobalCfg;
							rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

							if (rxBF[0] == ipcCMD.ACK)
							{
								int fullTxNo = (int)ipcDefines.ipcGlobalConfigSize / packetData;
								int signleBytes = (int)ipcDefines.ipcGlobalConfigSize % packetData;

								txBF[0] = ipcCMD.PSH;
								txBF[1] = ipcCMD.PSHNXT;
								txBF[2] = ipcCMD.SVAL;
								int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : bytesToSend + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
								txBF[packetLastByteIndex] = ipcCMD.EVAL;
								for (int j = 0; j < fullTxNo; j++)
								{
									int startAddr = j * packetData;
									for (int k = 0; k < packetData; k++)
									{
										txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.globalConfig.memCFG[(startAddr + k)];
									}

									rxBF = client.berkeleySendMsg(txBF, bfSize);
									if (rxBF[0] != ipcCMD.ACKNXT)
									{
										site.siteStat.StopConnectionTimer();
										site.siteStat.FailedConnections++;
										return false;
									}
								}
								//last packet
								if (signleBytes > 0)
								{
									for (int l = 0; l < signleBytes; l++)
									{
										txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.globalConfig.memCFG[(fullTxNo * packetData) + l];
									}
									rxBF = client.berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
									//ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.deviceConfigSize + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);

									if (rxBF[0] != ipcCMD.ACKNXT)
									{
										site.siteStat.StopConnectionTimer();
										site.siteStat.FailedConnections++;
										return false;
									}
								}
								//signal finish
								txBF[0] = ipcCMD.PSH;
								txBF[1] = ipcCMD.PSHFIN;
								txBF[2] = ipcDefines.NET_PACKET_TYPE_GCFG;
								rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
								if (rxBF[0] != ipcCMD.ACKFIN)
								{
									site.siteStat.StopConnectionTimer();
									site.siteStat.FailedConnections++;
									return false;
								}

                            return true;

							}
							else
							{
								site.siteStat.StopConnectionTimer();
								site.siteStat.FailedConnections++;
								return false;
							}
					}
					catch (Exception e)
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;
						return false;
					}
		}


		public string SendGsmCommandDirect(sconnSite site, string command)
		{
			try
			{
				SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
				client.ConnectionTimeoutMs = 4000; //long timeout
				site.siteStat.StartConnectionTimer();

				int bfSize = ipcDefines.NET_MAX_TX_SIZE;

				// Receiving byte array  
				byte[] txBF = new byte[bfSize];
				byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
				txBF[0] = ipcCMD.GET;
				txBF[1] = ipcCMD.getGsmModemResponse;

				//copy command
				byte[] strb = Encoding.ASCII.GetBytes(command);
				for (int i = 0; i < strb.Length; i++)
				{
					txBF[i + 2] = strb[i];
				}
				txBF[strb.Length+2] = 0x0D; 

				rxBF = client.berkeleySendMsg(txBF, strb.Length + 3);

				string resp = Encoding.ASCII.GetString(rxBF);
				return resp;
			}
			catch (Exception ex)
			{
				return null;
			}


		}

	    public bool WriteAuthorizedDevicesCfg(sconnSite site)
	    {
	        return WriteDeviceDevAuthCfgSingle(site, 0x00);
	    }

		public bool WriteDeviceCfg(sconnSite site)
		{         
			if (site.siteCfg == null)
			{
				return false;
			}
		  int devices = site.siteCfg.deviceNo;
		  for (int i = 0; i < devices; i++)
		  {
			  if (!WriteDeviceCfgSingle(site, i))
				  return false;
		  }//each device
		  return true;
		}


		public bool WriteSiteGsmCfg(sconnSite site)
		{
			try
			{
				SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
				site.siteStat.StartConnectionTimer();
				if (site.siteCfg == null)
				{
					return false;
				}

				int bfSize = ipcDefines.NET_MAX_TX_SIZE;
				int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
				int bytesToSend = ipcDefines.RAM_SMS_RECP_NO*ipcDefines.RAM_SMS_RECP_SIZE;

				// Receiving byte array  
				byte[] txBF = new byte[bfSize];
				byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
				txBF[0] = ipcCMD.SET;
				txBF[1] = ipcCMD.setGsmRcptCfg;

				rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN); 

				if (rxBF[0] == ipcCMD.ACK)
				{
					int fullTxNo = (int)bytesToSend / packetData;
					int signleBytes = (int)bytesToSend % packetData;

					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHNXT;
					txBF[2] = ipcCMD.SVAL;

					//serialize
					byte[] gsmRcptsBytes = new byte[bytesToSend];
					for (int i = 0; i < ipcDefines.RAM_SMS_RECP_NO; i++)
					{
						byte[] rcpbytes = site.siteCfg.gsmRcpts[i].Serialized;
						for (int j = 0; j < ipcDefines.RAM_SMS_RECP_SIZE; j++)
						{
							gsmRcptsBytes[(i * ipcDefines.RAM_SMS_RECP_SIZE) + j] = rcpbytes[j];
						}
					}
					

					int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : bytesToSend + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
					txBF[packetLastByteIndex] = ipcCMD.EVAL;
					for (int j = 0; j < fullTxNo; j++)
					{
						int startAddr = j * packetData;
						for (int k = 0; k < packetData; k++)
						{
							txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = gsmRcptsBytes[(startAddr + k)];
						}

						rxBF = client.berkeleySendMsg(txBF, bfSize);
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//last packet
					if (signleBytes > 0)
					{
						for (int l = 0; l < signleBytes; l++)
						{
							txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = gsmRcptsBytes[(fullTxNo * packetData) + l];
						}
						rxBF = client.berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//signal finish
					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHFIN;
					txBF[2] = ipcDefines.NET_PACKET_TYPE_GSMRCPTCFG;
					rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
					if (rxBF[0] != ipcCMD.ACKFIN)
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;
						return false;
					}
				}
				else
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return false;
				}



				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}


		public bool WriteDeviceCfgSingle(sconnSite site, int DevId)
		{
			if (site.siteCfg == null)
			{
				return false;
			}

			SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
			site.siteStat.StartConnectionTimer();
			ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
			try
				{
					int bfSize = ipcDefines.NET_MAX_TX_SIZE;
					int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
					int bytesToSend = ipcDefines.deviceConfigSize;

					// Receiving byte array  
					byte[] txBF = new byte[bfSize];
					byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
					txBF[0] = ipcCMD.SET;
					txBF[1] = ipcCMD.setDeviceCfg;
					txBF[2] = (byte)DevId;

					rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN); // ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.NET_CMD_PACKET_LEN);

					if (rxBF[0] == ipcCMD.ACK)
					{
						int fullTxNo = (int)ipcDefines.deviceConfigSize / packetData;
						int signleBytes = (int)ipcDefines.deviceConfigSize % packetData;

						txBF[0] = ipcCMD.PSH;
						txBF[1] = ipcCMD.PSHNXT;
						txBF[2] = ipcCMD.SVAL;
						int packetLastByteIndex = bytesToSend > packetData ? bfSize-1 : ipcDefines.deviceConfigSize + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
						txBF[packetLastByteIndex] = ipcCMD.EVAL;
						for (int j = 0; j < fullTxNo; j++)
						{
							int startAddr = j * packetData;
							for (int k = 0; k < packetData; k++)
							{
								txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].memCFG[(startAddr + k)];
							}

							rxBF = client.berkeleySendMsg(txBF, bfSize); 
							if (rxBF[0] != ipcCMD.ACKNXT)
							{
								site.siteStat.StopConnectionTimer();
								site.siteStat.FailedConnections++;
								return false;
							}
						}
						//last packet
						if (signleBytes > 0)
						{
							for (int l = 0; l < signleBytes; l++)
							{
								txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].memCFG[(fullTxNo * packetData) + l];
							}
							rxBF = client.berkeleySendMsg(txBF, signleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);
							if (rxBF[0] != ipcCMD.ACKNXT)
							{
								site.siteStat.StopConnectionTimer();
								site.siteStat.FailedConnections++;
								return false;
							}
						}
						//signal finish
						txBF[0] = ipcCMD.PSH;
						txBF[1] = ipcCMD.PSHFIN;
						txBF[2] = ipcDefines.NET_PACKET_TYPE_DEVCFG;
						rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
						if (rxBF[0] != ipcCMD.ACKFIN)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					else
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;
						return false;
					}
				}
				catch (Exception e)
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return false;
				}
			site.siteStat.StopConnectionTimer();
			return true;
		}


		public bool WriteDeviceNetCfg(sconnSite site, int DevId)
		{
			if (site.siteCfg == null)
			{
				return false;
			}

			SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
			site.siteStat.StartConnectionTimer();
			try
			{
				int bfSize = ipcDefines.NET_MAX_TX_SIZE;
				int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
				int bytesToSend = ipcDefines.NET_CFG_SIZE;

				// Receiving byte array  
				byte[] txBF = new byte[bfSize];
				byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
				txBF[0] = ipcCMD.SET;
				txBF[1] = ipcCMD.setDeviceNetworkCfg;
				txBF[2] = (byte)DevId;

				rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN); // ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.NET_CMD_PACKET_LEN);

				if (rxBF[0] == ipcCMD.ACK)
				{
					int fullTxNo = (int)ipcDefines.NET_CFG_SIZE / packetData;
					int signleBytes = (int)ipcDefines.NET_CFG_SIZE % packetData;

					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHNXT;
					txBF[2] = ipcCMD.SVAL;
					int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.NET_CFG_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
					txBF[packetLastByteIndex] = ipcCMD.EVAL;
					for (int j = 0; j < fullTxNo; j++)
					{
						int startAddr = j * packetData;
						for (int k = 0; k < packetData; k++)
						{
							txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NetworkConfig[(startAddr + k)];
						}

						rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CFG_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
						// ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.deviceConfigSize + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//last packet
					if (signleBytes > 0)
					{
						for (int l = 0; l < signleBytes; l++)
						{
							txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NetworkConfig[(fullTxNo * packetData) + l];
						}
						rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CFG_SIZE + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);
						//ethernet.berkeleySendMsg(site.serverIP, txBF, site.serverPort, ipcDefines.deviceConfigSize + ipcDefines.NET_DATA_PACKET_CONTROL_BYTES);

						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//signal finish
					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHFIN;
					txBF[2] = ipcDefines.NET_PACKET_TYPE_NETCFG;
					rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
					if (rxBF[0] != ipcCMD.ACKFIN)
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;
						return false;
					}
				}
				else
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return false;
				}
			}
			catch (Exception e)
			{
				site.siteStat.StopConnectionTimer();
				site.siteStat.FailedConnections++;
				return false;
			}
			site.siteStat.StopConnectionTimer();
			return true;
		}


		public bool WriteDeviceNamesCfgSingle(sconnSite site, int DevId)
		{
			if (site.siteCfg == null)
			{
				return false;
			}
			SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

			site.siteStat.StartConnectionTimer();
			ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
			try
			{
				int bfSize = ipcDefines.NET_MAX_TX_SIZE;
				int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
				int bytesToSend = ipcDefines.RAM_DEVICE_NAMES_SIZE;

				// Receiving byte array  
				byte[] txBF = new byte[bfSize];
				byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
				txBF[0] = ipcCMD.SET;
				txBF[1] = ipcCMD.setDeviceNamesCfg;
				txBF[2] = (byte)DevId;

				rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

				if (rxBF[0] == ipcCMD.ACK)
				{
					int fullTxNo = (int)ipcDefines.RAM_DEVICE_NAMES_SIZE / packetData;
					int singleBytes = (int)ipcDefines.RAM_DEVICE_NAMES_SIZE % packetData;

					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHNXT;
					txBF[2] = ipcCMD.SVAL;
					int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.RAM_DEVICE_NAMES_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
					txBF[packetLastByteIndex] = ipcCMD.EVAL;

					int nameinc = 0;
					int namecharinc = 0;
					for (int j = 0; j < fullTxNo; j++)
					{            
						for (int k = 0; k < packetData; k++)
						{
							if (namecharinc >= ipcDefines.RAM_NAME_SIZE)
							{ nameinc++; namecharinc=0;}
							txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NamesCFG[nameinc][namecharinc]; //site.siteCfg.deviceConfigs[DevId].memCFG[(startAddr + k)];
							namecharinc++;
						}

						rxBF = client.berkeleySendMsg(txBF, bfSize);
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//last packet

					if (singleBytes > 0)
					{
						for (int l = 0; l < singleBytes; l++)
						{
							if (namecharinc >= ipcDefines.RAM_NAME_SIZE)
							{ nameinc++;  namecharinc=0;}
							txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].NamesCFG[nameinc][namecharinc];
							namecharinc++;
						}
						rxBF = client.berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//signal finish
					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHFIN;
					txBF[2] = ipcDefines.NET_PACKET_TYPE_DEVNAMECFG;
					rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
					if (rxBF[0] != ipcCMD.ACKFIN)
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;
						return false;
					}
				}
				else
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return false;
				}
			}
			catch (Exception e)
			{
				site.siteStat.StopConnectionTimer();
				site.siteStat.FailedConnections++;
				return false;
			}
			site.siteStat.StopConnectionTimer();
			return true;
		}


		public bool WriteDeviceSchedulesCfgSingle(sconnSite site, int DevId)
		{
			if (site.siteCfg == null)
			{
				return false;
			}
			SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

			site.siteStat.StartConnectionTimer();
			ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
			try
			{
				int bfSize = ipcDefines.NET_MAX_TX_SIZE;
				int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
				int bytesToSend = ipcDefines.RAM_DEV_SCHED_SIZE;

				// Receiving byte array  
				byte[] txBF = new byte[bfSize];
				byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
				txBF[0] = ipcCMD.SET;
				txBF[1] = ipcCMD.setDeviceSchedulesCfg;
				txBF[2] = (byte)DevId;

				rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

				if (rxBF[0] == ipcCMD.ACK)
				{
					int fullTxNo = (int)ipcDefines.RAM_DEV_SCHED_SIZE / packetData;
					int singleBytes = (int)ipcDefines.RAM_DEV_SCHED_SIZE % packetData;

					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHNXT;
					txBF[2] = ipcCMD.SVAL;

					int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.RAM_DEV_SCHED_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
					txBF[packetLastByteIndex] = ipcCMD.EVAL;

					int scheduleinc = 0;
					int schedulebyteinc = 0;
					for (int j = 0; j < fullTxNo; j++)
					{
						for (int k = 0; k < packetData; k++)
						{
							if (schedulebyteinc >= ipcDefines.RAM_DEV_SCHED_MEM_SIZE)
							{ scheduleinc++; schedulebyteinc = 0; }
							txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].ScheduleCFG[scheduleinc][schedulebyteinc]; 
							schedulebyteinc++;
						}

						rxBF = client.berkeleySendMsg(txBF, bfSize);
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//last packet

					if (singleBytes > 0)
					{
						for (int l = 0; l < singleBytes; l++)
						{
							if (schedulebyteinc >= ipcDefines.RAM_DEV_SCHED_MEM_SIZE)
							{ scheduleinc++; schedulebyteinc = 0; }
							txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].ScheduleCFG[scheduleinc][schedulebyteinc];
							schedulebyteinc++;
						}
						rxBF = client.berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//signal finish
					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHFIN;
					txBF[2] = ipcDefines.NET_PACKET_TYPE_SCHEDCFG;
					rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
					if (rxBF[0] != ipcCMD.ACKFIN)
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;      
						return false;
					}
				}
				else
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return false;
				}
			}
			catch (Exception e)
			{
				site.siteStat.StopConnectionTimer();
				site.siteStat.FailedConnections++;
				return false;
			}
			site.siteStat.StopConnectionTimer();
			return true;
		}




		public bool WriteDeviceDevAuthCfgSingle(sconnSite site, int DevId)
		{
			if (site.siteCfg == null)
			{
				return false;
			}
			SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

			site.siteStat.StartConnectionTimer();
			ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (ipcDefines.deviceConfigSize * DevId));
			try
			{
				int bfSize = ipcDefines.NET_MAX_TX_SIZE;
				int packetData = bfSize - ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES; // CMD1 -> CMD2 -> SVAL -> DATA... -> EVAL
				int bytesToSend = ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE;

				// Receiving byte array  
				byte[] txBF = new byte[bfSize];
				byte[] rxBF = new byte[ipcDefines.NET_MAX_RX_SIZE];
				txBF[0] = ipcCMD.SET;
				txBF[1] = ipcCMD.setAuthDevCfg;
				txBF[2] = (byte)DevId;

				rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);

				if (rxBF[0] == ipcCMD.ACK)
				{
					int fullTxNo = (int)ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE / packetData;
					int singleBytes = (int)ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE % packetData;

					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHNXT;
					txBF[2] = ipcCMD.SVAL;

					int packetLastByteIndex = bytesToSend > packetData ? bfSize - 1 : ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET;
					txBF[packetLastByteIndex] = ipcCMD.EVAL;

					for (int j = 0; j < fullTxNo; j++)
					{
						for (int k = 0; k < packetData; k++)
						{
							txBF[k + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.AuthDevices[j*packetData+k];
						}

						rxBF = client.berkeleySendMsg(txBF, bfSize);
						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//last packet

					if (singleBytes > 0)
					{
						for (int l = 0; l < singleBytes; l++)
						{
							txBF[l + ipcDefines.NET_UPLOAD_PACKET_DATA_OFFSET] = site.siteCfg.deviceConfigs[DevId].AuthDevicesCFG[fullTxNo * packetData + l];
						}
						rxBF = client.berkeleySendMsg(txBF, singleBytes + ipcDefines.NET_UPLOAD_PACKET_CONTROL_BYTES);

						if (rxBF[0] != ipcCMD.ACKNXT)
						{
							site.siteStat.StopConnectionTimer();
							site.siteStat.FailedConnections++;
							return false;
						}
					}
					//signal finish
					txBF[0] = ipcCMD.PSH;
					txBF[1] = ipcCMD.PSHFIN;
					txBF[2] = ipcDefines.NET_PACKET_TYPE_DEVAUTHCFG;
					rxBF = client.berkeleySendMsg(txBF, ipcDefines.NET_CMD_PACKET_LEN);
					if (rxBF[0] != ipcCMD.ACKFIN)
					{
						site.siteStat.StopConnectionTimer();
						site.siteStat.FailedConnections++;
						return false;
					}
				}
				else
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return false;
				}
			}
			catch (Exception e)
			{
				site.siteStat.StopConnectionTimer();
				site.siteStat.FailedConnections++;
				return false;
			}
			site.siteStat.StopConnectionTimer();
			return true;
		}




	   public  bool updateSiteStatus(sconnSite site) //read I/O values
			{
				int devices = 0;
				if (site.siteCfg != null)
				{
					devices = site.siteCfg.deviceNo;
				}
				else
				{
					devices = getSiteDeviceNo(  site);
					site.siteCfg = new ipcSiteConfig(devices); //init device configs 
				}

				ushort siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
				try
				{
					for (int i = 0; i < devices; i++)
					{
						siteMemAddr = (ushort)(ipcDefines.mAdrDevStart + (i * ipcDefines.deviceConfigSize)); //device start addresss
						byte[] deviceCFG = new byte[ipcDefines.deviceConfigSize];         
						if (deviceCFG.GetLength(0) == ipcDefines.deviceConfigSize)
						{
							site.siteCfg.deviceConfigs[i].memCFG = deviceCFG;
						}
						else
						{
							return false;
						}
					}
				}
				catch (Exception e)
				{
                    Debug.WriteLine(e.Message + " | " + e.InnerException.Message);
					return false;
				}

				return true;
			}

	   public bool ReadSiteStartupConfig(sconnSite site)
		{
			site.siteStat.StartConnectionTimer();
			bool globalUploadStat = false;
			bool deviceUploadStat = false;
			int devices = 0;
			byte[] cmd = new byte[32];

			if (authMCU(site.serverIP, site.serverPort,  site.authPasswd))  //update only if authed to site
			{

				if (site.siteCfg != null)
				{
					devices = site.siteCfg.deviceNo;
				}
				else
				{
					devices = getSiteDeviceNo(  site);
					site.siteCfg = new ipcSiteConfig(devices); //init device configs 
				}

				/**********  Read global config  ***********/


				ushort siteMemAddr = (ushort)(ipcDefines.mAdrGlobalConfig);
				byte[] rxBF = new byte[ipcDefines.ipcGlobalConfigSize + 2];

				cmd[0] = ipcCMD.GET;
				cmd[1] = ipcCMD.getGlobCfg;
				rxBF = ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
				//rxBF = ethernet.berkeleyReadLen(site.serverIP, cmd, site.serverPort, ipcDefines.ipcGlobalConfigSize + 2);

				if (rxBF[0] == ipcCMD.SVAL)
				{
					for (int j = 0; j < ipcDefines.ipcGlobalConfigSize; j++)
					{
						site.siteCfg.globalConfig.memCFG[j] = rxBF[j + 1];
					}
					globalUploadStat = true;
				}




				/**********  Get device configs **********/

				cmd = new byte[32];
				rxBF = new byte[ipcDefines.deviceConfigSize + 2];
				siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
				try
				{
					for (int i = 0; i < devices; i++)
					{
						cmd[0] = ipcCMD.GET;
						cmd[1] = ipcCMD.getDevCfg;
						cmd[2] = (byte)i; //device number
						rxBF = ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
						//rxBF = ethernet.berkeleyReadLen(site.serverIP, cmd, site.serverPort, ipcDefines.deviceConfigSize + 2);
						deviceUploadStat = true;
						if (rxBF[0] == ipcCMD.SVAL)
						{
							for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
							{
								site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + 1];
							}
						}
						else
						{
							deviceUploadStat = false;
						}
					}
					site.siteStat.StopConnectionTimer();
					return (bool)(deviceUploadStat & globalUploadStat);

				}
				catch (Exception e)
				{
					site.siteStat.StopConnectionTimer();
					site.siteStat.FailedConnections++;
					return (bool)(deviceUploadStat & globalUploadStat);
					throw;
				}

			}
			else
			{
				site.siteStat.StopConnectionTimer();
				site.siteStat.FailedConnections++;
				//MessageBoxResult result = MessageBox.Show("MCU Auth Fail");
				return (bool)(deviceUploadStat & globalUploadStat);
			}

		}


	   public bool ReadSiteRunningDeviceConfigs(sconnSite site)
	   {
		   /**********  Get device configs **********/
		   site.siteStat.StartConnectionTimer();
		   bool globalUploadStat = false;
		   bool deviceUploadStat = false;
		   int devices = 0;
		   byte[] cmd = new byte[32];
		   ushort siteMemAddr = (ushort)(ipcDefines.mAdrGlobalConfig);
		   byte[] rxBF = new byte[ipcDefines.ipcGlobalConfigSize + 2];

		   SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);

		   cmd = new byte[32];
		   rxBF = new byte[ipcDefines.deviceConfigSize + 2];
		   byte[,] narrNamesBF = new byte[ipcDefines.RAM_DEV_NAMES_NO, ipcDefines.RAM_NAME_SIZE];
		   byte[] SchedBF = new byte[ipcDefines.RAM_DEV_SCHED_SIZE];
		   byte[] gsmRcpBF = new byte[ipcDefines.RAM_SMS_RECP_MEM_SIZE];
		   byte[] devAuthBF = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
		   siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
		   int MsgByteOffset = 1;

		   try
		   {
			   for (int i = 0; i < devices; i++)
			   {
				   /*******  Get Device Config  ****/
				   cmd[0] = ipcCMD.GET;
				   cmd[1] = ipcCMD.getRunDevCfg;
				   cmd[2] = (byte)i; //devi ce number
				   rxBF = client.berkeleySendMsg(cmd);

				   if (rxBF[0] == ipcCMD.SVAL)
				   {
					   deviceUploadStat = true;

					   //read device config
					   for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
					   {
						   site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + MsgByteOffset];
					   }

				   }
				   else
				   {
					   deviceUploadStat = false;
				   }
			   }

			   //get events
			   cmd[0] = ipcCMD.GET;
			   cmd[1] = ipcCMD.getEventNo;
			   rxBF = client.berkeleySendMsg(cmd);

			   cmd[1] = ipcCMD.getEvent;
			   cmd[2] = (byte)1; //test event id 1
			   rxBF = client.berkeleySendMsg(cmd);

			   site.siteStat.StopConnectionTimer();

		   }
		   catch (Exception e)
		   {
			   site.siteStat.StopConnectionTimer();
			   site.siteStat.FailedConnections++;
			   client.CloseConnection();
			   return (bool)(deviceUploadStat & globalUploadStat);
			   throw;
		   }
		   client.CloseConnection();
		   return (bool)(deviceUploadStat & globalUploadStat);

	   }


        private bool HashMatch()
       {
           return false;
       }



        /*********** Device config update only **************/

	    public bool ReadSiteRunningConfigIO(sconnSite site)
	    {
            return ReadSiteRunningConfigMin(site, false);
	    }



	    /***********  Minimum config update **********/
	   public bool ReadSiteRunningConfigMin(sconnSite site, bool ReadSpecialRegs)
	   {
           SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
           bool globalUploadStat = false;
           bool deviceUploadStat = false;
           int devices = 0;
           byte[] cmd = new byte[32];
           byte[] rxBF = new byte[2048];
           byte[] gcfgRx = new byte[ipcDefines.ipcGlobalConfigSize + 2];
           bool configChanged = true;
           int rxOffset = 1;
           ushort siteMemAddr = (ushort)(ipcDefines.mAdrGlobalConfig);

           try
           {
		       site.siteStat.StartConnectionTimer();
		      
		       //Get config hash first to verify if config has changed at all
		       cmd[0] = ipcCMD.GET;
		       cmd[1] = ipcCMD.getConfigHash;
		       rxBF = client.berkeleySendMsg(cmd); 
		       if(rxBF[0] == ipcCMD.SVAL){
			       byte[] hashrx = new byte[ipcDefines.SHA256_DIGEST_SIZE];

		            SHA256 shaHash = SHA256.Create();
                    byte[] clientHash = shaHash.ComputeHash((site.siteCfg.globalConfig.memCFG));
		           site.siteCfg.Hash = clientHash; //update local hash based on what we actually have

                    for (int i = 0; i < ipcDefines.SHA256_DIGEST_SIZE; i++)
				    {
					    hashrx[i] = rxBF[i + rxOffset];
				    }
			       configChanged = !(hashrx.SequenceEqual(site.siteCfg.Hash));
		       }
		       else
		       {
			       return false;
		       }
               bool DeviceChanged;
               bool NamesChanged;


		   /**********  Read global config  ***********/
		   if (configChanged)
		   {
               //update hash
               for (int i = 0; i < ipcDefines.SHA256_DIGEST_SIZE; i++)
               {
                   site.siteCfg.Hash[i] = rxBF[i + rxOffset];
               }

			   cmd[0] = ipcCMD.GET;
			   cmd[1] = ipcCMD.getRunGlobCfg;
               gcfgRx = client.berkeleySendMsg(cmd);     //    ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);
               bool GcfgOk =  (gcfgRx[0] == ipcCMD.SVAL);
                   
			   /**********  Get device configs **********/

			   cmd = new byte[32];
			   rxBF = new byte[ipcDefines.deviceConfigSize + 2];
			   byte[,] narrNamesBF = new byte[ipcDefines.RAM_DEV_NAMES_NO, ipcDefines.RAM_NAME_SIZE];
			   byte[] SchedBF = new byte[ipcDefines.RAM_DEV_SCHED_SIZE];
			   byte[] gsmRcpBF = new byte[ipcDefines.RAM_SMS_RECP_MEM_SIZE];
			   byte[] devAuthBF = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
			   siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
			   int MsgByteOffset = 1;

               if (GcfgOk)
               {

                   devices = gcfgRx[2];
                   if (site.siteCfg.deviceNo  != devices)
                   {
                       site.siteCfg = new ipcSiteConfig(devices);
                   }
                   //site.siteCfg.globalConfig.memCFG = gcfgRx;
                    for (int i = 0; i < ipcDefines.ipcGlobalConfigSize; i++)
                    {
                            site.siteCfg.globalConfig.memCFG[i] = gcfgRx[i + ipcDefines.NET_UPLOAD_DATA_END_OFFSET];
                    }


                    for (int i = 0; i < devices; i++)
                    {

                       //find out which configs changed by gcfg registers
                       //long cfgIncBefore = CfgOper.GetLongFromBufferAtPos(site.siteCfg.globalConfig.memCFG,ipcDefines.GCFG_DEV_MOD_CTR_START_POS + (i * ipcDefines.GCFG_DEV_MOD_CTR_LEN));
                       //long cfgIncNow = CfgOper.GetLongFromBufferAtPos(gcfgRx, ipcDefines.GCFG_DEV_MOD_CTR_START_POS + (i * ipcDefines.GCFG_DEV_MOD_CTR_LEN) + rxOffset);
                       //DeviceChanged = cfgIncBefore < cfgIncNow;
                       byte[] devhashrx = new byte[ipcDefines.SHA256_DIGEST_SIZE];
                       for (int j = 0; j < ipcDefines.SHA256_DIGEST_SIZE; j++)
                       {
                           devhashrx[j] = gcfgRx[ipcDefines.GCFG_DEV_MOD_CTR_START_POS + (i*ipcDefines.GCFG_DEV_MOD_CTR_LEN) + j + rxOffset];
                       }
                       DeviceChanged = !(devhashrx.SequenceEqual(site.siteCfg.deviceConfigs[i].Hash));
                       
                       if (DeviceChanged)
                       {
                           for (int h = 0; h < ipcDefines.SHA256_DIGEST_SIZE; h++)
                           {
                               site.siteCfg.deviceConfigs[i].Hash[h] = gcfgRx[ipcDefines.GCFG_DEV_MOD_CTR_START_POS + (i * ipcDefines.GCFG_DEV_MOD_CTR_LEN) + h + rxOffset];
                           }
                           /*******  Get Device Config  ****/

                           cmd[0] = ipcCMD.GET;
                           cmd[1] = ipcCMD.getRunDevCfg;
                           cmd[2] = (byte)i; //device number
                           rxBF = client.berkeleySendMsg(cmd);

                           if (rxBF[0] == ipcCMD.SVAL)
                           {

                               //read device config
                               for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
                               {
                                   site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + MsgByteOffset];
                               }
                               
                           }    //dev rx ok
                       }

                       // ReadSpecialRegs = true;
                            if (ReadSpecialRegs)
                            {
                                /*****  Get Device AUTH CFG ******/

                                cmd[1] = ipcCMD.getAuthDevices;
                                devAuthBF = client.berkeleySendMsg(cmd);
                                site.siteCfg.AuthDevices = devAuthBF;

                                /*****  Get GSM RCPT ******/

                                cmd[1] = ipcCMD.getGsmRecpCfg;
                                gsmRcpBF = client.berkeleySendMsg(cmd);

                                deviceUploadStat = true;


                                //read  schedule config
                                if (SchedBF[0] == ipcCMD.SVAL)
                                {
                                    for (int schedule = 0; schedule < ipcDefines.RAM_DEV_SCHED_NO; schedule++)
                                    {
                                        for (int schedbyte = 0; schedbyte < ipcDefines.RAM_DEV_SCHED_MEM_SIZE; schedbyte++)
                                        {
                                            site.siteCfg.deviceConfigs[i].ScheduleCFG[schedule][schedbyte] = SchedBF[(schedule * ipcDefines.RAM_DEV_SCHED_MEM_SIZE) + schedbyte + MsgByteOffset]; //1 byte buffer offset
                                        }
                                    }
                                }

                                //read GSM config
                                if (gsmRcpBF[0] == ipcCMD.SVAL)
                                {
                                    try
                                    {
                                        int bufferOffset = 1;
                                        ipcRcpt[] rcpts = new ipcRcpt[ipcDefines.RAM_SMS_RECP_SIZE];
                                        for (int r = 0; r < ipcDefines.RAM_SMS_RECP_SIZE; r++)
                                        {
                                            byte[] record = new byte[ipcDefines.RAM_SMS_RECP_SIZE];
                                            for (int btc = 0; btc < ipcDefines.RAM_SMS_RECP_SIZE; btc++)
                                            {
                                                record[btc] = gsmRcpBF[bufferOffset + (r * ipcDefines.RAM_SMS_RECP_SIZE) + btc];
                                            }
                                            ipcRcpt rcpt = new ipcRcpt(record);
                                            rcpts[r] = rcpt;
                                        }
                                        site.siteCfg.gsmRcpts = rcpts;
                                    }
                                    catch (Exception e)
                                    {
                                        //TODO - buffer overflow
                                    }

                                }
                            }


                            //cfgIncBefore = CfgOper.GetLongFromBufferAtPos(site.siteCfg.globalConfig.memCFG,ipcDefines.GCFG_NAMES_MOD_CTR_POS);
                            //cfgIncNow = CfgOper.GetLongFromBufferAtPos(gcfgRx, ipcDefines.GCFG_NAMES_MOD_CTR_POS + rxOffset);
                            //NamesChanged = cfgIncBefore < cfgIncNow;

                            if (ReadSpecialRegs)
                            {
                           byte[] nameshashrx = new byte[ipcDefines.SHA256_DIGEST_SIZE];
                           for (int j = 0; j < ipcDefines.SHA256_DIGEST_SIZE; j++)
                           {
                               nameshashrx[j] = gcfgRx[ipcDefines.GCFG_NAMES_MOD_CTR_POS + j + rxOffset];
                           }
                           NamesChanged = !(nameshashrx.SequenceEqual(site.siteCfg.NamesHash));


                           if (NamesChanged)
                           {
                               for (int h = 0; h < ipcDefines.SHA256_DIGEST_SIZE; h++)
                               {
                                   site.siteCfg.NamesHash[h] = gcfgRx[ipcDefines.GCFG_NAMES_MOD_CTR_POS + h + rxOffset];
                               }
                               ///*****  Get Device Names ******/
                               cmd[1] = ipcCMD.getDeviceName;
                               for (int n = 0; n < ipcDefines.RAM_DEV_NAMES_NO; n++)
                               {

                                   cmd[2] = (byte)n;
                                   cmd[3] = (byte)rxBF[ipcDefines.mAdrDevID + 1]; //TODO 2 byte addressing
                                   byte[] narr = client.berkeleySendMsg(cmd);
                                   if (narr[0] == ipcCMD.SVAL)
                                   {
                                       for (int txtbyte = MsgByteOffset; txtbyte < ipcDefines.RAM_NAME_SIZE + MsgByteOffset; txtbyte++)
                                       {
                                           site.siteCfg.deviceConfigs[i].NamesCFG[n][txtbyte - MsgByteOffset] = narr[txtbyte]; //1 byte buffer offset
                                       }
                                   }
                               }

                               ///*****  Get Global Names ******/
                               cmd[1] = ipcCMD.getGlobalNames;
                               byte[] nresp = client.berkeleySendMsg(cmd);
                               if (nresp[0] == ipcCMD.SVAL)
                               {
                                   for (int n = 0; n < ipcDefines.RAM_NAMES_Global_Total_Size; n++)
                                   {
                                       site.siteCfg.GlobalNameConfig[n] = nresp[n + MsgByteOffset];
                                   }
                               }
                           }

                           
                           //get events
                           cmd[0] = ipcCMD.GET;
                           cmd[1] = ipcCMD.getEventNo;
                           rxBF = client.berkeleySendMsg(cmd);
                           int events = ( ((int)rxBF[1]<<8) | (int)rxBF[2]);
                           site.siteCfg.events = new ipcEvent[events];
                           //TODO events track change
                           for (int j = 0; j < events%100; j++) //get events but not more then 100
                           {
                                    cmd[1] = ipcCMD.getEvent;
                                    cmd[2] = (byte)j; 
                                    rxBF = client.berkeleySendMsg(cmd);
                                    byte[] evBF = new byte[ipcDefines.EVENT_DB_RECORD_LEN];
                                   for (int k = 0; k < ipcDefines.EVENT_DB_RECORD_LEN; k++)
                                   {
                                       evBF[k] = rxBF[k + 1];
                                   }
                                    site.siteCfg.events[j] = new ipcEvent(evBF);
                           }

                                //Auth cfg
                                cmd[0] = ipcCMD.GET;
                                cmd[1] = ipcCMD.getPasswdCfg;
                                rxBF = client.berkeleySendMsg(cmd);
                                byte[] AuthBf = new byte[ipcDefines.AUTH_MAX_USERS * ipcDefines.AUTH_RECORD_SIZE];
                                for (int j = 0; j < ipcDefines.AUTH_MAX_USERS * ipcDefines.AUTH_RECORD_SIZE; j++)
                                {
                                    AuthBf[j] = rxBF[j + 1];
                                }
                                site.siteCfg.UserConfig = AuthBf;


                            }

                        
                   }


                       deviceUploadStat = true;
                       globalUploadStat = true;

                       site.siteStat.StopConnectionTimer();

                       for (int j = 0; j < ipcDefines.ipcGlobalConfigSize; j++)
                       {
                           site.siteCfg.globalConfig.memCFG[j] = gcfgRx[j + rxOffset];
                       }
                      
				   }    //gcfg ok

	
                 }  //config changed

                   client.CloseConnection();
                   return true;
			   }
			   catch (Exception e)
			   {
				   site.siteStat.StopConnectionTimer();
				   site.siteStat.FailedConnections++;
				   client.CloseConnection();
				   return false;
				   throw;
			   }

	   }


	   public bool ReadSiteRunningConfig(sconnSite site)
	   {
           return ReadSiteRunningConfigMin(site, true);


		   site.siteStat.StartConnectionTimer();
		   bool globalUploadStat = false;
		   bool deviceUploadStat = false;
		   int devices = 0;
		   byte[] cmd = new byte[32];
		   SconnClient client = new SconnClient(site.serverIP, site.serverPort, site.authPasswd,true);


			   /**********  Read global config  ***********/

			   ushort siteMemAddr = (ushort)(ipcDefines.mAdrGlobalConfig);
			   byte[] rxBF = new byte[ipcDefines.ipcGlobalConfigSize + 2];

			   cmd[0] = ipcCMD.GET;
			   cmd[1] = ipcCMD.getRunGlobCfg;
			   rxBF = client.berkeleySendMsg(cmd);     //    ethernet.berkeleySendMsg(site.serverIP, cmd, site.serverPort);

			   if (rxBF[0] == ipcCMD.SVAL)
			   {
				   devices = rxBF[2];
				   site.siteCfg = new ipcSiteConfig(devices);
				   
				   for (int j = 0; j < ipcDefines.ipcGlobalConfigSize; j++)
				   {
					   site.siteCfg.globalConfig.memCFG[j] = rxBF[j + 1];
				   }
				   globalUploadStat = true;

			   }

			   /**********  Get device configs **********/

			   cmd = new byte[32];
			   rxBF = new byte[ipcDefines.deviceConfigSize + 2];
			   byte[,] narrNamesBF = new byte[ipcDefines.RAM_DEV_NAMES_NO,ipcDefines.RAM_NAME_SIZE];
			   byte[] SchedBF = new byte[ipcDefines.RAM_DEV_SCHED_SIZE];
			   byte[] gsmRcpBF = new byte[ipcDefines.RAM_SMS_RECP_MEM_SIZE];
			   byte[] devAuthBF = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
			   siteMemAddr = (ushort)(ipcDefines.mAdrDevStart);
			   int MsgByteOffset = 1;

			   try
			   {
				   for (int i = 0; i < devices; i++)
				   {
					   /*******  Get Device Config  ****/
					   cmd[0] = ipcCMD.GET;
					   cmd[1] = ipcCMD.getRunDevCfg;
					   cmd[2] = (byte)i; //device number
					   rxBF = client.berkeleySendMsg(cmd);

					   if (rxBF[0] == ipcCMD.SVAL)
					   {


						   /*****  Get Device AUTH CFG ******/

						   cmd[1] = ipcCMD.getAuthDevices;
						   devAuthBF = client.berkeleySendMsg(cmd);


						   /*****  Get GSM RCPT ******/

						   cmd[1] = ipcCMD.getGsmRecpCfg;
						   gsmRcpBF = client.berkeleySendMsg(cmd);
						   
						   deviceUploadStat = true;
								
						   //read device config
						   for (int j = 0; j < ipcDefines.deviceConfigSize; j++)
						   {
							   site.siteCfg.deviceConfigs[i].memCFG[j] = rxBF[j + MsgByteOffset];
						   }


						   //read names config

						   ///*****  Get Device Names ******/
						   cmd[1] = ipcCMD.getDeviceName;
						   for (int n = 0; n < ipcDefines.RAM_DEV_NAMES_NO; n++)
						   {

							   cmd[2] = (byte)n;
							   cmd[3] = (byte)rxBF[ipcDefines.mAdrDevID + 1]; //TODO 2 byte addressing
							   byte[] narr = client.berkeleySendMsg(cmd);
							   if (narr[ 0] == ipcCMD.SVAL)
							   {
								   for (int txtbyte = MsgByteOffset; txtbyte < ipcDefines.RAM_NAME_SIZE + MsgByteOffset; txtbyte++)
								   {
									   //if (narr[txtbyte] == (byte)32) //decode space sign
									   //{
									   //    narr[txtbyte] = 0;
									   //}
									   site.siteCfg.deviceConfigs[i].NamesCFG[n][txtbyte - MsgByteOffset] = narr[txtbyte]; //1 byte buffer offset
								   }
							   }
						   }



						   ///*****  Get Global Names ******/
						   cmd[1] = ipcCMD.getGlobalNames;
						   byte[] nresp = client.berkeleySendMsg(cmd);
						   if (nresp[0] == ipcCMD.SVAL)
						   {
							   for (int n = 0; n < ipcDefines.RAM_NAMES_Global_Total_Size; n++)
							   {
								   site.siteCfg.GlobalNameConfig[n] = nresp[n + MsgByteOffset];
							   }
						   }

						   
						   //read  schedule config
						   if (SchedBF[0] == ipcCMD.SVAL)
						   {
							   for (int schedule = 0; schedule < ipcDefines.RAM_DEV_SCHED_NO; schedule++)
							   {
								   for (int schedbyte = 0; schedbyte < ipcDefines.RAM_DEV_SCHED_MEM_SIZE; schedbyte++)
								   {
									   site.siteCfg.deviceConfigs[i].ScheduleCFG[schedule][schedbyte] = SchedBF[(schedule * ipcDefines.RAM_DEV_SCHED_MEM_SIZE) + schedbyte + MsgByteOffset]; //1 byte buffer offset
								   }
							   } 
						   }

						   //read GSM config
						   if (gsmRcpBF[0] == ipcCMD.SVAL)
						   {
							   try
							   {
								   int bufferOffset = 1;
								    ipcRcpt[] rcpts = new  ipcRcpt[ipcDefines.RAM_SMS_RECP_SIZE];
								   for (int r = 0; r < ipcDefines.RAM_SMS_RECP_SIZE; r++)
								   {
									   byte[] record = new byte[ipcDefines.RAM_SMS_RECP_SIZE];
									   for (int btc = 0; btc < ipcDefines.RAM_SMS_RECP_SIZE; btc++)
									   {
										   record[btc] = gsmRcpBF[bufferOffset + (r * ipcDefines.RAM_SMS_RECP_SIZE) + btc];
									   }
									    ipcRcpt rcpt = new  ipcRcpt(record);
									   rcpts[r] = rcpt;
								   }
								   site.siteCfg.gsmRcpts = rcpts;
							   }
							   catch (Exception e)
							   {
								   //TODO - buffer overflow
							   }

						   }

					   }
					   else
					   {
						   deviceUploadStat = false;
					   }
				   }

				   //get events
				   cmd[0] = ipcCMD.GET;
				   cmd[1] = ipcCMD.getEventNo;
				   rxBF = client.berkeleySendMsg(cmd);
									
				   cmd[1] = ipcCMD.getEvent;
				   cmd[2] = (byte)1; //test event id 1
				   rxBF = client.berkeleySendMsg(cmd);

				   site.siteStat.StopConnectionTimer();
				   
			   }
			   catch (Exception e)
			   {
				   site.siteStat.StopConnectionTimer();
				   site.siteStat.FailedConnections++;
				   client.CloseConnection();
				   return (bool)(deviceUploadStat & globalUploadStat);
				   throw;
			   }
			   client.CloseConnection();
			   return (bool)(deviceUploadStat & globalUploadStat);
	   }

		public bool  updateSiteConfig( sconnSite site) //read entire running config
			{
                return ReadSiteRunningConfigMin(site,true);
			}    
	}
}
