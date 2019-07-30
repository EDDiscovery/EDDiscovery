function write_header()
{
    document.write("<header><h1> Elite Dangerous Discovery </h1> <h1> <img src=\"Images/EdLogo600.png\" alt=\"EDD\" width=\"80\"> </h1> </header>");
}

function write_nav()
{
    document.write("<nav><ul> <li><a href=\"index.html\">History</a></li> <li><a href=\"status.html\">Status</a></li> </ul> </nav>");
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

