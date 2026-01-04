#!/bin/bash
# Makefile-style commands for the project

set -e

case "$1" in
  "build")
    echo "Building all services..."
    dotnet build backend/services/NotificationService/NotificationService.sln
    dotnet build backend/gateway/ApiGateway/ApiGateway.csproj
    ;;
    
  "test")
    echo "Running tests..."
    # Add test commands as test projects are created
    echo "No tests configured yet"
    ;;
    
  "docker-build")
    echo "Building Docker images..."
    docker build -t ecommerce/notification-service:latest \
      -f backend/services/NotificationService/NotificationService.API/Dockerfile \
      backend/services/NotificationService
    docker build -t ecommerce/api-gateway:latest \
      -f backend/gateway/ApiGateway/Dockerfile \
      backend/gateway/ApiGateway
    ;;
    
  "docker-up")
    echo "Starting Docker Compose services..."
    docker-compose up -d
    ;;
    
  "docker-down")
    echo "Stopping Docker Compose services..."
    docker-compose down
    ;;
    
  "docker-logs")
    docker-compose logs -f ${2:-}
    ;;
    
  "k8s-deploy")
    echo "Deploying to Kubernetes..."
    kubectl apply -f k8s/
    ;;
    
  "k8s-delete")
    echo "Deleting from Kubernetes..."
    kubectl delete -f k8s/
    ;;
    
  "migrate")
    echo "Running database migrations..."
    SERVICE=${2:-notification}
    case "$SERVICE" in
      "notification")
        cd backend/services/NotificationService/NotificationService.API
        dotnet ef database update
        ;;
      *)
        echo "Unknown service: $SERVICE"
        exit 1
        ;;
    esac
    ;;
    
  "clean")
    echo "Cleaning build artifacts..."
    find . -name "bin" -o -name "obj" | xargs rm -rf
    docker-compose down -v
    ;;
    
  *)
    echo "Usage: $0 {build|test|docker-build|docker-up|docker-down|docker-logs|k8s-deploy|k8s-delete|migrate|clean}"
    echo ""
    echo "Commands:"
    echo "  build         - Build all .NET projects"
    echo "  test          - Run all tests"
    echo "  docker-build  - Build Docker images"
    echo "  docker-up     - Start Docker Compose"
    echo "  docker-down   - Stop Docker Compose"
    echo "  docker-logs   - View Docker logs (optionally specify service)"
    echo "  k8s-deploy    - Deploy to Kubernetes"
    echo "  k8s-delete    - Delete from Kubernetes"
    echo "  migrate       - Run database migrations (specify service)"
    echo "  clean         - Clean build artifacts and Docker volumes"
    exit 1
    ;;
esac
