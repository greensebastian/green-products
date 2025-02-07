import ProductsTable from "../components/ProductsTable";

function Products() {
  return (
    <div className="bg-white shadow-lg rounded-2xl p-6">
      <h2 className="text-xl font-semibold mb-2">Products</h2>
      <ProductsTable />
    </div>
  );
}

export default Products;
