import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderItemReq, OrderModel } from '../interfaces/OrderInterface';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class OrderService {
  constructor(private http: HttpClient) {}

  private baseUrl = `${environment}'/api/orders'`;

  CreateOrder(orderItems: OrderItemReq[]): Observable<any> {
    return this.http.post(this.baseUrl, orderItems);
  }

  getAllOrders(): Observable<OrderModel[]> {
    return this.http.get<OrderModel[]>(this.baseUrl);
  }

  cancelOrder(orderId: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/${orderId}/cancel`,null);
  }

  deleteOrder(orderId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${orderId}`);
  }
}