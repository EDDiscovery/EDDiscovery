# EDDSystems.Systems table

```sql
CREATE TABLE Systems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL UNIQUE,
  Name TEXT COLLATE NOCASE,                        -- for named systems
  SectorId INTEGER REFERENCES ProcgenSectors (Id), -- for procgen systems
  GridReference INTEGER,                           -- 3-bit LSB star class, 21-bit MSB grid ref
  GridRefSequence INTEGER,
  LastUpdated DATETIME,
  X INTEGER,                                       -- X coordinate in 1/64ly units
  Y INTEGER,                                       -- Y coordinate in 1/64ly units
  Z INTEGER,                                       -- Z coordinate in 1/64ly units
  GridId INTEGER NOT NULL,
  RandomId INTEGET NOT NULL
)
CREATE INDEX System_Name ON Systems (Name)
CREATE INDEX System_ProcGen ON Systems (SectorId,GridReference,GridRefSequence)
CREATE INDEX System_EdsmId ON Systems (SystemEdsmId)
CREATE INDEX System_Coords ON Systems (Z, X, Y)
CREATE INDEX System_GridId on Systems (GridId)
CREATE INDEX System_RandomId on Systems (RandomId)
```

Re-imported from EDSM dump.  Grid ID and RandomID are used for new 3dmap star painting system.  Grid ID is assigned based on x/z position, RandomID is 0 to 99 inclusive.

For procgen systems: SectorId, GridReference and GridRefSequence are used to store the star name in a compact form.  See the [RV Sonnenkreis - Decoding Universal Cartographics](https://forums.frontier.co.uk/showthread.php/196297-RV-Sonnenkreis-Decoding-Universal-Cartographics) thread for details on decoding the `AB-C d1-0` name.  SectorId, GridReference and GridRefSequence are used when Name is null, and are ignored when Name is not null.

TBD: The grid reference will likely be compressed by allocating only as many bits as necessary for the star class - e.g. 21 bits (128x128x128) for class `a`, 12 bits (16x16x16) for class `d`, and 0 bits (1x1x1) for class `h`

The X, Y and Z coordinates are stored in 1/64ly (or 1/32ly) units from Sol, resulting in a 24-bit integer for all stars within the galaxy, and a 16-bit integer for stars within 512ly (or 1024ly) of Sol.
