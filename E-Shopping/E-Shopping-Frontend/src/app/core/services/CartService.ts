import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CartItem } from '../interfaces/CardInterface';
import { Product } from '../interfaces/ProductInterface';

@Injectable({ providedIn: 'root' })
export class CartService {
  private cartItems: CartItem[] = [];
  private cartSubject = new BehaviorSubject<CartItem[]>([]);
  cart$ = this.cartSubject.asObservable();

  constructor() {}

  addToCart(product: Product, quantity: number): void {
    const index = this.cartItems.findIndex(item => item.product.id === product.id);
    if (index !== -1) {
      this.cartItems[index].quantity += quantity;
    } else {
      this.cartItems.push({ product, quantity });
    }
    this.cartSubject.next([...this.cartItems]);
  }

  removeFromCart(productId: string): void {
    this.cartItems = this.cartItems.filter(item => item.product.id !== productId);
    this.cartSubject.next([...this.cartItems]);
  }

  updateQuantity(productId: string, quantity: number): void {
    const index = this.cartItems.findIndex(item => item.product.id === productId);
    if (index !== -1) {
      this.cartItems[index].quantity = quantity;
      this.cartSubject.next([...this.cartItems]);
    }
  }

  decreaseQuantity(productId: string): void {
    const index = this.cartItems.findIndex(item => item.product.id === productId);
    if (index !== -1 && this.cartItems[index].quantity > 1) {
      this.cartItems[index].quantity--;
      this.cartSubject.next([...this.cartItems]);
    }
  }

  clearCart(): void {
    this.cartItems = [];
    this.cartSubject.next([]);
  }

  isInCart(productId: string): boolean {
    return this.cartItems.some(item => item.product.id === productId);
  }

  getItems(): CartItem[] {
    return [...this.cartItems];
  }

  get cartItemsSnapshot(): CartItem[] {
    return [...this.cartItems];
  }

  getTotalPrice(): number {
    return this.cartItems.reduce((total, item) => total + item.product.price * item.quantity, 0);
  }

  getTotalQuantity(): number {
    return this.cartItems.reduce((total, item) => total + item.quantity, 0);
  }
}