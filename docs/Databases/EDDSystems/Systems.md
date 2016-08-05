# EDDSystems.Systems table

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
CREATE INDEX System_Name ON Systems (Name)
CREATE INDEX System_EdsmId ON Systems (SystemEdsmId)
CREATE INDEX System_Coords ON Systems (Z, X, Y)
CREATE INDEX System_GridId on Systems (GridId)
CREATE INDEX System_RandomId on Systems (RandomId)
```

Re-imported from EDSM dump.  Grid ID and RandomID are used for new 3dmap star painting system.  Grid ID is assigned based on x/z position, RandomID is 0 to 99 inclusive.
