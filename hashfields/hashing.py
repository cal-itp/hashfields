import hashlib
import pandas

ALL_ALGS = ["sha256", "sha384", "sha512"]
DEFAULT_ALG = "sha512"


def hexdigest(value: str, hash_alg: str = DEFAULT_ALG) -> str:
    """Apply a hash function to an input string to produce its hex digest.

    Args:
        value (str): The input string to hash.

        hash_alg (str): The name of a supported hash algorithm to use.

    Returns (str):
        A hex digest of the input string.
    """
    return hashlib.new(hash_alg, str(value).encode("utf-8")).hexdigest()


def hash_data(
    dataframe: pandas.DataFrame, hash_alg: str = DEFAULT_ALG, skip: list | None = [], drop: list | None = []
) -> pandas.DataFrame:
    """Apply a hash function to each value in a dataframe.

    Args:
        dataframe (pandas.DataFrame): The input dataframe with values to hash.

        hash_alg (str): The name of a supported hash algorithm to use.

        skip (list): A list of column names to skip hashing in the output.

        drop (list): A list of column names to drop from the output.

    Returns (pandas.DataFrame):
        A new DataFrame with hashed values.
    """
    if drop is not None and len(drop) > 0:
        df = dataframe.drop(columns=drop)
    else:
        df = dataframe.copy(deep=True)

    target_cols = [c for c in df.columns if c not in skip]

    for col in target_cols:
        df[col] = df[col].apply(hexdigest, args=(hash_alg,))

    return df
