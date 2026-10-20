# TowerDefense

- [Report](../report/report.md).

From this directory, run:

```sh
dotnet run --project TowerDefense/TowerDefense.csproj
```

Left-click to build a tower for 60 gold. Press Space to start the next wave,
P to pause, R to restart, or Esc to exit. Defend the base through five waves.

Run checks without opening a game window:

```sh
dotnet run --project TowerDefense.Checks/TowerDefense.Checks.csproj --configuration Release
```
