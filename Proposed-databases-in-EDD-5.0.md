# EDDUser database

This database contains user settings, journal entries, saved routes, etc.

## Register
```
CREATE TABLE Register (
  ID TEXT PRIMARY KEY NOT NULL, 
  ValueInt INTEGER, 
  ValueDouble DOUBLE, 
  ValueString TEXT, 
  ValueBlob BLOB
)
```

## Commander
```
CREATE TABLE Commander (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  Name TEXT NOT NULL,
  LogPath TEXT,
  EDSMApiKey TEXT
)
```

## Journal
```
CREATE TABLE Journal (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  Type TEXT NOT NULL,
  Name TEXT NOT NULL COLLATE NOCASE,
  CommanderId INTEGER REFERENCES Commander (Id),
  Path TEXT COLLATE NOCASE,
  Size INTEGER
)
CREATE INDEX Journal_Name ON Journal (Name)
CREATE INDEX Journal_Commander ON Journal (CommanderId)
```

Type:
* `NetLog`: travel log is a netlog
* `EDSM`: travel log was fetched from EDSM
* `Journal`: travel log is a E:D 2.2 journal

## JournalEntry
```
CREATE TABLE JournalEntry (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  JournalId INTEGER NOT NULL REFERENCES Journal (Id),
  EventType TEXT NOT NULL,
  EventTime DATETIME NOT NULL,
  EventData TEXT,
  CommanderId INTEGER NOT NULL REFERENCES Commander (Id),
  Synced INTEGER
)
CREATE INDEX JournalEntry_JournalId ON JournalEntry (JournalId)
CREATE INDEX JournalEntry_EventType ON JournalEntry (EventType)
CREATE INDEX JournalEntry_EventTime ON JournalEntry (EventTime)
CREATE INDEX JournalEntry_CommanderId ON JournalEntry (CommanderId)
```

## JournalProperty
```
CREATE TABLE JournalProperty (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntry (Id),
  PropertyName TEXT NOT NULL,
  SubPropertyName TEXT,
  PropertyArrayIndex INTEGER,
  ValueNumber DOUBLE,
  ValueString TEXT,
  CoordX DOUBLE,
  CoordY DOUBLE,
  CoordZ DOUBLE
)
CREATE INDEX JournalProperty_PropertyName ON JournalProperty (PropertyName)
CREATE INDEX JournalProperty_Coords ON JournalProperty (CoordZ, CoordX, CoordY)
```

## SavedRoute
```
CREATE TABLE SavedRoute (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE,
  Start DATETIME,
  End DATETIME
)
```

## SavedRouteEntry
```
CREATE TABLE SavedRouteEntry (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  RouteId INTEGER NOT NULL REFERENCES SavedRoute (Id),
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER
)
```

## WantedSystem
```
CREATE TABLE WantedSystem (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER
)
CREATE INDEX WantedSystem_EdsmId ON WantedSystem (SystemEdsmId)
```

## SystemNote
```
CREATE TABLE SystemNote (
  id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT, 
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER,
  JournalEntryId INTEGER REFERENCES JournalEntry (Id),
  Time DATETIME NOT NULL, 
  Note TEXT
)
CREATE INDEX SystemNote_SystemName ON SystemNote (SystemName)
CREATE INDEX SystemNote_EdsmId ON SystemNote (SystemEdsmId)
CREATE INDEX SystemNote_JournalEntryId ON SystemNote (JournalEntryId)
```

User notes created by UI.
Linked to journal entries by the `JournalEntryId` column
Linked to system by the `SystemEdsmId` column

# EDDSystems

Contains data downloaded from EDSM, EDDB, etc.

## System
```
CREATE TABLE System (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
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

## EddbSystem
```
CREATE TABLE EddbSystem (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  SystemId INTEGER NOT NULL REFERENCES System (Id),
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
CREATE INDEX EddbSystem_SystemId ON EddbSystem (SystemId)
CREATE INDEX EddbSystem_SystemEddbId ON EddbSystem (SystemEddbId)
```