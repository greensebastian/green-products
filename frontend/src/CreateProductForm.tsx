import toast, { Toaster } from "react-hot-toast";
import { useProductMutation } from "./products/product";


function CreateProductForm() {
  const mutation = useProductMutation({
    onSuccess: async (product) => {
      toast.success(`Product ${product.name} added successfully! 🎉`)
    },
    onFailure: async (request, problemDetails) => {
      toast.error(`Failed to add product ${request.name}. Error [${problemDetails.status}]: ${problemDetails.title} ${problemDetails.detail}`)
    },
    onError: async (request, message) => {
      toast.error(`Failed to add product ${request.name}. Error: ${message}`)
    }
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const formData = new FormData(e.target as HTMLFormElement);
    const name = formData.get("name") as string;
    const productTypeId = formData.get("productTypeId") as string;
    const colourIds = (formData.get("colourIds") as string).split(",");
    await mutation.mutateAsync({ name, productTypeId, colourIds });
  };
  return (
    <>
       <Toaster position="top-right" reverseOrder={false} />
      
      <form onSubmit={handleSubmit}>
        <input
          name="name"
          placeholder="Enter name"
          className="border p-2 rounded w-full mb-2"
        />
        <input
          name="productTypeId"
          placeholder="Enter product type id"
          className="border p-2 rounded w-full mb-2"
        />
        <input
          name="colourIds"
          placeholder="Enter comma separated colour ids"
          className="border p-2 rounded w-full mb-2"
        />
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded w-full"
          disabled={mutation.isPending}
        >
          {mutation.isPending ? "Creating..." : "Create product"}
        </button>
      </form>
    </>
  );
}

export default CreateProductForm;
