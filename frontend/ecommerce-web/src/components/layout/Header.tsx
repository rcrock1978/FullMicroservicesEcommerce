'use client';

import Link from 'next/link';
import { ShoppingCart, User, LogOut, Menu, X } from 'lucide-react';
import { useAuthStore } from '@/store/auth';
import { useCartStore } from '@/store/cart';
import { useState } from 'react';

export default function Header() {
  const { user, isAuthenticated, clearAuth } = useAuthStore();
  const { getTotalItems } = useCartStore();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const handleLogout = () => {
    clearAuth();
    window.location.href = '/';
  };

  const cartItemCount = getTotalItems();

  return (
    <header className="bg-white shadow-sm sticky top-0 z-50">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link href="/" className="text-2xl font-bold text-primary-600">
            E-Commerce
          </Link>

          {/* Desktop Navigation */}
          <nav className="hidden md:flex items-center space-x-8">
            <Link href="/products" className="text-gray-700 hover:text-primary-600 transition">
              Products
            </Link>
            <Link href="/categories" className="text-gray-700 hover:text-primary-600 transition">
              Categories
            </Link>
            {isAuthenticated() && (
              <Link href="/orders" className="text-gray-700 hover:text-primary-600 transition">
                My Orders
              </Link>
            )}
          </nav>

          {/* Actions */}
          <div className="flex items-center space-x-4">
            {/* Cart */}
            <Link href="/cart" className="relative p-2 text-gray-700 hover:text-primary-600 transition">
              <ShoppingCart className="w-6 h-6" />
              {cartItemCount > 0 && (
                <span className="absolute -top-1 -right-1 bg-primary-600 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
                  {cartItemCount}
                </span>
              )}
            </Link>

            {/* User Menu */}
            {isAuthenticated() ? (
              <div className="hidden md:flex items-center space-x-4">
                <Link href="/profile" className="flex items-center gap-2 text-gray-700 hover:text-primary-600 transition">
                  <User className="w-5 h-5" />
                  <span>{user?.firstName}</span>
                </Link>
                <button
                  onClick={handleLogout}
                  className="flex items-center gap-2 text-gray-700 hover:text-red-600 transition"
                >
                  <LogOut className="w-5 h-5" />
                  Logout
                </button>
              </div>
            ) : (
              <div className="hidden md:flex items-center space-x-4">
                <Link href="/login" className="text-gray-700 hover:text-primary-600 transition">
                  Login
                </Link>
                <Link href="/register" className="btn-primary">
                  Sign Up
                </Link>
              </div>
            )}

            {/* Mobile Menu Toggle */}
            <button
              className="md:hidden p-2"
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
            >
              {mobileMenuOpen ? <X className="w-6 h-6" /> : <Menu className="w-6 h-6" />}
            </button>
          </div>
        </div>

        {/* Mobile Menu */}
        {mobileMenuOpen && (
          <div className="md:hidden py-4 border-t">
            <nav className="flex flex-col space-y-4">
              <Link
                href="/products"
                className="text-gray-700 hover:text-primary-600 transition"
                onClick={() => setMobileMenuOpen(false)}
              >
                Products
              </Link>
              <Link
                href="/categories"
                className="text-gray-700 hover:text-primary-600 transition"
                onClick={() => setMobileMenuOpen(false)}
              >
                Categories
              </Link>
              {isAuthenticated() ? (
                <>
                  <Link
                    href="/orders"
                    className="text-gray-700 hover:text-primary-600 transition"
                    onClick={() => setMobileMenuOpen(false)}
                  >
                    My Orders
                  </Link>
                  <Link
                    href="/profile"
                    className="text-gray-700 hover:text-primary-600 transition"
                    onClick={() => setMobileMenuOpen(false)}
                  >
                    Profile
                  </Link>
                  <button
                    onClick={() => {
                      handleLogout();
                      setMobileMenuOpen(false);
                    }}
                    className="text-left text-red-600 hover:text-red-700 transition"
                  >
                    Logout
                  </button>
                </>
              ) : (
                <>
                  <Link
                    href="/login"
                    className="text-gray-700 hover:text-primary-600 transition"
                    onClick={() => setMobileMenuOpen(false)}
                  >
                    Login
                  </Link>
                  <Link
                    href="/register"
                    className="btn-primary inline-block text-center"
                    onClick={() => setMobileMenuOpen(false)}
                  >
                    Sign Up
                  </Link>
                </>
              )}
            </nav>
          </div>
        )}
      </div>
    </header>
  );
}
