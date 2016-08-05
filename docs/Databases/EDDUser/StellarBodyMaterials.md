```sql
CREATE TABLE StellarBodyMaterials (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  StellarBodyId INTEGER NOT NULL REFERENCES StellarBodies (Id),
  MaterialName TEXT NOT NULL,
  Concentration DOUBLE
)
```

Filled by scans of stellar bodies
