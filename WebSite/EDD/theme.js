import { FetchNumber, StoreState } from "/jslib/localstorage.js"

export var menuicon;

export function Theme(themeno)
{
    var r = document.querySelector(':root');
    if ( themeno == 0 )
    {
      r.style.setProperty('--textcolor', 'darkorange');
      r.style.setProperty('--titlecolor', 'darkorange');
      r.style.setProperty('--backcolor', 'black');
      r.style.setProperty('--tabcolor', '#ff9c08');
      r.style.setProperty('--tabhighlighted', '#ffe028');
      r.style.setProperty('--tabhover', '#ffbc48');
      r.style.setProperty('--tabtext', '#000000');
      r.style.setProperty('--textbuttonback', 'darkorange');
      r.style.setProperty('--textbutton', 'black');
      r.style.setProperty('--textbuttonborder', 'lightorange');
      r.style.setProperty('--actionsbackground', '#ffac28');
      r.style.setProperty('--actionstext', '#000000');
      r.style.setProperty('--gridborders', 'rgb(200,128,0)');
      r.style.setProperty('--gridbuttonborders', 'rgb(200,128,0)');
      r.style.setProperty('--headerbackground', 'rgb(40,40,40)');
      r.style.setProperty('--scrollbartrack', 'rgb(40,40,40)');
      r.style.setProperty('--scrollthumb', 'darkorange');
      r.style.setProperty('--scrollthumbhover', 'rgb(255,160,0)');
      r.style.setProperty('--dropdownmenuback', 'darkorange');
      r.style.setProperty('--dropdownmenutext', 'black');
      r.style.setProperty('--dropdownmenuhover', '#ffe028');
      menuicon = "/Images/menu.png";
    }
    else if ( themeno == 1 )
    {
      r.style.setProperty('--textcolor', '#C00000');
      r.style.setProperty('--titlecolor', 'darkorange');
      r.style.setProperty('--backcolor', 'black');
      r.style.setProperty('--tabcolor', '#303030');
      r.style.setProperty('--tabtext', '#800000');
      r.style.setProperty('--tabhighlighted', '#808080');
      r.style.setProperty('--tabhover', '#C0C0C0');
      r.style.setProperty('--textbuttonback', '#800000');
      r.style.setProperty('--textbuttonborder', '#c00000');
      r.style.setProperty('--textbutton', '#000000');
      r.style.setProperty('--actionsbackground', '#800000');
      r.style.setProperty('--actionstext', '#000000');
      r.style.setProperty('--gridborders', '#500000');
      r.style.setProperty('--gridbuttonborders', '#500000');
      r.style.setProperty('--headerbackground', 'rgb(40,0,0)');
      r.style.setProperty('--scrollbartrack', 'rgb(40,0,0)');
      r.style.setProperty('--scrollthumb', '#800000');
      r.style.setProperty('--scrollthumbhover', '#a00000');
      r.style.setProperty('--dropdownmenuback', '#800000');
      r.style.setProperty('--dropdownmenutext', 'black');
      r.style.setProperty('--dropdownmenuhover', '#c00000');
      menuicon = "/Images/menured.png";
    }
    else if ( themeno == 2 )
    {
      r.style.setProperty('--textcolor', 'rgb(200,200,200)');
      r.style.setProperty('--backcolor', 'rgb(28,30,34)');
      r.style.setProperty('--titlecolor', 'rgb(200,200,200)');
      r.style.setProperty('--tabcolor', 'rgb(71,77,84)');
      r.style.setProperty('--tabtext', 'rgb(192,196,196)');
      r.style.setProperty('--tabhighlighted', 'rgb(114,123,134)');
      r.style.setProperty('--tabhover', 'rgb(92,100,104)');
      r.style.setProperty('--textbuttonback', 'rgb(71,74,84)');
      r.style.setProperty('--textbuttonborder', 'rgb(41,46,51)');
      r.style.setProperty('--textbutton', 'rgb(192,196,196)');
      r.style.setProperty('--actionsbackground', 'rgb(28,30,34)');
      r.style.setProperty('--actionstext', 'rgb(192,196,196)');
      r.style.setProperty('--gridborders', 'rgb(41,46,51)');
      r.style.setProperty('--gridbuttonborders', 'rgb(41,46,51)');
      r.style.setProperty('--headerbackground', 'rgb(28,30,34)');
      r.style.setProperty('--scrollbartrack', 'rgb(28,30,34)');
      r.style.setProperty('--scrollthumb', 'rgb(64,64,64)');
      r.style.setProperty('--scrollthumbhover', 'rgb(128,128,128)');
      r.style.setProperty('--dropdownmenuback', 'rgb(192,196,196)');
      r.style.setProperty('--dropdownmenutext', 'black');
      r.style.setProperty('--dropdownmenuhover', '#404040');
      menuicon = "/Images/menugray.png";
    }

}
export function SetupTheme()
{
    var themeno = FetchNumber("themeselected",0)
    Theme(themeno);
}
export function SetTheme(themeno)
{
    StoreState("themeselected",themeno)
    Theme(themeno);
}

export function GetThemeColor(name)
{
    var r = document.querySelector(':root');
    return r.style.getPropertyValue(name);
}

