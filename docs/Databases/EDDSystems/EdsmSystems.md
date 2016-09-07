# EDDSystems.Systems table

```sql
CREATE TABLE EdsmSystems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL,
  SystemEddbId INTEGER,                            -- NULL means unpopulated
  UpdateTimestamp INTEGER NOT NULL,                -- Seconds since 2015-01-01 00:00:00 UTC
  CreateTimestamp INTEGER NOT NULL,                -- Seconds since 2015-01-01 00:00:00 UTC
  VersionTimestamp INTEGER NOT NULL,               -- Seconds since 2015-01-01 00:00:00 UTC
  X INTEGER,                                       -- X coordinate in 1/128ly units
  Y INTEGER,                                       -- Y coordinate in 1/128ly units
  Z INTEGER,                                       -- Z coordinate in 1/128ly units
  GridId INTEGER NOT NULL,
  RandomId INTEGET NOT NULL
)
CREATE INDEX System_EdsmId ON Systems (SystemEdsmId)
CREATE INDEX System_EddbId ON Systems (SystemEddbId)
CREATE INDEX System_Coords ON Systems (Z, X, Y)
CREATE INDEX System_GridId on Systems (GridId)
CREATE INDEX System_RandomId on Systems (RandomId)
```

Re-imported from EDSM dump.  Grid ID and RandomID are used for new 3dmap star painting system.  Grid ID is assigned based on x/z position, RandomID is 0 to 99 inclusive.

The X, Y and Z coordinates are stored in 1/64ly (or 1/32ly) units from Sol, resulting in a 24-bit integer for all stars within the galaxy, and a 16-bit integer for stars within 512ly (or 1024ly) of Sol.
