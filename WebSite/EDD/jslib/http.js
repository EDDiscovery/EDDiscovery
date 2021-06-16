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

