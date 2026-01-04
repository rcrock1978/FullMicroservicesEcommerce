export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber?: string;
  role: string;
}

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl?: string;
  categoryId: number;
  categoryName?: string;
  stockQuantity: number;
  isActive: boolean;
}

export interface Category {
  id: number;
  name: string;
  description?: string;
  imageUrl?: string;
}

export interface CartItem {
  id: number;
  productId: number;
  product: Product;
  quantity: number;
  price: number;
}

export interface Cart {
  id: number;
  userId: number;
  items: CartItem[];
  totalAmount: number;
}

export interface Order {
  id: number;
  userId: number;
  status: OrderStatus;
  totalAmount: number;
  shippingAddress: Address;
  billingAddress: Address;
  items: OrderItem[];
  createdAt: string;
  updatedAt: string;
}

export interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface Address {
  street: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
}

export enum OrderStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  Processing = 'Processing',
  Shipped = 'Shipped',
  Delivered = 'Delivered',
  Cancelled = 'Cancelled',
}

export interface Payment {
  id: number;
  orderId: number;
  amount: number;
  status: PaymentStatus;
  paymentMethod: string;
  stripePaymentIntentId?: string;
  createdAt: string;
}

export enum PaymentStatus {
  Pending = 'Pending',
  Processing = 'Processing',
  Succeeded = 'Succeeded',
  Failed = 'Failed',
  Refunded = 'Refunded',
}

export interface Notification {
  id: number;
  userId: number;
  type: NotificationType;
  subject: string;
  message: string;
  status: NotificationStatus;
  createdAt: string;
  sentAt?: string;
}

export enum NotificationType {
  Email = 'Email',
  SMS = 'SMS',
  Push = 'Push',
  InApp = 'InApp',
}

export enum NotificationStatus {
  Pending = 'Pending',
  Sent = 'Sent',
  Failed = 'Failed',
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
}
