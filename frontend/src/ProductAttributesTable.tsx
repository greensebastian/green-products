import { createColumnHelper, flexRender, getCoreRowModel, useReactTable } from "@tanstack/react-table";
import { ProductAttribute, useProductAttributes } from "./products/product";

const productAttributeColumnHelper = createColumnHelper<ProductAttribute>()

const productAttributeColumns = [
  productAttributeColumnHelper.accessor(row => row.type, {
    id: "type",
    cell: info => info.getValue(),
    header: () => "Type",
  }),
  productAttributeColumnHelper.accessor(row => row.displayName, {
    id: "displayName",
    cell: info => info.getValue(),
    header: () => "Display Name",
  }),
  productAttributeColumnHelper.accessor(row => row.id, {
    id: "id",
    cell: info => info.getValue(),
    header: () => "Id",
  })
]

function ProductAttributesTable() {
  const productAttributesQuery = useProductAttributes();
  const productAttributesTable = useReactTable({
    data: productAttributesQuery.data?.items || [],
    columns: productAttributeColumns,
    getCoreRowModel: getCoreRowModel(),
  })

  return (
    <>
      <div>
      <table className="table-auto w-full">
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

export default ProductAttributesTable;
