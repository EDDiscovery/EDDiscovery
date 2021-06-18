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


export function CreatePara(text)
{
    var a2 = document.createElement("p");
    a2.innerHTML = text;
    return a2;
}

export function CreateElement(type,text)
{
    var a2 = document.createElement(type);
    a2.innerHTML = text;
    return a2;
}

export function CreateAnchor(text, link, newtab = false, clsname = null)
{
    var a2 = document.createElement("a");
    a2.href = link;
    if (newtab)
        a2.target = "_blank";

    var node = document.createTextNode(text)

    a2.appendChild(node);
    if (clsname != null)
        a2.classList.add(clsname);
    return a2;
}

// see tooltip.css. Tooltips activate on hover. they need a div with class tooltip, which is display:inline (so it does not interrupt the flow)
// then they need a inner space, class tooltiptext, which, is normally hidden, position absolute,
// and another class : .tooltip:hover .tooltiptext, which activates on hover, and overrides the visibilty. 
// item is the thing your attaching the tooltip to.

export function CreateTooltip(item, tooltiptext)
{
    var div = document.createElement("div");
    div.className = "tooltip";
    div.appendChild(item);
    var span = document.createElement("span");
    span.className = "tooltiptext";
    span.innerText = tooltiptext;
    div.appendChild(span);
    return div;
}


export function CreateImage(link,alt,width = null,callback = null, tagdata = null, tooltip = null, id = null)
{
    var a = document.createElement("img");
    a.src = link;
    a.alt = alt;

    if (width != null)
        a.width = width;

    if (tagdata != null)
        a.tag = tagdata;

    if (callback !== null)
        a.onclick = callback;

    if (id !== null)
        a.id = id;

    if (tooltip == null)
        return a;
    else
        return CreateTooltip(a,tooltip)
}

export function CreateButton(text,handler)
{
    var a = document.createElement("button");
    a.onclick = handler;
    a.innerHTML = text;
    return a;
}

export function InsertAfter(elem,entity)
{
    elem.parentNode.insertBefore( entity,elem.nextSibling)
}

export function AppendParaToElement(message, elementname)
{
    var pre = document.createElement("p");
    pre.style.wordWrap = "break-word";
    pre.innerHTML = message;
    output2 = document.getElementById(elementname);
    output2.appendChild(pre);
}

export function CreateInput(classname, id, type, onclick, checked = null, tag = null, name = null)
{
    var pre = document.createElement("input");
    if (classname != null)
        pre.className = classname;
    if (id != null)
        pre.id = id;
    if (name != null)
        pre.name = name;
    pre.type = type;
    pre.onclick = onclick;
    if (checked == true)
    {
        //console.log("Program " + id + " check on");
        pre.checked = true;
    }
    pre.tag = tag;
    //console.log("Input " + id + " " + type + " checkedit " + checked +  " got " + pre.checked);
    return pre;
}

export function CreateLabel(classname, id, forname, text, onclick = null, tag = null)
{
    var pre = document.createElement("label");
    if (classname != null)
        pre.className = classname;
    if (id != null)
        pre.id = id;
    if ( forname != null )
        pre.htmlFor = forname;
    pre.innerHTML = text;
    pre.onclick = onclick;
    pre.tag = tag;
   // console.log("Label " + id + " for " + forname + " text " + text);
    return pre;
} 

export function CreateDiv(classname = null, id = null) 
{
    var pre = document.createElement("div");
    if ( classname != null )
        pre.className = classname;
    if ( id != null )
        pre.id = id;
    return pre;
}

export function CreateBreak() 
{
    var pre = document.createElement("br");
    return pre;
}

export function RemoveChildren(tab)
{
    while (tab.hasChildNodes())
    {
        tab.removeChild(tab.firstChild);
    }
}

