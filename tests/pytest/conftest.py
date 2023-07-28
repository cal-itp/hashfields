import subprocess

import pytest


@pytest.fixture(scope="session")
def build_target():
    return "linux-x64"


@pytest.fixture(scope="session", autouse=True)
def publish_hashfields(build_target):
    subprocess.call(["dotnet", "build"])
    subprocess.call(
        [
            "dotnet",
            "publish",
            "-r",
            build_target,
            "-c",
            "Release",
            "-o",
            "./build",
            "src/Cli/hashfields.csproj",
        ]
    )


@pytest.fixture
def hashfields_exe():
    return "build/hashfields"
