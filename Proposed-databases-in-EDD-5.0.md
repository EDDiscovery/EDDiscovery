# EDDUser database

This database contains user settings, journal entries, saved routes, etc.

## Register
```
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
```
CREATE TABLE Commanders (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  LogPath TEXT,
  EDSMApiKey TEXT
)
```

Migrated from `EDCommander*` settings in register table

## Journals
```
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
```
CREATE TABLE JournalEntries (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalId INTEGER NOT NULL REFERENCES Journals (Id),
  EventType TEXT NOT NULL,
  EventTime DATETIME NOT NULL,
  EventData TEXT, # JSON String of complete line
  CommanderId INTEGER NOT NULL REFERENCES Commanders (Id),
  Synced INTEGER
)
CREATE INDEX JournalEntry_JournalId ON JournalEntries (JournalId)
CREATE INDEX JournalEntry_EventType ON JournalEntries (EventType)
CREATE INDEX JournalEntry_EventTime ON JournalEntries (EventTime)
CREATE INDEX JournalEntry_CommanderId ON JournalEntries (CommanderId)
```

This table contains entries from the journal or converted entries from the pre-2.2 netlogs.

The properties that we want to index on or join on, or display, get processed and inserted into `JournalProperties`. Only certain journal entries are to be initially decoded and expanded and stored.

FSDjumps get expanded into JournalFSDJumps table AND Journal Properties.  The reason for the second table is dB lookup speed.

## JournalProperties
```
CREATE TABLE JournalProperties (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntries (Id),
  PropertyName TEXT NOT NULL,
  SubPropertyName TEXT,
  PropertyArrayIndex INTEGER,
  ValueNumber DOUBLE,
  ValueString TEXT,
  CoordX DOUBLE,
  CoordY DOUBLE,
  CoordZ DOUBLE
)
CREATE INDEX JournalProperty_PropertyName ON JournalProperties (PropertyName)
CREATE INDEX JournalProperty_Coords ON JournalProperties (CoordZ, CoordX, CoordY)
```

This table contains data from the journal entries, for entries we want to process, display or join on.

* `PropertyName`: The name of the key in the journal entry JSON
* `SubPropertyName`: Used for `Materials` or `Killers` properties
* `PropertyArrayIndex`: Used for array properties such as `Killers` or `Discovered`
* `ValueNumber`: The numeric value of the property, null if a string property.
* `ValueString`: The string value of the property, null if a numeric property.

RJP Q. Still want these? Considering we have jump table.

* `CoordX`: X coordinate (used by `StarPos`) of the system were were in when this event happened
* `CoordY`: Y coordinate (used by `StarPos`)
* `CoordZ`: Z coordinate (used by `StarPos`)

## JournalFSDJumps
```
CREATE TABLE JournalFSDJumps (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntry (Id),
  SystemEdsmId INTEGER,
  Name TEXT NOT NULL,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE
)
CREATE INDEX JournalFSDJumps_Coords ON JournalFSDJumps (Z,X,Y)
CREATE INDEX JournalFSDJumps_Name ON JournalFSDJumps (Name)
CREATE INDEX JournalFSDJumps_EdsmId ON JournalFSDJumps (SystemEdsmId)
```

Used for fast system matching.  When a FSDJump is found in the journal, an entry in this table is created.

SystemEdsmId may be null if no matching EDSM system exists.

Migrated from [`EDDiscovery.VisitedSystems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#visitedsystems)

## SavedRoutes
```
CREATE TABLE SavedRoutes (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE,
  Start DATETIME,
  End DATETIME
)
```

Migrated from the [`EDDiscovery.routes_expeditions`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#route_expeditions) table

## SavedRouteEntries
```
CREATE TABLE SavedRouteEntries (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  RouteId INTEGER NOT NULL REFERENCES SavedRoutes (Id),
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER,
  Ordinal INTEGER
)
```

Migrated from the [`EDDiscovery.route_systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#route_systems) table

## WantedSystems
```
CREATE TABLE WantedSystems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER
)
CREATE INDEX WantedSystem_EdsmId ON WantedSystems (SystemEdsmId)
```

Migrated from the [`EDDiscovery.wanted_systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#wanted_systems) table

## Notes
```
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
Linked to journal entries by the `JournalEntryId` column.  May be null.  If set, its a note on a journal entry. SystemEdsmID will be null, SystemName will be null if the journal entry is not a FSDJump, else its the star name in the FSDJump.

Linked to EDSM system by the `SystemEdsmId` column. May be null.  If set, its a note on a star.  SystemName will be set, JournalEntryId will be null

One of either JournalEntryId or SystemEdsmId must be non null.

Migrated from the [`EDDiscovery.SystemNote`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systemnote) table

## Bookmarks
```
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

Contains data downloaded from EDSM only TBD Robby Edit.

klightspeed: If we are only wanting EDSM data, then do we not want the `EddbSystem` table below?
RJP A. We need eddb data.  We can either put it a properties of the EDSMsystems (as we do now) or we can have a different table, as in here.

## Systems
```
CREATE TABLE EdsmSystems (
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

Not migrated - redownloaded when we change.  

RJP: Delete Migrated from the [`EDDiscovery.Systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systems) table

## SystemAliases
```
CREATE TABLE SystemAliases (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  EdsmId INTEGER NOT NULL,
  MergedToEdsmId INTEGER NOT NULL
)
CREATE INDEX SystemAlias_EdsmId ON SystemAliases (EdsmId)
CREATE INDEX SystemAlias_MergedTo ON SystemAliases (MergedToEdsmId)
```

Migrated from the `EDDiscovery.SystemAliases` table

## EddbSystems
```
CREATE TABLE EddbSystems (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL REFERENCES EdsmSystems (SystemEdsmId),
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

Not migrated - redownloaded when we change.  
