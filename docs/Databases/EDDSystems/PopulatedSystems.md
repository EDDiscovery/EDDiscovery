# EDDSystems.PopulatedSystems table

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

`LastUpdated` can be converted from `last_updated_at` using:
```C#
LastUpdated = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((long)jo["last_updated_at"]);
```
