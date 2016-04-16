/*
Copyright (c) 2013, 2014 Paolo Patierno

All rights reserved. This program and the accompanying materials
are made available under the terms of the Eclipse Public License v1.0
and Eclipse Distribution License v1.0 which accompany this distribution. 

The Eclipse Public License is available at 
   http://www.eclipse.org/legal/epl-v10.html
and the Eclipse Distribution License is available at 
   http://www.eclipse.org/org/documents/edl-v10.php.

Contributors:
   Paolo Patierno - initial API and implementation and/or initial documentation
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uPLibrary.Networking.M2Mqtt.Managers
{
    /// <summary>
    /// Delegate for executing user authentication
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <returns></returns>
    public delegate bool MqttUserAuthenticationDelegate(string username, string password);


    /// <summary>
    /// Delegate for executing user authentication in domain
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <returns></returns>
    public delegate bool MqttUserDomainAuthenticationDelegate(string domainname, string username, string password);


    /// <summary>
    /// Manager for User Access Control
    /// </summary>
    public class MqttUacManager
    {
        // user authentication delegate
        private MqttUserAuthenticationDelegate userAuth;

        // user authentication delegate
        private MqttUserDomainAuthenticationDelegate userDomainAuth;

        /// <summary>
        /// User authentication method
        /// </summary>
        public MqttUserAuthenticationDelegate UserAuth
        {
            get { return this.userAuth; }
            set { this.userAuth = value; }
        }


        /// <summary>
        /// User domain authentication method
        /// </summary>
        public MqttUserDomainAuthenticationDelegate UserDomainAuth
        {
            get { return this.userDomainAuth; }
            set { this.userDomainAuth = value; }
        }


        /// <summary>
        /// Execute user authentication
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Access granted or not</returns>
        public bool UserAuthentication(string username, string password)
        {
            if (this.userAuth == null)
                return true;
            else
                return this.userAuth(username, password);
        }



        /// <summary>
        /// Execute user domain authentication
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="domain">domain</param>
        /// <returns>Access granted or not</returns>
        public bool UserDomainAuthentication(string domain, string username, string password)
        {
            if (this.userAuth == null)
                return true;
            else
                return this.userDomainAuth(domain, username, password);
        }


    }
}
