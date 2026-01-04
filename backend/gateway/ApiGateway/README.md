# API Gateway

API Gateway built with Ocelot for the E-commerce Microservices platform. Provides a unified entry point for all backend services.

## Features

- **Request Routing**: Routes requests to appropriate microservices based on URL patterns
- **Authentication**: JWT-based authentication with Bearer token support
- **Rate Limiting**: Protects services from excessive requests (100 requests per minute for products)
- **Caching**: Response caching for improved performance (products: 30s, categories: 60s)
- **Load Balancing**: Distributes requests across multiple service instances
- **Aggregated Health Checks**: Monitor health of all microservices from single endpoint

## Port Configuration

- **Gateway**: Port 5000
- **Identity Service**: Port 5001
- **Product Service**: Port 5002
- **Media Service**: Port 5003
- **Cart Service**: Port 5004
- **Order Service**: Port 5005
- **Payment Service**: Port 5006
- **Notification Service**: Port 5007

## Routes

### Public Routes (No Authentication)
- `POST /api/identity/auth/register` - User registration
- `POST /api/identity/auth/login` - User login
- `GET /api/products/*` - Browse products
- `GET /api/products/categories/*` - Browse categories
- `POST /api/payments/webhooks/*` - Payment webhooks (Stripe)

### Protected Routes (Requires JWT Token)
- `GET /api/identity/users/*` - User management
- `POST /api/media/*` - Upload media files
- `GET /api/cart/*` - Cart operations
- `POST /api/orders/*` - Order management
- `POST /api/payments/*` - Payment processing
- `GET /api/notifications/*` - User notifications

### Health Check Routes
- `GET /health/identity` - Identity service health
- `GET /health/products` - Product service health
- `GET /health/media` - Media service health
- `GET /health/cart` - Cart service health
- `GET /health/orders` - Order service health
- `GET /health/payments` - Payment service health
- `GET /health/notifications` - Notification service health

## Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "EcommerceApp",
    "Audience": "EcommerceAppUsers",
    "ExpirationMinutes": 60
  }
}
```

### Rate Limiting
Rate limiting is configured per route in `ocelot.json`:
```json
"RateLimitOptions": {
  "EnableRateLimiting": true,
  "Period": "1m",
  "PeriodTimespan": 60,
  "Limit": 100
}
```

### Caching
Response caching is configured per route:
```json
"FileCacheOptions": {
  "TtlSeconds": 30,
  "Region": "products"
}
```

## Running the Gateway

```bash
cd backend/gateway/ApiGateway
dotnet run
```

Gateway will start on http://localhost:5000

## Development

1. Update `ocelot.json` to add new routes
2. Configure authentication for protected routes
3. Add rate limiting and caching as needed
4. Update service host/port mappings for your environment

## Logging

Logs are written to:
- Console (for development)
- `logs/gateway-YYYYMMDD.txt` (rolling daily logs)

## Production Considerations

- Update `ocelot.Production.json` with production service URLs
- Use HTTPS for all downstream services
- Implement service discovery (Consul, Eureka) for dynamic routing
- Add circuit breaker patterns for resilience
- Configure distributed tracing (OpenTelemetry)
- Set up monitoring and alerting (Prometheus, Grafana)
