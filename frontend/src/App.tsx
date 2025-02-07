import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Route, Routes } from "react-router";
import Products from "./routes/Products";
import CreateProduct from "./routes/CreateProduct";
import NavigationBar from "./components/NavigationBar";

const queryClient = new QueryClient();

function App() {
  return (
    <>
      <QueryClientProvider client={queryClient}>
      <NavigationBar />
        <div className="flex justify-center min-h-screen bg-gray-100 p-4">
          <div className="space-y-4 w-full max-w-3xl text-left">
            <Routes>
              <Route path="/" element={<Products />} />
              <Route path="/create-product" element={<CreateProduct />} />
            </Routes>
          </div>
        </div>
      </QueryClientProvider>
    </>
  );
}

export default App;
