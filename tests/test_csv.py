import pandas
import pytest

from hashfields.csv import read


@pytest.fixture
def spy_pandas_read_csv(mocker):
    return mocker.spy(pandas, "read_csv")


def test_read_default(csv_string_io, spy_pandas_read_csv):
    dataframe = read(csv_string_io)

    spy_pandas_read_csv.assert_called_once()
    assert csv_string_io in spy_pandas_read_csv.call_args.args
    assert spy_pandas_read_csv.call_args.kwargs["delimiter"] == ","
    assert spy_pandas_read_csv.call_args.kwargs["dtype"] == str
    assert spy_pandas_read_csv.call_args.kwargs["skipinitialspace"] is True
    assert spy_pandas_read_csv.call_args.kwargs["skip_blank_lines"] is True

    for col in ["one", "two", "three"]:
        assert col in dataframe.columns


def test_read_delimiter(psv_string_io, spy_pandas_read_csv):
    dataframe = read(psv_string_io, delimiter="|")

    spy_pandas_read_csv.assert_called_once()
    assert psv_string_io in spy_pandas_read_csv.call_args.args
    assert spy_pandas_read_csv.call_args.kwargs["delimiter"] == "|"
    for col in ["one", "two", "three"]:
        assert col in dataframe.columns


def test_read_emptydata(csv_string_io, spy_pandas_read_csv):
    spy_pandas_read_csv.side_effect = pandas.errors.EmptyDataError()
    dataframe = read(csv_string_io)

    spy_pandas_read_csv.assert_called_once()
    assert dataframe.equals(pandas.DataFrame())
