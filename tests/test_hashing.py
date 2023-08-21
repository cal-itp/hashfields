import hashlib

import pytest

import hashfields.hashing
from hashfields.hashing import hexdigest, hash_data


@pytest.fixture
def spy_hexdigest(mocker):
    return mocker.spy(hashfields.hashing, "hexdigest")


def test_hexdigest_default():
    value = "the value"
    expected = hashlib.sha512(value.encode("utf-8")).hexdigest()

    actual = hexdigest(value)

    assert actual == expected


def test_hexdigest_hash_alg(hash_alg):
    value = "the value"
    expected = hashlib.new(hash_alg, value.encode("utf-8")).hexdigest()

    actual = hexdigest(value, hash_alg=hash_alg)

    assert actual == expected


def test_hash_data_default(mocker, spy_hexdigest, csv_dataframe):
    copy_spy = mocker.spy(csv_dataframe, "copy")

    result = hash_data(csv_dataframe)

    copy_spy.assert_called_once()
    assert spy_hexdigest.call_count > 0
    assert result.columns.equals(csv_dataframe.columns)
    assert result.index.equals(csv_dataframe.index)
    assert len(result) == len(csv_dataframe)


def test_hash_data_hash_alg(spy_hexdigest, hash_alg, csv_dataframe):
    hash_data(csv_dataframe, hash_alg=hash_alg)

    assert spy_hexdigest.call_count > 0
    assert hash_alg in spy_hexdigest.call_args.args


def test_hash_data_skip(csv_dataframe):
    skip = ["one", "three"]

    result = hash_data(csv_dataframe, skip=skip)

    for col in skip:
        assert result[col].equals(csv_dataframe[col])

    not_skipped = [col for col in result.columns if col not in skip]
    assert len(not_skipped) > 0
    for col in not_skipped:
        assert not result[col].equals(csv_dataframe[col])


def test_hash_data_drop(csv_dataframe):
    drop = ["one", "three"]

    result = hash_data(csv_dataframe, drop=drop)

    for col in drop:
        assert col not in result.columns

    not_dropped = [col for col in result.columns if col not in drop]
    assert len(not_dropped) > 0
    for col in not_dropped:
        assert col in result


def test_hash_data_skip_drop(csv_dataframe):
    skip = ["one", "three"]
    drop = ["two"]

    result = hash_data(csv_dataframe, skip=skip, drop=drop)

    for col in skip:
        assert result[col].equals(csv_dataframe[col])

    for col in drop:
        assert col not in result.columns
