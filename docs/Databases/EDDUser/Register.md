# EDDUser.Register table

```sql
CREATE TABLE Register (
  ID TEXT NOT NULL PRIMARY KEY, 
  ValueInt INTEGER, 
  ValueDouble DOUBLE, 
  ValueString TEXT, 
  ValueBlob BLOB
)
```

Migrated from [EDDiscovery.Register](https://github.com/EDDiscovery/EDDiscovery/wiki/Databases-in-EDD#register) table
```sql
INSERT INTO EDDUser.Register SELECT * FROM EDDiscovery.Register
```
