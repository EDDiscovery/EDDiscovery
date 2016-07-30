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

TBD: Q Robby: we read in this to an memory array? Do we expand out the properties of these into the memory array? For instance, the FSDJump entry, do we have x/y/z name as the decoded FSD entry in the class?  Do we have to parse it every time we need it (a lot) or do we parse one and extract data..

A: The properties that we want to index on or join on, such as X/Y/Z, get processed and inserted into `JournalProperties`

Q: Secondly, how is it assigned to an EDSM Star. We have to match on name+XYZ.  Without name and XYZ exposed in the record we can't do that with a JOIN ON.. like your doing at the moment.  Where do we keep the EDSM ID? Are we going to bury it in the JSON string (I would not, i think the string should be as is frontier provide it).  Are we using the JournalProperties for a FSD jump to do the match..

A: 
* X/Y/Z would be stored in the `StarPos` JournalProperties entry linked to the `FSDJump` or `Location` entry.
* The EDSM ID can be stored in an `EDSM-ID` JournalProperties entry.
* Joining should be not be appreciably slower matching on the X/Y/Z values where `JournalProperties.PropertyName='StarPos'` than it is with the X/Y/Z values stored directly on the `JournalEntry`.

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

This table contains data from the journal entries.

* `PropertyName`: The name of the key in the journal entry JSON
* `SubPropertyName`: Used for `Materials` or `Killers` properties
* `PropertyArrayIndex`: Used for array properties such as `Killers` or `Discovered`
* `ValueNumber`: The numeric value of the property
* `ValueString`: The string value of the property
* `CoordX`: X coordinate (used by `StarPos`)
* `CoordY`: Y coordinate (used by `StarPos`)
* `CoordZ`: Z coordinate (used by `StarPos`)

Q: Robby: you need to describe what some of these fields mean. (propertyArrayIndex).  Are you proposing to one time on journal entry decode the JSON and make an array of these.. are we doing it for every entry? (thats lots of data considering we will probably only use a few entries at the start)..

A: We only need to store values for properties we want to index on or join on (such as StarPos, SystemName, EDSM ID, etc.)

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

## SystemNotes
```
CREATE TABLE SystemNotes (
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  SystemName TEXT NOT NULL COLLATE NOCASE,
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
Linked to journal entries by the `JournalEntryId` column
Linked to system by the `SystemEdsmId` column (may be Null if no EDSM star)

RJP Q: linked to journal ID fine, presume always a FSDJump entry, we should state.  Why link to EDSM? just for ease?

A: System notes from EDSM are not linked to any one travel entry, but are instead linked to a system.
We can rename this to simply `Notes`, and have it be a general purpose note store.  Notes on systems would be linked by EDSM ID and/or the FSDJump / Location journal entry on which it was entered.  Notes on journal entries would not be linked to any system, but conversely would also not be uploaded to EDSM.

Migrated from the [`EDDiscovery.SystemNote`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systemnote) table

## Bookmarks
```
CREATE TABLE Bookmarks (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  StarName TEXT,
  SystemEdsmId INTEGER,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE,
  Time DATETIME,
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

TBD Robby - change to EDSM systems..?

## Systems
```
CREATE TABLE Systems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL UNIQUE,
  Name TEXT NOT NULL COLLATE NOCASE,
  LastUpdated DATETIME,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE
)
CREATE INDEX System_Name ON System (Name)
CREATE INDEX System_EdsmId ON System (SystemEdsmId)
CREATE INDEX System_Coords ON System (Z, X, Y)
```

Migrated from the [`EDDiscovery.Systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systems) table

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
  SystemId INTEGER NOT NULL REFERENCES Systems (Id),
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
CREATE INDEX EddbSystem_SystemId ON EddbSystems (SystemId)
CREATE INDEX EddbSystem_SystemEddbId ON EddbSystems (SystemEddbId)
```
Linked to a EDSM system entry by SystemId.

Migrated from the [`EDDiscovery.Systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systems) table