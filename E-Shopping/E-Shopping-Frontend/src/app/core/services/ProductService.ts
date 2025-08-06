import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../interfaces/ProductInterface';
import { environment } from '../../../environments/environment';
 
@Injectable({ providedIn: 'root' })
export class ProductService {
 
  constructor(private http: HttpClient) {}
 
  getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${environment.apiUrl}/products`);
  }
}