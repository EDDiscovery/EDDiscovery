# EDDSystems database

This database (`EDDSystems.sqlite`) contains the system data sourced from EDSM and EDDB.

It contains the following tables:
* [`Systems`](EDDSystems/Systems.md): System data imported from EDSM
* [`SystemAliases`](EDDSystems/SystemAliases.md): Systems that have been renamed or hidden in EDSM
* [`PopulatedSystems`](EDDSystems/PopulatedSystems.md): Data on populated systems imported from EDDB
* [`ProcgenSectors`](EDDSystems/ProcgenSectors.md): Procedurally generated sector names

EDSM data is stored in the Systems table. EDDB data is stored in the PopulatedSystems table.

It also contains the following view:
* [`SystemsView`](EDDSystems/SystemsView.md): Combination of Systems and PopulatedSystems
