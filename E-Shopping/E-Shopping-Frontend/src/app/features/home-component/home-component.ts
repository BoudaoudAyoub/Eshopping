import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Product } from '../../core/interfaces/ProductInterface';
import { ProductService } from '../../core/services/ProductService';
import { CartService } from '../../core/services/CartService';
import { CommonModule } from '@angular/common';
import { CartComponent } from '../cart-component/cart-component';

@Component({
  selector: 'app-home-component',
  standalone: true,
  imports: [CommonModule, CartComponent],
  templateUrl: './home-component.html'
})
export class HomeComponent implements OnInit {
  products: Product[] = [];
  productQuantities: Record<string, number> = {};

  constructor(private productService: ProductService, private cartService: CartService, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe((productsRes) => {
      this.cartService.cart$.subscribe((cartItems) => {
        this.products = productsRes;

        this.products.forEach(product => {
          const cartItem = cartItems.find(item => item.product.id === product.id);

          if (cartItem) {
            this.productQuantities[product.id] = cartItem.quantity;
            product.stockQuantity--;
          } else {
            this.productQuantities[product.id] = 1;
          }
        });

        this.cd.detectChanges();
      });
    });
  }

  increaseQty(productId: string): void {
    const product = this.products.find(p => p.id === productId);
    if (!product) return;

    if (product.stockQuantity > 0) {
      this.productQuantities[productId]++;
      
      if (this.cartService.isInCart(productId)) {
        const quantity = this.productQuantities[productId];
        this.cartService.updateQuantity(product.id, quantity);
      }
    } else {
      alert('Not enough stock!');
    }
  }

  decreaseQty(productId: string): void {
    const product = this.products.find(p => p.id === productId);
    if (!product) return;

    const currentQty = this.productQuantities[productId];

    if (currentQty <= 1) return;

    this.productQuantities[productId]--;

    if (this.cartService.isInCart(productId)) {
      this.cartService.updateQuantity(product.id, this.productQuantities[productId]);
    }
  }

  isInCart(productId: string): Boolean{
    return this.cartService.isInCart(productId)
  }

  getProductQuantity(productId: string): number {
    return this.productQuantities[productId] || 1;
  }

  addToCart(product: Product): void {
    if (product.stockQuantity >= 1) {
      this.cartService.addToCart(product, 1);
      product.stockQuantity - 1;
    } else {
      alert(`Only ${product.stockQuantity} item(s) left in stock`);
    }
  }

  isProductInCart(productId: string): boolean {
    return this.cartService.isInCart(productId);
  }
}
