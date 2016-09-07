# EDDSystems.EddbSystems table

```sql
CREATE TABLE EddbSystems (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  EdsmId INTEGER NOT NULL REFERENCES Systems (SystemEdsmId),
  EddbId INTEGER NOT NULL,
  Name TEXT NOT NULL,
  Faction TEXT,
  GovernmentId INTEGER,
  AllegianceId INTEGER,
  PrimaryEconomyId INTEGER,
  Security INTEGER,
  State INTEGER,
  NeedsPermit INTEGER,
  Population INTEGER,
  EddbUpdatedAt INTEGER   -- Seconds since 1970-01-01 00:00:00 UTC
)
CREATE UNIQUE INDEX EddbSystem_EdsmId ON EddbSystems (EdsmId)
CREATE UNIQUE INDEX EddbSystem_EddbId ON EddbSystems (EddbId)
```

Linked to a EDSM system entry by SystemEdsmId.

Re-imported from EDDB dump
