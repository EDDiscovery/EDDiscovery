```sql
CREATE TABLE StellarBodyStarDetails (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  StellarBodyId INTEGER NOT NULL UNIQUE REFERENCES StellarBodies (Id),
  StarType TEXT, -- [Stellar classification](https://en.wikipedia.org/wiki/Stellar_classification) of star
  StellarMass DOUBLE, -- [Solar mass](https://en.wikipedia.org/wiki/Solar_mass) of star
  AbsoluteMagnitude DOUBLE -- [Absolute magnitude](https://en.wikipedia.org/wiki/Absolute_magnitude) of star
)
```

Filled by scans of stellar bodies.
