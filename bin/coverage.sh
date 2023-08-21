#!/usr/bin/env bash
set -eu

# run pytests with coverage
coverage run -m pytest

# clean out old coverage results
rm -rf ./tests/coverage

# regenerate coverate report
coverage html --directory ./tests/coverage
