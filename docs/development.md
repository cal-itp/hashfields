# Development getting started

- Open in VS Code
- Rebuild and Reopen in Container
- Run `hashfields -h` in the devcontainer, review the usage notes

## Run the tests

Use the _Testing_ window in VS Code.

## Package an executable

The app is packaged as a standalone, self-contained executable with [`pyinstaller`](https://pyinstaller.org/).

### Docker

Package a version of `hashfields` for a Linux-based Docker container:

```console
docker compose build dist
```

Now run `dist` just like you would `hashfields`:

```console
$ docker compose run dist -v
hashfields <version>
```

### Local

Or package the app for your current system:

```console
pip install -e .[dist]

./bin/package.sh
```

The resulting executable is located at `./dist/hashfields`:

```console
$ ./dist/hashfields -v
hashfields <version>
```

## Make a release

Releases happen automatically with GitHub Actions. Push a calver version tag of
the form `YYYY.0M.N` where:

- `YYYY` is the 4 digit year
- `0M` is the 0-prefixed month, e.g. 01 for January, 10 for October
- `N` is the 1-based release number for the given month, incremented with
  each release that year and month

See [`.github/workflows/release.yml`][release.yml] for more details.

[release.yml]: https://github.com/cal-itp/hashfields/blob/main/.github/workflows/release.yml
