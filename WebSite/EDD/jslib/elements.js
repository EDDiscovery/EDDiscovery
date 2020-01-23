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

function CreateAnchor(text, link, newtab = false)
{
    var a2 = document.createElement("a");
    a2.href = link;
    if (newtab)
        a2.target = "_blank";
    a2.appendChild(document.createTextNode(text));
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