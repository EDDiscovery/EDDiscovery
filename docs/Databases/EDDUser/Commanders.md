# EDDUser.Commanders table

```sql
CREATE TABLE Commanders (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  EdsmApiKey TEXT NOT NULL,
  NetLogPath TEXT,
  Deleted INTEGER NOT NULL
)
```

Migrated from `EDCommander*` settings in register table
