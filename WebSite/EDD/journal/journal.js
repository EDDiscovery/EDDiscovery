// journal requires a Jarticle Article and a Journal table.

var journalnextrow = -9999;     // meaning, no load

function ClearJournalTable()
{
    var jtable = document.getElementById("Journal");
    removeChildren(jtable);
}

function FillJournalTable(jdata, insert)
{  
    var jtable = document.getElementById("Journal");

    var firstrow = jdata.firstrow;

    if (firstrow >= 0)
    {
        var rows = jdata.rows;
        for (var i = 0; i < rows.length; i++)
        {
            var rowno = firstrow - i;

            var obj = rows[i];
            var image = CreateImage("journalicons/" + obj[0] + ".png", obj[1], 24, ClickJournalItem, firstrow - i);
            var t1 = document.createTextNode(obj[1]);
            var t2 = document.createTextNode(obj[2]);
            //var t3 = document.createTextNode(rowno + " " + obj[3]);
            var t3 = document.createTextNode(obj[3]);
            var t4 = document.createTextNode(obj[4]);

            if (insert)
                jtable.insertBefore(tablerowmultitdlist([image, t1, t2, t3, t4]), jtable.firstChild);
            else
                jtable.appendChild(tablerowmultitdlist([image, t1, t2, t3, t4]));
        }

        var nextrow = firstrow - rows.length;
        if (journalnextrow == -9999 || nextrow < journalnextrow)
            journalnextrow = nextrow;

        console.log("JLog " + firstrow + " count " + rows.length + " Next is " + journalnextrow);
    }
    else
    {
        jtable.appendChild(tablerowmultitdlist([document.createTextNode("No Data")]));
        console.log("No data");
    }
}

function ClickJournalItem(e)
{
    console.log("Clicked" + e.target.tag);
    RequestStatus(e.target.tag);
}

function ArticleScrolled()      // called by article on scrolling
{
    var article = document.getElementById("JArticle");

    if (journalnextrow >= 0 && article.scrollTop + article.clientHeight >= article.scrollHeight - 10 )
    {
        RequestJournal(journalnextrow, 50);
    }
//    console.log(article.scrollTop + " " + article.scrollHeight + article.clientHeight + " " + article.innerHeight);
}

function RequestMore(count)
{
    if (journalnextrow >= 0 )
        RequestJournal(journalnextrow, count);
}

function RequestJournal(start, len)
{
    var msg = {
        requesttype: "journal",
        start: start,	// -1 means send me the latest journal entry first, followed by length others.  else its the starting journal index (always latest followed by older)
        length: len
    };

    websocket.send(JSON.stringify(msg));
}

