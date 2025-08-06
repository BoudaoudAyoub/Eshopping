import { Product } from "./ProductInterface";

export interface CartItem {
  product: Product;
  quantity: number;
}