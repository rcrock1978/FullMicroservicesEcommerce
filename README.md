# 🛒 Full Stack E-Commerce Microservices Platform

A complete, production-ready e-commerce platform built with modern microservices architecture, featuring .NET 10 backend services, Next.js 15 frontend, and comprehensive DevOps infrastructure.

## 🎯 Project Overview

This is a full-featured e-commerce platform demonstrating enterprise-grade best practices:

- **Microservices Architecture**: 7 independent, scalable .NET services
- **Event-Driven Communication**: RabbitMQ for inter-service messaging
- **CQRS Pattern**: Command Query Responsibility Segregation
- **Domain-Driven Design**: Clean architecture with clear boundaries
- **API Gateway**: Ocelot with routing, caching, and rate limiting
- **Modern Frontend**: Next.js 15 with React 19, TypeScript, and Tailwind CSS
- **Containerization**: Docker multi-stage builds with Docker Compose orchestration
- **Kubernetes**: Production-ready manifests with StatefulSets and Ingress
- **CI/CD**: GitHub Actions with automated testing and deployment
- **Authentication**: JWT-based security with refresh tokens
- **Payment Integration**: Stripe Elements for secure payment processing
- **Notifications**: SendGrid email and Twilio SMS integration
- **Media Storage**: Cloud-ready image upload and management
- **Caching**: Redis for performance optimization
- **Monitoring & Logging**: Serilog structured logging with Seq

## 🏗️ Architecture

### Backend Microservices (.NET 10)

1. **Identity Service** (Port 5001) - User authentication, JWT tokens, role management
2. **Product Service** (Port 5002) - Product catalog, categories, Redis caching
3. **Media Service** (Port 5003) - Image upload, cloud storage integration
4. **Cart Service** (Port 5004) - Shopping cart, wishlist, session management
5. **Order Service** (Port 5005) - Order processing, status tracking, Saga pattern
6. **Payment Service** (Port 5006) - Stripe integration, payment intent, webhooks
7. **Notification Service** (Port 5007) - SendGrid emails, Twilio SMS, templates
8. **API Gateway** (Port 5000) - Ocelot reverse proxy with:
   - Dynamic routing to all services
   - JWT authentication middleware
   - Response caching (30s products, 60s categories)
   - Rate limiting (100 req/min for products)
   - Request logging and exception handling

### Frontend (Next.js 15)

- **React 19** with Server Components
- **TypeScript 5.7** for type safety
- **Tailwind CSS 3.4** for styling
- **Zustand 5.0** for state management
- **React Hook Form + Zod** for form validation
- **Stripe Elements** for payment processing
- **Axios** with request/response interceptors
- **Sonner** for toast notifications
- **Lucide React** for icons
- **Image optimization** with Next.js Image

### Pages & Features

- 🔐 **Authentication**: Login, registration, profile management, password change
- 🛍️ **Products**: Grid view, search, category filters, detailed product pages
- 🛒 **Shopping Cart**: Add/remove items, quantity control, persistent storage
- 💳 **Checkout**: Stripe payment, shipping/billing addresses, order confirmation
- 📦 **Orders**: Order history, status tracking, order details, cancellation
- 📱 **Responsive Design**: Mobile-first, works on all devices
- ⚡ **Performance**: Code splitting, lazy loading, optimized images
- ♿ **Accessibility**: Semantic HTML, ARIA labels, keyboard navigation

### Infrastructure

- **Database**: PostgreSQL 17 (one database per service)
- **Cache**: Redis 7 with persistence
- **Message Broker**: RabbitMQ 3 with management UI
- **API Gateway**: Ocelot 23.4 with CacheManager
- **Logging**: Serilog + Seq for centralized logging
- **Containerization**: Docker multi-stage builds
- **Orchestration**: Docker Compose for local dev, Kubernetes for production
- **CI/CD**: GitHub Actions with matrix builds
- **Security**: JWT authentication, HTTPS, CORS policies
## 📁 Project Structure

```
FullMicroservicesEcommerce/
├── backend/
│   ├── Shared/                          # Shared libraries across services
│   │   ├── Shared.Common/               # Common utilities, DTOs, responses
│   │   ├── Shared.Messaging/            # RabbitMQ integration, event bus
│   │   └── Shared.Security/             # JWT auth, security utilities
│   ├── services/
│   │   ├── IdentityService/             # Auth, users, JWT (Port 5001)
│   │   │   ├── IdentityService.Domain/
│   │   │   ├── IdentityService.Application/
│   │   │   ├── IdentityService.Infrastructure/
│   │   │   └── IdentityService.API/
│   │   ├── ProductService/              # Products, categories (Port 5002)
│   │   │   ├── ProductService.Domain/
│   │   │   ├── ProductService.Application/
│   │   │   ├── ProductService.Infrastructure/
│   │   │   └── ProductService.API/
│   │   ├── MediaService/                # Image upload (Port 5003)
│   │   │   ├── MediaService.Domain/
│   │   │   ├── MediaService.Application/
│   │   │   ├── MediaService.Infrastructure/
│   │   │   └── MediaService.API/
│   │   ├── CartService/                 # Shopping cart (Port 5004)
│   │   │   ├── CartService.Domain/
│   │   │   ├── CartService.Application/
│   │   │   ├── CartService.Infrastructure/
│   │   │   └── CartService.API/
│   │   ├── OrderService/                # Orders, Saga (Port 5005)
│   │   │   ├── OrderService.Domain/
│   │   │   ├── OrderService.Application/
│   │   │   ├── OrderService.Infrastructure/
│   │   │   └── OrderService.API/
│   │   ├── PaymentService/              # Stripe payments (Port 5006)
│   │   │   ├── PaymentService.Domain/
│   │   │   ├── PaymentService.Application/
│   │   │   ├── PaymentService.Infrastructure/
│   │   │   └── PaymentService.API/
│   │   └── NotificationService/         # Email, SMS (Port 5007)
│   │       ├── NotificationService.Domain/
│   │       ├── NotificationService.Application/
│   │       ├── NotificationService.Infrastructure/
│   │       └── NotificationService.API/
│   └── ApiGateway/                      # Ocelot gateway (Port 5000)
├── frontend/
│   └── ecommerce-web/                   # Next.js 15 app
│       ├── src/
│       │   ├── app/                     # Pages (App Router)
│       │   │   ├── login/
│       │   │   ├── register/
│       │   │   ├── profile/
│       │   │   ├── products/
│       │   │   ├── categories/
│       │   │   ├── cart/
│       │   │   ├── checkout/
│       │   │   └── orders/
│       │   ├── components/              # Reusable components
│       │   │   ├── layout/              # Header, Footer
│       │   │   ├── ErrorBoundary.tsx
│       │   │   └── Skeleton.tsx
│       │   ├── services/                # API integration
│       │   │   ├── auth.service.ts
│       │   │   ├── product.service.ts
│       │   │   └── order.service.ts
│       │   ├── store/                   # Zustand state management
│       │   │   ├── auth.ts
│       │   │   └── cart.ts
│       │   ├── lib/                     # Utilities
│       │   │   ├── api-client.ts        # Axios instance
│       │   │   └── utils.ts
│       │   └── types/                   # TypeScript types
│       │       └── index.ts
│       ├── Dockerfile
│       ├── next.config.js
│       ├── tailwind.config.js
│       └── package.json
├── infrastructure/                      # DevOps configs
│   ├── docker/                          # Dockerfiles
│   │   ├── notification-service/
│   │   └── api-gateway/
│   ├── kubernetes/                      # K8s manifests
│   │   ├── namespace.yaml
│   │   ├── configmaps.yaml
│   │   ├── secrets.yaml
│   │   ├── postgres-statefulset.yaml
│   │   ├── redis-statefulset.yaml
│   │   ├── rabbitmq-statefulset.yaml
│   │   ├── services-deployment.yaml
│   │   └── ingress.yaml
│   └── scripts/
│       └── dev.sh                       # Development helper script
├── .github/
│   └── workflows/
│       ├── backend-ci-cd.yml            # Backend CI/CD
│       └── frontend-ci-cd.yml           # Frontend CI/CD
├── docker-compose.yml                   # Local development
├── INFRASTRUCTURE.md                    # DevOps documentation
└── README.md
```
├── infrastructure/
## 🚀 Quick Start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)
- [PostgreSQL 17](https://www.postgresql.org/) (or use Docker)
- [Redis 7](https://redis.io/) (or use Docker)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/FullMicroservicesEcommerce.git
   cd FullMicroservicesEcommerce
   ```

2. **Setup environment variables**
   ```bash
   # Backend services
   cp backend/services/IdentityService/IdentityService.API/appsettings.Development.json.example appsettings.Development.json
   
   # Frontend
   cp frontend/ecommerce-web/.env.local.example frontend/ecommerce-web/.env.local
   
   # Edit with your configuration:
   # - Database connection strings
   # - JWT secret keys
   # - Stripe API keys
   # - SendGrid API keys
   # - Twilio credentials
   # - AWS S3 credentials (optional)
   ```

3. **Start infrastructure services with Docker Compose**
   ```bash
   docker-compose up -d postgres redis rabbitmq seq
   
   # Wait for services to be healthy
   docker-compose ps
   ```

4. **Run database migrations** (automatic on first API start, or manual)
   ```bash
   cd backend/services/IdentityService/IdentityService.API
   dotnet ef database update
   
   # Repeat for other services with databases
   ```

5. **Start backend services**
   ```bash
   # Terminal 1 - Identity Service
   cd backend/services/IdentityService/IdentityService.API
   dotnet run
   
   # Terminal 2 - Product Service
   cd backend/services/ProductService/ProductService.API
   dotnet run
   
   # Terminal 3 - API Gateway
   cd backend/ApiGateway
   dotnet run
   
   # ... repeat for other services
   
   # Or use the development script:
   cd infrastructure/scripts
   chmod +x dev.sh
   ./dev.sh start-all
   ```

6. **Start frontend**
   ```bash
   cd frontend/ecommerce-web
   npm install --legacy-peer-deps
   npm run dev
   ```

7. **Access the application**
   - 🌐 **Frontend**: http://localhost:3000
   - 🔌 **API Gateway**: http://localhost:5000
   - 🔍 **Seq Logs**: http://localhost:5341
   - 🐰 **RabbitMQ**: http://localhost:15672 (guest/guest)
   - 🐘 **PostgreSQL**: localhost:5432 (ecommerce_user/dev_password_123)
   - 🔴 **Redis**: localhost:6379

## 🐳 Docker Development

Run the entire stack with Docker Compose:

```bash
# Build and start all services
docker-compose up --build

# Or run in detached mode
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down

# Stop and remove volumes (clean slate)
docker-compose down -v
```

## 🔧 Development

### Running Individual Services

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
cd backend/services/IdentityService/src/IdentityService.API
dotnet run

# Product Service
cd backend/services/ProductService/src/ProductService.API
dotnet run
```

### Running Tests

```bash
# All tests
./scripts/run-tests.sh

# Specific service
cd backend/services/IdentityService
dotnet test
```

### Database Migrations

```bash
# Add migration
cd backend/services/IdentityService/src/IdentityService.Infrastructure
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## 🐳 Docker Deployment

### Development
```bash
docker-compose up --build
```

### Production
```bash
docker-compose -f docker-compose.prod.yml up --build -d
```

## 📚 Documentation

- [Architecture Overview](docs/architecture/SYSTEM_ARCHITECTURE.md)
- [Microservices Details](docs/architecture/MICROSERVICES_OVERVIEW.md)
- [API Documentation](docs/api/API_CONVENTIONS.md)
- [Local Development Setup](docs/setup/LOCAL_DEVELOPMENT_SETUP.md)
- [Deployment Guide](docs/deployment/DEPLOYMENT_GUIDE.md)
- [Contributing Guide](docs/guides/CONTRIBUTION_GUIDE.md)

## 🛠️ Technology Stack

### Backend
- .NET 8
- Entity Framework Core
- MassTransit (RabbitMQ)
- Serilog
- FluentValidation
- AutoMapper
- MediatR
- YARP

### Frontend
- Next.js 14
- TypeScript
- Tailwind CSS
- Shadcn UI
- Zustand
- React Hook Form
- Axios
- NextAuth.js

### Infrastructure
- PostgreSQL
- Redis
- RabbitMQ
- AWS S3
- Docker
- Kubernetes
- Nginx

### Third-party Services
- Stripe (Payments)
- SendGrid (Emails)
- AWS S3 (File Storage)

## 🌟 Features

### Customer Features
- User registration and authentication
- Product browsing and search
- Product reviews and ratings
- Shopping cart management
- Wishlist functionality
- Secure checkout process
- Stripe payment integration
- Order tracking
- Order history
- User profile management
- Address management

### Admin Features
- Product management (CRUD)
- Category management
- Order management
- Customer management
- Inventory management
- Dashboard analytics

## 🔐 Security

- JWT-based authentication
- Password hashing with BCrypt
- HTTPS enforcement
- CORS configuration
- Input validation
- SQL injection prevention
- XSS protection
- Rate limiting
- API authentication

## 📈 Performance

- Redis caching
- Database indexing
- Query optimization
- Image optimization
- Lazy loading
- Code splitting
- CDN integration
- Response compression

## 🧪 Testing

- Unit tests (xUnit)
- Integration tests
- API tests (Postman/Newman)
- E2E tests (Playwright)
- Test containers

## 📊 Monitoring & Logging

- Structured logging with Serilog
- Centralized log aggregation with Seq
- Correlation IDs across services
- Health checks
- Performance metrics

## 🤝 Contributing

Please read our [Contributing Guide](docs/guides/CONTRIBUTION_GUIDE.md) for details on our code of conduct and the process for submitting pull requests.

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Ricardo Castro** - *Initial work* - [rcrock1978](https://github.com/rcrock1978)

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- Microservices patterns by Chris Richardson
- .NET Microservices Architecture Guide by Microsoft

## 📞 Support

For support, create an issue in the repository or contact the maintainers.

## 🗺️ Roadmap

- [ ] Complete all microservices
- [ ] Implement frontend features
- [ ] Add Kubernetes deployment
- [ ] Set up CI/CD pipelines
- [ ] Add comprehensive tests
- [ ] Implement monitoring dashboards
- [ ] Add mobile app (React Native)
- [ ] Multi-language support
- [ ] Advanced analytics
- [ ] Real-time notifications (SignalR)

---

**Status**: 🚧 Under Active Development

**Last Updated**: January 4, 2026
