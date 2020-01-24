/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function write_header()
{
    document.write("<header><h1> Elite Dangerous Discovery </h1> <h1> <input type=\"button\" value=\"fullscreen\" id=\"fullscreen_button\" /><img src=\"/Images/EdLogo600.png\" alt=\"EDD\" width=\"80\"> </h1> </header>");
    document.getElementById('fullscreen_button').onclick = toggle_fullscreen;

//    document.write("<header><h1> Elite Dangerous Discovery </h1> <h1> <img src=\"/Images/EdLogo600.png\" alt=\"EDD\" width=\"80\"> </h1> </header>");
}

function write_nav()
{
    document.write("<nav><ul> "+
        "<li> <a href=\"/index.html\">History</a></li>" +
        "<li> <a href=\"/status/status.html\">Status</a></li>" +
        "<li> <a href=\"/grid/grid.html\">Grid</a></li>" +
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

    html += "<p>(C) Elite Dangerous Discovery Team 2019-2020</p > ";

    html += "<p> <a href=\"https://github.com/EDDiscovery/EDDiscovery/wiki\"> Help </a></p>  </footer>";
    document.write(html);
} 

function highlight_nav_tab(tab)     // tab 0,1,2 etc.
{
    var ul = document.getElementsByTagName("nav")[0];
    var lis = ul.getElementsByTagName("li");
    lis[tab].classList.add("tabhighlighted");       // adding a class on, instead of just setting the style backcolor, allows hover to still work.
}


