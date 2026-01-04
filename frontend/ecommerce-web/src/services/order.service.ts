import apiClient from '@/lib/api-client';
import { Order, Address } from '@/types';

export interface CreateOrderRequest {
  shippingAddress: Address;
  billingAddress: Address;
  paymentMethodId: string;
}

export const orderService = {
  async createOrder(data: CreateOrderRequest): Promise<Order> {
    const response = await apiClient.post('/api/orders', data);
    return response.data;
  },

  async getOrders(): Promise<Order[]> {
    const response = await apiClient.get('/api/orders');
    return response.data;
  },

  async getOrder(id: number): Promise<Order> {
    const response = await apiClient.get(`/api/orders/${id}`);
    return response.data;
  },

  async cancelOrder(id: number): Promise<Order> {
    const response = await apiClient.post(`/api/orders/${id}/cancel`);
    return response.data;
  },
};
