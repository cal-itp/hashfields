#!/usr/bin/env bash
set -eu

# restore dependencies
dotnet restore HashFields.sln && \

# build the solution
dotnet build HashFields.sln
