# Start with the official Jenkins LTS image
FROM jenkins/jenkins:lts

# Switch to root user for installing packages
USER root

# Update and install dependencies
RUN apt-get update && apt-get install -y \
    wget \
    curl \
    git \
    software-properties-common \
    apt-transport-https \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Install .NET SDK 8.0
RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y dotnet-sdk-8.0 \
    && rm packages-microsoft-prod.deb

# Verify installations
RUN dotnet --version && git --version

# Switch back to Jenkins user
USER jenkins

# Expose Jenkins default port
EXPOSE 8080

# Add Jenkins volume
VOLUME /var/jenkins_home
