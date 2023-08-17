import argparse
import sys

from hashfields import csv, hashing, __version__ as version


RESULT_SUCCESS = 0


def main(argv=None):
    argv = argv if argv is not None else sys.argv[1:]
    parser = argparse.ArgumentParser(prog="hashfields")

    # https://stackoverflow.com/a/8521644/812183
    parser.add_argument(
        "-v",
        "--version",
        action="version",
        version=f"%(prog)s {version}",
    )

    parser.add_argument(
        "-a",
        "--alg",
        choices=hashing.ALL_ALGS,
        default=hashing.DEFAULT_ALG,
        dest="hash_alg",
        help="The hash algorithm to use.",
    )

    parser.add_argument("-t", "--delimiter", default=",", dest="delimiter", help="Field delimiter in the input data.")

    parser.add_argument(
        "-d",
        "--drop",
        action="extend",
        default=[],
        dest="drop",
        help="Column names to drop from the output.",
        nargs="+",
        type=str,
    )

    parser.add_argument(
        "-s",
        "--skip",
        action="extend",
        default=[],
        dest="skip",
        help="Column names to skip hashing in the output.",
        nargs="+",
        type=str,
    )

    parser.add_argument(
        "--dedupe",
        action="store_true",
        dest="dedupe",
        help="With this flag, remove duplicate records from the output.",
    )

    parser.add_argument("-i", "--input", default=sys.stdin, dest="input", help="Readable location for input data.")

    parser.add_argument("-o", "--output", default=sys.stdout, dest="output", help="Writable location for output results.")

    args = parser.parse_args(argv)

    data = csv.read(args.input, args.delimiter)

    hashed_data = hashing.hash_data(data, hash_alg=args.hash_alg, skip=args.skip, drop=args.drop)

    if args.dedupe:
        hashed_data.drop_duplicates(inplace=True, ignore_index=True)

    csv.write(hashed_data, args.output)

    return RESULT_SUCCESS
