﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace sconnConnector.POCO.Config.sconn.User
{


    public class sconnAlarmUserPermission
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    public class sconnAlarmSystemUser : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
            [Required]
            public int Id { get; set; }

            [Required]
            public int DomainId { get; set; }
        
            [Required]
            [DisplayName("Code")]
            public string Code { get; set; }

            [Required]
            [DisplayName("Card")]
            public string Card { get; set; }

            [Required]
            [DisplayName("Enabled")]
            public bool Enabled { get; set; }
        
            [Required]
            [DisplayName("Allowed Until")]
            public DateTime AllowedUntil { get; set; }

            [Required]
            [DisplayName("Permissions")]
            public ObservableCollection<sconnAlarmUserPermission> Permissions { get; set; }


            public int Value { get; set; }

            private static Logger _logger = LogManager.GetCurrentClassLogger();

            public sconnAlarmSystemUser()
            {
                UUID = Guid.NewGuid().ToString();
            }

            public sconnAlarmSystemUser(byte[] serialized) : this()
            {
                this.Deserialize(serialized);
            }

            private void SerializePermissions(byte[] buffer)
            {
            
            }

            private string GetPermissionNameForIndex(int index)
            {
                return "";  //TODO
            }

            private void DeserializePermissions(byte[] config)
            {
                BitArray b = new BitArray(config);
                for (int i = ipcDefines.USER_DB_UPERM_POS; i < (ipcDefines.USER_DB_UPERM_POS + ipcDefines.USER_DB_UPERM_LEN*8); i++)
                {
                    sconnAlarmUserPermission perm = new sconnAlarmUserPermission {Name = GetPermissionNameForIndex(i)};
                    if (b.Get(i))
                    {
                        perm.Enabled = true;
                    }
                    perm.Id = i;

                    this.Permissions.Add(perm);
                }
        }

            public byte[] Serialize()
            {
                try
                {
                    byte[] buffer = new byte[ipcDefines.USER_DB_USER_RECORD_LEN];

                    SerializePermissions(buffer); //buffer[ipcDefines.USER_DB_UPERM_POS+1] = (byte)Permissions;

                    buffer[ipcDefines.USER_DB_ENABLED_POS] = (byte)(Enabled ? 1 : 0);
                    byte[] passB = Encoding.ASCII.GetBytes(Code);   //only numeric ascii code
                    int passwdBytes = passB.Length > ipcDefines.USER_DB_CODE_LEN ? ipcDefines.USER_DB_CODE_LEN : passB.Length;
                    buffer[ipcDefines.USER_DB_CODESIZE_POS] = (byte) passwdBytes;
                    for (int i = 0; i < passwdBytes; i++)
                    {
                        buffer[ipcDefines.USER_DB_CODE_POS + i] = (byte)passB[i];
                    }
                    for (int i = passwdBytes; i < ipcDefines.USER_DB_CODE_LEN; i++)
                    {
                        buffer[ipcDefines.USER_DB_CODE_POS + i] = 0x00;    //clear remaning bytes
                    }

                    byte[] logB = Encoding.ASCII.GetBytes(Card);
                    int loginBytes = logB.Length > ipcDefines.USER_DB_CARD_LEN ? ipcDefines.USER_DB_CARD_LEN : logB.Length;
                    for (int i = 0; i < loginBytes; i++)
                    {
                        buffer[ipcDefines.USER_DB_CARD_POS + i] = (byte)logB[i];
                    }
                    for (int i = loginBytes; i < ipcDefines.USER_DB_CARD_LEN; i++)
                    {
                        buffer[ipcDefines.USER_DB_CARD_POS + i] = 0x00;    //clear remaning bytes
                    }

                    return buffer;
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    return null;
                }

            }

            public void Deserialize(byte[] buffer)
            {
                try
                {
                    if (buffer.Length >= ipcDefines.USER_DB_USER_RECORD_LEN)
                {
                        DeserializePermissions(buffer);   // Permissions = buffer[ipcDefines.USER_DB_UPERM_POS+1];

                        Enabled = buffer[ipcDefines.USER_DB_ENABLED_POS] > 0;

                        byte passLen = buffer[ipcDefines.USER_DB_CODESIZE_POS];
                        if (passLen <= ipcDefines.USER_DB_CODE_LEN)
                        {
                            byte[] passwdBf = new byte[ipcDefines.USER_DB_CODE_LEN];
                            for (int i = 0; i < passLen; i++)
                            {
                                passwdBf[i] = buffer[ipcDefines.USER_DB_CODE_POS + i];
                            }
                            Code = Encoding.ASCII.GetString(passwdBf);
                        }


                        byte logLen = ipcDefines.USER_DB_CARD_LEN;  //[ipcDefines.AUTH_RECORD_LOGIN_LEN];
                        if (logLen <= ipcDefines.USER_DB_CARD_LEN)
                        {
                            byte[] logBf = new byte[ipcDefines.USER_DB_CARD_LEN];
                            for (int i = 0; i < logLen; i++)
                            {
                                logBf[i] = buffer[ipcDefines.USER_DB_CARD_POS + i];
                            }
                            Card = Encoding.ASCII.GetString(logBf);
                        }

                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                }
            }

            public void Fake()
            {
                try
                {
                    this.Id = 0;
                    this.Enabled = true;
                    this.AllowedUntil = DateTime.MaxValue;
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                }


            }

            public string UUID { get; set; }

        }
    
}
