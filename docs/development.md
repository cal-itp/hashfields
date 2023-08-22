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

Releases run automatically with GitHub Actions.

### Push a tag

The release process starts by pushing a [calver](https://calver.org/) version
tag of the form `YYYY.0M.N` where:

- `YYYY` is the 4 digit year
- `0M` is the 0-prefixed month, e.g. 01 for January, 10 for October
- `N` is the 1-based release number for the given month, incremented with
  each release that year and month

Pushing a new tag runs the [`release.yml`](https://github.com/cal-itp/hashfields/blob/main/.github/workflows/release.yml)
workflow.

A new [GitHub Release](https://github.com/cal-itp/hashfields/releases) is created automatically, with pre-build binaries
for each platform attached as Release Assets. Edit the release notes as-needed.

### Publish to PyPI

When a new release is published from the workflow above, the
[`pypi.yaml`](https://github.com/cal-itp/hashfields/blob/main/.github/workflows/pypi.yml) runs to publish the new package to
PyPI.
