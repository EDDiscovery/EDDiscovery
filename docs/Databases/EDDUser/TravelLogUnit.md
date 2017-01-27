# EDDUser.Journals table

We use the old TravelLogunit table. but with added commander ID.   Everything named Journals was confusing. 

```sql

TravelLogUnit

CREATE TABLE TravelLogUnit(
  id INTEGER PRIMARY KEY  NOT NULL,
  type INTEGER NOT NULL,
  name TEXT NOT NULL,
  size INTEGER,
  path TEXT,
  CommanderId INTEGER REFERENCES Commanders (Id),
)
Names of netlog files searched by ED



CREATE TABLE Journals (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Type INTEGER NOT NULL,
  Name TEXT NOT NULL COLLATE NOCASE,
  CommanderId INTEGER REFERENCES Commanders (Id),
  Path TEXT COLLATE NOCASE,
  Size INTEGER
)
CREATE INDEX Journal_Name ON Journals (Name)
CREATE INDEX Journal_Commander ON Journals (CommanderId)
```

Migrated from [`EDDiscovery.TravelLogUnit`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#travellogunit) table.

Type:
* `NetLog`: 1 : travel log is a netlog
* `EDSM`: 2 :travel log was fetched from EDSM
* `Journal`: 3 :travel log is a E:D 2.2 journal
