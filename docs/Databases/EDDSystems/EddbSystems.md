# EDDSystems.EddbSystems table

```sql
CREATE TABLE EddbSystems (
  Id INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  EdsmId INTEGER NOT NULL,
  EddbId INTEGER NOT NULL,
  Name TEXT NOT NULL,
  Population INTEGER,
  Faction TEXT,
  GovernmentId INTEGER,
  AllegianceId INTEGER,
  PrimaryEconomyId INTEGER,
  Security INTEGER,
  EddbUpdatedAt INTEGER,   -- Seconds since 1970-01-01 00:00:00 UTC
  State INTEGER,
  NeedsPermit INTEGER
)
CREATE UNIQUE INDEX EddbSystems_EdsmId ON EddbSystems (EdsmId)
CREATE UNIQUE INDEX EddbSystems_EddbId ON EddbSystems (EddbId)
```

Linked to a EDSM system entry by SystemEdsmId.

Re-imported from EDDB dump

Introduced in 7b90386
