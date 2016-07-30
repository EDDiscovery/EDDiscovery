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

Migrated from [`EDDiscovery.TravelLogUnit`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#travellogunit) table

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
  EventData TEXT,
  CommanderId INTEGER NOT NULL REFERENCES Commanders (Id),
  Synced INTEGER
)
CREATE INDEX JournalEntry_JournalId ON JournalEntries (JournalId)
CREATE INDEX JournalEntry_EventType ON JournalEntries (EventType)
CREATE INDEX JournalEntry_EventTime ON JournalEntries (EventTime)
CREATE INDEX JournalEntry_CommanderId ON JournalEntries (CommanderId)
```

This table contains entries from the journal or converted entries from the pre-2.2 netlogs.

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
Linked to system by the `SystemEdsmId` column

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

Migrated from the `EDDiscovery.Bookmarks` table

# EDDSystems

Contains data downloaded from EDSM, EDDB, etc.

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

Migrated from the [`EDDiscovery.Systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systems) table