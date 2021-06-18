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

/* pop ups attached to body, with
    <div class="rightsidednotification" id="scanobjectnotification">
    </div>

style:
.rightsidednotification {
    position: absolute; 
    visibility: hidden; 
    left: 500px; 
    top: 500px;
    z-index: 1; 
}
*/

// show a pop up, optionally set notification to element as its contents.  If timetodisplayms = null, click closes it

import { RemoveChildren } from "/jslib/elements.js"

export function ShowPopup(id, element = null,timetodisplayms = null, toppos = null)
{
    var notification = document.getElementById(id);

    if (element != null)
    {
        RemoveChildren(notification);
        notification.appendChild(element);
    }

    if (toppos != null)
    {
        notification.style.top = toppos + "px";
        notification.style.bottom = "auto";
    }

    notification.style.visibility = "visible";

    if (timetodisplayms != null)
        setTimeout(function () { notification.style.visibility = null; }, timetodisplayms);
    else
        notification.onclick = function () { notification.style.visibility = null; };

    return notification;
}

export function HidePopup(id)
{
    var notification = document.getElementById("scanobjectnotification");
    RemoveChildren(notification);
    notification.style.visibility = null;
}

