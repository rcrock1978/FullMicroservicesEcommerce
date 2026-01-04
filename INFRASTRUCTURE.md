# DevOps & Infrastructure - Phase 10

This directory contains all the infrastructure and deployment configurations for the E-commerce microservices platform.

## Overview

The infrastructure is designed for both local development and production deployment with the following components:

- **Docker**: Containerization for all services
- **Docker Compose**: Local development environment orchestration
- **Kubernetes**: Production-ready orchestration platform
- **CI/CD**: GitHub Actions for automated build and deployment

## Quick Start

### Local Development with Docker Compose

```bash
# Create .env file from example
cp .env.example .env

# Edit .env and add your API keys (SendGrid, Twilio, Stripe, AWS)
nano .env

# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

### Services Access

Once running, services are available at:

- **API Gateway**: http://localhost:5000
- **Notification Service**: http://localhost:5007
- **PostgreSQL**: localhost:5432
- **Redis**: localhost:6379
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **Seq Logs**: http://localhost:5341

## Docker

### Building Individual Services

```bash
# Build Notification Service
cd backend/services/NotificationService
docker build -t ecommerce/notification-service:latest -f NotificationService.API/Dockerfile .

# Build API Gateway
cd backend/gateway/ApiGateway
docker build -t ecommerce/api-gateway:latest -f Dockerfile .
```

### Dockerfile Structure

Each service uses multi-stage builds:
1. **base**: Runtime image (ASP.NET Core)
2. **build**: Build environment (SDK)
3. **publish**: Published artifacts
4. **final**: Minimal runtime image

## Kubernetes

### Prerequisites

- Kubernetes cluster (minikube, kind, EKS, AKS, GKE)
- kubectl configured
- Docker images pushed to registry

### Deployment

```bash
# Apply all manifests
kubectl apply -f k8s/

# Or step by step:
kubectl apply -f k8s/00-namespace.yaml
kubectl apply -f k8s/01-postgres.yaml
kubectl apply -f k8s/02-redis.yaml
kubectl apply -f k8s/10-notification-service.yaml
kubectl apply -f k8s/20-api-gateway.yaml

# Check status
kubectl get pods -n ecommerce
kubectl get services -n ecommerce

# View logs
kubectl logs -f deployment/notification-service -n ecommerce
```

### Configuration

Update secrets before deploying to production:

```bash
kubectl create secret generic app-secrets \
  --from-literal=JWT_SECRET_KEY='your-secret-key' \
  --from-literal=POSTGRES_PASSWORD='your-password' \
  --from-literal=SENDGRID_API_KEY='your-key' \
  --from-literal=TWILIO_ACCOUNT_SID='your-sid' \
  --from-literal=TWILIO_AUTH_TOKEN='your-token' \
  --from-literal=STRIPE_SECRET_KEY='your-key' \
  --from-literal=STRIPE_WEBHOOK_SECRET='your-secret' \
  -n ecommerce
```

### Ingress

The API Gateway is exposed via Ingress with TLS:

```yaml
# Update host in k8s/20-api-gateway.yaml
- host: api.yourdomain.com
```

Install NGINX Ingress Controller:

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml
```

Install cert-manager for automatic TLS:

```bash
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.0/cert-manager.yaml
```

## CI/CD

### GitHub Actions

The pipeline automatically:
1. **Builds** all services on push to main/develop
2. **Tests** with unit and integration tests
3. **Publishes** Docker images to GitHub Container Registry
4. **Deploys** to Kubernetes cluster on main branch

### Setup

1. Add repository secrets:
   - `KUBE_CONFIG`: Base64 encoded kubeconfig
   - `SENDGRID_API_KEY`
   - `TWILIO_ACCOUNT_SID`
   - `TWILIO_AUTH_TOKEN`
   - `STRIPE_SECRET_KEY`
   - `STRIPE_WEBHOOK_SECRET`

2. Configure Docker registry access:
   ```bash
   # GitHub Container Registry is used by default
   # Images are pushed to ghcr.io/your-username/ecommerce-*
   ```

### Manual Deployment

To deploy manually from CI/CD:

```bash
# Build
dotnet build backend/services/NotificationService/NotificationService.sln --configuration Release

# Test
dotnet test backend/services/NotificationService/NotificationService.sln --configuration Release

# Build Docker image
docker build -t ghcr.io/your-username/ecommerce-notification-service:latest \
  -f backend/services/NotificationService/NotificationService.API/Dockerfile \
  backend/services/NotificationService

# Push to registry
docker push ghcr.io/your-username/ecommerce-notification-service:latest

# Deploy to Kubernetes
kubectl set image deployment/notification-service \
  notification-service=ghcr.io/your-username/ecommerce-notification-service:latest \
  -n ecommerce
```

## Monitoring & Logging

### Seq

Centralized structured logging:
- Access: http://localhost:5341
- All services send logs to Seq
- Query and filter logs in real-time

### Health Checks

Each service exposes `/health` endpoint:

```bash
# Check notification service
curl http://localhost:5007/health

# Check via gateway
curl http://localhost:5000/health/notifications
```

## Scaling

### Docker Compose

```bash
# Scale notification service to 3 instances
docker-compose up -d --scale notification-service=3
```

### Kubernetes

```bash
# Scale notification service
kubectl scale deployment notification-service --replicas=5 -n ecommerce

# Horizontal Pod Autoscaler
kubectl autoscale deployment notification-service \
  --cpu-percent=70 \
  --min=2 \
  --max=10 \
  -n ecommerce
```

## Environment Variables

### Required for Production

| Variable | Description | Example |
|----------|-------------|---------|
| `JWT_SECRET_KEY` | JWT signing key (32+ chars) | `your-secret-key` |
| `POSTGRES_PASSWORD` | PostgreSQL password | `secure-password` |
| `SENDGRID_API_KEY` | SendGrid API key | `SG.xxx` |
| `TWILIO_ACCOUNT_SID` | Twilio Account SID | `ACxxx` |
| `TWILIO_AUTH_TOKEN` | Twilio Auth Token | `xxx` |
| `STRIPE_SECRET_KEY` | Stripe Secret Key | `sk_live_xxx` |
| `STRIPE_WEBHOOK_SECRET` | Stripe Webhook Secret | `whsec_xxx` |

## Troubleshooting

### Docker Compose Issues

```bash
# View logs for specific service
docker-compose logs notification-service

# Restart service
docker-compose restart notification-service

# Rebuild image
docker-compose build notification-service
docker-compose up -d notification-service

# Clean and restart
docker-compose down -v
docker-compose up -d
```

### Kubernetes Issues

```bash
# Check pod status
kubectl describe pod <pod-name> -n ecommerce

# View pod logs
kubectl logs <pod-name> -n ecommerce

# Get events
kubectl get events -n ecommerce --sort-by='.lastTimestamp'

# Exec into pod
kubectl exec -it <pod-name> -n ecommerce -- /bin/sh
```

## Production Checklist

- [ ] Update all secrets in Kubernetes
- [ ] Configure proper DNS for Ingress
- [ ] Set up TLS certificates
- [ ] Configure resource limits
- [ ] Enable monitoring (Prometheus/Grafana)
- [ ] Set up backup for PostgreSQL
- [ ] Configure log retention
- [ ] Enable RBAC
- [ ] Set up network policies
- [ ] Configure pod security policies

## Next Steps

1. Add monitoring with Prometheus and Grafana
2. Implement distributed tracing with OpenTelemetry
3. Set up service mesh (Istio/Linkerd)
4. Configure backup and disaster recovery
5. Implement blue-green deployment strategy
