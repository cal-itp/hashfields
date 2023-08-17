from typing import BinaryIO, TextIO

import pandas


def read(path_or_buf: BinaryIO | TextIO, delimiter: str = ",") -> pandas.DataFrame:
    """Read a CSV from a file path or buffer into a DataFrame.

     Args:
        path_or_buf (BinaryIO | TextIO): File path or buffer with CSV data.

        delimiter (str): The field delimiter used in the CSV data.

    Returns (pandas.DataFrame):
        A DataFrame of the CSV data, or an empty DataFrame.
    """
    try:
        return pandas.read_csv(path_or_buf, delimiter=delimiter, dtype=str, skipinitialspace=True, skip_blank_lines=True)
    except pandas.errors.EmptyDataError:
        return pandas.DataFrame()
