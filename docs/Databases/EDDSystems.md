# EDDSystem database

This database (`EDDSystem.sqlite`) contains the system data sourced from EDSM and EDDB.

It contains the following tables:
* [`EdsmSystems`](EDDSystems/EdsmSystems.md): System data imported from EDSM
* [`SystemNames`](EDDSystems/SystemNames.md): System names and EDSM IDs
* [`SystemAliases`](EDDSystems/SystemAliases.md): Systems that have been renamed or hidden in EDSM
* [`EddbSystems`](EDDSystems/EddbSystems.md): Data on populated systems imported from EDDB

EDSM data is stored in the EdsmSystems table. EDDB data is stored in the EddbSystems table.

System names are separated out to a separate SystemNames table in order to reduce the amount of data that needs to be read on startup.

It also contains the following view:
* [`SystemsView`](EDDSystems/SystemsView.md): Combination of Systems and PopulatedSystems
