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

import { CreateDiv, CreateInput, CreateLabel, CreateBreak } from "/jslib/elements.js"
import { FetchState } from "/jslib/localstorage.js"

// make a menu, attached and displayed under button, with id menuname, class menuclass, and the menulist
// menulist is in the format of an variable length array containing array elements
//
// normal structure needed is:
//              <div>
//                    <img id="menuicon_button" src="/Images/menu.png" alt="Tab Menu" onclick="ToggleMenu('firstmenu')">
//                    <script>
//                        WriteMenu(..)
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
// appended to the element given in para 1
// menulist contains an array of arrays. item[0] in the inner array denoted menu type
//  ["checkbox", "moon", "Show Moons", scandisplaychange, true],           
//      item[1] is menu item name, item[2] is text, item[3] is callback, item[4] is default state
//      tag on menu menu contains storagekey = menunameid.itemname.checkboxstate
//      input id = menunameid+"_"+itemname+"_checkbox"
//      input class = menuclass + "_checkbox"
//      label id = menunameid + "_" + id + "_label"
//      label class = menuclass + "_label"
//   ["radio", "s16", "16", sizedisplaychange, "sizegroup", "s16"],
//      item[1] is menu item name, item[2] is text, item[3] is callback, item[4] is radio group, item[5] (only on first item) is default state
//      tag on menu item contains [storagekey, value to set]. storagekey = menunameid.itemname.radiostate
//      input id = menunameid+"_"+itemname+"_radio"
//      input class = menuclass+"_radio"
//      label id = menunameid + "_" + id + "_label"
//      label class = menuclass + "_label"
//    ["submenu", "size", "Set Size", "submenusize"],
//      item[1] is menu item name, item[2] is text, item[3] is submenu name
//      label id = menunameid + "_" + id + "_submenu"
//      label class = menuclass + "_submenu"
//    ["button", "click", "Click here", functocall],
//      item[1] is menu item name, item[2] is text, item[3] is callback
//      label id = menunameid + "_" + id + "_button"
//      label class = menuclass + "_button"

export function WriteMenu(appendto, menunameid, menuclass, menulist)
{
    var div = CreateDiv(menuclass, menunameid);

    menulist.forEach(function (item) 
    {
        var id = item[1];
        var mid = menunameid + "_" + id;

        var itemdiv = CreateDiv(menuclass + "_item",  mid + "_item");

        if (item[0] == "checkbox")
        {
            var storagekey = menunameid + "." + id + ".checkboxstate";
            var state = FetchState(storagekey, item[4], true);     // item[4] is bool

            //console.log("Checkbox Storage " + storagekey + " state " + state);

            if (state == null)
            {
                state = item[4];
                //console.log("default set " + id + " to " + state);
            }

            var mi = CreateInput(menuclass + "_checkbox", mid + "_checkbox", item[0], item[3], state, storagekey);      // item[0] is type, item[3] is callback
            var lb = CreateLabel(menuclass + "_label", mid + "_label", mid + "_checkbox", item[2]);     // item[2] is text, hook via for to checkbox
            itemdiv.appendChild(mi);
            itemdiv.appendChild(lb);
        }
        else if (item[0] == "radio")
        {
            var storagekey = menunameid + "." + item[4] + ".radiostate";        // first entry should have item[5], default state. item[4] is the group key name
            var state = FetchState(storagekey, item.length >= 6 ? item[5] : null, true);

            // console.log("Radio Storage " + storagekey + " state " + state);

            var radiostate = state == id;
            var mi = CreateInput(menuclass + "_radio", mid + "_radio", item[0], item[3], radiostate, [storagekey, id], item[4]);     // item[0] is type, item[3] is callback
            var lb = CreateLabel(menuclass + "_label", mid + "_label", mid + "_radio", item[2]);
            itemdiv.appendChild(mi);
            itemdiv.appendChild(lb);
        }
        else if (item[0] == "submenu")
        {
            var lb = CreateLabel(menuclass + "_submenu", mid + "_submenu", null, item[2], OpenSubMenu, item[3]);
            lb.style.padding = "0px 0px 0px 20px";
            itemdiv.appendChild(lb);
        }
        else if (item[0] == "button")
        {
            var lb = CreateLabel(menuclass + "_button", mid + "_button", null, item[2], item[3]);
            lb.style.padding = "0px 0px 0px 20px";
            itemdiv.appendChild(lb);
        }
        else
        {
            console.assert(false);
        }

        div.appendChild(itemdiv);
        div.appendChild(CreateBreak());
    });

    // attach after item used a position anchor
    appendto.append(div);
}

// return bool with check state
export function GetMenuItemCheckState(menuid, itemid)
{
    var storagekey = menuid + "." + itemid + ".checkboxstate";
    var jstate = window.localStorage.getItem(storagekey);           // state is stored in JSON, when we parse, we get back a boolean
    var state = JSON.parse(jstate);
    return state;
}

var menusopen = []       // remember menu open, array for submenus

export function ToggleMenu(id)
{
    if (menusopen.length > 0)
        CloseMenus();
    else
        OpenMenu(id);
}

export function OpenMenu(id)
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

export function CloseMenus()
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

export function CloseMenusBelow(id)
{
    var closebelow = false;
    menusopen.forEach(function (x)
    {
        if (x == id)
            closebelow = true;
        else if (closebelow)
        {
            var menu = document.getElementById(x);
            if (menu != null)
            {
                menu.style.display = "";
                console.log("Close " + x);
            }
        }
    });
}


export function OpenSubMenu(mouseevent)
{
    var ct = mouseevent.currentTarget;
    var openingmenu = ct.parentNode.parentNode;
    var submenu = ct.tag;
    console.log("MI " + ct.id + " open " + submenu + " from menu " + openingmenu.id);

    CloseMenusBelow(openingmenu.id);

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

