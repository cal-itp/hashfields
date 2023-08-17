import subprocess

import pytest


@pytest.fixture
def hashfields():
    return "hashfields"


def test_hashfields_default(capfd, hashfields):
    subprocess.call([hashfields])
    capture = capfd.readouterr()

    assert capture.out.strip() == ""
    assert capture.err.strip() == ""


@pytest.mark.parametrize("arg_variant", ["-v", "--version"])
def test_hashfields_version(capfd, hashfields, arg_variant):
    subprocess.call([hashfields, arg_variant])
    capture = capfd.readouterr()

    assert "hashfields" in capture.out
    assert capture.err.strip() == ""


@pytest.mark.parametrize("arg_variant", ["-h", "--help"])
def test_hashfields_help(capfd, hashfields, arg_variant):
    subprocess.call([hashfields, arg_variant])
    capture = capfd.readouterr()

    assert "usage: hashfields" in capture.out
    assert "options:" in capture.out
    assert capture.err.strip() == ""


def test_hashfields_stdin(capfd, hashfields, csv_string_io):
    subprocess.run([hashfields], input=csv_string_io.getvalue(), text=True)
    capture = capfd.readouterr()

    assert capture.out.startswith("one,two,three\n")


def test_hashfields_maintains_leading_zeros(capfd, hashfields):
    csv = """leading_zero,another_column
    01234,Something
    """

    subprocess.run([hashfields, "--skip", "leading_zero"], input=csv, text=True)
    capture = capfd.readouterr()

    assert capture.out.startswith("leading_zero,another_column")
    assert "\n01234," in capture.out


@pytest.mark.parametrize(
    ("source_file", "expected_file"),
    [
        ("./tests/samples/large.csv", "./tests/samples/large.hashed.csv"),
        ("./tests/samples/small.csv", "./tests/samples/small.hashed.csv"),
    ],
)
def test_hashfields_file(capfd, hashfields, source_file, expected_file):
    with open(expected_file, "r") as f:
        expected = f.read()

    subprocess.run([hashfields, "--input", source_file])
    capture = capfd.readouterr()

    assert capture.out == expected


def test_hashfields_skip_drop(capfd, hashfields):
    source_file = "./tests/samples/small.csv"
    expected_file = "./tests/samples/small.hashed.skip_sub.drop_type.csv"
    with open(expected_file, "r") as f:
        expected = f.read()

    subprocess.run([hashfields, "--input", source_file, "--skip", "sub", "--drop", "type"])
    capture = capfd.readouterr()

    assert capture.out == expected


@pytest.mark.parametrize(
    ("source_file", "expected_file"),
    [
        ("./tests/samples/large.dupes.csv", "./tests/samples/large.hashed.csv"),
        ("./tests/samples/small.dupes.csv", "./tests/samples/small.hashed.csv"),
    ],
)
def test_hashfields_dedupe(capfd, hashfields, source_file, expected_file):
    with open(expected_file, "r") as f:
        expected = f.read()

    subprocess.run([hashfields, "--input", source_file, "--dedupe"])
    capture = capfd.readouterr()

    assert capture.out == expected
