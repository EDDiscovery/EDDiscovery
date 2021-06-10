/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function storestate(id, state)
{
    var storage = window.localStorage;
    var jstate = JSON.stringify(state);
    storage.setItem(id, jstate);
    console.log("Set storage " + id + "= '" + jstate + "'");
}

function fetchstate(id, defaultstate, writebackdefault = false)
{
    var storage = window.localStorage;
    var state = storage.getItem(id);
    console.log("Fetch storage " + id + "= '" + state + "'");
    if (state == null)
    {
        if (writebackdefault)
            storestate(id, defaultstate);
        return defaultstate;
    }
    else
        return JSON.parse(state);
}