import subprocess

import pytest


@pytest.fixture(scope="session", autouse=True)
def build_hashfields():
    subprocess.call(["dotnet", "build"])


@pytest.fixture
def hashfields_exe():
    return "build/hashfields"
