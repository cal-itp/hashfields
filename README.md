# hashfields

Selectively hash, drop, or keep fields from a flat file (e.g. CSV).

Cross-platform command-line tool built using [.NET Core 5.0](https://dotnet.microsoft.com/)

## Example

Given this input CSV file:

```csv
column1,column2,column3,column4
r1c1,r1c2,r1c3,r1c4
r2c1,r2c2,r2c3,r2c4
r3c1,r3c2,r3c3,r3c4
r4c1,r4c2,r4c3,r4c4
```

And this configuration:

* `SHA256` hash algorithm
* drop `column1`, `column3`
* skip `column2`
* output hashed strings in lowercase and remove hyphens

`hashfields` produces this CSV file:

```csv
column2,column4
r1c2,fb66e41761a74ea0c042e1c226c04fa2ce1a1334d7063d86230d17f33f109b68
r2c2,6051c006caee661a6ccb390b8cf7a43230c5cd7b54861f7306a598b612f924b9
r3c2,7e32c53b7729f5dce7ac54232b7f2d93d6c78ed19fc8d096b0fde948f513e9dc
r4c2,f8d0624d128daf97c61ec28f4396e8f14be2ca2940d18fdf33e939cda9bd1824
```

## Development getting started

* Open in VS Code
* Rebuild and Reopen in Container
* Review the [`appsettings.json`](https://github.com/cal-itp/hashfields/blob/main/src/Cli/appsettings.json) and [sample data](https://github.com/cal-itp/hashfields/blob/main/src/Cli/samples/data.csv) files
* Set a breakpoint in [`src/Cli/Program.cs`](src/Cli/Program.cs) and hit `F5` to run in debug mode
* After the run completes, look in your local `src/Cli/sample` directory for the hashed output file

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
