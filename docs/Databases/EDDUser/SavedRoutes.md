# EDDUser.SavedRoutes table

```sql
CREATE TABLE SavedRoutes (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE,
  Start DATETIME,
  End DATETIME
)
```

Migrated from the [`EDDiscovery.routes_expeditions`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#route_expeditions) table
```sql
INSERT INTO EDDUser.SavedRoutes SELECT * FROM EDDiscovery.routes_expeditions
```
