/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function toggle_fullscreen(event)
{
    console.log(document.fullscreen)
    if (!document.fullscreen)
    {
        open_fullscreen();
    }
    else
    {
        close_fullscreen();
    }
}


var elem = document.documentElement;

function open_fullscreen()
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

function close_fullscreen()
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