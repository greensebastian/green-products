import { createColumnHelper, flexRender, getCoreRowModel, useReactTable } from "@tanstack/react-table";
import { useProducts } from "../products/product";
import { BarLoader } from "react-spinners";

const productColumnHelper = createColumnHelper<ProductRow>()

const productColumns = [
  productColumnHelper.accessor(row => row.id, {
    id: "id",
    cell: info => info.getValue(),
    header: () => "Id",
  }),
  productColumnHelper.accessor(row => row.name, {
    id: "name",
    cell: info => info.getValue(),
    header: () => "Name",
  }),
  productColumnHelper.accessor(row => row.productType, {
    id: "productType",
    cell: info => info.getValue(),
    header: () => "Product Type",
  }),
  productColumnHelper.accessor(row => row.availableColours, {
    id: "availableColours",
    cell: info => info.getValue(),
    header: () => "Available Colours",
  }),
  productColumnHelper.accessor(row => row.createdOn, {
    id: "createdOn",
    cell: info => info.getValue(),
    header: () => "Created On",
  })
]

type ProductRow = {
  id: string;
  name: string;
  productType: string;
  availableColours: string;
  createdOn: string;
}

function ProductsTables() {
  const productsQuery = useProducts();
  const productsTable = useReactTable({
    data: productsQuery.data?.items.map(p => {
      return {
        id: p.id,
        name: p.name,
        productType: p.productType.displayName,
        availableColours: p.availableColours.map(pc => pc.displayName).join(', '),
        createdOn: p.createdOn
      }
     }) || [],
    columns: productColumns,
    getCoreRowModel: getCoreRowModel(),
  })

  return (
    <>
      <div className="mt-2">
      <BarLoader loading={productsQuery.isPending || productsQuery.isRefetching} width="100%" speedMultiplier={0.5} color="oklch(0.623 0.214 259.815)" className="my-2" />
      <table className="table-auto w-full">
        <thead>
          {productsTable.getHeaderGroups().map(headerGroup => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map(header => (
                <th key={header.id}>
                  {header.isPlaceholder
                    ? null
                    : flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          {productsTable.getRowModel().rows.map(row => (
            <tr key={row.id}>
              {row.getVisibleCells().map(cell => (
                <td key={cell.id}>
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    </>
  );
}

export default ProductsTables;
