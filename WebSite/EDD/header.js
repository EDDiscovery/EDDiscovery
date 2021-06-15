/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function write_header()
{
    document.write("<h1> Elite Dangerous Discovery </h1> <h1> <input type=\"button\" value=\"fullscreen\" id=\"fullscreen_button\" /><img src=\"/Images/EdLogo600.png\" alt=\"EDD\" width=\"80\"> </h1>");
    document.getElementById('fullscreen_button').onclick = toggle_fullscreen;
}

function write_nav(tab)
{
    document.write("<ul>");
    document.write(
        "<li> <a href=\"/index.html\">History</a></li>" +
        "<li> <a href=\"/scandisplay/scandisplay.html\">Scan</a></li>" +
        "<li> <a href=\"/scandata/scandata.html\">Scan Grid</a></li>" +
        "<li> <a href=\"/status/status.html\">Status</a></li>" +
        "<li> <a href=\"/grid/grid.html\">Grid</a></li>");
    document.write("</ul>");

    var ul = document.getElementsByTagName("nav")[0];
    var lis = ul.getElementsByTagName("li");
    lis[tab].classList.add("tabhighlighted");       // adding a class on, instead of just setting the style backcolor, allows hover to still work.
}

function write_footer(buttons)
{
    var html = "<footer><p>"

    var but = 1;

    buttons.forEach(function (item) 
    {
        if (item != null)
        {
            html += "<button onclick=\"footerbutton" + (but++) + "click()\">" + item + "</button>";
        }
    });

    html += "<a href=\"https://github.com/EDDiscovery/EDDiscovery/wiki\"> Help </a></p > ";
    html += "</footer>";
    document.write(html);
} 


