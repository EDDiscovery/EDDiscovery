# EDDSystems.SystemsView view

```sql
CREATE VIEW SystemsView
SELECT 
  s.Id AS SystemId, 
  p.Id AS PopulatedSystemId,
  s.SystemEdsmId,
  s.LastUpdated AS EdsmLastUpdated,
  CASE 
    WHEN s.Name IS NOT NULL THEN s.Name
    ELSE
      pgsect.SectorName + 
      ' ' + 
      CHAR(65 + (ProcgenGridRef % 26)) +
      CHAR(65 + ((ProcgenGridRef / 26) % 26)) +
      '-' +
      CHAR(65 + ((ProcgenGridRef / (26 * 26)) % 26)) +
      ' ' +
      CHAR(104 - ProcgenStarClass) +
      CASE WHEN ProcgenGridRef >= (26 * 26 * 26) THEN CAST((ProcgenGridRef / (26 * 26 * 26)) AS TEXT) + '-' ELSE '' END +
      CAST(ProcgenGridRefSequence AS TEXT)
  END AS Name
  s.X / 64.0 AS X,
  s.Y / 64.0 AS Y,
  s.Z / 64.0 AS Z,
  s.GridId,
  p.SystemEddbId,
  p.LastUpdated AS EddbLastUpdated,
  p.Faction,
  p.Government,
  p.Allegiance,
  p.Power,
  p.PowerState,
  p.PrimaryEconomy,
  p.Security,
  p.State,
  p.SimbadRef,
  p.NeedsPermit,
  p.Population
FROM (
  SELECT
    *,
    ((ProcgenGridReference >> 3) & ((1 << ProcgenStarClass) - 1)) +
    ((ProcgenGridReference >> (3 + ProcgenStarClass)) & ((1 << ProcgenStarClass) - 1)) * 128 +
    ((ProcgenGridReference >> (3 + ProcgenStarClass * 2)) & ((1 << ProcgenStarClass) - 1)) * 16384 AS ProcgenGridRef
  FROM (SELECT *, ProcgenGridReference & 7 AS ProcgenStarClass FROM Systems)
) s
LEFT JOIN PopulatedSystems p ON p.SystemEdsmId = s.SystemEdsmId
LEFT JOIN ProcgenSectors pgsect ON pgsect.Id = s.SectorId
```

Combination of Systems and PopulatedSystems into a single view
