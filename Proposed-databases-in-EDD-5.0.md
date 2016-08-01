# EDDUser database

This database contains user settings, journal entries, saved routes, etc.

## Register
```sql
CREATE TABLE Register (
  ID TEXT NOT NULL PRIMARY KEY, 
  ValueInt INTEGER, 
  ValueDouble DOUBLE, 
  ValueString TEXT, 
  ValueBlob BLOB
)
```

Migrated from [EDDiscovery.register](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#register) table

## Commanders
```sql
CREATE TABLE Commanders (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  LogPath TEXT,
  EDSMApiKey TEXT
)
```

Migrated from `EDCommander*` settings in register table

## Journals
```sql
CREATE TABLE Journals (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Type TEXT NOT NULL,
  Name TEXT NOT NULL COLLATE NOCASE,
  CommanderId INTEGER REFERENCES Commander (Id),
  Path TEXT COLLATE NOCASE,
  Size INTEGER
)
CREATE INDEX Journal_Name ON Journals (Name)
CREATE INDEX Journal_Commander ON Journals (CommanderId)
```

Migrated from [`EDDiscovery.TravelLogUnit`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#travellogunit) table.

Type:
* `NetLog`: travel log is a netlog
* `EDSM`: travel log was fetched from EDSM
* `Journal`: travel log is a E:D 2.2 journal

## JournalEntries
```sql
CREATE TABLE JournalEntries (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalId INTEGER NOT NULL REFERENCES Journals (Id),
  EventType TEXT NOT NULL,
  EventTime DATETIME NOT NULL,
  EventData TEXT, -- JSON String of complete line
  CommanderId INTEGER NOT NULL REFERENCES Commanders (Id),
  Synced INTEGER
)
CREATE INDEX JournalEntry_JournalId ON JournalEntries (JournalId)
CREATE INDEX JournalEntry_EventType ON JournalEntries (EventType)
CREATE INDEX JournalEntry_EventTime ON JournalEntries (EventTime)
CREATE INDEX JournalEntry_CommanderId ON JournalEntries (CommanderId)
```

This table contains entries from the journal or converted entries from the pre-2.2 netlogs.

All events we want to understand, at journal entry, their parameters are decoded and stored into `JournalProperties` table. Only certain journal entries are to be initially decoded and expanded and stored.

Additionally, FSDjumps get expanded into JournalFSDJumps table AND Journal Properties.  The reason for the second table is dB lookup speed.


## JournalProperties
```sql
CREATE TABLE JournalProperties (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntries (Id),
  PropertyName TEXT NOT NULL,
  SubPropertyName TEXT,
  Value VARIANT
)
CREATE INDEX JournalProperty_JournalEntryId ON JournalProperties (JournalEntryId)
CREATE INDEX JournalProperty_PropertyName ON JournalProperties (PropertyName)
```

This table contains data from the journal entries, for entries we want to process, display or join on.

* `PropertyName`: The name of the key in the journal entry JSON
* `SubPropertyName`: Used for properties that are objects (e.g. `Materials`) or arrays of objects (e.g. `Killers`)
* `Value`: The value of the property (or sub-property when the property is an object or array of objects)

## JournalTravelEntries
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

## IN Memory representation for the data grid travel view

The VisitedSystemsClass is repurposed for holding the data for this tab, keyed on the CommanderID, for display on the data view travel grid.  Journal class has a function for filling in this array (FillVisitedSystem(cmdr id)).  It goes thru the journal, in time order, and picks out interesting events to populate this list.

DGV grid will have : Time, Type, Text, Distance, Notes, MapColour.  

```C#
Class VisitedSystemsClass
{
int journalentry; // which journal entry is this associated with, must be set
int journaltravelentry; // to go to the JournalTravelEntries for looking up X,Y,Z etc.

Systems system;  // or null if not a FSDJump entry.. if edsm system, its a copy of the db row from Systems table.  it's not an edsm system, its a in-memory class only with id=0,Name=name,X/Y/Z populated from the journal entry.

PopulatedSystems  eddbinfo; // class representation of PopulatedSystems.. holds eddb info, or null if non there.
DateTime Time;

string type;  // for the type column, type of entry.. "Jump", "Dock", "Undock", "Land", "Take off" etc.
string text;  // for the text column.  For "Jump" it would be system name, for "Dock" maybe the space station name (can we get that)
}
```

Rather than copying the data in here, just have direct indexes?  we could put the data in here for speed.. not sure.

## SavedRoutes
```sql
CREATE TABLE SavedRoutes (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE,
  Start DATETIME,
  End DATETIME
)
```

Migrated from the [`EDDiscovery.routes_expeditions`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#route_expeditions) table

## SavedRouteEntries
```sql
CREATE TABLE SavedRouteEntries (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  RouteId INTEGER NOT NULL REFERENCES SavedRoutes (Id),
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER,
  Ordinal INTEGER
)
CREATE INDEX SavedRouteEntry_RouteId ON SavedRouteEntries (RouteId)
```

Migrated from the [`EDDiscovery.route_systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#route_systems) table

## WantedSystems
```sql
CREATE TABLE WantedSystems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER
)
CREATE INDEX WantedSystem_Name ON WantedSystems (SystemName)
CREATE INDEX WantedSystem_EdsmId ON WantedSystems (SystemEdsmId)
```

Migrated from the [`EDDiscovery.wanted_systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#wanted_systems) table

## Notes
```sql
CREATE TABLE Notes (
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  SystemName TEXT COLLATE NOCASE,
  SystemEdsmId INTEGER,
  JournalEntryId INTEGER REFERENCES JournalEntry (Id),
  Time DATETIME NOT NULL, 
  Note TEXT
)
CREATE INDEX SystemNote_SystemName ON SystemNotes (SystemName)
CREATE INDEX SystemNote_EdsmId ON SystemNotes (SystemEdsmId)
CREATE INDEX SystemNote_JournalEntryId ON SystemNotes (JournalEntryId)
```

User notes created by UI.
Linked to journal entries by the `JournalEntryId` column.  May be null.  If set, its a note on a journal entry. SystemEdsmID will be null, SystemName will be null if the journal entry is not a FSDJump or Location, else its the star name in the FSDJump or Location.

Linked to EDSM system by the `SystemEdsmId` column. May be null.  If set, its a note on a star.  SystemName will be set, JournalEntryId will be null

One of either JournalEntryId or SystemEdsmId must be non null.

Migrated from the [`EDDiscovery.SystemNote`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systemnote) table

## Bookmarks
```sql
CREATE TABLE Bookmarks (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemEdsmId INTEGER,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE,
  Time DATETIME,
  StarName TEXT,
  Heading TEXT,
  Note TEXT
)
```
Linked to system by the `SystemEdsmId` column (may be Null if no EDSM star)
StarName = null then Heading is set, region mark
StarName != null then heading is null, bookmark

Migrated from the `EDDiscovery.Bookmarks` table. 

# EDDSystems

Contains system data imported from EDSM and EDDB

EDSM data is stored in the Systems table.

EDDB data is stored in the PopulatedSystems table.

Tables can be combined at runtime using
```sql
SELECT *
FROM EdsmSystems edsm
LEFT JOIN EddbSystems eddb ON eddb.SystemEdsmId = edsm.SystemEdsmId
```

## Systems
```sql
CREATE TABLE Systems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL UNIQUE,
  Name TEXT NOT NULL COLLATE NOCASE,
  LastUpdated DATETIME,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE
)
CREATE INDEX EdsmSystem_Name ON EdsmSystems (Name)
CREATE INDEX EdsmSystem_EdsmId ON EdsmSystems (SystemEdsmId)
CREATE INDEX EdsmSystem_Coords ON EdsmSystems (Z, X, Y)
```

Re-imported from EDSM dump

## SystemAliases
```sql
CREATE TABLE SystemAliases (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  EdsmId INTEGER NOT NULL,
  MergedToEdsmId INTEGER NOT NULL
)
CREATE INDEX SystemAlias_EdsmId ON SystemAliases (EdsmId)
CREATE INDEX SystemAlias_MergedTo ON SystemAliases (MergedToEdsmId)
```

Re-imported from EDSM hidden-systems JSON

## PopulatedSystems
```sql
CREATE TABLE PopulatedSystems (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL UNIQUE REFERENCES EdsmSystems (SystemEdsmId),
  SystemEddbId INTEGER NOT NULL UNIQUE,
  Faction TEXT,
  Government TEXT,
  Allegiance TEXT,
  Power TEXT,
  PowerState TEXT,
  PrimaryEconomy TEXT,
  Security TEXT,
  State TEXT,
  SimbadRef TEXT,
  NeedsPermit INTEGER,
  Population INTEGER,
  LastUpdated DATETIME
)
CREATE INDEX EddbSystem_SystemEdsmId ON EddbSystems (SystemEdsmId)
CREATE INDEX EddbSystem_SystemEddbId ON EddbSystems (SystemEddbId)
```

Linked to a EDSM system entry by SystemEdsmId.

Re-imported from EDDB dump