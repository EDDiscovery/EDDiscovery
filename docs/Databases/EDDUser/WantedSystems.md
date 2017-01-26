# EDDUser.WantedSystems table

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
