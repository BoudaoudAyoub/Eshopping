import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartItem } from '../../core/interfaces/CardInterface';
import { CartService } from '../../core/services/CartService';
import { OrderService } from '../../core/services/OrderService';

@Component({
  selector: 'app-cart-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart-component.html'
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  subtotal = 0;
  tax = 0;
  shipping = 5.99;
  total = 0;

  constructor(private cartService: CartService, private orderService: OrderService) {}

  ngOnInit(): void {
    this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
      this.calculateTotals();
    });
  }

  calculateTotals(): void {
    this.subtotal = this.cartItems.reduce((acc, item) => acc + item.product.price * item.quantity, 0);
    this.tax = 3.40;
    this.total = this.subtotal + this.tax + this.shipping;
  }

  removeItem(productId: string): void {
    
    const item = this.cartItems.find(ci => ci.product.id === productId);
    if (!item) return;

    const product = this.cartItems.find(p => p.product.id === productId)?.product;
    if (product) {
      product.stockQuantity += item.quantity;
    }

    this.cartService.removeFromCart(productId);
  }

  proceedToCheckout(): void {
    const orderItems = this.cartItems.map(item => ({
        ProductId: item.product.id,
        Quantity: item.quantity
      }));

    this.orderService.CreateOrder(orderItems).subscribe({
      next: () => {
        alert('Order created successfully!');
        this.cartService.clearCart();
      },
      error: (err) => {
        console.error('Failed to create an order', err);
        alert('There was a problem creating your order');
      }
    });
  }
}