import { useQuery } from "@tanstack/react-query";

export function useProducts() {
  return useQuery({
    queryKey: ['products'],
    queryFn: async () => {
      const response = await fetch("http://localhost:5068/products?page=1&pageSize=1000");
      return await response.json() as PaginatedResponse<Product>;
    },
  })
}

export function useProductAttributes() {
  return useQuery({
    queryKey: ['productAttributes'],
    queryFn: async () => {
      const response = await fetch("http://localhost:5068/products/attributes?page=1&pageSize=1000");
      return await response.json() as PaginatedResponse<ProductAttribute>;
    },
  })
}

export type PaginatedResponse<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export type Product = {
  id: string;
  name: string;
  productType: ProductAttribute;
  availableColours: ProductAttribute[];
  createdOn: string;
}

export type ProductAttribute = {
  id: string;
  type: string;
  value: string;
  displayName: string;
  createdOn: string;
}