from importlib.metadata import version, PackageNotFoundError

try:
    __version__ = version("hashfields")
except PackageNotFoundError:
    # package is not installed
    pass
