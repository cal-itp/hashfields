import io

import pytest


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
def psv_string_io():
    return io.StringIO(
        """one|two|three
        1|2|3
        a|b|c
        true|false|true
        2023-01-01|1974-02-28|2034-07-04"""
    )
