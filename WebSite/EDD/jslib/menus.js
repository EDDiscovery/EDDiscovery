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


// make a menu, attached and displayed under button, with id menuname, class menuclass, and the menulist
// menulist is in the format of an variable length array containing array elements
//
// structure needed is:
//              <div class="menubutton" id="menubutton1">
//                    <img id="menuicon_button" src="/Images/menu.png" alt="Tab Menu" onclick="ToggleMenu('firstmenu')">
//                    <script>
//                        WriteMenu("menubutton1", "firstmenu", "navmenu", [["checkbox","e1","one", menuchange, false], [null,"e2","two", menuchange], ["checkbox","e3","three", menuchange, true]]);
//                    </script>
//                </div>

// menubutton: display:inline, position:relative
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

// menu div: is called id=menunameid class=menuclass
// WriteMenu menulist should be formatted as:
//           ["checkbox", "moon", "Show Moons", scandisplaychange, true],           - tag contains storagekey 
//           ["radio", "s16", "16", sizedisplaychange, "sizegroup", "s16"],         - item tag contains [storagekey (item[4]),value to set]. First entry needs to have the default value
//           ["radio", "s32", "32", sizedisplaychange, "sizegroup"],
//           ["submenu", "size", "Set Size", "submenusize"],
//           ["normal", "click", "Click here", functocall],
//
//          each menu item is enclosed in a div of class = menuclass + "_item" id = menunameid + "_" + itemid + "_item"
//          type            input id                                        input class                     id_label                        class_label             storage
//          checkbox        menunameid+"_"+id+"_checkbox                    menuclass_checkbox              menunameid+"_"+id+"_label       menuclass_label         menunameid.id.checkboxstate
//          radio           menunameid+"_"+id+"_radio                       menuclass_radio                 menunameid+"_"+id+"_label       menuclass_label         item[4] in entry
//          normal                                                                                          menunameid+"_"+id+"_label       menuclass_label
//          submenu                                                                                         menunameid+"_"+id+"_submenu     menuclass_submenu

function WriteMenu(posid, menunameid, menuclass, menulist)
{
    var div = CreateDiv(menuclass, menunameid);
    var storage = window.localStorage;

    menulist.forEach(function (item) 
    {
        var id = item[1];
        var mid = menunameid + "_" + id;

        var itemdiv = CreateDiv(menuclass + "_item",  mid + "_item");

        if (item[0] == "checkbox" )
        {
            var storagekey = menunameid + "." + id + ".checkboxstate";
            var state = FetchState(storagekey, item[4], true);

            console.log("Checkbox Storage " + storagekey + " state " + state);

            if (state == null)
            {
                state = item[4];
                //console.log("default set " + id + " to " + state);
            }

            var mi = CreateInput(menuclass + "_checkbox", mid + "_checkbox", item[0], item[3], state, storagekey);
            var lb = CreateLabel(menuclass + "_label", mid + "_label", mid + "_checkbox", item[2]);
            itemdiv.appendChild(mi);
            itemdiv.appendChild(lb);
        }
        else if (item[0] == "radio")
        {
            var storagekey = menunameid + "." + item[4] + ".radiostate";        // first entry should have item[5], default state
            var state = FetchState(storagekey, item.length >= 6 ? item[5] : null, true);

            console.log("Radio Storage " + storagekey + " state " + state);

            var radiostate = state == id;
            var mi = CreateInput(menuclass + "_radio", mid + "_radio", item[0], item[3], radiostate, [storagekey,id], item[4]);
            var lb = CreateLabel(menuclass + "_label", mid + "_label", mid + "_radio", item[2]);
            itemdiv.appendChild(mi);
            itemdiv.appendChild(lb);
        }
        else if (item[0] == "submenu")
        {
            var lb = CreateLabel(menuclass + "_submenu", mid + "_submenu", null, item[2],OpenSubMenu, item[3]);
            lb.style.padding = "0px 0px 0px 20px";
            itemdiv.appendChild(lb);
        } 
        else
        {
            var lb = CreateLabel(menuclass + "_label", mid + "_label", null, item[2], item[3]);
            lb.style.padding = "0px 0px 0px 20px";
            itemdiv.appendChild(lb);
        }
        div.appendChild(itemdiv);
        div.appendChild(CreateBreak());
    });

    // attach after item used a position anchor
    var posid = document.getElementById(posid);
    posid.append(div);
}

function GetMenuItemCheckState(menuid, itemid)
{
    var storagekey = menuid + "." + itemid + ".checkboxstate";
    var jstate = window.localStorage.getItem(storagekey);           // state is stored in JSON, when we parse, we get back a boolean
    var state = JSON.parse(jstate);
    return state;
}

var menusopen = []       // remember menu open, array for submenus

function ToggleMenu(id)
{
    if (menusopen.length > 0)
        CloseMenus();
    else
        OpenMenu(id);
}

function OpenMenu(id)
{
    CloseMenus();

    var menu = document.getElementById(id);
    if (menu != null)
    {
        menu.style.display = "inline-block";
        menusopen.push(id);
        console.log("Menu " + id + " open");
    }
    else
        console.log("ERROR: No such Menu " + id);
}

function CloseMenus()
{
    menusopen.forEach(function (x)
    {
        var menu = document.getElementById(x);
        if (menu != null)
        {
            menu.style.display = "";
            console.log("Close " + x);
        }
    });

    menusopen = []
}


function OpenSubMenu(mouseevent)
{
    var ct = mouseevent.currentTarget;
    var openingmenu = ct.parentNode.parentNode;
    var submenu = ct.tag;
    console.log("MI " + ct.id + " open " + submenu + " from menu " + openingmenu.id);

    var menu = document.getElementById(submenu);
    if (menu != null)
    {
        var ctbounds = ct.getBoundingClientRect();        
        var menubounds = openingmenu.getBoundingClientRect();
        menu.style.display = "inline-block";
        menu.style.left = menubounds.right + "px";
        menu.style.top = ctbounds.top + "px";
        menusopen.push(submenu);
        console.log("SubMenu " + submenu + " open");
    }
    else
        console.log("ERROR: No such Menu " + id);
} 

