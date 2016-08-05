```sql
CREATE TABLE Bookmarks (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  SystemEdsmId INTEGER,
  X DOUBLE,
  Y DOUBLE,
  Z DOUBLE,
  Time DATETIME,
  StarName TEXT,
  Heading TEXT,
  Note TEXT
)
```
Linked to system by the `SystemEdsmId` column (may be Null if no EDSM star)
StarName = null then Heading is set, region mark
StarName != null then heading is null, bookmark

Migrated from the `EDDiscovery.Bookmarks` table. 

```sql
INSERT INTO EDDUser.Bookmarks (Id, StarName, X, Y, Z, Time, Heading, Note)
SELECT * FROM EDDiscovery.Bookmarks
```
