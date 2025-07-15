/*
 * Copyright  2015 - 2025 EDDiscovery development team
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
 */

namespace EDDiscovery.Icons
{
    public static class ForceInclusion
    {
        public static void Include()         // if no direct references to the assembly is present, it gets optimised away.
        {
            //System.Diagnostics.Debug.WriteLine("Included EDDicons");
        }
    }
}
