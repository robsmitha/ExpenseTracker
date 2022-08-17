import * as React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Button from '@mui/material/Button';

interface Props {
    items: Array<any>;
    total: number;
    caption: string;
    onCategorySelected: (category: string, estimate: number) => void;
}
const BudgetCategories: React.FunctionComponent<Props> = ({ items, total, caption, onCategorySelected }) => {
  return (
    <TableContainer component={Paper} sx={{overflow: 'hidden' }}>
      <Table size="small" aria-label="budget category table">
        <caption>{caption}</caption>
        <TableHead>
          <TableRow>
            <TableCell>Category</TableCell>
            <TableCell align="right">Actual</TableCell>
            <TableCell align="right">Estimate</TableCell>
            <TableCell align="right">Leftover</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {items.map((row: any) => (
            <TableRow
              key={row.category}
              sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
            >
              <TableCell component="th" scope="row">
                {row.category}
              </TableCell>
              <TableCell align="right">{row.sum}</TableCell>
              <TableCell align="right">
                <Button disabled={row.category === "Uncategorized"} onClick={() => onCategorySelected(row.category, row.estimate)} size="small">
                  {row.estimate}
                </Button>
              </TableCell>
              <TableCell align="right">{row.estimate - row.sum}</TableCell>
            </TableRow>
          ))}
          <TableRow>
            <TableCell component="th" scope="row">
              Total
            </TableCell>
            <TableCell align="right">{total}</TableCell>
            <TableCell align="right">0</TableCell>
            <TableCell align="right">0</TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </TableContainer>
  );
}

export default BudgetCategories;
