# Prerequisites

This document outlines all the prerequisites needed to develop, build, and run the E-Commerce Microservices Platform.

## Table of Contents
- [System Requirements](#system-requirements)
- [Required Software](#required-software)
- [Optional Software](#optional-software)
- [Third-Party Services](#third-party-services)
- [Development Tools](#development-tools)
- [Installation Guides](#installation-guides)
- [Verification](#verification)

---

## System Requirements

### Minimum Requirements
- **CPU**: 4 cores (2.5 GHz or higher)
- **RAM**: 8 GB
- **Storage**: 50 GB free space
- **OS**: 
  - Windows 10/11 (64-bit)
  - macOS 10.15 (Catalina) or later
  - Linux (Ubuntu 20.04+, Debian 10+, CentOS 8+)

### Recommended Requirements
- **CPU**: 8 cores (3.0 GHz or higher)
- **RAM**: 16 GB or more
- **Storage**: 100 GB SSD
- **Internet**: Stable broadband connection

---

## Required Software

### 1. .NET 8 SDK

**Purpose**: Build and run backend microservices

**Version**: 8.0 or higher

**Download**: https://dotnet.microsoft.com/download/dotnet/8.0

**Installation**:
```bash
# Windows: Download and run installer

# macOS (Homebrew):
brew install --cask dotnet-sdk

# Linux (Ubuntu/Debian):
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
```

**Verify**:
```bash
dotnet --version
# Expected output: 8.0.x
```

---

### 2. Node.js and npm

**Purpose**: Build and run frontend application

**Version**: Node.js 18.x or higher, npm 9.x or higher

**Download**: https://nodejs.org/

**Installation**:
```bash
# Windows/macOS: Download and run installer

# macOS (Homebrew):
brew install node

# Linux (Ubuntu/Debian):
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt-get install -y nodejs

# Verify both Node and npm
node --version   # Should show v18.x.x or higher
npm --version    # Should show 9.x.x or higher
```

---

### 3. Docker Desktop

**Purpose**: Run infrastructure services (databases, message queues, etc.)

**Version**: 20.10 or higher

**Download**: https://www.docker.com/products/docker-desktop

**Installation**:
- **Windows**: Download Docker Desktop for Windows
- **macOS**: Download Docker Desktop for Mac
- **Linux**: Follow instructions at https://docs.docker.com/engine/install/

**Verify**:
```bash
docker --version
docker-compose --version
```

**Post-Installation**:
```bash
# Enable Docker to start on boot
# Windows/Mac: Enabled by default

# Linux:
sudo systemctl enable docker
sudo systemctl start docker

# Add user to docker group (Linux)
sudo usermod -aG docker $USER
# Log out and back in for changes to take effect
```

---

### 4. Git

**Purpose**: Version control

**Version**: 2.30 or higher

**Download**: https://git-scm.com/downloads

**Installation**:
```bash
# Windows: Download installer

# macOS:
brew install git

# Linux (Ubuntu/Debian):
sudo apt-get update
sudo apt-get install git
```

**Configure Git**:
```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

**Verify**:
```bash
git --version
```

---

### 5. Entity Framework Core Tools

**Purpose**: Database migrations

**Installation**:
```bash
dotnet tool install --global dotnet-ef
```

**Verify**:
```bash
dotnet ef --version
# Expected output: 8.0.x
```

---

## Optional Software

### IDE/Code Editors

#### Visual Studio 2022 Community Edition (Recommended for Windows)
- **Purpose**: Full-featured IDE for .NET development
- **Download**: https://visualstudio.microsoft.com/vs/community/
- **Workloads to install**:
  - ASP.NET and web development
  - .NET desktop development
  - Azure development

#### Visual Studio Code (Cross-platform)
- **Purpose**: Lightweight code editor
- **Download**: https://code.visualstudio.com/
- **Required Extensions**:
  - C# (ms-dotnettools.csharp)
  - C# Dev Kit (ms-dotnettools.csdevkit)
  - Docker (ms-azuretools.vscode-docker)
  - ESLint (dbaeumer.vscode-eslint)
  - Prettier (esbenp.prettier-vscode)

#### JetBrains Rider
- **Purpose**: Cross-platform .NET IDE
- **Download**: https://www.jetbrains.com/rider/

---

### Database Tools

#### Azure Data Studio (Recommended)
- **Purpose**: PostgreSQL database management
- **Download**: https://docs.microsoft.com/en-us/sql/azure-data-studio/download

#### pgAdmin
- **Purpose**: PostgreSQL administration
- **Download**: https://www.pgadmin.org/download/

#### DBeaver
- **Purpose**: Universal database tool
- **Download**: https://dbeaver.io/download/

---

### API Testing Tools

#### Postman
- **Purpose**: API testing and documentation
- **Download**: https://www.postman.com/downloads/

#### Insomnia
- **Purpose**: API testing
- **Download**: https://insomnia.rest/download

---

### Redis Tools

#### Redis Insight
- **Purpose**: Redis GUI
- **Download**: https://redis.com/redis-enterprise/redis-insight/

#### Another Redis Desktop Manager
- **Purpose**: Redis GUI (Alternative)
- **Download**: https://github.com/qishibo/AnotherRedisDesktopManager

---

### Container Management

#### Portainer
- **Purpose**: Docker container management UI
- **Installation**:
```bash
docker volume create portainer_data
docker run -d -p 9000:9000 --name portainer --restart=always \
  -v /var/run/docker.sock:/var/run/docker.sock \
  -v portainer_data:/data \
  portainer/portainer-ce
```
- **Access**: http://localhost:9000

---

## Third-Party Services

### Required for Full Functionality

#### 1. AWS Account (for S3 storage)
- **Purpose**: File and image storage
- **Sign up**: https://aws.amazon.com/
- **What you need**:
  - AWS Access Key ID
  - AWS Secret Access Key
  - S3 Bucket created
- **Free Tier**: 5 GB storage, 20,000 Get requests

#### 2. Stripe Account (for payments)
- **Purpose**: Payment processing
- **Sign up**: https://stripe.com/
- **What you need**:
  - Stripe Publishable Key
  - Stripe Secret Key
  - Stripe Webhook Secret
- **Test Mode**: Free for testing

#### 3. SendGrid Account (for emails)
- **Purpose**: Sending transactional emails
- **Sign up**: https://sendgrid.com/
- **What you need**:
  - SendGrid API Key
  - Verified sender email
- **Free Tier**: 100 emails/day

---

### Alternative Services (Optional)

#### Email Alternatives
- **Mailgun**: https://www.mailgun.com/
- **AWS SES**: https://aws.amazon.com/ses/
- **SMTP Server**: Use any SMTP server

#### Storage Alternatives
- **Azure Blob Storage**: https://azure.microsoft.com/en-us/services/storage/blobs/
- **Google Cloud Storage**: https://cloud.google.com/storage
- **MinIO**: Self-hosted S3-compatible storage

---

## Development Tools

### CLI Tools

#### Windows
```powershell
# Install Chocolatey (package manager)
Set-ExecutionPolicy Bypass -Scope Process -Force
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))

# Install tools
choco install git nodejs dotnet-sdk docker-desktop vscode
```

#### macOS
```bash
# Install Homebrew (package manager)
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install tools
brew install git node
brew install --cask dotnet-sdk docker visual-studio-code
```

#### Linux (Ubuntu/Debian)
```bash
# Update package list
sudo apt-get update

# Install Git
sudo apt-get install git

# Install Node.js
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt-get install -y nodejs

# Install .NET SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0

# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
sudo usermod -aG docker $USER
```

---

## Installation Guides

### Step-by-Step Installation

#### 1. Install Core Software (Windows)
```powershell
# Run PowerShell as Administrator

# 1. Install Chocolatey
Set-ExecutionPolicy Bypass -Scope Process -Force
iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))

# 2. Install all required software
choco install git nodejs dotnet-sdk docker-desktop vscode -y

# 3. Install EF Core tools
dotnet tool install --global dotnet-ef

# 4. Restart computer
```

#### 2. Install Core Software (macOS)
```bash
# Install Homebrew
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install required software
brew install git node
brew install --cask dotnet-sdk docker visual-studio-code

# Install EF Core tools
dotnet tool install --global dotnet-ef

# Restart Terminal
```

#### 3. Install Core Software (Linux - Ubuntu)
```bash
# Update system
sudo apt-get update && sudo apt-get upgrade -y

# Install Git
sudo apt-get install git -y

# Install Node.js
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt-get install -y nodejs

# Install .NET SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> ~/.bashrc
source ~/.bashrc

# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
sudo usermod -aG docker $USER

# Install VS Code
sudo snap install code --classic

# Install EF Core tools
dotnet tool install --global dotnet-ef

# Log out and back in for docker group to take effect
```

---

## Verification

### Verify All Installations

Run this script to verify all installations:

```bash
#!/bin/bash

echo "Verifying installations..."
echo ""

echo "1. .NET SDK:"
dotnet --version || echo "❌ .NET SDK not found"
echo ""

echo "2. Node.js:"
node --version || echo "❌ Node.js not found"
echo ""

echo "3. npm:"
npm --version || echo "❌ npm not found"
echo ""

echo "4. Git:"
git --version || echo "❌ Git not found"
echo ""

echo "5. Docker:"
docker --version || echo "❌ Docker not found"
echo ""

echo "6. Docker Compose:"
docker-compose --version || echo "❌ Docker Compose not found"
echo ""

echo "7. EF Core Tools:"
dotnet ef --version || echo "❌ EF Core Tools not found"
echo ""

echo "Verification complete!"
```

### Expected Output
```
Verifying installations...

1. .NET SDK:
8.0.x

2. Node.js:
v18.x.x

3. npm:
9.x.x

4. Git:
git version 2.x.x

5. Docker:
Docker version 20.x.x

6. Docker Compose:
Docker Compose version v2.x.x

7. EF Core Tools:
Entity Framework Core .NET Command-line Tools 8.0.x

Verification complete!
```

---

## Troubleshooting

### Common Issues

#### .NET SDK not found
```bash
# Verify installation path
where dotnet  # Windows
which dotnet  # macOS/Linux

# Add to PATH if necessary
```

#### Docker permission denied (Linux)
```bash
sudo usermod -aG docker $USER
# Log out and back in
```

#### npm permission errors
```bash
# Fix npm permissions
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.bashrc
source ~/.bashrc
```

---

## Next Steps

After installing all prerequisites:

1. ✅ Verify all installations using the verification script
2. 📖 Continue to [Local Development Setup](LOCAL_DEVELOPMENT_SETUP.md)
3. 🚀 Start building!

---

## Additional Resources

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Node.js Documentation](https://nodejs.org/docs/)
- [Docker Documentation](https://docs.docker.com/)
- [Git Documentation](https://git-scm.com/doc)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Redis Documentation](https://redis.io/documentation)
- [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)

---

**Ready to develop? Let's go! 🚀**
