# 🛒 Full Stack E-Commerce Microservices Platform

A complete, production-ready e-commerce platform built with modern microservices architecture, featuring .NET 8 backend services, Next.js 14 frontend, and comprehensive DevOps practices.

## 🎯 Project Overview

This is a full-featured e-commerce platform demonstrating best practices in microservices architecture, including:

- **Microservices Architecture**: Independent, scalable services
- **Event-Driven Communication**: RabbitMQ for inter-service messaging
- **CQRS Pattern**: Command Query Responsibility Segregation
- **Domain-Driven Design**: Clean architecture with clear boundaries
- **API Gateway**: Centralized entry point with YARP
- **Modern Frontend**: Next.js 14 with TypeScript and Tailwind CSS
- **Containerization**: Docker and Docker Compose
- **Authentication & Authorization**: JWT-based security
- **Payment Integration**: Stripe payment processing
- **Cloud Storage**: AWS S3 for media files
- **Caching**: Redis for performance optimization
- **Monitoring & Logging**: Serilog and Seq

## 🏗️ Architecture

### Backend Microservices (.NET 8)

1. **Identity Service** - Authentication & authorization
2. **Product Catalog Service** - Product management with Redis caching
3. **Shopping Cart Service** - Cart and wishlist management
4. **Order Service** - Order processing with Saga pattern
5. **Payment Service** - Stripe payment integration
6. **Media Service** - Image upload and management (AWS S3)
7. **Notification Service** - Email notifications (SendGrid)
8. **API Gateway** - YARP reverse proxy

### Frontend (Next.js 14)

- Server-side rendering (SSR)
- TypeScript
- Tailwind CSS
- Shadcn UI components
- Zustand state management
- NextAuth.js authentication

### Infrastructure

- **Databases**: PostgreSQL per service
- **Cache**: Redis
- **Message Broker**: RabbitMQ
- **API Gateway**: YARP (Yet Another Reverse Proxy)
- **Logging**: Serilog + Seq
- **Containerization**: Docker & Docker Compose
- **Orchestration**: Kubernetes (optional)

## 📁 Project Structure

```
ecommerce-microservices/
├── backend/
│   ├── Shared/
│   │   ├── Shared.Common/
│   │   ├── Shared.Messaging/
│   │   └── Shared.Security/
│   ├── services/
│   │   ├── IdentityService/
│   │   ├── ProductService/
│   │   ├── CartService/
│   │   ├── OrderService/
│   │   ├── PaymentService/
│   │   ├── MediaService/
│   │   └── NotificationService/
│   └── ApiGateway/
├── frontend/
│   └── ecommerce-web/
├── infrastructure/
│   ├── docker/
│   ├── k8s/
│   └── scripts/
├── docs/
│   ├── architecture/
│   ├── api/
│   ├── setup/
│   ├── deployment/
│   └── guides/
└── tests/
    ├── integration/
    └── e2e/
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/rcrock1978/FullMicroservicesEcommerce.git
   cd FullMicroservicesEcommerce
   ```

2. **Setup environment variables**
   ```bash
   cp .env.example .env
   # Edit .env with your configuration
   ```

3. **Start infrastructure services**
   ```bash
   docker-compose up -d postgres redis rabbitmq seq
   ```

4. **Run database migrations**
   ```bash
   ./scripts/run-migrations.sh
   ```

5. **Start backend services**
   ```bash
   ./scripts/start-services.sh
   ```

6. **Start frontend**
   ```bash
   cd frontend/ecommerce-web
   npm install
   npm run dev
   ```

7. **Access the application**
   - Frontend: http://localhost:3000
   - API Gateway: http://localhost:5000
   - Seq Logs: http://localhost:5341
   - RabbitMQ Management: http://localhost:15672

## 🔧 Development

### Running Individual Services

```bash
# Identity Service
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
