FROM python:3.11

USER root
WORKDIR /hashfields

# See: https://docs.microsoft.com/en-us/dotnet/core/install/linux

RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb

RUN apt-get update && \
    apt-get install -y apt-transport-https && \
    apt-get update && \
    apt-get install -y dotnet-sdk-5.0 dotnet-sdk-6.0

RUN python -m pip install --upgrade pip && \
    pip install --no-cache-dir pre-commit
