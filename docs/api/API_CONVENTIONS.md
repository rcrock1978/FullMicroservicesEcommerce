# API Conventions

This document defines the API conventions and standards for all microservices in the e-commerce platform.

## Table of Contents
- [General Principles](#general-principles)
- [HTTP Methods](#http-methods)
- [URL Structure](#url-structure)
- [Request/Response Format](#requestresponse-format)
- [Status Codes](#status-codes)
- [Error Handling](#error-handling)
- [Versioning](#versioning)
- [Authentication](#authentication)
- [Pagination](#pagination)
- [Filtering & Sorting](#filtering--sorting)
- [Rate Limiting](#rate-limiting)

---

## General Principles

### RESTful Design
- Use resource-based URLs
- Use HTTP verbs appropriately
- Stateless communication
- HATEOAS (Hypermedia as the Engine of Application State) where applicable

### Consistency
- Follow consistent naming conventions
- Use the same patterns across all services
- Maintain backward compatibility

### Documentation
- All APIs must be documented with OpenAPI/Swagger
- Include request/response examples
- Document all possible error codes

---

## HTTP Methods

### GET
**Purpose**: Retrieve resources

**Characteristics**:
- Safe (no side effects)
- Idempotent
- Cacheable

**Examples**:
```http
GET /api/v1/products              # Get all products
GET /api/v1/products/123          # Get specific product
GET /api/v1/products?category=electronics  # Get filtered products
```

### POST
**Purpose**: Create new resources

**Characteristics**:
- Not safe
- Not idempotent
- Not cacheable

**Examples**:
```http
POST /api/v1/products             # Create new product
POST /api/v1/orders               # Create new order
POST /api/v1/auth/login           # Authenticate user
```

**Response**: 
- Status: `201 Created`
- Include `Location` header with URL of created resource

### PUT
**Purpose**: Update entire resource or create if doesn't exist

**Characteristics**:
- Not safe
- Idempotent
- Not cacheable

**Examples**:
```http
PUT /api/v1/products/123          # Update entire product
PUT /api/v1/users/profile         # Update user profile
```

**Response**: 
- Status: `200 OK` (updated) or `201 Created` (created)

### PATCH
**Purpose**: Partial update of resource

**Characteristics**:
- Not safe
- Not idempotent (generally)
- Not cacheable

**Examples**:
```http
PATCH /api/v1/products/123        # Update specific fields
PATCH /api/v1/orders/456/status   # Update order status only
```

**Response**: 
- Status: `200 OK` or `204 No Content`

### DELETE
**Purpose**: Remove resources

**Characteristics**:
- Not safe
- Idempotent
- Not cacheable

**Examples**:
```http
DELETE /api/v1/products/123       # Delete product
DELETE /api/v1/cart/items/456     # Remove item from cart
```

**Response**: 
- Status: `204 No Content` or `200 OK`

---

## URL Structure

### Base URL Format
```
https://api.ecommerce.com/api/v{version}/{resource}
```

### Resource Naming
- Use **plural nouns** for collections: `/products`, `/orders`, `/users`
- Use **lowercase** with **hyphens** for multi-word resources: `/product-categories`
- Use **nested routes** for sub-resources: `/products/123/reviews`

### Examples
```
✅ Good:
/api/v1/products
/api/v1/products/123
/api/v1/products/123/reviews
/api/v1/product-categories
/api/v1/orders/456/items

❌ Bad:
/api/v1/getProducts
/api/v1/product
/api/v1/ProductCategories
/api/v1/order/456/item
```

### Hierarchy Depth
- Limit nesting to 2-3 levels maximum
- Use query parameters for complex relationships

```
✅ Good:
/api/v1/users/123/orders
/api/v1/products?category=electronics

❌ Bad:
/api/v1/categories/1/products/123/reviews/456/likes
```

---

## Request/Response Format

### Content Type
- Default: `application/json`
- Support: `application/xml` (optional)
- Set `Content-Type` header in requests
- Set `Accept` header for desired response format

### Request Body (JSON)
```json
{
  "name": "Laptop",
  "price": 999.99,
  "categoryId": 5,
  "tags": ["electronics", "computers"]
}
```

### Response Body (JSON)

#### Single Resource
```json
{
  "id": 123,
  "name": "Laptop",
  "price": 999.99,
  "category": {
    "id": 5,
    "name": "Electronics"
  },
  "createdAt": "2026-01-04T10:00:00Z",
  "updatedAt": "2026-01-04T10:00:00Z"
}
```

#### Collection
```json
{
  "data": [
    {
      "id": 123,
      "name": "Laptop",
      "price": 999.99
    },
    {
      "id": 124,
      "name": "Mouse",
      "price": 29.99
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalItems": 100
  }
}
```

### Naming Conventions
- Use **camelCase** for JSON properties
- Boolean fields: prefix with `is`, `has`, `can`: `isActive`, `hasStock`
- Dates: ISO 8601 format: `2026-01-04T10:00:00Z`

---

## Status Codes

### Success Codes

| Code | Meaning | Usage |
|------|---------|-------|
| 200 | OK | Successful GET, PUT, PATCH, DELETE |
| 201 | Created | Successful POST (resource created) |
| 204 | No Content | Successful DELETE or PUT with no response body |

### Client Error Codes

| Code | Meaning | Usage |
|------|---------|-------|
| 400 | Bad Request | Invalid request format, validation errors |
| 401 | Unauthorized | Missing or invalid authentication |
| 403 | Forbidden | Authenticated but not authorized |
| 404 | Not Found | Resource doesn't exist |
| 409 | Conflict | Resource conflict (e.g., duplicate) |
| 422 | Unprocessable Entity | Validation errors |
| 429 | Too Many Requests | Rate limit exceeded |

### Server Error Codes

| Code | Meaning | Usage |
|------|---------|-------|
| 500 | Internal Server Error | Unexpected server error |
| 502 | Bad Gateway | Invalid response from upstream server |
| 503 | Service Unavailable | Server temporarily unavailable |
| 504 | Gateway Timeout | Upstream server timeout |

---

## Error Handling

### Error Response Format
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Validation failed",
    "details": [
      {
        "field": "email",
        "message": "Email is required"
      },
      {
        "field": "password",
        "message": "Password must be at least 8 characters"
      }
    ],
    "timestamp": "2026-01-04T10:00:00Z",
    "path": "/api/v1/users/register",
    "traceId": "abc123xyz"
  }
}
```

### Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| VALIDATION_ERROR | 400 | Input validation failed |
| UNAUTHORIZED | 401 | Authentication required |
| FORBIDDEN | 403 | Insufficient permissions |
| NOT_FOUND | 404 | Resource not found |
| CONFLICT | 409 | Resource already exists |
| RATE_LIMIT_EXCEEDED | 429 | Too many requests |
| INTERNAL_ERROR | 500 | Internal server error |
| SERVICE_UNAVAILABLE | 503 | Service temporarily down |

### Field Validation Errors
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Validation failed",
    "details": [
      {
        "field": "email",
        "value": "invalid-email",
        "message": "Email must be a valid email address",
        "code": "INVALID_FORMAT"
      },
      {
        "field": "price",
        "value": -10,
        "message": "Price must be greater than 0",
        "code": "OUT_OF_RANGE"
      }
    ]
  }
}
```

---

## Versioning

### URL Versioning (Recommended)
```
/api/v1/products
/api/v2/products
```

### Header Versioning (Alternative)
```http
GET /api/products
Accept: application/vnd.ecommerce.v1+json
```

### Version Lifecycle
- **v1**: Current stable version
- **v2**: New version (backward incompatible changes)
- **Deprecated**: Mark old versions as deprecated (6-12 months notice)
- **Sunset**: Remove deprecated versions

### Breaking Changes
Require new version:
- Removing fields
- Changing field types
- Changing endpoint behavior
- Changing status codes

### Non-Breaking Changes
Don't require new version:
- Adding optional fields
- Adding new endpoints
- Adding optional query parameters

---

## Authentication

### JWT Bearer Token
```http
GET /api/v1/products
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Token Response
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

### Refresh Token
```http
POST /api/v1/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "refresh_token_here"
}
```

---

## Pagination

### Request Parameters
```http
GET /api/v1/products?page=2&pageSize=20
```

### Response
```json
{
  "data": [...],
  "pagination": {
    "page": 2,
    "pageSize": 20,
    "totalPages": 10,
    "totalItems": 200,
    "hasNextPage": true,
    "hasPreviousPage": true
  },
  "links": {
    "self": "/api/v1/products?page=2&pageSize=20",
    "first": "/api/v1/products?page=1&pageSize=20",
    "previous": "/api/v1/products?page=1&pageSize=20",
    "next": "/api/v1/products?page=3&pageSize=20",
    "last": "/api/v1/products?page=10&pageSize=20"
  }
}
```

### Default Values
- `page`: 1 (first page)
- `pageSize`: 20 (items per page)
- `maxPageSize`: 100 (maximum allowed)

---

## Filtering & Sorting

### Filtering
```http
GET /api/v1/products?category=electronics&minPrice=100&maxPrice=1000
GET /api/v1/products?tags=laptop,gaming&inStock=true
```

### Sorting
```http
GET /api/v1/products?sortBy=price&sortOrder=asc
GET /api/v1/products?sortBy=createdAt&sortOrder=desc
```

### Combined
```http
GET /api/v1/products?category=electronics&minPrice=500&sortBy=price&sortOrder=asc&page=1&pageSize=20
```

### Search
```http
GET /api/v1/products?q=laptop
GET /api/v1/products/search?query=gaming laptop&category=electronics
```

---

## Rate Limiting

### Rate Limit Headers
```http
HTTP/1.1 200 OK
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1609459200
```

### Rate Limit Exceeded
```http
HTTP/1.1 429 Too Many Requests
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 0
X-RateLimit-Reset: 1609459200
Retry-After: 60

{
  "error": {
    "code": "RATE_LIMIT_EXCEEDED",
    "message": "Rate limit exceeded. Try again in 60 seconds."
  }
}
```

### Rate Limits (Per User)
- Anonymous: 100 requests/minute
- Authenticated: 1000 requests/minute
- Admin: 10000 requests/minute

---

## CORS

### Allowed Origins
- Development: `http://localhost:3000`
- Production: `https://ecommerce.com`

### Headers
```http
Access-Control-Allow-Origin: https://ecommerce.com
Access-Control-Allow-Methods: GET, POST, PUT, PATCH, DELETE, OPTIONS
Access-Control-Allow-Headers: Content-Type, Authorization
Access-Control-Max-Age: 86400
```

---

## Best Practices

### Do's ✅
- Use plural nouns for collections
- Use HTTP methods correctly
- Return appropriate status codes
- Include proper error messages
- Document all endpoints
- Version your APIs
- Implement rate limiting
- Use HTTPS in production
- Validate all inputs
- Return consistent response formats

### Don'ts ❌
- Don't use verbs in URLs
- Don't ignore HTTP status codes
- Don't return sensitive data
- Don't implement custom auth (use standards)
- Don't skip pagination for large collections
- Don't return different formats inconsistently
- Don't expose internal errors to clients

---

## Example API Endpoints

### Authentication
```
POST   /api/v1/auth/register
POST   /api/v1/auth/login
POST   /api/v1/auth/refresh-token
POST   /api/v1/auth/logout
POST   /api/v1/auth/forgot-password
POST   /api/v1/auth/reset-password
```

### Products
```
GET    /api/v1/products
GET    /api/v1/products/{id}
POST   /api/v1/products
PUT    /api/v1/products/{id}
PATCH  /api/v1/products/{id}
DELETE /api/v1/products/{id}
GET    /api/v1/products/search
GET    /api/v1/products/{id}/reviews
POST   /api/v1/products/{id}/reviews
```

### Cart
```
GET    /api/v1/cart
POST   /api/v1/cart/items
PUT    /api/v1/cart/items/{productId}
DELETE /api/v1/cart/items/{productId}
DELETE /api/v1/cart
```

### Orders
```
GET    /api/v1/orders
GET    /api/v1/orders/{id}
POST   /api/v1/orders
PUT    /api/v1/orders/{id}/cancel
GET    /api/v1/orders/{id}/tracking
```

---

**Note**: All services must follow these conventions for consistency and maintainability.
