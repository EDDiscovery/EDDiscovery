/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function WSURIFromLocation()
{
    var loc = window.location;

    if (loc.protocol === "https:")
    {
        new_uri = "wss:";
    }
    else
    {
        new_uri = "ws:";
    }

    new_uri += "//" + loc.host + "/";
    return new_uri;
}

