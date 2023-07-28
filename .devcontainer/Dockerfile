FROM python:3.11

# See: https://docs.microsoft.com/en-us/dotnet/core/install/linux

RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb

RUN apt-get update && \
    apt-get install -y apt-transport-https && \
    apt-get update && \
    apt-get install -y dotnet-sdk-6.0

ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1 \
    USER=calitp

RUN useradd --create-home --shell /bin/bash $USER && \
    chown -R $USER /home/$USER

WORKDIR /home/$USER/hashfields

USER $USER

# update PATH for local pip installs
ENV PATH "$PATH:/home/$USER/.local/bin"

COPY .devcontainer/requirements.txt .devcontainer/requirements.txt
RUN python -m pip install --upgrade pip && \
    pip install -r .devcontainer/requirements.txt
