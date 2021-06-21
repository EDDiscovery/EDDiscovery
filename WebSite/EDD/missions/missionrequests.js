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


import { RemoveChildren } from "/jslib/elements.js"
import { TableRowMultitdlist } from "/jslib/tables.js"

export function RequestMissions(websocket, entry)
{
    var msg = {
        requesttype: "missions",
        entry: entry,	// -1 means send me the latest journal entry missions 
    };

    websocket.send(JSON.stringify(msg));
}

export function FillMissionsTable(jdata)
{
    var jtable = document.getElementById("Missions");
    RemoveChildren(jtable);

    var current = jdata.current;

    for (var i = 0; i < current.length; i++)
    {
        var obj = current[i];
        jtable.appendChild(TableRowMultitdlist(obj, "highlightentry"));
    }

    var previous = jdata.previous;

    for (var i = 0; i < previous.length; i++)
    {
        var obj = previous[i];
        jtable.appendChild(TableRowMultitdlist(obj));
    }

}
