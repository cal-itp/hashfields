# Usage

## Get the executable

`hashfields` is a command-line executable program. Get the latest release for
your platform:

### Download a pre-built binary

From the [Releases page][releases]

### Install from source

Clone and install from the `main` branch:

```console
git clone https://github.com/cal-itp/hashfields

cd hashfields

pip install -e .
```

## Run the executable

```console
$ hashfields --help

usage: hashfields [-h] [-v] [-a {sha256,sha384,sha512}] [-t DELIMITER]
                  [-d DROP [DROP ...]] [-s SKIP [SKIP ...]]
                  [-i INPUT] [-o OUTPUT]

options:
  -h, --help            show this help message and exit
  -v, --version         show program's version number and exit
  -a {sha256,sha384,sha512}, --alg {sha256,sha384,sha512}
                        The hash algorithm to use.
  -t DELIMITER, --delimiter DELIMITER
                        Field delimiter in the input data.
  -d DROP [DROP ...], --drop DROP [DROP ...]
                        Column names to drop from the output.
  -s SKIP [SKIP ...], --skip SKIP [SKIP ...]
                        Column names to skip hashing in the output.
  -i INPUT, --input INPUT
                        Readable location for input data.
  -o OUTPUT, --output OUTPUT
                        Writable location for output results.
```

### Providing input to `hashfields`

#### `stdin`

By default `hashfields` reads from `stdin`. Pipe data in the Unix-style:

```console
echo "one,two,three" | hashfields
```

### File input

Alternatively, `hashfields` can read from a file:

```console
hashfields --input /path/to/file.csv
```

### `hashfields` Output

### `stdout`

By default `hashfields` writes to `stdout`:

```console
$ echo "one,two,three" | hashfields
one,two,three
```

### File output

Alternatively, `hashfields` can write to a file:

```console
hashfields --output /path/to/file.csv
```

[releases]: https://github.com/cal-itp/hashfields/releases
