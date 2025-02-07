import { createColumnHelper, flexRender, getCoreRowModel, useReactTable } from "@tanstack/react-table";
import "./App.css";
import { ProductAttribute, useProductAttributes } from "./products/product";

const productAttributeColumnHelper = createColumnHelper<ProductAttribute>()

const productAttributeColumns = [
  productAttributeColumnHelper.accessor(row => row.type, {
    id: "type",
    cell: info => info.getValue(),
    header: () => "Type"
  }),
  productAttributeColumnHelper.accessor(row => row.displayName, {
    id: "displayName",
    cell: info => info.getValue(),
    header: () => "DisplayName",
  }),
]

type ProductRow = {
  name: string;
  productType: string;
  availableColours: string;
  createdOn: string;
}

function ProductsTables() {
  const productAttributesQuery = useProductAttributes();
  const productAttributesTable = useReactTable({
    data: productAttributesQuery.data?.items || [],
    columns: productAttributeColumns,
    getCoreRowModel: getCoreRowModel()
  })

  return (
    <>
      <div className="p-2">
      <table>
        <thead>
          {productAttributesTable.getHeaderGroups().map(headerGroup => (
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
          {productAttributesTable.getRowModel().rows.map(row => (
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
