/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

// Tables

function removeChildren(tab)
{
    while(tab.hasChildNodes())
    {
        tab.removeChild(tab.firstChild);
    }
}

function tablerow2tdjson(jdata, text, prop, prop2 = "")  // write a table row with two cells from jdata of properties
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

function tabledata(child)  // wrap a child in a td.
{
    var td1 = document.createElement("td");
    td1.appendChild(child);
    return td1;
}

function tablerow2tdstring(text,value)  // write a table row from string
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

function tablerow2tdanchor(text,value,link)  // write a table row with a link
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

function tablerow1tdlist(elements, classname="", colspan = null) // single row containing multiple elements and assign a classname to them for styling
{
    var tr = document.createElement("tr");
    var td = document.createElement("td");
    tr.appendChild(td);

    elements.forEach( function(item) 
    {
        if (item != null)
        {
            if (classname !== "")
                item.classList.add(classname);

            td.appendChild(item);
        }
    });

    if ( colspan !== null )
        td.colSpan = colspan;
    return tr;
}

function tablerowmultitdlist(elements, classnames = null) // single col containing elements and assign a classname to them for styling
{
    var tr = document.createElement("tr");

    for( var i = 0 ; i < elements.length; i++)
    {
        if (elements[i] != null)
        {
            var td = document.createElement("td");
            tr.appendChild(td);

            if (classnames !== null)
            {
                elements[i].classList.add(classnames[i]);
            }

            td.appendChild(elements[i]);
        }
    }

    return tr;
}



