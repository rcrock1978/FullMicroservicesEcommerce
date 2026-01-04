#!/bin/bash

# Run database migrations for all services

echo "🔄 Running database migrations..."

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Function to run migration
run_migration() {
    local service_name=$1
    local infra_path=$2
    local api_path=$3
    
    echo -e "${BLUE}Running migrations for $service_name...${NC}"
    
    cd "$infra_path"
    
    # Check if migrations exist
    if [ -d "Migrations" ]; then
        dotnet ef database update --startup-project "$api_path" --verbose
        
        if [ $? -eq 0 ]; then
            echo -e "${GREEN}✅ $service_name migrations completed${NC}"
        else
            echo -e "${RED}❌ $service_name migrations failed${NC}"
            cd - > /dev/null
            exit 1
        fi
    else
        echo -e "${RED}No migrations found for $service_name${NC}"
    fi
    
    cd - > /dev/null
    echo ""
}

# Ensure infrastructure is running
echo "Checking if PostgreSQL is running..."
if ! docker-compose ps | grep -q postgres; then
    echo "Starting PostgreSQL..."
    docker-compose up -d postgres
    sleep 10
fi

# Run migrations for each service
run_migration "Identity Service" \
    "backend/services/IdentityService/src/IdentityService.Infrastructure" \
    "../IdentityService.API"

run_migration "Product Service" \
    "backend/services/ProductService/src/ProductService.Infrastructure" \
    "../ProductService.API"

run_migration "Cart Service" \
    "backend/services/CartService/src/CartService.Infrastructure" \
    "../CartService.API"

run_migration "Order Service" \
    "backend/services/OrderService/src/OrderService.Infrastructure" \
    "../OrderService.API"

run_migration "Payment Service" \
    "backend/services/PaymentService/src/PaymentService.Infrastructure" \
    "../PaymentService.API"

run_migration "Media Service" \
    "backend/services/MediaService/src/MediaService.Infrastructure" \
    "../MediaService.API"

run_migration "Notification Service" \
    "backend/services/NotificationService/src/NotificationService.Infrastructure" \
    "../NotificationService.API"

echo -e "${GREEN}🎉 All migrations completed successfully!${NC}"
