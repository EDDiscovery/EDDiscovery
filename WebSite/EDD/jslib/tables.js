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

// Tables

// write a table row with two cells from jdata of properties

export function TableRow2tdjson(jdata, text, prop, prop2 = "")  
{
    var td1 = document.createElement("td");
    var t1 = document.createTextNode(text);
    td1.appendChild(t1);

    var value = (prop2 !== "") ? jdata[prop][prop2] : jdata[prop];

    var td2 = document.createElement("td");
    var t2 = document.createTextNode(value);
    td2.appendChild(t2);

    var tr = document.createElement("tr");
    tr.appendChild(td1);
    tr.appendChild(td2);
    return tr;
}

// write a table row with col1 being text, col2 being a list of html items
export function TableRow2tdtextitem(text, elements)  
{
    var td1 = document.createElement("td");
    var t1 = document.createTextNode(text);
    td1.appendChild(t1);

    var td2 = document.createElement("td");

    elements.forEach(function (htmlitem)
    { if ( htmlitem!=null) td2.appendChild(htmlitem) });

    var tr = document.createElement("tr");
    tr.appendChild(td1);
    tr.appendChild(td2);
    return tr;
}

// wrap a child in a td.
export function TableData(child)  
{
    var td1 = document.createElement("td");
    td1.appendChild(child);
    return td1;
}

// write a table row from string
export function TableRow2tdstring(text,value)  
{
    var td1 = document.createElement("td");
    var t1 = document.createTextNode(text);
    td1.appendChild(t1);
    
    var td2 = document.createElement("td");
    var t2 = document.createTextNode(value);
    td2.appendChild(t2);
    
    var tr = document.createElement("tr");
    tr.appendChild(td1);
    tr.appendChild(td2);
    return tr;
}

// write a table row with a link
export function TableRow2tdanchor(text,value,link)  
{
    var td1 = document.createElement("td");
    var t1 = document.createTextNode(text);
    td1.appendChild(t1);
    
    var td2 = document.createElement("td");
    var a2 = document.createElement("a");
    td2.appendChild(a2);
    a2.href = link;
    a2.appendChild( document.createTextNode(value));
    
    var tr = document.createElement("tr");
    tr.appendChild(td1);
    tr.appendChild(td2);
    return tr;
}

// single row containing multiple columns
// elements are an array of items for each cell
// items are either a single item or an array of items.
// an item can be a string, or a node object. If strings, they are BR spaced from the above

// optionally if tdclassname == string, assign this style class to all td's.
// optionally if tdclassname == array, assign each element in turn style class (can be shorter)

export function TableRowMultitdlist(elements, tdclassnames = null)
{
    var tr = document.createElement("tr");

    for (var i = 0; i < elements.length; i++)
    {
        if (elements[i] != null)
        {
            var td = document.createElement("td");
            tr.appendChild(td);

            if (tdclassnames != null)
            {
                if (typeof (tdclassnames) == "string")
                    td.classList.add(tdclassnames);
                else if (i < tdclassnames.length)
                    td.classList.add(tdclassnames[i]);
            }

            var e = elements[i];

            if (Array.isArray(e))
            {
                var doneone = false;

                for (var j = 0; j < e.length; j++)
                {
                    if (typeof (e[j]) === 'string')
                    {
                        if (e[j] != "")
                        {
                            if (doneone)
                                td.appendChild(document.createElement("br"));
                            var te = document.createTextNode(e[j]);
                            td.appendChild(te);
                            doneone = true;
                        }
                    }
                    else if (e[j] != null)
                    {
                        td.appendChild(e[j]);
                        doneone = true;
                    }
                }
            }
            else
            {
                var tt = typeof (e);
                if (typeof (e) === 'object')
                {
                    td.appendChild(e);
                }
                else
                {
                    var te = document.createTextNode(e);
                    td.appendChild(te);
                }
            } 
        }
    }

    return tr;
}
