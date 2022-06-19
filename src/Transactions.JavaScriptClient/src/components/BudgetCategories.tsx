import * as React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

interface Props {
    items: Array<any>;
    total: number;
}

const BudgetCategories: React.FunctionComponent<Props> = ({ items, total }) => {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 350 }} aria-label="simple table">
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
              <TableCell align="right">0</TableCell>
              <TableCell align="right">0</TableCell>
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
