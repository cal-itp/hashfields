import sys

import pytest

from hashfields.main import RESULT_SUCCESS, main
from hashfields.hashing import DEFAULT_ALG


@pytest.fixture
def mock_csv_read(mocker):
    return mocker.patch("hashfields.main.csv.read")


@pytest.fixture
def mock_csv_write(mocker):
    return mocker.patch("hashfields.main.csv.write")


@pytest.fixture
def mock_hashing_hash_data(mocker):
    return mocker.patch("hashfields.main.hashing.hash_data")


def test_main_default(mock_csv_read, mock_hashing_hash_data, mock_csv_write):
    result = main([])

    assert result == RESULT_SUCCESS

    mock_csv_read.assert_called_once()
    assert mock_csv_read.call_args.args == (sys.stdin, ",")

    mock_hashing_hash_data.assert_called_once()
    assert mock_hashing_hash_data.call_args.kwargs["hash_alg"] == DEFAULT_ALG
    assert mock_hashing_hash_data.call_args.kwargs["skip"] == []
    assert mock_hashing_hash_data.call_args.kwargs["drop"] == []

    mock_csv_write.assert_called_once()
    assert sys.stdout in mock_csv_write.call_args.args


@pytest.mark.usefixtures("mock_csv_read", "mock_csv_write")
@pytest.mark.parametrize("arg_variant", ["-a", "--alg"])
def test_main_hash_alg(mock_hashing_hash_data, hash_alg, arg_variant):
    main([arg_variant, hash_alg])

    mock_hashing_hash_data.assert_called_once()
    assert mock_hashing_hash_data.call_args.kwargs["hash_alg"] == hash_alg


@pytest.mark.usefixtures("mock_hashing_hash_data", "mock_csv_write")
@pytest.mark.parametrize("arg_variant", ["-t", "--delimiter"])
def test_main_delimiter(mock_csv_read, arg_variant):
    delim = "|"
    main([arg_variant, delim])

    mock_csv_read.assert_called_once()
    assert delim in mock_csv_read.call_args.args


@pytest.mark.usefixtures("mock_csv_read", "mock_csv_write")
@pytest.mark.parametrize("arg_variant", ["-s", "--skip"])
def test_main_skip(mock_hashing_hash_data, arg_variant):
    skip = ["one", "two"]
    main([arg_variant, *skip])

    mock_hashing_hash_data.assert_called_once()
    assert mock_hashing_hash_data.call_args.kwargs["skip"] == skip


@pytest.mark.usefixtures("mock_csv_read", "mock_csv_write")
@pytest.mark.parametrize("arg_variant", ["-d", "--drop"])
def test_main_drop(mock_hashing_hash_data, arg_variant):
    drop = ["one", "two"]
    main([arg_variant, *drop])

    mock_hashing_hash_data.assert_called_once()
    assert mock_hashing_hash_data.call_args.kwargs["drop"] == drop


@pytest.mark.usefixtures("mock_csv_read", "mock_csv_write")
def test_main_dedupe(mocker, mock_hashing_hash_data):
    mock_hashed_data = mocker.Mock()
    mock_hashing_hash_data.return_value = mock_hashed_data
    main(["--dedupe"])

    mock_hashing_hash_data.assert_called_once()
    mock_hashed_data.drop_duplicates.assert_called_once()


@pytest.mark.usefixtures("mock_hashing_hash_data", "mock_csv_write")
@pytest.mark.parametrize("arg_variant", ["-i", "--input"])
def test_main_input(mock_csv_read, arg_variant):
    input = "file.csv"
    main([arg_variant, input])

    mock_csv_read.assert_called_once()
    assert input in mock_csv_read.call_args.args


@pytest.mark.usefixtures("mock_hashing_hash_data", "mock_csv_read")
@pytest.mark.parametrize("arg_variant", ["-o", "--output"])
def test_main_output(mock_csv_write, arg_variant):
    output = "file.csv"
    main([arg_variant, output])

    mock_csv_write.assert_called_once()
    assert output in mock_csv_write.call_args.args
