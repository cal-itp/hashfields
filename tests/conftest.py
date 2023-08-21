import io

import pytest

from hashfields import csv


@pytest.fixture
def csv_string_io():
    return io.StringIO(
        """one,two,three
        1,2,3
        a,b,c
        true,false,true
        2023-01-01,1974-02-28,2034-07-04"""
    )


@pytest.fixture
def csv_dataframe(csv_string_io):
    return csv.read(csv_string_io)


@pytest.fixture
def psv_string_io():
    return io.StringIO(
        """one|two|three
        1|2|3
        a|b|c
        true|false|true
        2023-01-01|1974-02-28|2034-07-04"""
    )


@pytest.fixture
def hash_alg():
    return "sha384"
