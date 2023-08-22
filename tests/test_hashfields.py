import os
import subprocess

import pytest


def normalize_output(output: str):
    """Normalizes string output across OSes"""
    return output.replace("\r\r", "\r").strip()


@pytest.fixture
def hashfields():
    return "hashfields"


def test_hashfields_default(capfd, hashfields):
    subprocess.call([hashfields])
    capture = capfd.readouterr()

    assert normalize_output(capture.out) == ""
    assert normalize_output(capture.err) == ""


@pytest.mark.parametrize("arg_variant", ["-v", "--version"])
def test_hashfields_version(capfd, hashfields, arg_variant):
    subprocess.call([hashfields, arg_variant])
    capture = capfd.readouterr()

    assert "hashfields" in normalize_output(capture.out)
    assert normalize_output(capture.err) == ""


@pytest.mark.parametrize("arg_variant", ["-h", "--help"])
def test_hashfields_help(capfd, hashfields, arg_variant):
    subprocess.call([hashfields, arg_variant])
    capture = capfd.readouterr()

    assert "usage: hashfields" in normalize_output(capture.out)
    assert "options:" in normalize_output(capture.out)
    assert normalize_output(capture.err) == ""


def test_hashfields_stdin(capfd, hashfields, csv_string_io):
    subprocess.run([hashfields], input=csv_string_io.getvalue(), text=True)
    capture = capfd.readouterr()

    assert normalize_output(capture.out).startswith("one,two,three")


def test_hashfields_maintains_leading_zeros(capfd, hashfields):
    csv = """leading_zero,another_column
    01234,Something
    """

    subprocess.run([hashfields, "--skip", "leading_zero"], input=csv, text=True)
    capture = capfd.readouterr()
    normalized_out = normalize_output(capture.out)

    assert normalized_out.startswith("leading_zero,another_column")
    assert f"{os.linesep}01234," in normalized_out


@pytest.mark.parametrize(
    ("source_file", "expected_file"),
    [
        ("./tests/samples/large.csv", "./tests/samples/large.hashed.csv"),
        ("./tests/samples/small.csv", "./tests/samples/small.hashed.csv"),
    ],
)
def test_hashfields_file(capfd, hashfields, source_file, expected_file):
    with open(expected_file, "r") as f:
        expected = normalize_output(f.read()).replace("\n", os.linesep)

    subprocess.run([hashfields, "--input", source_file])
    capture = capfd.readouterr()

    assert normalize_output(capture.out) == expected


# skip test on windows because it reads files written for *nix OSes
def test_hashfields_skip_drop(capfd, hashfields):
    source_file = "./tests/samples/small.csv"
    expected_file = "./tests/samples/small.hashed.skip_sub.drop_type.csv"
    with open(expected_file, "r") as f:
        expected = normalize_output(f.read()).replace("\n", os.linesep)

    subprocess.run([hashfields, "--input", source_file, "--skip", "sub", "--drop", "type"])
    capture = capfd.readouterr()

    assert normalize_output(capture.out) == expected


# skip test on windows because it reads files written for *nix OSes
@pytest.mark.parametrize(
    ("source_file", "expected_file"),
    [
        ("./tests/samples/large.dupes.csv", "./tests/samples/large.hashed.csv"),
        ("./tests/samples/small.dupes.csv", "./tests/samples/small.hashed.csv"),
    ],
)
def test_hashfields_dedupe(capfd, hashfields, source_file, expected_file):
    with open(expected_file, "r") as f:
        expected = normalize_output(f.read()).replace("\n", os.linesep)

    subprocess.run([hashfields, "--input", source_file, "--dedupe"])
    capture = capfd.readouterr()

    assert normalize_output(capture.out) == expected


def test_hashfields_readme(capfd, hashfields):
    readme_input = normalize_output(
        """column1,column2,column3,column4
r1c1,r1c2,r1c3,r1c4
r2c1,r2c2,r2c3,r2c4
r3c1,r3c2,r3c3,r3c4
r4c1,r4c2,r4c3,r4c4
"""
    ).replace("\n", os.linesep)

    readme_output = normalize_output(
        """column2,column4
r1c2,fb66e41761a74ea0c042e1c226c04fa2ce1a1334d7063d86230d17f33f109b68
r2c2,6051c006caee661a6ccb390b8cf7a43230c5cd7b54861f7306a598b612f924b9
r3c2,7e32c53b7729f5dce7ac54232b7f2d93d6c78ed19fc8d096b0fde948f513e9dc
r4c2,f8d0624d128daf97c61ec28f4396e8f14be2ca2940d18fdf33e939cda9bd1824
"""
    ).replace("\n", os.linesep)

    subprocess.run(
        [hashfields, "--alg", "sha256", "--drop", "column1", "column3", "--skip", "column2"],
        input=readme_input,
        text=True,
    )
    capture = capfd.readouterr()
    normalized_out = normalize_output(capture.out)

    assert normalized_out == readme_output
