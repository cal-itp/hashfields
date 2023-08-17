#!/usr/bin/env bash
set -eux

pyinstaller --onefile hashfields/main.py --name hashfields
