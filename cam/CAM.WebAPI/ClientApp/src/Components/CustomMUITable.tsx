import * as React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  TableCellProps,
  TableRowProps,
  IconButton,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { tableCellClasses } from "@mui/material/TableCell";
import { MdDelete, MdEdit } from "react-icons/md";

const StyledTableCell = styled(TableCell)<TableCellProps>(({ theme }) => ({
  [`&.${tableCellClasses.head}`]: {
    backgroundColor: "#6c757d99 !important",
    color: "black",
    fontWeight: 600,
  },
  [`&.${tableCellClasses.body}`]: {
    fontSize: 14,
  },
}));

const StyledTableRow = styled(TableRow)<TableRowProps>(({ theme }) => ({
  border: "1px solid #999",
  // "&:last-child td, &:last-child th": {
  //   border: 0,
  // },
}));

type Column<T> = {
  key: keyof T;
  label: string;
  align?: "left" | "right" | "center";
};

type CustomTableProps<T> = {
  columns: Column<T>[];
  data: T[];
  size: "small" | "medium";
  page?: string;
  onEdit?: (row: T, index?: number) => void;
  onDelete?: (row: T, index?: number) => void;
  onRowClick?: (row: T, index?: number) => void;
  showDelete?: boolean;
  activeIndex?: any;
  tableName?: string;
};

function CustomTable<T extends { [key: string]: any }>({
  columns,
  data,
  size = "medium",
  page,
  onEdit,
  onDelete,
  showDelete,
  onRowClick,
  activeIndex,
  tableName,
}: CustomTableProps<T>) {
  return (
    <TableContainer component={Paper} sx={{ maxWidth: "65rem" }}>
      <Table stickyHeader aria-label="sticky table" size={size}>
        <TableHead>
          <TableRow>
            {(onEdit || onDelete) && (
              <StyledTableCell
                key={"Action"}
                align={"center"}
                sx={{ minWidth: 110, padding: "12px" }}
              >
                {"Actions"}
              </StyledTableCell>
            )}
            {columns.map((col) => (
              <StyledTableCell
                key={String(col.key)}
                align={col.align || "left"}
                sx={{ minWidth: 200, padding: "12px" }}
              >
                {col.label}
              </StyledTableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {data?.map((row, idx) => {
            const deleteId =
              (row?.vnfinfoid && row?.vnfinfoid !== 0) ||
              (row?.vnfvmcapacityid && row?.vnfvmcapacityid !== 0)
                ? false
                : true;
            // if (tableName === "VnfInstance") console.log("row", row);
            return (
              <StyledTableRow
                key={idx}
                onClick={() => onRowClick && onRowClick(row, idx)}
                className={`${activeIndex === idx ? "activeStateRow" : ""} 
                `}
                // className={`${activeIndex === idx ? "activeStateRow" : ""}
                // ${
                //   tableName === "VnfInstance" &&
                //   row?.vnfvmcapacity?.length === 0
                //     ? "noRecordCSS"
                //     : ""
                // }
                // `}
              >
                {(onEdit || onDelete) && (
                  <StyledTableCell align="center">
                    {onEdit && (
                      <IconButton
                        size="small"
                        sx={{ mr: "1rem" }}
                        onClick={() => onEdit(row, idx)}
                      >
                        <MdEdit />
                      </IconButton>
                    )}
                    {onDelete && deleteId && (
                      <IconButton
                        size="small"
                        onClick={() => onDelete(row, idx)}
                      >
                        <MdDelete />
                      </IconButton>
                    )}
                  </StyledTableCell>
                )}
                {columns.map((col) => (
                  <StyledTableCell
                    key={String(col.key)}
                    align={col.align || "left"}
                  >
                    {row[col.key]}
                  </StyledTableCell>
                ))}
              </StyledTableRow>
            );
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

export default CustomTable;
