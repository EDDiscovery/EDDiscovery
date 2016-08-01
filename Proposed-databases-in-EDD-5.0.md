# Overview

In EDDiscovery 5.0, it is proposed that we split the database into two databases:
* [`EDDUser` database](#edduser-database): contains the user settings, journal entries, saved routes, bookmarks, notes, etc
* [`EDDSystems` database](#eddsystems-database): contains data imported from EDSM and EDDB.

# EDDUser database

This database (`EDDUser.sqlite`) contains user settings, journal entries, saved routes, etc.

It contains the following tables:
* [`Register`](#register): user settings
* [`Commanders`](#commanders): commander name, logpath and API key
* [`Journals`](#journals): Journal file info
* [`JournalEntries`](#journalentries): Journal entry data
* [`JournalProperties`](#journalproperties): Exported properties from journal entry data
* [`JournalTravelEntries`](#journaltravelentries): Travel log
* [`SavedRoutes`](#savedroutes): Saved route / expedition info
* [`SavedRouteEntries`](#savedrouteentries): Entries in saved routes / expeditions
* [`WantedSystems`](#wantedsystems): Systems the player has set as wanted / favourite
* [`Notes`](#notes): Player notes on systems and journal entries
* [`Bookmarks`](#bookmarks): Bookmarked systems or regions

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
  Synced INTEGER,
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

## In Memory representation of Journal Entries

The Event Class (replacing VisitedSystemsClass) holds, for the purposes of the travel history page DGV display, and the 3dmap etc, Events which the commander has performed.

It is keyed on the CommanderID. The journal class has a function for filling in this array (FillVisitedSystem(cmdr id)).  It goes thru the journal, in time order, and picks out interesting events to populate a List<EventClass>.

DGV grid will have : Time, Type, Text, Distance, Notes, Icons.  

```C#
Class EventClass
{
int journalentry; // which journal entry is this associated with, must be set

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

# EDDSystems database

A database (`EDDSystems.sqlite`), containing the system data sourced from EDSM and EDDB.

It contains the following tables:
* [`Systems`](#systems): System data imported from EDSM
* [`SystemAliases`](#systemaliases): Systems that have been renamed or hidden in EDSM
* [`PopulatedSystems`](#populatedsystems): Data on populated systems imported from EDDB

EDSM data is stored in the Systems table. EDDB data is stored in the PopulatedSystems table.

It also contains the following view:
* [`SystemsView`](#systemsview): Combination of Systems and PopulatedSystems

## Systems
```sql
CREATE TABLE Systems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL UNIQUE,
  Name TEXT NOT NULL COLLATE NOCASE,
  LastUpdated DATETIME,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE,
  GridId INTEGER NOT NULL
)
CREATE INDEX EdsmSystem_Name ON EdsmSystems (Name)
CREATE INDEX EdsmSystem_EdsmId ON EdsmSystems (SystemEdsmId)
CREATE INDEX EdsmSystem_Coords ON EdsmSystems (Z, X, Y)
```

Re-imported from EDSM dump.  Grid ID is used for new 3dmap star painting system.

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

## SystemsView
```sql
CREATE VIEW SystemsView
SELECT 
  s.Id AS SystemId, 
  p.Id AS PopulatedSystemId,
  s.SystemEdsmId,
  s.LastUpdated AS EdsmLastUpdated,
  s.Name,
  s.X,
  s.Y,
  s.Z,
  s.GridId,
  p.SystemEddbId,
  p.LastUpdated AS EddbLastUpdated,
  p.Faction,
  p.Government,
  p.Allegiance,
  p.Power,
  p.PowerState,
  p.PrimaryEconomy,
  p.Security,
  p.State,
  p.SimbadRef,
  p.NeedsPermit,
  p.Population
FROM Systems edsm
LEFT JOIN PopulatedSystems eddb ON eddb.SystemEdsmId = edsm.SystemEdsmId
```

Combination of Systems and PopulatedSystems into a single view