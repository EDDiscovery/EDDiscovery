```sql
CREATE TABLE SystemNotes (
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  SystemName TEXT NOT NULL COLLATE NOCASE,
  SystemEdsmId INTEGER,
  Time DATETIME NOT NULL, 
  Note TEXT
)
CREATE INDEX SystemNote_SystemName ON SystemNotes (SystemName)
CREATE INDEX SystemNote_EdsmId ON SystemNotes (SystemEdsmId)
```

User system notes created by UI.

Linked to EDSM system by the `SystemEdsmId` column.  SystemEdsmId may be null if system does not yet exist in EDSM.

If SystemEdsmId is null, then system is linked using SystemName.

Migrated from the [`EDDiscovery.SystemNote`](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#systemnote) table

```sql
INSERT INTO EDDUser.SystemNotes (Id, SystemName, Time, Note)
SELECT Id, Name AS SystemName, Time, Note FROM EDDiscovery.SystemNote
```
