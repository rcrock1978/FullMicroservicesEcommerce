#!/bin/bash

# Start all microservices for local development

echo "🚀 Starting E-Commerce Microservices..."
echo ""

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Start infrastructure services
echo -e "${BLUE}Starting infrastructure services (PostgreSQL, Redis, RabbitMQ, Seq)...${NC}"
docker-compose up -d postgres redis rabbitmq seq

echo "Waiting for infrastructure services to be ready..."
sleep 10

# Check if services are running
echo -e "${GREEN}Checking infrastructure health...${NC}"
docker-compose ps

echo ""
echo -e "${BLUE}Starting backend services...${NC}"

# Function to start a service
start_service() {
    local service_name=$1
    local service_path=$2
    local port=$3
    
    echo -e "${GREEN}Starting $service_name on port $port...${NC}"
    cd "$service_path"
    
    # Restore packages if not already done
    if [ ! -d "bin" ]; then
        dotnet restore
    fi
    
    # Start the service in background
    nohup dotnet run --urls="http://localhost:$port" > "/tmp/$service_name.log" 2>&1 &
    echo $! > "/tmp/$service_name.pid"
    
    cd - > /dev/null
}

# Start each service
start_service "Identity Service" "backend/services/IdentityService/src/IdentityService.API" "5001"
sleep 5

start_service "Product Service" "backend/services/ProductService/src/ProductService.API" "5002"
sleep 5

start_service "Cart Service" "backend/services/CartService/src/CartService.API" "5003"
sleep 5

start_service "Order Service" "backend/services/OrderService/src/OrderService.API" "5004"
sleep 5

start_service "Payment Service" "backend/services/PaymentService/src/PaymentService.API" "5005"
sleep 5

start_service "Media Service" "backend/services/MediaService/src/MediaService.API" "5006"
sleep 5

start_service "Notification Service" "backend/services/NotificationService/src/NotificationService.API" "5007"
sleep 5

start_service "API Gateway" "backend/ApiGateway" "5000"

echo ""
echo -e "${GREEN}✅ All services started!${NC}"
echo ""
echo "Services are running on:"
echo "  - API Gateway:            http://localhost:5000"
echo "  - Identity Service:       http://localhost:5001"
echo "  - Product Service:        http://localhost:5002"
echo "  - Cart Service:           http://localhost:5003"
echo "  - Order Service:          http://localhost:5004"
echo "  - Payment Service:        http://localhost:5005"
echo "  - Media Service:          http://localhost:5006"
echo "  - Notification Service:   http://localhost:5007"
echo ""
echo "Infrastructure:"
echo "  - PostgreSQL:             localhost:5432"
echo "  - Redis:                  localhost:6379"
echo "  - RabbitMQ Management:    http://localhost:15672"
echo "  - Seq Logging:            http://localhost:5341"
echo ""
echo "Logs are available in /tmp/*.log"
echo "To stop all services, run: ./scripts/stop-services.sh"
