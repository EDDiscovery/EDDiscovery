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
* [`SystemNotes`](#systemnotes): Player notes on systems
* [`JournalNotes`](#journalnotes): Player notes on journal entries
* [`Bookmarks`](#bookmarks): Bookmarked systems or regions
* [`StellarBodies`](#stellarbodies): Stellar bodies (stars, planets) that have been scanned
* [`StellarBodyStarDetails`](#stellarbodystardetails): Star details
* [`StellarBodyPlanetMoonDetails`](#stellarbodyplanetmoondetails): Planet / moon details
* [`StellarBodyMaterials`](#stellarbodymaterials): Material concentrations on stellar bodies

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

Migrated from [EDDiscovery.Register](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#register) table
```sql
INSERT INTO EDDUser.Register SELECT * FROM EDDiscovery.Register
```


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
```sql
INSERT INTO EDDUser.Commanders
SELECT n.Num AS Id, n.Name, p.LogPath, k.EDSMApiKey
FROM (SELECT SUBSTR(ID,16) + 0 AS Num, ValueString AS Name 
      FROM EDDiscovery.Register 
      WHERE ID LIKE 'EDCommanderName%') AS n
JOIN (SELECT SUBSTR(ID,22) + 0 AS Num, ValueString AS LogPath 
      FROM EDDiscovery.Register 
      WHERE ID LIKE 'EDCommanderNetLogPath%') AS p ON p.Num = n.Num
JOIN (SELECT SUBSTR(ID,18) + 0 AS Num, ValueString AS EDSMApiKey 
      FROM EDDiscovery.Register 
      WHERE ID LIKE 'EDCommanderApiKey%') AS k ON k.Num = n.Num
```

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
```sql
INSERT INTO EDDUser.SavedRoutes SELECT * FROM EDDiscovery.routes_expeditions
```

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
```sql
INSERT INTO EDDUser.SavedRouteEntries (Id, RouteId, SystemName)
SELECT * FROM route_systems
```

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

```sql
INSERT INTO EDDUser.WantedSystems (Id, SystemName)
SELECT * FROM EDDiscovery.wanted_systems
```

## SystemNotes
```sql
CREATE TABLE SystemNotes (
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER,
  Time DATETIME NOT NULL, 
  Note TEXT
)
CREATE INDEX SystemNote_SystemName ON SystemNotes (SystemName)
CREATE INDEX SystemNote_EdsmId ON SystemNotes (SystemEdsmId)
```

User system notes created by UI.

Linked to EDSM system by the `SystemEdsmId` column.  SystemEdsmId may be null if system does not yet exist in EDSM.

If SystemEdsmId is null, then system is linked using SystemName.

Migrated from the [`EDDiscovery.SystemNote`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systemnote) table

```sql
INSERT INTO EDDUser.SystemNotes (Id, SystemName, Time, Note)
SELECT Id, Name AS SystemName, Time, Note FROM EDDiscovery.SystemNote
```

## JournalNotes
```sql
CREATE TABLE JournalNotes (
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntry (Id),
  Time DATETIME NOT NULL, 
  Note TEXT
)
CREATE INDEX SystemNote_JournalEntryId ON JournalNotes (JournalEntryId)
```

Linked to journal entries by the `JournalEntryId` column.

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

```sql
INSERT INTO EDDUser.Bookmarks (Id, StarName, X, Y, Z, Time, Heading, Note)
SELECT * FROM EDDiscovery.Bookmarks
```

## StellarBodies
```sql
CREATE TABLE StellarBodies (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  BodyName TEXT NOT NULL,
  DistanceFromArrivalLS DOUBLE, -- Distance from arrival point in Light Seconds
  Radius DOUBLE, -- Radius of body in metres
  OrbitalPeriod DOUBLE, -- Orbital period in seconds
  RotationPeriod DOUBLE, -- Rotation period in seconds
  Rings INTEGER -- Number of rings
)
```

Filled by scans of stellar bodies.

## StellarBodyStarDetails
```sql
CREATE TABLE StellarBodyStarDetails (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  StellarBodyId INTEGER NOT NULL UNIQUE REFERENCES StellarBodies (Id),
  StarType TEXT, -- [Stellar classification](https://en.wikipedia.org/wiki/Stellar_classification) of star
  StellarMass DOUBLE, -- [Solar mass](https://en.wikipedia.org/wiki/Solar_mass) of star
  AbsoluteMagnitude DOUBLE -- [Absolute magnitude](https://en.wikipedia.org/wiki/Absolute_magnitude) of star
)
```

Filled by scans of stellar bodies.

## StellarBodyPlanetMoonDetails
```sql
CREATE TABLE StellarBodyPlanetMoonDetails (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  StellarBodyId INTEGER NOT NULL UNIQUE REFERENCES StellarBodies (Id),
  TidalLock BOOL, -- True if tidally locked to parent body
  TerraformState TEXT, -- Terraformable, Terraforming, Terraformed, or null
  PlanetClass TEXT, -- Type of planet or moon - e.g. Metal rich body, Rocky body
  Atmosphere TEXT, -- Type of atmosphere
  Vulcanism TEXT,
  SurfaceTemperature DOUBLE,
  SurfacePressure DOUBLE,
  SurfaceGravity DOUBLE,
  PlanetMass DOUBLE, -- Mass of body as multiple of [Earth's mass](https://en.wikipedia.org/wiki/Earth_mass)
  Landable BOOL
)
```

Filled by scans of stellar bodies.

## StellarBodyMaterials
```sql
CREATE TABLE StellarBodyMaterials (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  StellarBodyId INTEGER NOT NULL REFERENCES StellarBodies (Id),
  MaterialName TEXT NOT NULL,
  Concentration DOUBLE
)
```

Filled by scans of stellar bodies

# EDDSystems database

This database (`EDDSystems.sqlite`) contains the system data sourced from EDSM and EDDB.

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
  GridId INTEGER NOT NULL,
  RandomId INTEGET NOT NULL
)
CREATE INDEX EdsmSystem_Name ON Systems (Name)
CREATE INDEX EdsmSystem_EdsmId ON Systems (SystemEdsmId)
CREATE INDEX EdsmSystem_Coords ON Systems (Z, X, Y)
CREATE INDEX EdsmSystem_GridId on Systems (GridId)
CREATE INDEX EdsmSystem_RandomId on Systems (RandomId)
```

Re-imported from EDSM dump.  Grid ID and RandomID are used for new 3dmap star painting system.  Grid ID is assigned based on x/z position, RandomID is 0 to 99 inclusive.

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