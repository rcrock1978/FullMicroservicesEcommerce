# Microservices Overview

## Service Catalog

This document provides a detailed overview of each microservice in the e-commerce platform.

---

## 1. Identity Service

### Purpose
Handles user authentication, authorization, and identity management.

### Responsibilities
- User registration and login
- JWT token generation and validation
- Password management (reset, change)
- Email verification
- Refresh token management
- Role and permission management
- User profile management

### Technology Stack
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT authentication
- BCrypt password hashing

### Database Schema
- Users
- Roles
- UserRoles
- RefreshTokens
- EmailVerificationTokens

### API Endpoints
```
POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/refresh-token
POST   /api/auth/logout
POST   /api/auth/verify-email
POST   /api/auth/forgot-password
POST   /api/auth/reset-password
GET    /api/users/{id}
GET    /api/users/profile
PUT    /api/users/{id}
DELETE /api/users/{id}
POST   /api/users/change-password
```

### Events Published
- `UserRegisteredEvent`
- `UserEmailVerifiedEvent`
- `UserPasswordChangedEvent`
- `UserDeletedEvent`

### Dependencies
- None (foundational service)

---

## 2. Product Catalog Service

### Purpose
Manages products, categories, and product reviews.

### Responsibilities
- Product CRUD operations
- Category management
- Product search and filtering
- Product reviews and ratings
- Inventory tracking
- Product image associations
- Price management

### Technology Stack
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Redis (caching)
- Elasticsearch (search - optional)

### Database Schema
- Products
- Categories
- ProductImages
- Reviews
- Inventory

### API Endpoints
```
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
PATCH  /api/products/{id}/stock
GET    /api/products/search
GET    /api/categories
GET    /api/categories/{id}
POST   /api/categories
GET    /api/categories/{id}/products
GET    /api/products/{id}/reviews
POST   /api/products/{id}/reviews
```

### Events Published
- `ProductCreatedEvent`
- `ProductUpdatedEvent`
- `ProductDeletedEvent`
- `StockChangedEvent`
- `ProductReviewedEvent`
- `ProductPriceChangedEvent`

### Events Consumed
- None

### Caching Strategy
- Product details cached in Redis (15 minutes TTL)
- Category list cached (1 hour TTL)
- Invalidate cache on product updates

---

## 3. Shopping Cart Service

### Purpose
Manages shopping carts and wishlists for users.

### Responsibilities
- Add/remove items from cart
- Update cart item quantities
- Cart persistence (both guest and authenticated)
- Merge guest cart with user cart on login
- Wishlist management
- Cart expiration handling

### Technology Stack
- ASP.NET Core Web API
- Redis (primary storage for carts)
- PostgreSQL (wishlist persistence)
- Entity Framework Core

### Database Schema
- Wishlists (PostgreSQL)
- WishlistItems (PostgreSQL)
- Carts (Redis - in-memory)

### API Endpoints
```
GET    /api/cart
POST   /api/cart/items
PUT    /api/cart/items/{productId}
DELETE /api/cart/items/{productId}
DELETE /api/cart
POST   /api/cart/merge
GET    /api/wishlist
POST   /api/wishlist/items
DELETE /api/wishlist/items/{productId}
POST   /api/wishlist/move-to-cart/{productId}
```

### Events Published
- `CartItemAddedEvent`
- `CartItemRemovedEvent`
- `CartClearedEvent`

### Events Consumed
- `ProductStockChangedEvent` - Update cart if product out of stock
- `ProductPriceChangedEvent` - Update cart prices
- `ProductDeletedEvent` - Remove from carts

### Data Storage
- Cart data stored in Redis with 7-day expiration
- Guest carts identified by session ID
- User carts identified by user ID

---

## 4. Order Service

### Purpose
Handles order creation, processing, and tracking.

### Responsibilities
- Order creation from cart
- Order status management
- Order history
- Order cancellation
- Shipping address management
- Order item management
- Saga orchestration for order processing

### Technology Stack
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MassTransit with Saga pattern

### Database Schema
- Orders
- OrderItems
- OrderStatuses
- ShippingAddresses
- SagaStates (for order processing saga)

### API Endpoints
```
POST   /api/orders
GET    /api/orders
GET    /api/orders/{id}
GET    /api/orders/user/{userId}
PUT    /api/orders/{id}/confirm
PUT    /api/orders/{id}/cancel
PUT    /api/orders/{id}/status
GET    /api/orders/{id}/tracking
GET    /api/orders/track-by-number/{orderNumber}
```

### Events Published
- `OrderCreatedEvent`
- `OrderConfirmedEvent`
- `OrderShippedEvent`
- `OrderDeliveredEvent`
- `OrderCancelledEvent`
- `InventoryReservationRequestEvent`

### Events Consumed
- `PaymentProcessedEvent` - Confirm order
- `PaymentFailedEvent` - Cancel order
- `InventoryReservedEvent` - Continue order processing
- `InventoryReservationFailedEvent` - Cancel order

### Order Processing Saga
1. Order created
2. Reserve inventory
3. Process payment
4. Confirm order
5. Send notification
6. Update shipping status

### Compensation Logic
- Inventory rollback on payment failure
- Refund on order cancellation
- Stock restoration

---

## 5. Payment Service

### Purpose
Handles payment processing and refunds.

### Responsibilities
- Payment intent creation
- Payment processing via Stripe
- Payment method storage
- Refund processing
- Webhook handling
- Transaction history
- Payment verification

### Technology Stack
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Stripe SDK
- Webhook signature verification

### Database Schema
- Payments
- PaymentMethods
- Transactions
- Refunds

### API Endpoints
```
POST   /api/payments/intent
POST   /api/payments/process
GET    /api/payments/{id}
GET    /api/payments/order/{orderId}
POST   /api/payments/{id}/refund
GET    /api/payment-methods
POST   /api/payment-methods
DELETE /api/payment-methods/{id}
POST   /api/webhooks/stripe
```

### Events Published
- `PaymentInitiatedEvent`
- `PaymentProcessedEvent`
- `PaymentFailedEvent`
- `RefundProcessedEvent`

### Events Consumed
- `OrderCreatedEvent` - Initiate payment

### Security
- PCI DSS compliance via Stripe
- No credit card data stored
- Webhook signature validation
- Idempotency keys for payments

---

## 6. Media Service

### Purpose
Handles file uploads, storage, and management.

### Responsibilities
- Image/file upload
- File storage in AWS S3
- Image processing (resize, thumbnail)
- File metadata management
- Presigned URL generation
- File deletion

### Technology Stack
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (metadata)
- AWS S3 SDK
- ImageSharp (image processing)

### Database Schema
- MediaFiles
- ImageMetadata

### API Endpoints
```
POST   /api/media/upload
POST   /api/media/upload-multiple
GET    /api/media/{id}
DELETE /api/media/{id}
GET    /api/media/presigned-url/{id}
POST   /api/images/upload
POST   /api/images/generate-thumbnails
GET    /api/images/{id}/thumbnail/{size}
```

### Events Published
- `FileUploadedEvent`
- `FileDeletedEvent`

### Events Consumed
- `ProductDeletedEvent` - Delete associated images

### Image Processing
- Thumbnail generation (multiple sizes)
- Image optimization
- Format conversion
- EXIF data removal

---

## 7. Notification Service

### Purpose
Handles all notifications (email, SMS, push).

### Responsibilities
- Email sending
- Email templates
- Notification history
- Retry logic for failed notifications
- Template management
- Bulk email sending

### Technology Stack
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- SendGrid (email)
- Hangfire (background jobs)

### Database Schema
- Notifications
- NotificationTemplates
- EmailLogs

### API Endpoints
```
POST   /api/notifications/email
GET    /api/notifications/{id}
GET    /api/notifications/user/{userId}
GET    /api/templates
POST   /api/templates
PUT    /api/templates/{id}
DELETE /api/templates/{id}
```

### Events Published
- `NotificationSentEvent`
- `NotificationFailedEvent`

### Events Consumed
- `UserRegisteredEvent` - Welcome email
- `OrderCreatedEvent` - Order confirmation
- `OrderShippedEvent` - Shipping notification
- `PaymentProcessedEvent` - Payment receipt
- `UserPasswordChangedEvent` - Password change confirmation

### Email Templates
- Welcome Email
- Order Confirmation
- Order Shipped
- Payment Receipt
- Password Reset
- Email Verification

---

## 8. API Gateway

### Purpose
Single entry point for all client requests.

### Responsibilities
- Request routing to appropriate services
- Authentication and authorization
- Rate limiting and throttling
- Request/response logging
- Load balancing
- Request aggregation (optional)
- CORS handling

### Technology Stack
- ASP.NET Core
- YARP (Yet Another Reverse Proxy)
- JWT authentication
- Redis (rate limiting state)

### Routes Configuration
```
/api/auth/*        → Identity Service
/api/users/*       → Identity Service
/api/products/*    → Product Service
/api/categories/*  → Product Service
/api/cart/*        → Cart Service
/api/wishlist/*    → Cart Service
/api/orders/*      → Order Service
/api/payments/*    → Payment Service
/api/media/*       → Media Service
/api/notifications/* → Notification Service
```

### Features
- JWT token validation
- Role-based routing
- Request transformation
- Response caching
- Health check aggregation
- Swagger documentation aggregation

---

## Inter-Service Communication

### Synchronous (HTTP/REST)
- Used for critical, immediate operations
- API Gateway → Microservices
- Minimal direct service-to-service calls

### Asynchronous (RabbitMQ)
- Used for eventual consistency
- Event-driven architecture
- Decoupled services
- Saga pattern implementation

### Message Bus Topics
- `identity.*` - Identity events
- `products.*` - Product events
- `orders.*` - Order events
- `payments.*` - Payment events
- `cart.*` - Cart events
- `notifications.*` - Notification events

---

## Service Dependencies

```
API Gateway
  └─> All Services

Identity Service
  └─> (No dependencies)

Product Service
  └─> Redis (caching)

Cart Service
  ├─> Redis (primary storage)
  ├─> Product Service (product info)
  └─> Events from Product Service

Order Service
  ├─> Cart Service (get cart)
  ├─> Product Service (validate products)
  ├─> Payment Service (via events)
  └─> Notification Service (via events)

Payment Service
  ├─> Stripe API
  └─> Order Service (via events)

Media Service
  └─> AWS S3

Notification Service
  ├─> SendGrid API
  └─> Events from all services
```

---

## Deployment Considerations

### Resource Requirements (Per Service)
- **CPU**: 0.5 - 1 core
- **Memory**: 512MB - 1GB
- **Storage**: 10GB (except Media Service: 50GB+)

### Scaling Strategy
- **Identity**: 2-3 instances
- **Product**: 3-5 instances (high read)
- **Cart**: 2-3 instances
- **Order**: 2-4 instances
- **Payment**: 2-3 instances
- **Media**: 2-3 instances
- **Notification**: 1-2 instances
- **API Gateway**: 3-5 instances

### Health Checks
All services expose:
- `/health` - Liveness probe
- `/health/ready` - Readiness probe

---

## Development Guidelines

### Service Structure (Per Service)
```
ServiceName/
├── src/
│   ├── ServiceName.Domain/
│   ├── ServiceName.Application/
│   ├── ServiceName.Infrastructure/
│   ├── ServiceName.API/
│   └── ServiceName.Contracts/
├── tests/
│   ├── ServiceName.UnitTests/
│   └── ServiceName.IntegrationTests/
└── ServiceName.sln
```

### Best Practices
- Each service owns its database
- Use DTOs for API contracts
- Implement CQRS with MediatR
- Validate inputs with FluentValidation
- Use AutoMapper for object mapping
- Implement proper exception handling
- Add comprehensive logging
- Write unit and integration tests
- Document APIs with Swagger
- Version your APIs

---

## Monitoring & Observability

### Logging
- Structured logging with Serilog
- Correlation IDs across requests
- Log levels: Debug, Information, Warning, Error, Critical

### Metrics
- Request count and duration
- Error rates
- Database query performance
- Cache hit/miss ratios
- Message processing times

### Tracing
- Distributed tracing across services
- Performance bottleneck identification
- Service dependency visualization

---

## Security Considerations

### Each Service Implements
- Input validation
- Authorization checks
- Secure configuration management
- Dependency vulnerability scanning
- SQL injection prevention
- XSS protection

### Secrets Management
- Environment variables
- Azure Key Vault / AWS Secrets Manager
- Never commit secrets to version control

---

This overview provides a comprehensive understanding of each microservice, their responsibilities, and how they interact within the e-commerce platform.
