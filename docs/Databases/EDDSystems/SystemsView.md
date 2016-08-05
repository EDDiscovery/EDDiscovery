```sql
CREATE VIEW SystemsView
SELECT 
  s.Id AS SystemId, 
  p.Id AS PopulatedSystemId,
  s.SystemEdsmId,
  s.LastUpdated AS EdsmLastUpdated,
  s.Name,
  s.X,
  s.Y,
  s.Z,
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
FROM Systems edsm
LEFT JOIN PopulatedSystems eddb ON eddb.SystemEdsmId = edsm.SystemEdsmId
```

Combination of Systems and PopulatedSystems into a single view
