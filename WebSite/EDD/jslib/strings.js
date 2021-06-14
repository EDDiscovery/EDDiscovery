/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

// String

function SplitCapsWord(str)
{
    return str.replace(/([a-z])([A-Z])/g, '$1 $2');
}

function Append(str, text, append = ", ")
{
    if (str != "")
        return str + append + text;
    else
        return text;
}

