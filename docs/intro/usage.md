# Usage

## Get the executable

`hashfields` is a command-line executable program. Get the latest release for
your platform:

### Download a pre-built binary

From the [Releases page][releases]

### Build from source

Clone and build from the `main` branch:

```bash
git clone https://github.com/cal-itp/hashfields
cd hashfields
dotnet build
dotnet publish -r <runtime> -c Release -o <output> src/Cli/hashfields.csproj
```

* `<runtime>` is based on your platform: `linux-x64`, `osx.11.0-x64`, `win10-x64`
* `<output>` is the folder to place the build artifacts and executable

## Configure runtime settings

In the same directory as the executable, create an `appsettings.json` file using
[the sample][appsettings.json] as a guide.

A description of each of the settings (by section) follows:

### `DataOptions`

| Setting           | Default  | Description                                                |
| ----------------- | -------- | ---------------------------------------------------------- |
| Delimiter       | `,`      | The string delimiter used between fields of source data.   |
| Drop            | `[]`     | A list of column names to drop in the output.              |
| HashAlgorithm   | `SHA512` | The algorithm to use when hashing column values.           |
| HyphenateHashes | `true`   | If `true` hashed strings use hyphens between hexadecimal digits. If `false`, hyphens are not used. |
| LowercaseHashes | `false`  | If `true` hashed strings are output using lowercase hexadecimal digits. If `false`, uppercase hexadecimal digits. |
| Skip            | `[]`     | A list of column names to skip from hashing in the output. |

### `StreamOptions.Input`

| Setting | Default   | Description                                                           |
| --------| --------- | --------------------------------------------------------------------- |
| Type    | `StdIn`   | The type of source to read data from. One of `File` or `StdIn`.       |
| Path    |           | Required for `Type: File`; the path to a readable delimited data file. |

### `StreamOptions.Output`

| Setting | Default  | Description                                                           |
| --------| -------- | --------------------------------------------------------------------- |
| Type    | `StdOut` | The type of target to write data to. One of `File` or `StdOut`.       |
| Path    |          | Required for `Type: File`; the path to a writeable file.              |

## Run the executable

**Windows Powershell:**

```powershell
.\hashfields
```

**Linux/macOS terminals:**

```bash
./hashfields
```

[appsettings.json]: https://github.com/cal-itp/hashfields/blob/main/src/Cli/appsettings.json
[releases]: https://github.com/cal-itp/hashfields/releases
