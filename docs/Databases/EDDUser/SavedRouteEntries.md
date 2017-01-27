# EDDUser.SavedRouteEntries table

```sql
CREATE TABLE SavedRouteEntries (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  RouteId INTEGER NOT NULL REFERENCES SavedRoutes (Id),
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER,
  Ordinal INTEGER
)
CREATE INDEX SavedRouteEntry_RouteId ON SavedRouteEntries (RouteId)
```

Migrated from the [`EDDiscovery.route_systems`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#route_systems) table
```sql
INSERT INTO EDDUser.SavedRouteEntries (Id, RouteId, SystemName)
SELECT * FROM route_systems
```
