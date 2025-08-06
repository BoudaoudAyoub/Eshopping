import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { OrderService } from '../../core/services/OrderService';
import { OrderModel } from '../../core/interfaces/OrderInterface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-orders',
  templateUrl: './order-component.html',
  standalone: true,
  imports: [CommonModule]
})
export class OrdersComponent implements OnInit {
  orders: OrderModel[] = [];
  selectedOrder: OrderModel | null = null;
  showModal: boolean = false;

  constructor(private orderService: OrderService, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadOrders()
  }

  loadOrders(): void {
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.orders = orders;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Failed to fetch orders', err);
      }
    });
  }

  cancelOrder(orderId: string): void {

    if(!this.orders.find(x => x.id === orderId)) {
      alert("This order could not be found");
    }

    this.orderService.cancelOrder(orderId).subscribe({
      next: () => {
        this.loadOrders();
        alert("Order has been cancel");
      },
      error: (err) => {
        alert("Failed to cancel order");
      }
    });
  }

  deleteOrder(orderId: string): void {
    if(!this.orders.find(x => x.id === orderId)) {
      alert("This order could not be found");
    }

    this.orderService.deleteOrder(orderId).subscribe({
      next: () => {
        this.loadOrders();
        alert("Order has been deleted");
      },
      error: (err) => {
        alert("Failed to cancel delete");
      }
    });
  }
}