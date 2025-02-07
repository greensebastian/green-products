import CreateProductForm from "../components/CreateProductForm";
import ProductAttributesTable from "../components/ProductAttributesTable";
import ProductsTable from "../components/ProductsTable";

function CreateProduct() {
  return (
    <>
      <div className="bg-white shadow-lg rounded-2xl p-6">
        <h2 className="text-xl font-semibold mb-2">Products</h2>
        <CreateProductForm />
        <ProductsTable />
      </div>

      <div className="bg-white shadow-lg rounded-2xl p-6">
        <h2 className="text-xl font-semibold mb-2">Product Attributes</h2>
        <ProductAttributesTable />
      </div>
    </>
  );
}

export default CreateProduct;
