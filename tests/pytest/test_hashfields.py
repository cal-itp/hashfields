import subprocess


def test_can_call_hashfields(capfd, hashfields_exe):
    subprocess.call([hashfields_exe])
    capture = capfd.readouterr()

    assert "HashFields.Cli.Worker" in capture.out
