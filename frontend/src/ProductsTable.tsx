import { createColumnHelper, flexRender, getCoreRowModel, useReactTable } from "@tanstack/react-table";
import { useProducts } from "./products/product";

const productAttributeColumnHelper = createColumnHelper<ProductRow>()

const productAttributeColumns = [
  productAttributeColumnHelper.accessor(row => row.name, {
    id: "name",
    cell: info => info.getValue(),
    header: () => "Name",
  }),
  productAttributeColumnHelper.accessor(row => row.productType, {
    id: "productType",
    cell: info => info.getValue(),
    header: () => "Product Type",
  }),
  productAttributeColumnHelper.accessor(row => row.availableColours, {
    id: "availableColours",
    cell: info => info.getValue(),
    header: () => "Available Colours",
  })
]

type ProductRow = {
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
        name: p.name,
        productType: p.productType.displayName,
        availableColours: p.availableColours.map(pc => pc.displayName).join(', '),
        createdOn: p.createdOn
      }
     }) || [],
    columns: productAttributeColumns,
    getCoreRowModel: getCoreRowModel(),
  })

  return (
    <>
      <div>
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
