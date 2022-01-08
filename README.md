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

## Usage

### Get the executable

`hashfields` is a command-line executable program. Get the latest release for
your platform:

#### Download a pre-built binary

From the [Releases page](https://github.com/cal-itp/hashfields/releases)

#### Build from source

Clone and build from the `main` branch:

```bash
git clone https://github.com/cal-itp/hashfields
cd hashfields
dotnet build
dotnet publish -r <runtime> -c Release -o <output> src/Cli/hashfields.csproj
```

* `<runtime>` is based on your platform: `linux-x64`, `osx.11.0-x64`, `win10-x64`
* `<output>` is the folder to place the build artifacts and executable

### Configure runtime settings

In the same directory as the executable, create an `appsettings.json` file using
[the sample][appsettings.json] as a guide.

A description of each of the settings (by section) follows:

#### `DataOptions`

| Setting           | Default  | Description                                                |
| ----------------- | -------- | ---------------------------------------------------------- |
| `Delimiter`       | `,`      | The string delimiter used between fields of source data.   |
| `Drop`            | `[]`     | A list of column names to drop in the output.              |
| `HashAlgorithm`   | `SHA512` | The algorithm to use when hashing column values.           |
| `HyphenateHashes` | `true`   | If `true` hashed strings use hyphens between hexadecimal digits. If `false`, hyphens are not used. |
| `LowercaseHashes` | `false`  | If `true` hashed strings are output using lowercase hexadecimal digits. If `false`, uppercase hexadecimal digits.       |
| `Skip`            | `[]`     | A list of column names to skip from hashing in the output. |

#### `StreamOptions.Input`

| Setting | Default | Description                                                           |
| --------| ------- | --------------------------------------------------------------------- |
| `Type`  | `StdIn` | The type of source to read data from. One of `File` or `StdIn`.       |
| `Path`  |         | Required for `Type: File`; the path to a readble delimited data file. |

#### `StreamOptions.Output`

| Setting | Default  | Description                                                           |
| --------| -------- | --------------------------------------------------------------------- |
| `Type`  | `StdOut` | The type of target to write data to. One of `File` or `StdOut`.       |
| `Path`  |          | Required for `Type: File`; the path to a writable file.               |

### Run the executable

**Windows Powershell:**

```powershell
.\hashfields
```

**Linux/macOS terminals:**

```bash
./hashfields
```

## Development getting started

* Open in VS Code
* Rebuild and Reopen in Container
* Review the [`appsettings.json`][appsettings.json] and [sample data][data.csv] files
* Set a breakpoint in [`src/Cli/Program.cs`][program.cs] and hit `F5` to run in debug mode
* After the run completes, look in your local `src/Cli/sample` directory for the hashed output file

### Run the tests

Use the *Task Explorer* or enter the following commands:

#### Run tests once

* `Ctrl/Cmd+P` to bring up the Command Palette
* Type `tasks: run test task` and hit `Enter`

#### Watch/Run tests

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

## Make a release

Releases happen automatically with GitHub Actions. Push a calver version tag of
the form `YYYY.0M.N` where:

* `YYYY` is the 4 digit year
* `0M` is the 0-prefixed month, e.g. 01 for January, 10 for October
* `N` is the 1-based release number for the given month, incremented with
  each release that year and month

See [`.github/workflows/release.yml`][release.yml] for more details.

[appsettings.json]: src/Cli/appsettings.json
[data.csv]: src/Cli/samples/data.csv
[program.cs]: src/Cli/Program.cs
[release.yml]: .github/workflows/release.yml
