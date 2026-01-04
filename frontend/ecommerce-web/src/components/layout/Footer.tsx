import Link from 'next/link';
import { Facebook, Twitter, Instagram, Mail } from 'lucide-react';

export default function Footer() {
  return (
    <footer className="bg-gray-900 text-gray-300">
      <div className="container mx-auto px-4 py-12">
        <div className="grid md:grid-cols-4 gap-8">
          {/* Company Info */}
          <div>
            <h3 className="text-white text-lg font-semibold mb-4">E-Commerce</h3>
            <p className="text-sm mb-4">
              Your trusted online shopping destination with quality products and excellent service.
            </p>
            <div className="flex space-x-4">
              <a href="#" className="hover:text-primary-400 transition">
                <Facebook className="w-5 h-5" />
              </a>
              <a href="#" className="hover:text-primary-400 transition">
                <Twitter className="w-5 h-5" />
              </a>
              <a href="#" className="hover:text-primary-400 transition">
                <Instagram className="w-5 h-5" />
              </a>
              <a href="#" className="hover:text-primary-400 transition">
                <Mail className="w-5 h-5" />
              </a>
            </div>
          </div>

          {/* Quick Links */}
          <div>
            <h4 className="text-white text-lg font-semibold mb-4">Quick Links</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link href="/products" className="hover:text-primary-400 transition">
                  Products
                </Link>
              </li>
              <li>
                <Link href="/categories" className="hover:text-primary-400 transition">
                  Categories
                </Link>
              </li>
              <li>
                <Link href="/cart" className="hover:text-primary-400 transition">
                  Shopping Cart
                </Link>
              </li>
              <li>
                <Link href="/orders" className="hover:text-primary-400 transition">
                  My Orders
                </Link>
              </li>
            </ul>
          </div>

          {/* Customer Service */}
          <div>
            <h4 className="text-white text-lg font-semibold mb-4">Customer Service</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link href="/contact" className="hover:text-primary-400 transition">
                  Contact Us
                </Link>
              </li>
              <li>
                <Link href="/shipping" className="hover:text-primary-400 transition">
                  Shipping Info
                </Link>
              </li>
              <li>
                <Link href="/returns" className="hover:text-primary-400 transition">
                  Returns
                </Link>
              </li>
              <li>
                <Link href="/faq" className="hover:text-primary-400 transition">
                  FAQ
                </Link>
              </li>
            </ul>
          </div>

          {/* Legal */}
          <div>
            <h4 className="text-white text-lg font-semibold mb-4">Legal</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link href="/privacy" className="hover:text-primary-400 transition">
                  Privacy Policy
                </Link>
              </li>
              <li>
                <Link href="/terms" className="hover:text-primary-400 transition">
                  Terms of Service
                </Link>
              </li>
              <li>
                <Link href="/cookies" className="hover:text-primary-400 transition">
                  Cookie Policy
                </Link>
              </li>
            </ul>
          </div>
        </div>

        <div className="border-t border-gray-800 mt-8 pt-8 text-center text-sm">
          <p>&copy; {new Date().getFullYear()} E-Commerce. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
}
