A list of databases and their sources.

# Distances

```
CREATE TABLE Distances (
  id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL  UNIQUE ,
  NameA TEXT NOT NULL , 
  NameB TEXT NOT NULL , 
  Dist FLOAT NOT NULL , 
  CommanderCreate TEXT NOT NULL , 
  CreateTime DATETIME NOT NULL , 
  Status INTEGER NOT NULL , 
  id_edsm Integer
)
CREATE INDEX DistanceName ON Distances (NameA ASC, NameB ASC)
CREATE INDEX Distances_EDSM_ID_Index ON Distances (id_edsm ASC)
```

When enabled by the setting page, EDSM distance data base is synced with this table every 28 days, plus the db is updated from EDSM at each run.  Synced to EDSM on id_edsm.  Used during fill of visited systems for pairs of systems which do not have co-ords.

# Objects
for materials, not in active use. 

# Register
```
CREATE TABLE Register (
  ID TEXT PRIMARY KEY  NOT NULL  UNIQUE , 
  "ValueInt" INTEGER, 
  "ValueDouble" DOUBLE, 
  "ValueString" TEXT, 
  "ValueBlob" BLOB
)
```

value save

# Stations
```
CREATE TABLE Stations (
  id INTEGER PRIMARY KEY  NOT NULL ,
  system_id INTEGER, 
  name TEXT NOT NULL ,   
  max_landing_pad_size INTEGER, 
  distance_to_star INTEGER, 
  faction Text, 
  government_id INTEGER, 
  allegiance_id Integer,  
  state_id INTEGER, 
  type_id Integer, 
  has_commodities BOOL DEFAULT (null), 
  has_refuel BOOL DEFAULT (null), 
  has_repair BOOL DEFAULT (null), 
  has_rearm BOOL DEFAULT (null), 
  has_outfitting BOOL DEFAULT (null),  
  has_shipyard BOOL DEFAULT (null), 
  has_blackmarket BOOL DEFAULT (null),   
  eddb_updated_at Integer 
)
CREATE INDEX StationsIndex_ID  ON Stations (id ASC)
CREATE INDEX StationsIndex_system_ID  ON Stations (system_id ASC)
CREATE INDEX StationsIndex_system_Name  ON Stations (Name ASC)
```

Not updated at present.  EDDB sync does not occur as from V4.

# SystemNote
```
CREATE TABLE SystemNote (
  id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , 
  Name TEXT NOT NULL , 
  Time DATETIME NOT NULL , 
  Note TEXT
)
```

User notes created by UI.  Name is used to find the system so may occur across EDSM duplicates.

# Systems
```
CREATE TABLE Systems (
  id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE ,
  name TEXT NOT NULL COLLATE NOCASE ,   ; NOTE THE NOCASE! Case insensitive search
  x FLOAT,
  y FLOAT,
  z FLOAT,
  cr INTEGER, ; Unknown field use
  commandercreate TEXT,
  createdate DATETIME,
  commanderupdate TEXT,
  updatedate DATETIME,
  status INTEGER,
  population INTEGER ,

  Note TEXT,  ; NOT the note shown on screen, not in use in program currently.

; EDDB INFO
  id_eddb Integer,
  faction TEXT,
  government_id Integer,
  allegiance_id Integer,
  primary_economy_id Integer,
  security Integer,
  eddb_updated_at Integer,
  state Integer,
  needs_permit Integer,
  FirstDiscovery BOOL,
  versiondate DATETIME,

  id_edsm Integer ; EDSM Key used to link to EDSM and keep systems in sync
)
CREATE INDEX SystemsIndex ON Systems (name ASC)
CREATE INDEX Systems_EDDB_ID_Index ON Systems (id_eddb ASC)
CREATE INDEX Systems_EDSM_ID_Index ON Systems (id_edsm ASC)
CREATE INDEX IDX_Systems_versiondate ON Systems (versiondate ASC)
```

This is the system table populated by EDSM on a weekly sync, and updated on each run.  Only contains EDSM systems at present, since V4.  Any systems without EDSM ID are purged from the database as of V4.  id_edsm is used to sync to EDSM, not the name since V4.

EDSM data can contain duplicates of names.

cr is unknown use.

status is an enum of
 - Unknown = 0,
 - EDSC = 1,
 - RedWizzard = 2,
 - EDDiscovery = 3,
 - EDDB = 4, 
 - Inhumierer = 5
and records where the record was read from. 
EDSC is used for EDSM data.  The db only has EDSC entries as of V4.  In memory it may have EDDiscovery if the class was created automatically due to visited system entry having a x/y/z but does not having a matching db entry.

id_eddb records the ID used during the EDDB sync, but is not used for data matching. 

id_edsm is used for matching against the EDSM JSON file and update.  It is also used for hidden system removal. For the EDDB sync, the EDDB data contains the EDSM ID and thus this id_edsm is used to match the EDDB data to the EDSM DB entry.

FirstDiscovery is not in use


# TravelLogUnit
```
CREATE TABLE TravelLogUnit(
  id INTEGER PRIMARY KEY  NOT NULL,
  type INTEGER NOT NULL,
  name TEXT NOT NULL,
  size INTEGER,
  path TEXT
)
```

Names of netlog files searched by ED 

# VisitedSystems

```
CREATE TABLE VisitedSystems(
  id INTEGER PRIMARY KEY  NOT NULL,
  Name TEXT NOT NULL,
  Time DATETIME NOT NULL,
  Unit Text,
  Commander Integer,
  Source Integer,
  edsm_sync BOOL DEFAULT (null),
  Map_colour INTEGER DEFAULT (-65536),
  Status BOOL,
  X double,
  Y double,
  Z double
)
```

Obtained from ED netlog files.
Contains the Name, Time, logfile, commander, map colour, if sent to EDSM of the system visited information from the elite dangerous log files.
Canoniocal source of travelled places.
Since ED 2.1 it has x/y/z as well to indicate position that elite dangerous said the star was at.
source is the travel log unit number
status is not in use in the code.

# route_systems
```
CREATE TABLE route_systems (
  id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
  routeid INTEGER NOT NULL, 
  systemname TEXT NOT NULL
)
```

Contains route stars indexed into a route by routeid.  Order of id determines the order of the stars.

# route_expeditions
```
CREATE TABLE routes_expeditions (
  id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  name TEXT UNIQUE NOT NULL,
  start DATETIME,
  end DATETIME
)
```

Contain route expeditions.  A name, start/end date, and the id is the route ID used in route_systems.

# station_commodities
```
CREATE TABLE station_commodities (
  station_id INTEGER PRIMARY KEY NOT NULL,
  commodity_id INTEGER,
  type INTEGER
)
```

Not updated at present.  EDDB sync does not occur as from V4.

# wanted_systems
```
CREATE TABLE wanted_systems (
  id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  systemname TEXT UNIQUE NOT NULL
)
```

List of wanted systems, manually entered via the tri system.





