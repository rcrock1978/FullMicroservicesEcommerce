# E-Commerce Frontend

Modern, responsive Next.js e-commerce application built with React 19, TypeScript, and Tailwind CSS.

## Features

- 🛍️ **Product Browsing**: Search, filter, and browse products by category
- 🔐 **Authentication**: Secure login and registration with JWT
- 🛒 **Shopping Cart**: Add, remove, and manage cart items with persistence
- 💳 **Checkout**: Secure payment processing with Stripe
- 📦 **Order Management**: Track orders and view order history
- 📱 **Responsive Design**: Mobile-first design with Tailwind CSS
- 🎨 **Modern UI**: Clean interface with smooth transitions
- ⚡ **Fast Performance**: Next.js 15 with App Router and React Server Components

## Tech Stack

- **Framework**: Next.js 15
- **Language**: TypeScript
- **Styling**: Tailwind CSS
- **State Management**: Zustand
- **Forms**: React Hook Form + Zod
- **HTTP Client**: Axios
- **Payments**: Stripe
- **Icons**: Lucide React
- **Notifications**: Sonner

## Getting Started

### Prerequisites

- Node.js 18+ 
- npm or yarn
- Backend API running on http://localhost:5000

### Installation

```bash
cd frontend/ecommerce-web
npm install
```

### Environment Variables

Create a `.env.local` file:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
NEXTAUTH_URL=http://localhost:3000
NEXTAUTH_SECRET=your-secret-key-min-32-characters-long
NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY=pk_test_your_stripe_key
```

### Development

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000)

### Build

```bash
npm run build
npm start
```

## Project Structure

```
src/
├── app/                    # Next.js app directory
│   ├── layout.tsx         # Root layout
│   ├── page.tsx           # Homepage
│   ├── login/             # Login page
│   ├── register/          # Registration page
│   ├── products/          # Products listing & detail
│   ├── cart/              # Shopping cart
│   ├── checkout/          # Checkout flow
│   └── orders/            # Order management
├── components/            # Reusable components
│   └── layout/            # Layout components
├── lib/                   # Utilities
│   ├── api-client.ts     # Axios instance
│   └── utils.ts           # Helper functions
├── services/              # API service layers
│   ├── auth.service.ts
│   ├── product.service.ts
│   └── order.service.ts
├── store/                 # Zustand stores
│   ├── auth.ts            # Auth state
│   └── cart.ts            # Cart state
└── types/                 # TypeScript types
    └── index.ts
```

## Key Features

### Authentication

- JWT-based authentication
- Persistent sessions with Zustand
- Protected routes
- Auto-redirect on token expiration

### Shopping Cart

- Add/remove products
- Update quantities
- Local storage persistence
- Real-time total calculation

### Checkout

- Stripe payment integration
- Address validation
- Order summary
- Secure payment processing

### Product Management

- Search functionality
- Category filtering
- Product details with images
- Stock availability

### Order Tracking

- Order history
- Order status tracking
- Order details view
- Order cancellation

## API Integration

All API calls go through the API Gateway at `http://localhost:5000`:

- `/api/identity/*` - Authentication & user management
- `/api/products/*` - Product catalog
- `/api/cart/*` - Shopping cart
- `/api/orders/*` - Order management
- `/api/payments/*` - Payment processing

## Styling

Custom Tailwind CSS utilities in `globals.css`:

- `.btn` - Base button styles
- `.btn-primary` - Primary action buttons
- `.btn-secondary` - Secondary buttons
- `.btn-outline` - Outlined buttons
- `.input` - Form input fields
- `.card` - Card containers
- `.link` - Text links

## State Management

### Auth Store

```typescript
const { user, token, setAuth, clearAuth, isAuthenticated } = useAuthStore();
```

### Cart Store

```typescript
const { items, addItem, removeItem, updateQuantity, getTotalPrice } = useCartStore();
```

## Deployment

### Vercel (Recommended)

```bash
npm install -g vercel
vercel
```

### Docker

```bash
docker build -t ecommerce-frontend .
docker run -p 3000:3000 ecommerce-frontend
```

## Environment Configuration

### Development
- API URL: `http://localhost:5000`
- Stripe: Test mode keys

### Production
- API URL: Your production API gateway
- Stripe: Live mode keys
- Enable HTTPS
- Set secure NEXTAUTH_SECRET

## Performance Optimization

- ✅ Next.js Image Optimization
- ✅ Code Splitting
- ✅ Lazy Loading
- ✅ Client-side Caching
- ✅ Optimized Bundle Size

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## License

MIT License - see LICENSE file for details
