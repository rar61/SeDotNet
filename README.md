# SeDotNet

An experiment for launching Space Engineers server under .NET.

**Current Status:** The server and GUI are generally functional.

### Building
The `GameBinaries` property must be set via a `.user` file, a command line parameter (e.g. `-p:GameBinaries=...`), or by placing the dedicated server at the default path (next to the project folder): `SpaceEngineersDedicatedServer/`.

### Running
The application expects the dedicated server to be located in a `SpaceEngineersDedicatedServer/` folder next to the application's execution directory.
