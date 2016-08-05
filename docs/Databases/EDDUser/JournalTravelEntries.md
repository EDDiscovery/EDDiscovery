# EDDUser.JournalTravelEntries table

```sql
CREATE TABLE JournalTravelEntries (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntry (Id),
  SystemEdsmId INTEGER,
  IsFSDJump BOOL,
  Name TEXT NOT NULL,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE,
  MapColour INTEGER NOT NULL,
)
CREATE INDEX JournalTravelEntry_EntryId ON JournalTravelEntries (JournalEntryId)
CREATE INDEX JournalTravelEntry_Coords ON JournalTravelEntries (Z,X,Y)
CREATE INDEX JournalTravelEntry_Name ON JournalTravelEntries (Name)
CREATE INDEX JournalTravelEntry_EdsmId ON JournalTravelEntries (SystemEdsmId)
```

Used for fast system matching.  When a FSDJump is found or the Location system changes in the journal, an entry in this table is created.

SystemEdsmId may be null if no matching EDSM system exists.
IsFSDJump shows thats its an explicit FSD Jump.
X Y Z may be null for pre 2.1 systems
MapColour is the current colour assigned for map display at the point of jump.

Migrated from [`EDDiscovery.VisitedSystems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#visitedsystems)

## In Memory representation of Journal Entries

The Event Class (replacing VisitedSystemsClass) holds, for the purposes of the travel history page DGV display, and the 3dmap etc, Events which the commander has performed.

It is keyed on the CommanderID. The journal class has a function for filling in this array (FillVisitedSystem(cmdr id)).  It goes thru the journal, in time order, and picks out interesting events to populate a List<EventClass>.

DGV grid will have : Time, Type, Text, Distance, Notes, Icons.  

```C#
Class EventClass
{
long journalentry; // which journal entry is this associated with, must be set

Systems system; // always not null.
PopulatedSystems  eddbinfo;
DateTime Time;

string type;  // for the type column, type of entry.. "Jump", "Dock", "Undock", "Land", "Take off" etc.
string text;  // for the text column.  For "Jump" it would be system name, for "Dock" maybe the space station name (can we get that)
}
```

As the journal is read, ignoring entries not matching the commander, then on each location/fsd jump change, then we pick up the System information relevant.  We then populate the jump entry, and all subsequent entries, with the same information until another jump occurs. To pick up the information, we look to see if an system exists in the Systems table by EDSMID (using the JournalTravelEntries.SystemEdsmId).  If so, this is a copy of the db row from the Systems table.  if EDSMID is not set, this is in memory representation of the system using info from the journal entry; id=0,Name=name,X/Y/Z populated from the journal entry.

if EDSMID is available for the journal entry, and EDDB has the edsm id, then this is the inmemory class representation of row of the PopulatedSystems db. Null if non there.

Icons holds map colour, and also probably a EDSM first discovered flag, etc.
```
class Systems cursys = null
class PopulatedSystems curinfo = null

for all journal entries, old to new
    if cmdr id matches
        if relevant event
            if fsd event or location change
            {
                lookup entry in JournalTravelEntries

                if edsmid
                    cursys = db lookup from System table

                    lookup in db PopulatedSystems edsm id
                    if found
                        curinfo = db entry
                else
                    cursys = in memory create of class System from info in the JournalTravelEntries
                             using the x/y/z
            }

            create entry in EventLog:
                type = event log name
                text = relevant text

                journalentry = journal entry id
                system = cursys
                eddbinfo = curinfo
```
