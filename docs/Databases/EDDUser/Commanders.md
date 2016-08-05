```sql
CREATE TABLE Commanders (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  LogPath TEXT,
  EDSMApiKey TEXT
)
```

Migrated from `EDCommander*` settings in register table
```sql
INSERT INTO EDDUser.Commanders
SELECT n.Num AS Id, n.Name, p.LogPath, k.EDSMApiKey
FROM (SELECT SUBSTR(ID,16) + 0 AS Num, ValueString AS Name 
      FROM EDDiscovery.Register 
      WHERE ID LIKE 'EDCommanderName%') AS n
JOIN (SELECT SUBSTR(ID,22) + 0 AS Num, ValueString AS LogPath 
      FROM EDDiscovery.Register 
      WHERE ID LIKE 'EDCommanderNetLogPath%') AS p ON p.Num = n.Num
JOIN (SELECT SUBSTR(ID,18) + 0 AS Num, ValueString AS EDSMApiKey 
      FROM EDDiscovery.Register 
      WHERE ID LIKE 'EDCommanderApiKey%') AS k ON k.Num = n.Num
```
