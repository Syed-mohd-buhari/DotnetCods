import * as React from "react";
import {
  Collapse,
  Box,
  Typography,
  IconButton,
  Table,
  TableHead,
  TableBody,
  TableCell,
  TableRow,
  TableCellProps,
  TableRowProps,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { tableCellClasses } from "@mui/material/TableCell";
import { MdOutlineExpandLess, MdOutlineExpandMore } from "react-icons/md";

// Styled MUI components
const StyledTableCell = styled(TableCell)<TableCellProps>(({ theme }) => ({
  [`&.${tableCellClasses.head}`]: {
    backgroundColor: "#d90000 !important",
    color: theme.palette.common.white,
    fontWeight: 600,
  },
  [`&.${tableCellClasses.body}`]: {
    fontSize: 14,
  },
}));

const StyledTableRow = styled(TableRow)<TableRowProps>(({ theme }) => ({
  "&:nth-of-type(odd)": {
    backgroundColor: theme.palette.action.hover,
  },
  "&:last-child td, &:last-child th": {
    border: 0,
  },
}));

// Types
export type History = {
  date: string;
  customerId: string;
  amount: number;
};

export type CollapsibleRowData = {
  name: string;
  calories: number;
  fat: number;
  carbs: number;
  protein: number;
  price: number;
  history: History[];
};

type CollapsibleTableRowProps = {
  row: CollapsibleRowData;
};

const CollapsibleTableRow: React.FC<CollapsibleTableRowProps> = ({ row }) => {
  const [open, setOpen] = React.useState(false);

  return (
    <>
      <StyledTableRow>
        <StyledTableCell>
          <IconButton size="small" onClick={() => setOpen(!open)}>
            {open ? <MdOutlineExpandMore /> : <MdOutlineExpandLess />}
          </IconButton>
        </StyledTableCell>
        <StyledTableCell component="th" scope="row">
          {row.name}
        </StyledTableCell>
        <StyledTableCell align="right">{row.calories}</StyledTableCell>
        <StyledTableCell align="right">{row.fat}</StyledTableCell>
        <StyledTableCell align="right">{row.carbs}</StyledTableCell>
        <StyledTableCell align="right">{row.protein}</StyledTableCell>
      </StyledTableRow>
      <TableRow>
        <TableCell colSpan={6} sx={{ py: 0 }}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Box sx={{ m: 1 }}>
              <Typography variant="h6" gutterBottom>
                History
              </Typography>
              <Table size="small" aria-label="purchase-history">
                <TableHead>
                  <TableRow>
                    <StyledTableCell>Date</StyledTableCell>
                    <StyledTableCell>Customer</StyledTableCell>
                    <StyledTableCell align="right">Amount</StyledTableCell>
                    <StyledTableCell align="right">
                      Total price ($)
                    </StyledTableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {row.history.map((historyRow) => (
                    <StyledTableRow key={historyRow.date}>
                      <StyledTableCell>{historyRow.date}</StyledTableCell>
                      <StyledTableCell>{historyRow.customerId}</StyledTableCell>
                      <StyledTableCell align="right">
                        {historyRow.amount}
                      </StyledTableCell>
                      <StyledTableCell align="right">
                        {Math.round(historyRow.amount * row.price * 100) / 100}
                      </StyledTableCell>
                    </StyledTableRow>
                  ))}
                </TableBody>
              </Table>
            </Box>
          </Collapse>
        </TableCell>
      </TableRow>
    </>
  );
};

export default CollapsibleTableRow;
