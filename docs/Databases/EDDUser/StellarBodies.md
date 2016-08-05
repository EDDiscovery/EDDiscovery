```sql
CREATE TABLE StellarBodies (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  BodyName TEXT NOT NULL,
  DistanceFromArrivalLS DOUBLE, -- Distance from arrival point in Light Seconds
  Radius DOUBLE, -- Radius of body in metres
  OrbitalPeriod DOUBLE, -- Orbital period in seconds
  RotationPeriod DOUBLE, -- Rotation period in seconds
  Rings INTEGER -- Number of rings
)
```

Filled by scans of stellar bodies.
