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
      pgsect.SectorName || 
      ' ' || 
      CHAR(65 + ((ProcgenGridReference >> 3) & 31)) ||
      CHAR(65 + ((ProcgenGridReference >> 8) & 31)) ||
      '-' ||
      CHAR(65 + ((ProcgenGridReference >> 13) & 31)) ||
      ' ' ||
      CHAR(97 + (ProcgenGridReference & 7)) ||
      CASE WHEN (ProcgenGridReference >> 18) != 0 THEN (ProcgenGridReference >> 18) || '-' ELSE '' END ||
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
FROM Systems s
LEFT JOIN PopulatedSystems p ON p.SystemEdsmId = s.SystemEdsmId
LEFT JOIN ProcgenSectors pgsect ON pgsect.Id = s.SectorId
```

Combination of Systems and PopulatedSystems into a single view
