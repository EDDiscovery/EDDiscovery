/*
 * Copyright 2021-2021 Robbyxp1 @ github.com
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

export function WSURIFromLocation()
{
    var loc = window.location;
    var new_uri;

    if (loc.protocol === "https:")
    {
        new_uri = "wss:";
    }
    else
    {
        new_uri = "ws:";
    }

    new_uri += "//" + loc.host + "/";
    return new_uri;
}

