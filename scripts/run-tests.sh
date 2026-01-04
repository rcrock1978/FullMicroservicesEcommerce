#!/bin/bash

# Run all tests across the solution

echo "🧪 Running all tests..."

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Track test results
TOTAL_TESTS=0
PASSED_TESTS=0
FAILED_TESTS=0

# Function to run tests for a service
run_tests() {
    local service_name=$1
    local test_path=$2
    
    echo -e "${BLUE}Running tests for $service_name...${NC}"
    
    if [ -d "$test_path" ]; then
        cd "$test_path"
        
        # Run tests with detailed output
        dotnet test --logger "console;verbosity=normal" --collect:"XPlat Code Coverage"
        
        if [ $? -eq 0 ]; then
            echo -e "${GREEN}✅ $service_name tests passed${NC}"
            ((PASSED_TESTS++))
        else
            echo -e "${RED}❌ $service_name tests failed${NC}"
            ((FAILED_TESTS++))
        fi
        
        cd - > /dev/null
        ((TOTAL_TESTS++))
    else
        echo -e "${RED}Test directory not found: $test_path${NC}"
    fi
    
    echo ""
}

# Run unit tests for shared libraries
echo -e "${BLUE}=== Shared Libraries Tests ===${NC}"
run_tests "Shared.Common" "backend/Shared/Shared.Common/tests/Shared.Common.UnitTests"
run_tests "Shared.Messaging" "backend/Shared/Shared.Messaging/tests/Shared.Messaging.UnitTests"

# Run tests for each microservice
echo -e "${BLUE}=== Microservices Tests ===${NC}"
run_tests "Identity Service" "backend/services/IdentityService/tests/IdentityService.UnitTests"
run_tests "Identity Service Integration" "backend/services/IdentityService/tests/IdentityService.IntegrationTests"

run_tests "Product Service" "backend/services/ProductService/tests/ProductService.UnitTests"
run_tests "Product Service Integration" "backend/services/ProductService/tests/ProductService.IntegrationTests"

run_tests "Cart Service" "backend/services/CartService/tests/CartService.UnitTests"
run_tests "Cart Service Integration" "backend/services/CartService/tests/CartService.IntegrationTests"

run_tests "Order Service" "backend/services/OrderService/tests/OrderService.UnitTests"
run_tests "Order Service Integration" "backend/services/OrderService/tests/OrderService.IntegrationTests"

run_tests "Payment Service" "backend/services/PaymentService/tests/PaymentService.UnitTests"
run_tests "Payment Service Integration" "backend/services/PaymentService/tests/PaymentService.IntegrationTests"

run_tests "Media Service" "backend/services/MediaService/tests/MediaService.UnitTests"
run_tests "Media Service Integration" "backend/services/MediaService/tests/MediaService.IntegrationTests"

run_tests "Notification Service" "backend/services/NotificationService/tests/NotificationService.UnitTests"
run_tests "Notification Service Integration" "backend/services/NotificationService/tests/NotificationService.IntegrationTests"

# Frontend tests
echo -e "${BLUE}=== Frontend Tests ===${NC}"
if [ -d "frontend/ecommerce-web" ]; then
    cd frontend/ecommerce-web
    
    echo "Running frontend tests..."
    npm test -- --watchAll=false --coverage
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✅ Frontend tests passed${NC}"
        ((PASSED_TESTS++))
    else
        echo -e "${RED}❌ Frontend tests failed${NC}"
        ((FAILED_TESTS++))
    fi
    
    cd - > /dev/null
    ((TOTAL_TESTS++))
fi

# Print summary
echo ""
echo "========================================"
echo -e "${BLUE}Test Summary${NC}"
echo "========================================"
echo "Total test suites: $TOTAL_TESTS"
echo -e "${GREEN}Passed: $PASSED_TESTS${NC}"
echo -e "${RED}Failed: $FAILED_TESTS${NC}"
echo "========================================"

if [ $FAILED_TESTS -eq 0 ]; then
    echo -e "${GREEN}🎉 All tests passed!${NC}"
    exit 0
else
    echo -e "${RED}❌ Some tests failed!${NC}"
    exit 1
fi
