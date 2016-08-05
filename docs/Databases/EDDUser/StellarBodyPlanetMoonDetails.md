# EDDUser.StellarBodyPlanetMoonDetails table

```sql
CREATE TABLE StellarBodyPlanetMoonDetails (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  StellarBodyId INTEGER NOT NULL UNIQUE REFERENCES StellarBodies (Id),
  TidalLock BOOL, -- True if tidally locked to parent body
  TerraformState TEXT, -- Terraformable, Terraforming, Terraformed, or null
  PlanetClass TEXT, -- Type of planet or moon - e.g. Metal rich body, Rocky body
  Atmosphere TEXT, -- Type of atmosphere
  Vulcanism TEXT,
  SurfaceTemperature DOUBLE,
  SurfacePressure DOUBLE,
  SurfaceGravity DOUBLE,
  PlanetMass DOUBLE, -- Mass of body as multiple of [Earth's mass](https://en.wikipedia.org/wiki/Earth_mass)
  Landable BOOL
)
```

Filled by scans of stellar bodies.
