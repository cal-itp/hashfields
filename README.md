# hashfields

Selectively hash, drop, or keep fields from a flat file (e.g. CSV).

Cross-platform command-line tool built using [.NET Core 5.0](https://dotnet.microsoft.com/)

## Getting started

* Open in VS Code
* Rebuild and Reopen in Container
* Set a breakpoint in [`src/Cli/Program.cs`](src/Cli/Program.cs) and hit `F5` to run in debug mode

## Run the tests

Use the *Task Explorer* or enter the following commands:

### Run tests once

* `Ctrl/Cmd+P` to bring up the Command Palette
* Type `tasks: run test task` and hit `Enter`

### Watch/Run tests

* `Ctrl/Cmd+P` to bring up the Command Palette
* Type `tasks: run task` and hit `Enter`
* Type or select `watch-test` and hit `Enter`
* Edit a `src` or `tests` file to see the solution rebuild and tests rerun

## Deploy a self-contained app

The `Cli` app is deployed as a standalone, self-contained executable.

Test this (deploying to an otherwise-empty Alpine Linux target):

```bash
cd .devcontainer
docker compose up --build deploy-alpine
```

The `Cli` program's output is displayed in the terminal before the container shuts down.
