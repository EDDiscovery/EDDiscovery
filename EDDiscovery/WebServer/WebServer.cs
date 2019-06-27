/*
 * Copyright © 2019 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.WebServer
{
    public class WebServer
    {
        public int Port { get { return port; } set { port = value; } }
        private int port = 0;
        private EDDiscoveryForm discoveryform;

        public WebServer( EDDiscoveryForm frm )
        {
            discoveryform = frm;
        }

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }


    }
}
