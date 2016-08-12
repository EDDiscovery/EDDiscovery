# ProcgenSectors table

```
CREATE TABLE ProcgenSectors (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SectorName TEXT NOT NULL COLLATE NOCASE,
  BaseX DOUBLE,
  BaseY DOUBLE,
  BaseZ DOUBLE
)
CREATE UNIQUE INDEX ProcgenSectors_SectorName ON ProcgenSectors (SectorName)
```

Table for storing Procgen sector names (e.g. `Barnard's Loop Sector` or `Ceeckia`) for compact storage of procgen systems
