```sql
CREATE TABLE PopulatedSystems (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  SystemEdsmId INTEGER NOT NULL UNIQUE REFERENCES Systems (SystemEdsmId),
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
CREATE INDEX PopulatedSystem_SystemEdsmId ON PopulatedSystems (SystemEdsmId)
CREATE INDEX PopulatedSystem_SystemEddbId ON PopulatedSystems (SystemEddbId)
```

Linked to a EDSM system entry by SystemEdsmId.

Re-imported from EDDB dump
