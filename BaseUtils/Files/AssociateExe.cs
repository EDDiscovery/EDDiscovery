/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public static class AssociateExe
    {
        public static string AssocQueryString(BaseUtils.Win32.UnsafeNativeMethods.AssocStr association, string extension)
        {
            const int S_OK = 0;
            const int S_FALSE = 1;

            uint length = 0;
            uint ret = BaseUtils.Win32.UnsafeNativeMethods.AssocQueryString(BaseUtils.Win32.UnsafeNativeMethods.AssocF.None, association, extension, null, null, ref length);
            if (ret != S_FALSE)
            {
                throw new InvalidOperationException("Could not determine associated string");
            }

            var sb = new StringBuilder((int)length); // (length-1) will probably work too as the marshaller adds null termination
            ret = BaseUtils.Win32.UnsafeNativeMethods.AssocQueryString(BaseUtils.Win32.UnsafeNativeMethods.AssocF.None, association, extension, null, sb, ref length);
            if (ret != S_OK)
            {
                throw new InvalidOperationException("Could not determine associated string");
            }

            return sb.ToString();
        }
    }
}
