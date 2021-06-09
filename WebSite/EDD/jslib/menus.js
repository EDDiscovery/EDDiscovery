/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

// make a menu, attached and displayed under button, with id menuname, class menuclass, and the menulist
// menulist is in the format of an array
// elements are: 
// menuclass should be absolute positioning to be placed under button

function writemenu(button, menuname, menuclass, menulist)
{
    var div = CreateDiv(menuclass, menuname);

    var  i = 0;
    menulist.forEach(function (item) 
    {
        var chk = item[1];
        var logicalname = menuname + i;
        var labellogicalname = menuname + "L" + i;
        i++;

        var p = CreatePara(null);

        if (chk == true || chk == false)
        {
            var mi = CreateInput(logicalname, "checkbox", item[2], item[1], item[3]);
            var lb = CreateLabel(labellogicalname, logicalname, item[0]);
            p.appendChild(mi);
            p.appendChild(lb);
        }
        else
        {
            var lb = CreateLabel(labellogicalname, null, item[0], item[2], item[3]);
            lb.style.padding = "0px 0px 0px 20px";
            p.appendChild(lb);
        }

        div.appendChild(p);
    });

    // grab the button, then grab the parent of the button, and append underneath it.  Don't append to button (as it will be inside button and constrained by it)
    var button = document.getElementById(button);
    var bp = button.parentElement;

    bp.appendChild(div);
    //button.appendChild(div);
}



// menu is presumed styled with display:none
function togglemenu(id)
{
    var menu = document.getElementById(id);

    if (menu.style.display == "")
    {
        menu.style.display = "inline-block";
    }
    else
    {
        menu.style.display = "";
    }
    console.log("Menu " + id + " toggle " + menu.style.display );
}

// menu is presumed styles with display:none
function closemenu(id)
{
    var menu = document.getElementById(id);

    if (menu.style.display == "inline-block")
    {
        menu.style.display = "";
    }
    console.log("Menu " + id + " close");
}
