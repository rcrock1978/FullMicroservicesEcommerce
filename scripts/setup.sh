#!/bin/bash

# Initial project setup script

echo "🚀 Setting up E-Commerce Microservices Platform..."
echo ""

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Check prerequisites
echo -e "${BLUE}Checking prerequisites...${NC}"

command -v dotnet >/dev/null 2>&1 || {
    echo -e "${RED}❌ .NET SDK is not installed. Please install .NET 8 SDK.${NC}"
    exit 1
}

command -v node >/dev/null 2>&1 || {
    echo -e "${RED}❌ Node.js is not installed. Please install Node.js 18+.${NC}"
    exit 1
}

command -v docker >/dev/null 2>&1 || {
    echo -e "${RED}❌ Docker is not installed. Please install Docker Desktop.${NC}"
    exit 1
}

command -v git >/dev/null 2>&1 || {
    echo -e "${RED}❌ Git is not installed. Please install Git.${NC}"
    exit 1
}

echo -e "${GREEN}✅ All prerequisites found${NC}"
echo ""

# Setup environment variables
echo -e "${BLUE}Setting up environment variables...${NC}"

if [ ! -f ".env" ]; then
    cp .env.example .env
    echo -e "${YELLOW}⚠️  Created .env file from .env.example${NC}"
    echo -e "${YELLOW}⚠️  Please edit .env file with your configuration${NC}"
else
    echo -e "${GREEN}✅ .env file already exists${NC}"
fi

echo ""

# Create necessary directories
echo -e "${BLUE}Creating directory structure...${NC}"

mkdir -p backend/{Shared,services,ApiGateway}
mkdir -p backend/Shared/{Shared.Common,Shared.Messaging,Shared.Security}
mkdir -p frontend
mkdir -p infrastructure/{docker,k8s,scripts}
mkdir -p docs/{architecture,api,setup,deployment,guides}
mkdir -p tests/{integration,e2e}

echo -e "${GREEN}✅ Directory structure created${NC}"
echo ""

# Make scripts executable
echo -e "${BLUE}Making scripts executable...${NC}"

chmod +x scripts/*.sh
chmod +x infrastructure/docker/postgres/init-multiple-databases.sh

echo -e "${GREEN}✅ Scripts are now executable${NC}"
echo ""

# Pull Docker images
echo -e "${BLUE}Pulling Docker images...${NC}"

docker pull postgres:16-alpine
docker pull redis:7-alpine
docker pull rabbitmq:3-management-alpine
docker pull datalust/seq:latest

echo -e "${GREEN}✅ Docker images pulled${NC}"
echo ""

# Install .NET tools
echo -e "${BLUE}Installing .NET tools...${NC}"

dotnet tool install --global dotnet-ef || echo ".NET EF tools already installed"

echo -e "${GREEN}✅ .NET tools installed${NC}"
echo ""

# Initialize Git hooks (if in a Git repository)
if [ -d ".git" ]; then
    echo -e "${BLUE}Setting up Git hooks...${NC}"
    
    # Pre-commit hook
    cat > .git/hooks/pre-commit << 'EOF'
#!/bin/bash
echo "Running pre-commit checks..."

# Format .NET code
dotnet format --verify-no-changes || {
    echo "Code formatting issues found. Running dotnet format..."
    dotnet format
    exit 1
}

# Lint frontend
if [ -d "frontend/ecommerce-web" ]; then
    cd frontend/ecommerce-web
    npm run lint || exit 1
    cd - > /dev/null
fi

echo "Pre-commit checks passed!"
EOF
    
    chmod +x .git/hooks/pre-commit
    echo -e "${GREEN}✅ Git hooks configured${NC}"
else
    echo -e "${YELLOW}⚠️  Not a Git repository, skipping Git hooks${NC}"
fi

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}✅ Setup completed successfully!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo "Next steps:"
echo "  1. Edit .env file with your configuration"
echo "  2. Start infrastructure: docker-compose up -d postgres redis rabbitmq seq"
echo "  3. Create and run migrations: ./scripts/run-migrations.sh"
echo "  4. Seed data (optional): ./scripts/seed-data.sh"
echo "  5. Start all services: ./scripts/start-services.sh"
echo ""
echo "For frontend development:"
echo "  cd frontend/ecommerce-web"
echo "  npm install"
echo "  npm run dev"
echo ""
echo "📚 Read the documentation:"
echo "  - Prerequisites: docs/setup/PREREQUISITES.md"
echo "  - Local Setup: docs/setup/LOCAL_DEVELOPMENT_SETUP.md"
echo "  - Architecture: docs/architecture/SYSTEM_ARCHITECTURE.md"
echo ""
echo -e "${BLUE}Happy coding! 🎉${NC}"
