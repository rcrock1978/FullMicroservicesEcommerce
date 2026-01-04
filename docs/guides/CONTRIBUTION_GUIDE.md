# Contributing Guide

Thank you for your interest in contributing to the E-Commerce Microservices Platform! This guide will help you get started.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)
- [Testing Requirements](#testing-requirements)
- [Documentation](#documentation)

---

## Code of Conduct

### Our Pledge
We are committed to providing a welcoming and inclusive experience for everyone. We expect all contributors to:

- Be respectful and professional
- Accept constructive criticism gracefully
- Focus on what is best for the community
- Show empathy towards other community members

### Unacceptable Behavior
- Harassment, discriminatory jokes, or personal attacks
- Trolling or insulting comments
- Public or private harassment
- Publishing others' private information
- Other conduct that could reasonably be considered inappropriate

---

## Getting Started

### 1. Fork the Repository
```bash
# Click "Fork" on GitHub, then clone your fork
git clone https://github.com/YOUR_USERNAME/FullMicroservicesEcommerce.git
cd FullMicroservicesEcommerce
```

### 2. Add Upstream Remote
```bash
git remote add upstream https://github.com/rcrock1978/FullMicroservicesEcommerce.git
```

### 3. Setup Development Environment
```bash
# Run the setup script
./scripts/setup.sh

# Or manually:
cp .env.example .env
docker-compose up -d postgres redis rabbitmq seq
./scripts/run-migrations.sh
```

### 4. Create a Branch
```bash
git checkout -b feature/your-feature-name
```

---

## Development Workflow

### Branch Naming Convention
- `feature/description` - New features
- `bugfix/description` - Bug fixes
- `hotfix/description` - Critical fixes
- `docs/description` - Documentation updates
- `refactor/description` - Code refactoring
- `test/description` - Test additions/updates

### Example
```bash
git checkout -b feature/add-product-reviews
git checkout -b bugfix/fix-cart-calculation
git checkout -b docs/update-api-docs
```

### Keep Your Fork Updated
```bash
git fetch upstream
git checkout main
git merge upstream/main
git push origin main
```

---

## Coding Standards

### Backend (.NET)

#### Naming Conventions
- **PascalCase**: Classes, methods, properties, enums
  ```csharp
  public class ProductService { }
  public async Task<Product> GetProductByIdAsync(int id) { }
  public string ProductName { get; set; }
  ```

- **camelCase**: Private fields, parameters, local variables
  ```csharp
  private readonly IProductRepository _productRepository;
  public Product GetProduct(int productId) { }
  ```

- **UPPER_CASE**: Constants
  ```csharp
  public const int MAX_PAGE_SIZE = 100;
  ```

#### Code Style
- Use `var` when type is obvious
- Use expression-bodied members when appropriate
- Async methods must end with `Async`
- Use `ConfigureAwait(false)` in libraries
- Always use braces for if/for/while statements

```csharp
// ✅ Good
public async Task<Product> GetProductAsync(int id)
{
    var product = await _repository.GetByIdAsync(id);
    return product;
}

// ❌ Bad
public async Task<Product> GetProduct(int id)
{
    Product product = await _repository.GetByIdAsync(id);
    return product;
}
```

#### SOLID Principles
- Single Responsibility Principle
- Open/Closed Principle
- Liskov Substitution Principle
- Interface Segregation Principle
- Dependency Inversion Principle

### Frontend (TypeScript/React)

#### Naming Conventions
- **PascalCase**: Components, interfaces, types, enums
  ```typescript
  function ProductCard() { }
  interface User { }
  type Product = { };
  enum OrderStatus { }
  ```

- **camelCase**: Functions, variables, props
  ```typescript
  const getUserData = () => { };
  const productName = "Laptop";
  ```

- **UPPER_CASE**: Constants
  ```typescript
  const API_BASE_URL = "http://localhost:5000";
  ```

#### React Best Practices
- Use functional components with hooks
- Keep components small and focused
- Extract custom hooks for reusable logic
- Use TypeScript for type safety
- Props destructuring in component signature

```typescript
// ✅ Good
interface ProductCardProps {
  product: Product;
  onAddToCart: (id: number) => void;
}

export function ProductCard({ product, onAddToCart }: ProductCardProps) {
  return (
    <div>
      <h3>{product.name}</h3>
      <button onClick={() => onAddToCart(product.id)}>Add to Cart</button>
    </div>
  );
}

// ❌ Bad
export function ProductCard(props: any) {
  return (
    <div>
      <h3>{props.product.name}</h3>
      <button onClick={() => props.onAddToCart(props.product.id)}>
        Add to Cart
      </button>
    </div>
  );
}
```

---

## Commit Guidelines

### Commit Message Format
```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting, etc.)
- **refactor**: Code refactoring
- **test**: Adding or updating tests
- **chore**: Maintenance tasks
- **perf**: Performance improvements

### Examples
```
feat(product): add product review functionality

- Add review entity and repository
- Create review API endpoints
- Implement review validation

Closes #123
```

```
fix(cart): correct total price calculation

The cart was not properly calculating tax.
This fix ensures tax is calculated before shipping.

Fixes #456
```

```
docs(api): update API conventions documentation

- Add pagination examples
- Update error handling section
- Fix typos
```

### Rules
- Use present tense ("add feature" not "added feature")
- Use imperative mood ("move cursor" not "moves cursor")
- First line should be 50 characters or less
- Reference issues and pull requests when applicable
- Explain **what** and **why**, not **how**

---

## Pull Request Process

### Before Submitting

1. **Update from upstream**
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Run tests**
   ```bash
   ./scripts/run-tests.sh
   ```

3. **Format code**
   ```bash
   # Backend
   dotnet format
   
   # Frontend
   cd frontend/ecommerce-web
   npm run format
   npm run lint
   ```

4. **Update documentation**
   - Update README if needed
   - Add/update API documentation
   - Update CHANGELOG.md

### Creating Pull Request

1. **Push your branch**
   ```bash
   git push origin feature/your-feature-name
   ```

2. **Create PR on GitHub**
   - Use clear, descriptive title
   - Fill out PR template completely
   - Link related issues
   - Add screenshots for UI changes
   - Request reviewers

### PR Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Comments added for complex code
- [ ] Documentation updated
- [ ] No new warnings
- [ ] Tests added/updated
- [ ] All tests pass
```

### Review Process
- At least one approval required
- All CI checks must pass
- Address all review comments
- Squash commits before merging (if requested)

### After Merge
```bash
# Update your main branch
git checkout main
git pull upstream main
git push origin main

# Delete feature branch
git branch -d feature/your-feature-name
git push origin --delete feature/your-feature-name
```

---

## Testing Requirements

### Backend Tests

#### Unit Tests
- Test business logic
- Mock dependencies
- Use xUnit framework
- Aim for 80%+ coverage

```csharp
public class ProductServiceTests
{
    [Fact]
    public async Task GetProductById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Product { Id = 1, Name = "Test" });
        var service = new ProductService(mockRepo.Object);

        // Act
        var result = await service.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }
}
```

#### Integration Tests
- Test API endpoints
- Use TestContainers for database
- Test actual HTTP requests

```csharp
public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetProducts_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/products");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

### Frontend Tests

#### Component Tests
```typescript
import { render, screen } from '@testing-library/react';
import { ProductCard } from './ProductCard';

test('renders product name', () => {
  const product = { id: 1, name: 'Laptop', price: 999 };
  render(<ProductCard product={product} />);
  
  const nameElement = screen.getByText(/Laptop/i);
  expect(nameElement).toBeInTheDocument();
});
```

#### E2E Tests
```typescript
test('user can add product to cart', async ({ page }) => {
  await page.goto('http://localhost:3000/products/1');
  await page.click('button:has-text("Add to Cart")');
  
  const cartCount = await page.textContent('[data-testid="cart-count"]');
  expect(cartCount).toBe('1');
});
```

### Running Tests
```bash
# All tests
./scripts/run-tests.sh

# Backend only
cd backend/services/IdentityService
dotnet test

# Frontend only
cd frontend/ecommerce-web
npm test
```

---

## Documentation

### Code Documentation

#### XML Comments (C#)
```csharp
/// <summary>
/// Retrieves a product by its unique identifier.
/// </summary>
/// <param name="id">The product ID.</param>
/// <returns>The product if found; otherwise, null.</returns>
/// <exception cref="ArgumentException">Thrown when id is invalid.</exception>
public async Task<Product?> GetProductByIdAsync(int id)
{
    // Implementation
}
```

#### JSDoc (TypeScript)
```typescript
/**
 * Fetches products from the API
 * @param {number} page - The page number
 * @param {number} pageSize - Number of items per page
 * @returns {Promise<ProductsResponse>} The products response
 * @throws {ApiError} When the API request fails
 */
export async function getProducts(
  page: number,
  pageSize: number
): Promise<ProductsResponse> {
  // Implementation
}
```

### API Documentation
- Use OpenAPI/Swagger annotations
- Include request/response examples
- Document all error codes
- Update Postman collections

### Architecture Documentation
- Update architecture diagrams
- Document design decisions
- Explain patterns used
- Add sequence diagrams for complex flows

---

## Need Help?

### Resources
- [Project Documentation](../README.md)
- [Architecture Overview](../docs/architecture/SYSTEM_ARCHITECTURE.md)
- [Local Setup Guide](../docs/setup/LOCAL_DEVELOPMENT_SETUP.md)
- [API Conventions](../docs/api/API_CONVENTIONS.md)

### Communication
- **Issues**: For bugs and feature requests
- **Discussions**: For questions and ideas
- **Pull Requests**: For code contributions

### Getting Help
1. Check existing issues and documentation
2. Search closed issues and PRs
3. Ask in GitHub Discussions
4. Create a new issue with detailed information

---

## Thank You! 🎉

Your contributions make this project better. We appreciate your time and effort!

**Happy coding! 🚀**
