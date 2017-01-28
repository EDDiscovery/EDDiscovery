# EDDSystem.SystemNames

``` sql
CREATE TABLE EDDSystem.SystemNames (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  EdsmId INTEGER NOT NULL,
  Name TEXT
)
```

Contains all of the system names.

Names are separated out to a separate table in order to reduce the amount of data that needs to be read from the database at startup.
