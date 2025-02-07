import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import ProductAttributesTable from "./ProductAttributesTable";
import ProductsTable from "./ProductsTable";
import CreateProductForm from "./CreateProductForm";

const queryClient = new QueryClient();

function App() {
  return (
    <>
      <QueryClientProvider client={queryClient}>
        <div className="flex justify-center items-center min-h-screen bg-gray-100 p-4">
          <div className="space-y-4 w-full max-w-3xl text-left">
            {/* Products card */}
            <div className="bg-white shadow-lg rounded-2xl p-6">
              <h2 className="text-xl font-semibold mb-2">Products</h2>
              <CreateProductForm/>
              <ProductsTable/>
            </div>
            
            {/* Product attributes card */}
            <div className="bg-white shadow-lg rounded-2xl p-6">
              <h2 className="text-xl font-semibold mb-2">Product Attributes</h2>
              <ProductAttributesTable/>
            </div>
          </div>
        </div>
      </QueryClientProvider>
    </>
  );
}

export default App;
