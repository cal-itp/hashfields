# Development getting started

* Open in VS Code
* Rebuild and Reopen in Container
* Review the [`appsettings.json`][appsettings.json] and [sample data][data.csv] files
* Set a breakpoint in [`src/Cli/Program.cs`][program.cs] and hit `F5` to run in debug mode
* After the run completes, look in your local `src/Cli/samples` directory for the hashed output file

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
docker compose up --build deploy-alpine
```

The `Cli` program's output is displayed in the terminal before the container shuts down.

## Make a release

Releases happen automatically with GitHub Actions. Push a calver version tag of
the form `YYYY.0M.N` where:

* `YYYY` is the 4 digit year
* `0M` is the 0-prefixed month, e.g. 01 for January, 10 for October
* `N` is the 1-based release number for the given month, incremented with
  each release that year and month

See [`.github/workflows/release.yml`][release.yml] for more details.

[appsettings.json]: https://github.com/cal-itp/hashfields/blob/main/src/Cli/appsettings.json
[data.csv]: https://github.com/cal-itp/hashfields/blob/main/src/Cli/samples/data.csv
[program.cs]: https://github.com/cal-itp/hashfields/blob/main/src/Cli/Program.cs
[release.yml]: https://github.com/cal-itp/hashfields/blob/main/.github/workflows/release.yml
