# SeDotNet

An experiment for launching Space Engineers server under .NET.

**Current Status:** The GUI is currently unsupported; use the `-console` flag to run in console mode.

### Building
The `GameBinaries` property must be set via a `.user` file, a command line parameter (e.g. `-p:GameBinaries=...`), or by placing the dedicated server at the default path (next to the project folder): `SpaceEngineersDedicatedServer/`.

### Running
The application expects the dedicated server to be located in a `SpaceEngineersDedicatedServer/` folder next to the application's execution directory.
