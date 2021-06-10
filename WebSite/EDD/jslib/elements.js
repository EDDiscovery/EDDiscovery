/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */


function CreatePara(text)
{
    var a2 = document.createElement("p");
    a2.innerHTML = text;
    return a2;
}

function CreateElement(type,text)
{
    var a2 = document.createElement(type);
    a2.innerHTML = text;
    return a2;
}

function CreateAnchor(text, link, newtab = false, clsname = null)
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

function CreateImage(link,alt,width,callback = null, tagdata = null, tooltip = "fred")
{
    var a = document.createElement("img");
    a.src = link;
    a.alt = alt;
    a.width = width;

    if (tagdata != null)
        a.tag = tagdata;

    if ( callback !== null )
        a.onclick = callback;

    if (tooltip == null)
        return a;
    else
    {
        var div = document.createElement("div");
        div.className = "tooltip";
        div.appendChild(a);
        var span = document.createElement("span");
        span.className = "tooltiptext";
        span.innerText = tooltip;
        //span.appendChild(CreatePara(tooltip));
        div.appendChild(span);
        return div;
    }
    
  //  console.log("Create image s:" + a.src + " t:" + a.tag );
}

function CreateButton(text,handler)
{
    var a = document.createElement("button");
    a.onclick = handler;
    a.innerHTML = text;
    return a;
}

function InsertAfter(elem,entity)
{
    elem.parentNode.insertBefore( entity,elem.nextSibling)
}

function AppendParaToElement(message, elementname)
{
    var pre = document.createElement("p");
    pre.style.wordWrap = "break-word";
    pre.innerHTML = message;
    output2 = document.getElementById(elementname);
    output2.appendChild(pre);
}

function CreateInput(classname, id, type, onclick, checked = null, tag = null)
{
    var pre = document.createElement("input");
    if (classname != null)
        pre.className = classname;
    if (id != null)
        pre.id = id;
    pre.type = type;
    pre.onclick = onclick;
    if (checked == true)
    {
        console.log("Program " + id + " check on");
        pre.checked = true;
    }
    pre.tag = tag;
    console.log("Input " + id + " " + type + " checkedit " + checked +  " got " + pre.checked);
    return pre;
}

function CreateLabel(classname, id, forname, text, onclick = null, tag = null)
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

function CreateDiv(classname = null, id = null) 
{
    var pre = document.createElement("div");
    if ( classname != null )
        pre.className = classname;
    if ( id != null )
        pre.id = id;
    return pre;
}

function CreateBreak() 
{
    var pre = document.createElement("br");
    return pre;
}
