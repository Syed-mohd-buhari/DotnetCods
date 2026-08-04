import * as React from "react";
import {
  Table,
  TableBody,
  TableContainer,
  TableHead,
  Paper,
  TableRow,
  TableCell,
  TableCellProps,
  TableRowProps,
  Collapse,
  Box,
  IconButton,
  Typography,
  Tooltip,
  Button,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { tableCellClasses } from "@mui/material/TableCell";
import {
  MdDelete,
  MdEdit,
  MdOutlineExpandLess,
  MdOutlineExpandMore,
} from "react-icons/md";

// Styled components
export const StyledTableCell = styled(TableCell)<TableCellProps>(
  ({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
      backgroundColor: "#6c757d99 !important",
      color: "black",
      fontWeight: 600,
    },
    [`&.${tableCellClasses.body}`]: {
      fontSize: 14,
    },
  })
);

export const StyledTableRow = styled(TableRow)<TableRowProps>(({ theme }) => ({
  "&:nth-of-type(odd)": {
    backgroundColor: theme.palette.action.hover,
  },
  "&:last-child td, &:last-child th": {
    border: 0,
  },
}));

// Column definition type
export type Column<T> = {
  key: keyof T;
  label: string;
  align?: "left" | "right" | "center";
};

// Component props
type CustomCollapsibleTableProps<T> = {
  columns: Column<T>[];
  data: T[];
  size: "small" | "medium";
  page?: string;
  renderExpand?: (row: T, index?: any) => React.ReactNode;
  onEdit?: (row: T, index?: number) => void;
  onDelete?: (row: T, index?: number) => void;
  setCapacityFlag?: (val: boolean) => void;
};

function CustomCollapsibleTable<T extends { [key: string]: any }>({
  columns,
  data,
  size = "medium",
  page,
  renderExpand,
  onEdit,
  onDelete,
  setCapacityFlag,
}: CustomCollapsibleTableProps<T>) {
  const [openRowIndex, setOpenRowIndex] = React.useState<number | null>(null);

  const toggleRow = (index: number) => {
    setOpenRowIndex(openRowIndex === index ? null : index);
  };
  return (
    <TableContainer component={Paper}>
      <Table aria-label="sticky table" size={size}>
        <TableHead>
          <TableRow>
            {renderExpand && <StyledTableCell />}
            {(onEdit || onDelete) && (
              <StyledTableCell key={"Action"} align={"center"}>
                {"Actions"}
              </StyledTableCell>
            )}
            {columns.map((col) => (
              <StyledTableCell
                key={String(col.key)}
                align={col.align || "left"}
              >
                {col.label}
              </StyledTableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((row, rowIndex) => {
            const updatedRow: any =
              page === "cbom" ? row?.cnfcapacity : row?.vnfvmcapacity;
            const deleteId =
              (row?.cnfcapacity?.cnfclusterinfoid &&
                row?.cnfcapacity?.cnfclusterinfoid) !== 0 ||
              (row?.vnfvmcapacity?.vnfvminstanceid &&
                row?.vnfvmcapacity?.vnfvminstanceid !== 0)
                ? false
                : true;
            return (
              <React.Fragment key={rowIndex}>
                <StyledTableRow>
                  {renderExpand && (
                    <StyledTableCell>
                      <Button
                        size="small"
                        color="error"
                        onClick={() => {
                          toggleRow(rowIndex);
                          setCapacityFlag &&
                            setCapacityFlag(
                              updatedRow?.length > 0 ? false : true
                            );
                        }}
                      >
                        {updatedRow?.length > 0 ? "View" : "Add"} Capacity
                      </Button>
                    </StyledTableCell>
                  )}
                  {(onEdit || onDelete) && (
                    <StyledTableCell align="center">
                      {onEdit && (
                        <IconButton
                          size="small"
                          sx={{ mr: "1rem" }}
                          onClick={() => onEdit(row, rowIndex)}
                        >
                          <MdEdit />
                        </IconButton>
                      )}
                      {onDelete && deleteId && (
                        <IconButton
                          size="small"
                          onClick={() => onDelete(row, rowIndex)}
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
                {renderExpand && (
                  <TableRow>
                    <TableCell
                      colSpan={columns.length + 1}
                      sx={{ paddingBottom: 0, paddingTop: 0 }}
                    >
                      <Collapse
                        in={openRowIndex === rowIndex}
                        sx={{
                          width: "min-content",
                        }}
                        timeout="auto"
                        unmountOnExit
                      >
                        <Box sx={{ margin: 1 }}>
                          {renderExpand(row, rowIndex)}
                        </Box>
                      </Collapse>
                    </TableCell>
                  </TableRow>
                )}
              </React.Fragment>
            );
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

export default CustomCollapsibleTable;
