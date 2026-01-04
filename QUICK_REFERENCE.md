# Quick Reference Guide

## 🚀 Common Commands

### Development

#### Start Infrastructure Services
```bash
# Start databases and message broker
docker-compose up -d postgres redis rabbitmq seq

# Check services are healthy
docker-compose ps

# View logs
docker-compose logs -f postgres
```

#### Run Backend Services Individually
```bash
# Identity Service (Port 5001)
cd backend/services/IdentityService/IdentityService.API
dotnet run

# Product Service (Port 5002)
cd backend/services/ProductService/ProductService.API
dotnet run

# Media Service (Port 5003)
cd backend/services/MediaService/MediaService.API
dotnet run

# Cart Service (Port 5004)
cd backend/services/CartService/CartService.API
dotnet run

# Order Service (Port 5005)
cd backend/services/OrderService/OrderService.API
dotnet run

# Payment Service (Port 5006)
cd backend/services/PaymentService/PaymentService.API
dotnet run

# Notification Service (Port 5007)
cd backend/services/NotificationService/NotificationService.API
dotnet run

# API Gateway (Port 5000)
cd backend/ApiGateway
dotnet run
```

#### Run Frontend
```bash
cd frontend/ecommerce-web

# Install dependencies (first time)
npm install --legacy-peer-deps

# Development mode
npm run dev

# Production build
npm run build

# Run production build
npm start

# Lint code
npm run lint
```

#### Using Development Script
```bash
cd infrastructure/scripts

# Make executable (first time)
chmod +x dev.sh

# Start all services
./dev.sh start-all

# Stop all services
./dev.sh stop-all

# Restart specific service
./dev.sh restart-service IdentityService

# View service logs
./dev.sh logs IdentityService

# Run migrations
./dev.sh migrate-all
```

### Docker

#### Full Stack with Docker Compose
```bash
# Build and start all services
docker-compose up --build

# Run in detached mode
docker-compose up -d

# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v

# View logs for specific service
docker-compose logs -f frontend

# Restart specific service
docker-compose restart api-gateway

# Build specific service
docker-compose build frontend

# Execute command in container
docker-compose exec postgres psql -U ecommerce_user -d ecommerce_db
```

#### Individual Docker Builds
```bash
# Build frontend
cd frontend/ecommerce-web
docker build -t ecommerce-frontend:latest .

# Build API Gateway
cd backend/ApiGateway
docker build -t ecommerce-gateway:latest -f ../../infrastructure/docker/api-gateway/Dockerfile ../..

# Build Notification Service
cd backend/services/NotificationService
docker build -t ecommerce-notification:latest -f ../../../infrastructure/docker/notification-service/Dockerfile ../../..
```

### Database

#### Migrations
```bash
# Create migration
cd backend/services/IdentityService/IdentityService.API
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# List migrations
dotnet ef migrations list
```

#### Direct Database Access
```bash
# Connect to PostgreSQL
docker-compose exec postgres psql -U ecommerce_user -d identity_db

# Common PostgreSQL commands:
\l                    # List databases
\c database_name      # Connect to database
\dt                   # List tables
\d table_name         # Describe table
\q                    # Quit
```

#### Redis Commands
```bash
# Connect to Redis
docker-compose exec redis redis-cli

# Common Redis commands:
KEYS *                # List all keys
GET key_name          # Get value
DEL key_name          # Delete key
FLUSHALL              # Clear all data
INFO                  # Server info
```

### Kubernetes

#### Deploy to Kubernetes
```bash
# Create namespace
kubectl apply -f infrastructure/kubernetes/namespace.yaml

# Deploy all resources
kubectl apply -f infrastructure/kubernetes/

# Check deployments
kubectl get all -n ecommerce

# Check pods
kubectl get pods -n ecommerce

# View logs
kubectl logs -n ecommerce <pod-name>

# Follow logs
kubectl logs -n ecommerce <pod-name> -f

# Describe pod
kubectl describe pod -n ecommerce <pod-name>

# Port forward to service
kubectl port-forward -n ecommerce service/frontend 3000:3000

# Delete all resources
kubectl delete namespace ecommerce
```

#### Scale Services
```bash
# Scale deployment
kubectl scale deployment frontend -n ecommerce --replicas=3

# Auto-scale
kubectl autoscale deployment frontend -n ecommerce --min=2 --max=10 --cpu-percent=80
```

### Git

#### Common Git Commands
```bash
# Check status
git status

# View changes
git diff

# Stage all changes
git add -A

# Commit with message
git commit -m "feat: your message"

# Push to remote
git push origin main

# Pull latest changes
git pull origin main

# View commit history
git log --oneline -10

# View specific file history
git log --oneline -- path/to/file

# Create new branch
git checkout -b feature/new-feature

# Switch branches
git checkout main

# Merge branch
git merge feature/new-feature
```

### Testing

#### Run Tests
```bash
# Run all tests
dotnet test

# Run tests for specific project
cd backend/services/IdentityService/IdentityService.Tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Frontend tests (when implemented)
cd frontend/ecommerce-web
npm test
npm run test:e2e
```

### Monitoring & Debugging

#### Check Service Health
```bash
# Identity Service
curl http://localhost:5001/health

# Product Service
curl http://localhost:5002/health

# API Gateway
curl http://localhost:5000/health

# All services health aggregation
curl http://localhost:5000/health-ui
```

#### View Logs
```bash
# Seq (Structured Logs)
# Open browser: http://localhost:5341

# Docker Compose logs
docker-compose logs -f <service-name>

# Kubernetes logs
kubectl logs -n ecommerce <pod-name> -f

# Follow logs for all pods with label
kubectl logs -n ecommerce -l app=frontend -f
```

#### Debug with VS Code
1. Open the service project in VS Code
2. Press F5 or select "Run > Start Debugging"
3. Set breakpoints in code
4. Make API requests to trigger breakpoints

### Environment Variables

#### Backend (.NET)
```bash
# Set environment for development
export ASPNETCORE_ENVIRONMENT=Development

# Set connection string
export ConnectionStrings__DefaultConnection="Host=localhost;Database=identity_db;Username=ecommerce_user;Password=dev_password_123"

# Or create appsettings.Development.json
```

#### Frontend (Next.js)
```bash
# Create .env.local file
NEXT_PUBLIC_API_URL=http://localhost:5000
NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY=pk_test_...

# Or export environment variables
export NEXT_PUBLIC_API_URL=http://localhost:5000
```

### API Testing

#### Using curl
```bash
# Register user
curl -X POST http://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "password": "Password123!"
  }'

# Login
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "Password123!"
  }'

# Get products (with auth token)
curl -X GET http://localhost:5002/api/products \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### Using Swagger UI
1. Navigate to service URL + /swagger
2. Example: http://localhost:5001/swagger
3. Use "Authorize" button to add JWT token
4. Try API endpoints interactively

### Cleanup

#### Clean Docker Resources
```bash
# Remove stopped containers
docker container prune

# Remove unused images
docker image prune

# Remove unused volumes
docker volume prune

# Remove everything (CAUTION!)
docker system prune -a --volumes

# Clean build artifacts
cd frontend/ecommerce-web
rm -rf .next node_modules
npm install --legacy-peer-deps
```

#### Clean .NET Build
```bash
# Clean solution
dotnet clean

# Remove bin and obj folders
find . -name "bin" -type d -exec rm -rf {} +
find . -name "obj" -type d -exec rm -rf {} +

# Restore packages
dotnet restore
```

## 🔗 Service URLs (Local Development)

- **Frontend**: http://localhost:3000
- **API Gateway**: http://localhost:5000
- **Identity Service**: http://localhost:5001
- **Product Service**: http://localhost:5002
- **Media Service**: http://localhost:5003
- **Cart Service**: http://localhost:5004
- **Order Service**: http://localhost:5005
- **Payment Service**: http://localhost:5006
- **Notification Service**: http://localhost:5007
- **Seq Logs**: http://localhost:5341
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **PostgreSQL**: localhost:5432 (ecommerce_user/dev_password_123)
- **Redis**: localhost:6379

## 📊 Service Swagger URLs

- http://localhost:5001/swagger - Identity Service
- http://localhost:5002/swagger - Product Service
- http://localhost:5003/swagger - Media Service
- http://localhost:5004/swagger - Cart Service
- http://localhost:5005/swagger - Order Service
- http://localhost:5006/swagger - Payment Service
- http://localhost:5007/swagger - Notification Service

## 🆘 Troubleshooting

### Port Already in Use
```bash
# Find process using port
lsof -i :5001

# Kill process
kill -9 <PID>

# Or use different port in launchSettings.json
```

### Database Connection Issues
```bash
# Check PostgreSQL is running
docker-compose ps postgres

# Restart PostgreSQL
docker-compose restart postgres

# Check logs
docker-compose logs postgres

# Test connection
docker-compose exec postgres psql -U ecommerce_user -d identity_db -c "SELECT 1"
```

### Frontend Build Issues
```bash
# Clear Next.js cache
rm -rf frontend/ecommerce-web/.next

# Clear node_modules
rm -rf frontend/ecommerce-web/node_modules

# Reinstall
cd frontend/ecommerce-web
npm install --legacy-peer-deps

# Clear npm cache if issues persist
npm cache clean --force
```

### Docker Issues
```bash
# Restart Docker daemon
sudo systemctl restart docker

# Or restart Docker Desktop

# Check Docker status
docker info

# Remove all containers and start fresh
docker-compose down -v
docker system prune -a --volumes
docker-compose up --build
```

## 📝 Quick Tips

1. **Always start infrastructure services first** (postgres, redis, rabbitmq)
2. **Check service health** before testing endpoints
3. **Use Seq** for centralized log viewing (http://localhost:5341)
4. **Use Swagger** for API testing and documentation
5. **Check Docker logs** when services don't start
6. **Use --legacy-peer-deps** for npm install in frontend
7. **Set environment variables** before running services
8. **Apply migrations** before starting services
9. **Use API Gateway** (port 5000) for all frontend requests
10. **Check CORS settings** if frontend can't connect to backend

---

**For more detailed information**, see:
- [README.md](README.md) - Project overview and setup
- [INFRASTRUCTURE.md](INFRASTRUCTURE.md) - DevOps documentation
- [PROJECT_COMPLETION.md](PROJECT_COMPLETION.md) - Completion summary
- [frontend/ecommerce-web/README.md](frontend/ecommerce-web/README.md) - Frontend docs
