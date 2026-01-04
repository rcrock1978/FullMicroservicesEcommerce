'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import Image from 'next/image';
import { toast } from 'sonner';
import { ShoppingCart, Search } from 'lucide-react';
import { productService } from '@/services/product.service';
import { Category, Product } from '@/types';
import { formatPrice } from '@/lib/utils';
import { useCartStore } from '@/store/cart';
import { ProductGridSkeleton } from '@/components/Skeleton';

export default function CategoryDetailPage() {
  const params = useParams();
  const router = useRouter();
  const categoryId = params.id as string;
  const { addItem } = useCartStore();

  const [category, setCategory] = useState<Category | null>(null);
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    loadCategoryAndProducts();
  }, [categoryId]);

  const loadCategoryAndProducts = async () => {
    try {
      setLoading(true);
      const [categoryData, productsData] = await Promise.all([
        productService.getCategory(parseInt(categoryId)),
        productService.getProducts({ categoryId: parseInt(categoryId) }),
      ]);
      setCategory(categoryData);
      setProducts(productsData.items);
    } catch (error: any) {
      toast.error('Failed to load category');
      router.push('/products');
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = (product: Product) => {
    if (product.stockQuantity <= 0) {
      toast.error('This product is out of stock');
      return;
    }
    addItem(product);
    toast.success(`${product.name} added to cart`);
  };

  const filteredProducts = products.filter((product) =>
    product.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    product.description.toLowerCase().includes(searchQuery.toLowerCase())
  );

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="animate-pulse mb-8">
          <div className="h-8 bg-gray-200 rounded w-1/4 mb-2"></div>
          <div className="h-4 bg-gray-200 rounded w-1/2"></div>
        </div>
        <ProductGridSkeleton />
      </div>
    );
  }

  if (!category) {
    return null;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Category Header */}
      <div className="mb-8">
        <h1 className="text-4xl font-bold mb-2">{category.name}</h1>
        {category.description && (
          <p className="text-gray-600 text-lg">{category.description}</p>
        )}
        <p className="text-gray-500 mt-2">
          {filteredProducts.length} {filteredProducts.length === 1 ? 'product' : 'products'}
        </p>
      </div>

      {/* Search Bar */}
      <div className="mb-6">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
          <input
            type="text"
            placeholder="Search products..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="input pl-10 w-full"
          />
        </div>
      </div>

      {/* Products Grid */}
      {filteredProducts.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-gray-500 text-lg">No products found in this category.</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {filteredProducts.map((product) => (
            <div key={product.id} className="card group">
              <div
                className="aspect-square relative mb-4 bg-gray-100 rounded-lg overflow-hidden cursor-pointer"
                onClick={() => router.push(`/products/${product.id}`)}
              >
                {product.imageUrl ? (
                  <Image
                    src={product.imageUrl}
                    alt={product.name}
                    fill
                    className="object-cover group-hover:scale-105 transition-transform duration-300"
                  />
                ) : (
                  <div className="absolute inset-0 flex items-center justify-center text-gray-400">
                    No Image
                  </div>
                )}
                {product.stockQuantity <= 0 && (
                  <div className="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <span className="bg-red-500 text-white px-4 py-2 rounded-lg font-semibold">
                      Out of Stock
                    </span>
                  </div>
                )}
              </div>

              <h3
                className="font-semibold text-lg mb-2 line-clamp-1 cursor-pointer hover:text-primary-600"
                onClick={() => router.push(`/products/${product.id}`)}
              >
                {product.name}
              </h3>
              <p className="text-gray-600 text-sm mb-3 line-clamp-2">{product.description}</p>

              <div className="flex items-center justify-between mt-auto">
                <span className="text-2xl font-bold text-primary-600">
                  {formatPrice(product.price)}
                </span>
                <button
                  onClick={() => handleAddToCart(product)}
                  disabled={product.stockQuantity <= 0}
                  className="btn btn-primary"
                >
                  <ShoppingCart className="w-4 h-4" />
                </button>
              </div>

              {product.stockQuantity > 0 && product.stockQuantity <= 10 && (
                <p className="text-amber-600 text-sm mt-2">
                  Only {product.stockQuantity} left in stock
                </p>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
