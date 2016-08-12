# ProcgenSectors table

```
CREATE TABLE ProcgenSectors (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SectorName TEXT NOT NULL UNIQUE,
  BaseX DOUBLE,
  BaseY DOUBLE,
  BaseZ DOUBLE
)
```

Table for storing Procgen sector names (e.g. `Barnard's Loop Sector` or `Ceeckia`) for compact storage of procgen systems
