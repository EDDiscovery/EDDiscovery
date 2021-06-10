/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

// make a menu, attached and displayed under button, with id menuname, class menuclass, and the menulist
// menulist is in the format of an variable length array containing array elements
// elements are: type (checkbox or null), id, text, function call, [checkbox default state]

// structure needed is:
//              <div class="menubutton">
//                    <img id="menuicon_button" src="/Images/menu.png" alt="Tab Menu" onclick="togglemenu('firstmenu')">
//                    <script>
//                        writemenu("menuicon_button", "firstmenu", "navmenu", [["checkbox","e1","one", menuchange, false], [null,"e2","two", menuchange], ["checkbox","e3","three", menuchange, true]]);
//                    </script>
//                </div>

// menuclass (navmenu above) should be display none, absolute positioning, with top/left offset so it can be placed under the button
//.navmenu {
//    display: none;
//    position: absolute;
//    top: 24px;      /* relative positioning from nav div */
//    left: 0px;
//    min - width: 100px;
//    background - color: darkorange;
//    color: black;
//    padding: 2px 10px 2px 4px;
//    z - index: 1;
//}

// menubutton itself can be styles as you see fit, but its needs to be position relative:
//.menubutton {
//    /* div needs to be inline so it flows against the ul's */
//    display: inline;
//    /* making it relative allows it to offset from the top of the nav bar and for the child to position absolutely inside the div */
//    position: relative;
//}

// writemenu names parts it makes as follows:
//      menu div: is called menunameid with class menuclass
//          each menu item is enclosed in a div of class = menuclass+"_item" id = itemid + "_item"
//          the label of a checkbox is id + "_label"
//          the menuclass of each label menuclass + "_label"
//          the id of each input is menuclass + "_input"

// checkbox states are looked up from localstorage using the menuclass.itemid.checkboxstate key

function writemenu(buttonid, menunameid, menuclass, menulist)
{
    var div = CreateDiv(menuclass, menunameid);
    var storage = window.localStorage;

    menulist.forEach(function (item) 
    {
        var id = item[1];

        var itemdiv = CreateDiv(menunameid + "_item",id + "_item");

        if (item[0] == "checkbox")
        {
            var storagekey = menunameid + "." + id + ".checkboxstate";
            var state = fetchstate(storagekey, item[4], true);

            console.log("Storage " + storagekey + " state " + state);

            if (state == null)
            {
                state = item[4];
                //console.log("default set " + id + " to " + state);
            }

            var mi = CreateInput(menuclass + "_input", id, item[0], item[3], state, storagekey);
            var lb = CreateLabel(menuclass + "_label", id + "_label", id, item[2]);
            itemdiv.appendChild(mi);
            itemdiv.appendChild(lb);
        }
        else
        {
            var lb = CreateLabel(menuclass + "_label", id, null, item[2], item[3]);
            lb.style.padding = "0px 0px 0px 20px";
            itemdiv.appendChild(lb);
        }
        div.appendChild(itemdiv);
        div.appendChild(CreateBreak());
    });

    // grab the button, then grab the parent of the button, and append underneath it.  Don't append to button (as it will be inside button and constrained by it)
    var buttonid = document.getElementById(buttonid);
    var bp = buttonid.parentElement;
    bp.append(div);
}

function getmenuitemcheckstate(menuid, itemid)
{
    var storagekey = menuid + "." + itemid + ".checkboxstate";
    var jstate = window.localStorage.getItem(storagekey);           // state is stored in JSON, when we parse, we get back a boolean
    var state = JSON.parse(jstate);
    return state;
}


var menuopen = ""       // remember menu open

function togglemenu(id)
{
    if (menuopen == id)
        closemenu(id);
    else
        openmenu(id);
}

function openmenu(id)
{
    if (menuopen != "" && menuopen != id)
        closemenu(menuopen);

    var menu = document.getElementById(id);
    if (menu != null)
    {
        menu.style.display = "inline-block";
        menuopen = id;
        console.log("Menu " + id + " open");
    }
    else
        console.log("ERROR: No such Menu " + id);
}

function closemenu(id)
{
    var menu = document.getElementById(id);

    if (menu != null)
    {
        menu.style.display = "";
        menuopen = "";
        console.log("Menu " + id + " close");
    }
    else
        console.log("ERROR: No such Menu " + id);
}

function closeallmenus()
{
    if (menuopen != "")
        closemenu(menuopen);
}
