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

import { ToggleFullscreen } from "/jslib/screencontrol.js"
import { CreateButton , CreateAnchor } from "/jslib/elements.js"

export function WriteHeader(header)
{
    var html = "<h1> Elite Dangerous Discovery </h1>" +
        "<h1> <input type=\"button\" value=\"fullscreen\" id=\"fullscreen_button\" /><img src=\"/Images/EdLogo600.png\" alt=\"EDD\" width=\"80\"> </h1>";
    header.insertAdjacentHTML("beforeend",html);
    document.getElementById('fullscreen_button').onclick = ToggleFullscreen;
}

export function WriteNav(nav, tab)
{
    var html = "<ul>" + 
        "<li> <a href=\"/index.html\">History</a></li>" +
        "<li> <a href=\"/scandisplay/scandisplay.html\">Scan</a></li>" +
        "<li> <a href=\"/scandata/scandata.html\">Scan Grid</a></li>" +
        "<li> <a href=\"/status/status.html\">Status</a></li>" +
        "<li> <a href=\"/grid/grid.html\">Grid</a></li>" +
        "<li> <a href=\"/missions/missions.html\">Missions</a></li>" +
        "<li> <a href=\"/texts/texts.html\">Texts</a></li>" +
        "</ul>";
        
    nav.insertAdjacentHTML("beforeend",html);

    var ul = document.getElementsByTagName("nav")[0];
    var lis = ul.getElementsByTagName("li");
    lis[tab].classList.add("tabhighlighted");       // adding a class on, instead of just setting the style backcolor, allows hover to still work.
}

export function WriteFooter(footer, buttons)
{
    var p = document.createElement("p");
    if (buttons != null)
    {
        buttons.forEach(function (x)
        {
            p.appendChild(CreateButton(x[0], x[1]));
        });
    }

    p.appendChild(CreateAnchor("Help", "https://github.com/EDDiscovery/EDDiscovery/wiki"));

    footer.appendChild(p);
} 


