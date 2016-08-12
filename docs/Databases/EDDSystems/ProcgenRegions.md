# ProcgenRegions table

```
CREATE TABLE ProcgenRegions (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE,
  BaseX DOUBLE,
  BaseY DOUBLE,
  BaseZ DOUBLE
)
```

Table for storing Procgen system region names (e.g. `Barnard's Loop Sector` or `Ceeckia`) for compact storage of procgen systems
