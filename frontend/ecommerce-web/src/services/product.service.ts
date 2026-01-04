import apiClient from '@/lib/api-client';
import { Product, Category, PaginatedResponse } from '@/types';

export interface ProductFilters {
  categoryId?: number;
  minPrice?: number;
  maxPrice?: number;
  search?: string;
  pageNumber?: number;
  pageSize?: number;
}

export const productService = {
  async getProducts(filters: ProductFilters = {}): Promise<PaginatedResponse<Product>> {
    const params = new URLSearchParams();
    Object.entries(filters).forEach(([key, value]) => {
      if (value !== undefined) params.append(key, value.toString());
    });
    const response = await apiClient.get(`/api/products?${params.toString()}`);
    return response.data;
  },

  async getProduct(id: number): Promise<Product> {
    const response = await apiClient.get(`/api/products/${id}`);
    return response.data;
  },

  async getCategories(): Promise<Category[]> {
    const response = await apiClient.get('/api/products/categories');
    return response.data;
  },

  async getCategory(id: number): Promise<Category> {
    const response = await apiClient.get(`/api/products/categories/${id}`);
    return response.data;
  },
};
