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

export function ToggleFullscreen(event)
{
    console.log(document.fullscreen)
    if (!document.fullscreen)
    {
        SetFullScreen();
    }
    else
    {
        SetNormalScreen();
    }
}


var elem = document.documentElement;

export function SetFullScreen()
{
    if (elem.requestFullscreen)
    {
        elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen)
    { /* Firefox */
        elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen)
    { /* Chrome, Safari and Opera */
        elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen)
    { /* IE/Edge */
        elem.msRequestFullscreen();
    }
}

export function SetNormalScreen()
{
    if (document.exitFullscreen)
    {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen)
    { /* Firefox */
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen)
    { /* Chrome, Safari and Opera */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen)
    { /* IE/Edge */
        document.msExitFullscreen();
    }
}