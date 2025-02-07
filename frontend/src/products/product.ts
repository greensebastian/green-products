import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export function useProducts() {
  return useQuery({
    queryKey: ['products'],
    queryFn: async () => {
      const response = await fetch("http://localhost:5068/products?page=1&pageSize=1000");
      return await response.json() as PaginatedResponse<Product>;
    },
  })
}

export type CreateProductRequest = {
  name: string,
  productTypeId: string,
  colourIds: string[]
}

export type ProblemDetails = {
  type?: string; // A URI reference that identifies the problem type
  title?: string; // A short, human-readable title
  status?: number; // The HTTP status code
  detail?: string; // A detailed explanation of the problem
  instance?: string; // A URI reference to the specific problem occurrence
  [key: string]: unknown; // Allows additional properties for extended information
}

export function useProductMutation(options: {
  onSuccess?: (product: Product) => Promise<void>,
  onFailure?: (request: CreateProductRequest, problemDetails: ProblemDetails) => Promise<void>
  onError?: (request: CreateProductRequest, message: string) => Promise<void>
}) {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: async (request: CreateProductRequest) => {
      const response = await fetch("http://localhost:5068/products", {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(request)
      })
      return { json: await response.json(), ok: response.ok };
    },
    onSuccess: async ({json, ok}, request) => {
      if (ok) {
        queryClient.invalidateQueries({queryKey: ["products"]});
        if (options.onSuccess) await options.onSuccess(json as Product);
      }
      else {
        if (options.onFailure) await options.onFailure(request, json as ProblemDetails)
      }
    },
    onError: async ({ message }, request) => {
      if (options.onError) await options.onError(request, message);
    },
  })

  return mutation
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

export function useSeedMutation(options: {
  onSuccess?: () => Promise<void>,
  onError?: (message: string) => Promise<void>
}) {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: async () => {
      const response = await fetch("http://localhost:5068/seed", {
        method: "PUT"
      })
      if (!response.ok) throw new Error("Database seeding failed");
    },
    onSuccess: async () => {
      queryClient.invalidateQueries({queryKey: ["productAttributes"]});
      if (options.onSuccess) await options.onSuccess();
    },
    onError: async ({ message }) => {
      if (options.onError) await options.onError(message);
    },
  })

  return mutation
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