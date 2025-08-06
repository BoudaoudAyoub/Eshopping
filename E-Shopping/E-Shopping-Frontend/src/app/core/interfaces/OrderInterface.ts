export interface OrderItemModel {
  productId: string;
  quantity: number;
  price: number;
  productName: string;
}

export interface OrderModel {
  id: string;
  client: string;
  createdDate: string;
  status: string;
  isSipped: boolean;
  totalAmount: number;
  orderItems: OrderItemModel[];
}

export interface OrderItemReq {
  ProductId: string;
  Quantity: number;
}