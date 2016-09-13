# EDDSystems.EdsmSystems table

```sql
CREATE TABLE EdsmSystems (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  EdsmId INTEGER NOT NULL,
  EddbId INTEGER,                                  -- NULL means unpopulated
  UpdateTimestamp INTEGER NOT NULL,                -- Seconds since 2015-01-01 00:00:00 UTC
  CreateTimestamp INTEGER NOT NULL,                -- Seconds since 2015-01-01 00:00:00 UTC
  VersionTimestamp INTEGER NOT NULL,               -- Seconds since 2015-01-01 00:00:00 UTC
  X INTEGER NOT NULL,                              -- X coordinate in 1/128ly units
  Y INTEGER NOT NULL,                              -- Y coordinate in 1/128ly units
  Z INTEGER NOT NULL,                              -- Z coordinate in 1/128ly units
  GridId INTEGER NOT NULL DEFAULT -1,
  RandomId INTEGET NOT NULL DEFAULT -1
)
CREATE INDEX EdsmSystems_EdsmId ON EdsmSystems (EdsmId)
CREATE INDEX EdsmSystems_EddbId ON EdsmSystems (EddbId)
CREATE INDEX EdsmSystems_Coords ON EdsmSystems (Z, X, Y)
CREATE INDEX EdsmSystems_GridId on EdsmSystems (GridId)
CREATE INDEX EdsmSystems_RandomId on EdsmSystems (RandomId)
```

Re-imported from EDSM dump.  Grid ID and RandomID are used for new 3dmap star painting system.  Grid ID is assigned based on x/z position, RandomID is 0 to 99 inclusive.

The X, Y and Z coordinates are stored in 1/128ly units from Sol, resulting in a 24-bit integer for most stars within the galaxy, and a 16-bit integer for stars within 256ly of Sol.
