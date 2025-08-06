import { Component, OnInit } from '@angular/core';
import { CartService } from '../../core/services/CartService';
import { CartItem } from '../../core/interfaces/CardInterface';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  imports:[CommonModule, RouterLink]
})
export class Navbar implements OnInit {
  cartItems: CartItem[] = [];
  totalPrice = 0;
  totalQuantity = 0;
  isDropdownOpen = false;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
      this.totalPrice = this.cartService.getTotalPrice();
      this.totalQuantity = this.cartService.getTotalQuantity();
    });
  }

  toggleCartDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
}