/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */


function Get(yourUrl)
{
    var Httpreq = new XMLHttpRequest(); // a new request
    Httpreq.open("GET",yourUrl,false);
    Httpreq.send(null);
    return Httpreq.responseText;          
}

// async.. this.responseText has the data
function AsyncGet(yourURL,yourcallback)
{
    var xhr = new XMLHttpRequest();
    xhr.callback = yourcallback;
    xhr.arguments = Array.prototype.slice.call(arguments, 2);
    xhr.onload = function() {this.callback.apply(this, this.arguments);};
    xhr.onerror = function() {console.error(this.statusText);};
    xhr.open("GET",yourURL,true);
    xhr.send(null);
}

