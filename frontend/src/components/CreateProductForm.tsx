import toast, { Toaster, ToastOptions } from "react-hot-toast";
import { useProductMutation, useSeedMutation } from "../products/product";

const toasterOptions: ToastOptions = {
  duration: 5000,
  className: "!max-w-3xl my-1",
}

function CreateProductForm() {
  const productMutation = useProductMutation({
    onSuccess: async (product) => {
      toast.success(`Product ${product.name} added successfully! 🎉`, toasterOptions)
    },
    onFailure: async (request, problemDetails) => {
      toast.error(`Failed to add product ${request.name}.\n\nError [${problemDetails.status}],\n${problemDetails.title}\n${problemDetails.detail}`, {...toasterOptions, duration: 10000})
    },
    onError: async (request, message) => {
      toast.error(`Failed to add product ${request.name}.\n\nError: ${message}`, {...toasterOptions, duration: 10000})
    }
  });

  const handleProductSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const formData = new FormData(e.target as HTMLFormElement);
    const name = formData.get("name") as string;
    const productTypeId = formData.get("productTypeId") as string;
    const colourIds = (formData.get("colourIds") as string).split(",");
    await productMutation.mutateAsync({ name, productTypeId, colourIds });
  };

  const seedMutation = useSeedMutation({
    onSuccess: async () => {
      toast.success("Successfully seeded database!")
    },
    onError: async (message) => {
      toast.error(`Faile to seed database:\n\n${message}`);
    }
  });

  const handleSeedSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await seedMutation.mutateAsync();
  };

  return (
    <>
       <Toaster position="top-right" reverseOrder={false}/>
      
      <form onSubmit={handleProductSubmit}>
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
          className="bg-blue-500 text-white px-4 py-2 rounded w-full disabled:opacity-50 disabled:cursor-not-allowed"
          disabled={productMutation.isPending}
        >
          {productMutation.isPending ? "Creating..." : "Create product"}
        </button>
      </form>

      <form onSubmit={handleSeedSubmit}>
        <button
          type="submit"
          className="bg-green-600 text-white px-4 py-2 my-2 rounded w-full disabled:opacity-50 disabled:cursor-not-allowed"
          disabled={seedMutation.isPending}
        >
          {seedMutation.isPending ? "Seeding..." : "Seed database"}
        </button>
      </form>
    </>
  );
}

export default CreateProductForm;
