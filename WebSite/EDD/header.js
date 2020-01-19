function write_header()
{
    document.write("<header><h1> Elite Dangerous Discovery </h1> <h1> <input type=\"button\" value=\"fullscreen\" id=\"fullscreen_button\" /><img src=\"Images/EdLogo600.png\" alt=\"EDD\" width=\"80\"> </h1> </header>");
    document.getElementById('fullscreen_button').onclick = toggle_fullscreen
}

function write_nav()
{
    document.write("<nav><ul> "+
        "<li> <a href=\"index.html\">History</a></li>" +
        "<li> <a href=\"status.html\">Status</a></li>" +
        "<li> <a href=\"grid.html\">Grid</a></li>" +
                   "</ul> </nav> ");
}

function write_footer(buttons)
{
    var html = "<footer>"

    var but = 1;

    buttons.forEach(function (item) 
    {
        if (item != null)
        {
            html += "<button onclick=\"footerbutton" + (but++) + "click()\">" + item + "</button>";
        }
    });

    html += "<p>(C) Elite Dangerous Discovery Team 2019</p > ";

    html += "<p> <a href=\"https://github.com/EDDiscovery/EDDiscovery/wiki\"> Help </a></p>  </footer>";
    document.write(html);
} 

function highlight_nav_tab(tab)     // tab 0,1,2 etc.
{
    var ul = document.getElementsByTagName("nav")[0];
    var lis = ul.getElementsByTagName("li");
    lis[tab].classList.add("tabhighlighted");       // adding a class on, instead of just setting the style backcolor, allows hover to still work.
}

var elem = document.documentElement;
function toggle_fullscreen(event)
{
    console.log(document.fullscreen)
    if (!document.fullscreen) {
        open_fullscreen();
    }
    else{
        close_fullscreen();
    }
}

function open_fullscreen() {
    if (elem.requestFullscreen) {
        elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen) { /* Firefox */
        elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
        elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) { /* IE/Edge */
       elem.msRequestFullscreen();
    }
}

function close_fullscreen() {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) { /* Firefox */
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { /* IE/Edge */
        document.msExitFullscreen();
    }
}
