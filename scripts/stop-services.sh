#!/bin/bash

# Stop all microservices

echo "🛑 Stopping all services..."

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
NC='\033[0m' # No Color

# Function to stop a service
stop_service() {
    local service_name=$1
    local pid_file="/tmp/$service_name.pid"
    
    if [ -f "$pid_file" ]; then
        local pid=$(cat "$pid_file")
        if ps -p $pid > /dev/null 2>&1; then
            echo -e "${RED}Stopping $service_name (PID: $pid)...${NC}"
            kill $pid
            rm "$pid_file"
        else
            echo "$service_name is not running"
            rm "$pid_file"
        fi
    else
        echo "$service_name PID file not found"
    fi
}

# Stop all services
stop_service "Identity Service"
stop_service "Product Service"
stop_service "Cart Service"
stop_service "Order Service"
stop_service "Payment Service"
stop_service "Media Service"
stop_service "Notification Service"
stop_service "API Gateway"

# Stop infrastructure services
echo ""
echo -e "${RED}Stopping infrastructure services...${NC}"
docker-compose down

echo ""
echo -e "${GREEN}✅ All services stopped!${NC}"
