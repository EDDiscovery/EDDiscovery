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

import { CreateImage, RemoveChildren } from "/jslib/elements.js"
import { TableRowMultitdlist } from "/jslib/tables.js"

var journalnextrow = -9999;     // meaning, no load

export function ClearJournalTable()
{
    var jtable = document.getElementById("Journal");
    RemoveChildren(jtable);
}

export function FillJournalTable(jdata, insert, clickrequest)
{  
    var jtable = document.getElementById("Journal");

    var firstrow = jdata.firstrow;

    if (firstrow >= 0)
    {
        var rows = jdata.rows;
        for (var i = 0; i < rows.length; i++)
        {
            var rowno = firstrow - i;

            var obj = rows[i];
            var image = CreateImage("journalicons/" + obj[0] + ".png", obj[1], 24, clickrequest, firstrow - i);
            var t1 = document.createTextNode(obj[1]);
            var t2 = document.createTextNode(obj[2]);
            //var t3 = document.createTextNode(rowno + " " + obj[3]);
            var t3 = document.createTextNode(obj[3]);
            var t4 = document.createTextNode(obj[4]);

            if (insert)
                jtable.insertBefore(TableRowMultitdlist([image, t1, t2, t3, t4]), jtable.firstChild);
            else
                jtable.appendChild(TableRowMultitdlist([image, t1, t2, t3, t4]));
        }

        var nextrow = firstrow - rows.length;
        if (journalnextrow == -9999 || nextrow < journalnextrow)
            journalnextrow = nextrow;

        console.log("JLog " + firstrow + " count " + rows.length + " Next is " + journalnextrow);
    }
    else
    {
        jtable.appendChild(TableRowMultitdlist([document.createTextNode("No Data")]));
        console.log("No data");
    }
}


export function RequestMore(websocket, count)
{
    if (journalnextrow >= 0)
        RequestJournal(websocket, journalnextrow, count);
}

export function JournalScrolled(websocket, journalscroll)
{
    console.log(journalscroll.scrollTop + " " + journalscroll.scrollHeight + journalscroll.clientHeight);

    if (journalnextrow >= 0 && journalscroll.scrollTop + journalscroll.clientHeight >= journalscroll.scrollHeight - 10)
    {
        RequestJournal(websocket,journalnextrow, 50);
    }
}

export function RequestJournal(websocket,start, len)
{
    var msg = {
        requesttype: "journal",
        start: start,	// -1 means send me the latest journal entry first, followed by length others.  else its the starting journal index (always latest followed by older)
        length: len
    };

    websocket.send(JSON.stringify(msg));
}

