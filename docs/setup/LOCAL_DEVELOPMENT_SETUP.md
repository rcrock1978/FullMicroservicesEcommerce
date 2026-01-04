# Local Development Setup Guide

This guide will help you set up the complete e-commerce microservices platform on your local machine.

## Prerequisites

Before you begin, ensure you have the following installed:

### Required Software

1. **Operating System**
   - Windows 10/11, macOS 10.15+, or Linux (Ubuntu 20.04+)

2. **.NET 8 SDK**
   ```bash
   # Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   
   # Verify installation
   dotnet --version
   # Should output: 8.0.x or higher
   ```

3. **Node.js 18+ and npm**
   ```bash
   # Download from: https://nodejs.org/
   
   # Verify installation
   node --version  # Should output: v18.x.x or higher
   npm --version   # Should output: 9.x.x or higher
   ```

4. **Docker Desktop**
   ```bash
   # Download from: https://www.docker.com/products/docker-desktop
   
   # Verify installation
   docker --version
   docker-compose --version
   ```

5. **Git**
   ```bash
   # Download from: https://git-scm.com/
   
   # Verify installation
   git --version
   ```

### Optional but Recommended

6. **Visual Studio 2022** or **VS Code**
   - Visual Studio 2022 Community (for backend development)
   - VS Code with C# extension (lightweight alternative)

7. **Azure Data Studio** or **pgAdmin** (PostgreSQL GUI)
8. **Redis Insight** (Redis GUI)
9. **Postman** (API testing)

---

## Initial Setup

### 1. Clone the Repository

```bash
git clone https://github.com/rcrock1978/FullMicroservicesEcommerce.git
cd FullMicroservicesEcommerce
```

### 2. Create Environment Variables

```bash
# Copy the example environment file
cp .env.example .env

# Edit the .env file with your configurations
# For local development, default values should work
```

### 3. Start Infrastructure Services

Start the required infrastructure services using Docker Compose:

```bash
# Start PostgreSQL, Redis, RabbitMQ, and Seq
docker-compose up -d postgres redis rabbitmq seq

# Verify all containers are running
docker-compose ps

# You should see:
# - postgres (Port 5432)
# - redis (Port 6379)
# - rabbitmq (Ports 5672, 15672)
# - seq (Port 5341)
```

### 4. Access Infrastructure Services

- **PostgreSQL**: `localhost:5432`
  - Username: `ecommerce_user`
  - Password: `your_secure_password_here`
  
- **Redis**: `localhost:6379`
  
- **RabbitMQ Management**: http://localhost:15672
  - Username: `guest`
  - Password: `guest`
  
- **Seq Logging**: http://localhost:5341

---

## Backend Setup

### 1. Restore NuGet Packages

```bash
cd backend

# Restore packages for all projects
dotnet restore
```

### 2. Setup Shared Libraries

```bash
# Build shared projects first
cd Shared/Shared.Common
dotnet build

cd ../Shared.Messaging
dotnet build

cd ../Shared.Security
dotnet build
```

### 3. Setup Individual Services

For each service, you need to:
1. Navigate to the service directory
2. Run database migrations
3. Start the service

#### Identity Service

```bash
cd backend/services/IdentityService

# Restore packages
dotnet restore

# Run migrations
cd src/IdentityService.Infrastructure
dotnet ef database update --startup-project ../IdentityService.API

# Run the service
cd ../IdentityService.API
dotnet run

# Service will start on: https://localhost:5001
```

#### Product Service

```bash
cd backend/services/ProductService

# Restore packages
dotnet restore

# Run migrations
cd src/ProductService.Infrastructure
dotnet ef database update --startup-project ../ProductService.API

# Run the service
cd ../ProductService.API
dotnet run

# Service will start on: https://localhost:5002
```

#### Cart Service

```bash
cd backend/services/CartService

# Restore and run
dotnet restore
cd src/CartService.API
dotnet run

# Service will start on: https://localhost:5003
```

#### Order Service

```bash
cd backend/services/OrderService

# Restore packages
dotnet restore

# Run migrations
cd src/OrderService.Infrastructure
dotnet ef database update --startup-project ../OrderService.API

# Run the service
cd ../OrderService.API
dotnet run

# Service will start on: https://localhost:5004
```

#### Payment Service

```bash
cd backend/services/PaymentService

# Restore packages
dotnet restore

# Run migrations
cd src/PaymentService.Infrastructure
dotnet ef database update --startup-project ../PaymentService.API

# Run the service
cd ../PaymentService.API
dotnet run

# Service will start on: https://localhost:5005
```

#### Media Service

```bash
cd backend/services/MediaService

# Restore packages
dotnet restore

# Run migrations
cd src/MediaService.Infrastructure
dotnet ef database update --startup-project ../MediaService.API

# Run the service
cd ../MediaService.API
dotnet run

# Service will start on: https://localhost:5006
```

#### Notification Service

```bash
cd backend/services/NotificationService

# Restore packages
dotnet restore

# Run migrations
cd src/NotificationService.Infrastructure
dotnet ef database update --startup-project ../NotificationService.API

# Run the service
cd ../NotificationService.API
dotnet run

# Service will start on: https://localhost:5007
```

#### API Gateway

```bash
cd backend/ApiGateway

# Restore and run
dotnet restore
dotnet run

# API Gateway will start on: https://localhost:5000
```

### 4. Use Helper Scripts (Recommended)

Instead of starting services manually, use the provided scripts:

```bash
# Make scripts executable (Linux/macOS)
chmod +x scripts/*.sh

# Run all migrations
./scripts/run-migrations.sh

# Seed test data
./scripts/seed-data.sh

# Start all services
./scripts/start-services.sh

# Stop all services
./scripts/stop-services.sh
```

For Windows:
```powershell
# Run migrations
.\scripts\run-migrations.ps1

# Start services
.\scripts\start-services.ps1
```

---

## Frontend Setup

### 1. Install Dependencies

```bash
cd frontend/ecommerce-web

# Install npm packages
npm install
```

### 2. Configure Environment Variables

```bash
# Create environment file
cp .env.example .env.local

# Edit .env.local with your API Gateway URL
# NEXT_PUBLIC_API_URL=http://localhost:5000
# NEXTAUTH_URL=http://localhost:3000
# NEXTAUTH_SECRET=your_nextauth_secret_here
```

### 3. Start Development Server

```bash
npm run dev

# Frontend will start on: http://localhost:3000
```

### 4. Build for Production (Optional)

```bash
npm run build
npm start
```

---

## Database Management

### View Databases

```bash
# Connect to PostgreSQL
docker exec -it postgres psql -U ecommerce_user -d postgres

# List databases
\l

# Connect to a specific database
\c identity_db

# List tables
\dt

# Exit
\q
```

### Run Migrations

```bash
# For a specific service
cd backend/services/IdentityService/src/IdentityService.Infrastructure

# Add a new migration
dotnet ef migrations add MigrationName --startup-project ../IdentityService.API

# Update database
dotnet ef database update --startup-project ../IdentityService.API

# Remove last migration (if not applied)
dotnet ef migrations remove --startup-project ../IdentityService.API
```

### Seed Data

```bash
# Run the seed data script
./scripts/seed-data.sh

# Or manually run seeders for each service
cd backend/services/IdentityService/src/IdentityService.Infrastructure
dotnet run --project ../IdentityService.API -- seed
```

---

## Testing

### Run Unit Tests

```bash
# All tests
dotnet test

# Specific service
cd backend/services/IdentityService
dotnet test

# With code coverage
dotnet test /p:CollectCoverage=true /p:CoverageDirectory=Coverage
```

### Run Integration Tests

```bash
cd backend/services/IdentityService
dotnet test --filter Category=Integration
```

### Frontend Tests

```bash
cd frontend/ecommerce-web

# Run tests
npm test

# Run E2E tests
npm run test:e2e
```

---

## Troubleshooting

### Issue: Port Already in Use

```bash
# Find process using port 5000
lsof -i :5000  # macOS/Linux
netstat -ano | findstr :5000  # Windows

# Kill the process
kill -9 <PID>  # macOS/Linux
taskkill /PID <PID> /F  # Windows
```

### Issue: Database Connection Failed

```bash
# Check if PostgreSQL is running
docker-compose ps postgres

# Restart PostgreSQL
docker-compose restart postgres

# Check logs
docker-compose logs postgres
```

### Issue: Migration Failed

```bash
# Drop and recreate database
cd backend/services/IdentityService/src/IdentityService.Infrastructure

# Remove database
dotnet ef database drop --startup-project ../IdentityService.API --force

# Recreate database
dotnet ef database update --startup-project ../IdentityService.API
```

### Issue: RabbitMQ Connection Error

```bash
# Check if RabbitMQ is running
docker-compose ps rabbitmq

# Restart RabbitMQ
docker-compose restart rabbitmq

# Check logs
docker-compose logs rabbitmq
```

### Issue: Redis Connection Error

```bash
# Check if Redis is running
docker-compose ps redis

# Test connection
docker exec -it redis redis-cli ping
# Should return: PONG
```

### Issue: Node Module Errors

```bash
cd frontend/ecommerce-web

# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install

# Or use npm ci for clean install
npm ci
```

---

## Development Workflow

### 1. Daily Development Routine

```bash
# 1. Start infrastructure
docker-compose up -d postgres redis rabbitmq seq

# 2. Start backend services
./scripts/start-services.sh

# 3. Start frontend (in new terminal)
cd frontend/ecommerce-web
npm run dev

# 4. Start coding!
```

### 2. Making Changes

```bash
# Create a feature branch
git checkout -b feature/your-feature-name

# Make changes
# ...

# Run tests
dotnet test

# Commit changes
git add .
git commit -m "feat: your feature description"

# Push to remote
git push origin feature/your-feature-name

# Create pull request on GitHub
```

### 3. Code Quality Checks

```bash
# Backend - Format code
dotnet format

# Backend - Run linter
dotnet build /warnaserror

# Frontend - Format code
cd frontend/ecommerce-web
npm run format

# Frontend - Run linter
npm run lint
```

---

## IDE Setup

### Visual Studio Code

Install these extensions:
- C# (ms-dotnettools.csharp)
- C# Dev Kit (ms-dotnettools.csdevkit)
- Docker (ms-azuretools.vscode-docker)
- ESLint (dbaeumer.vscode-eslint)
- Prettier (esbenp.prettier-vscode)
- REST Client (humao.rest-client)
- GitLens (eamodio.gitlens)

### Visual Studio 2022

Install these workloads:
- ASP.NET and web development
- .NET desktop development
- Azure development

---

## Next Steps

1. ✅ Complete setup following this guide
2. 📚 Read [API Documentation](../api/API_CONVENTIONS.md)
3. 🏗️ Review [Architecture Overview](../architecture/SYSTEM_ARCHITECTURE.md)
4. 🤝 Read [Contributing Guide](../guides/CONTRIBUTION_GUIDE.md)
5. 🚀 Start developing!

---

## Support

If you encounter issues:
1. Check the [Troubleshooting](#troubleshooting) section
2. Search existing [GitHub Issues](https://github.com/rcrock1978/FullMicroservicesEcommerce/issues)
3. Create a new issue with detailed information

---

**Happy Coding! 🚀**
