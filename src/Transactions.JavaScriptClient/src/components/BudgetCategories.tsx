import * as React from 'react';
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import Chip from '@mui/material/Chip';

interface Props {
    items: Array<any>;
    total: number;
    caption: string;
    onCategorySelected: (category: string, estimate: number) => void;
}

const BudgetCategories: React.FunctionComponent<Props> = ({ items, total, caption, onCategorySelected }) => {
  const [pageSize, setPageSize] = React.useState<number>(10);
  
  const columns: GridColDef[] = [
    {
      field: 'category',
      headerName: 'Category',
      flex: 1
    },
    {
      field: 'sum',
      headerName: 'Actual',
      valueGetter: (params: GridValueGetterParams) =>
          `$${params.row.sum.toFixed(2)}`
    },
    {
      field: 'estimate',
      headerName: 'Estimate',
      valueGetter: (params: GridValueGetterParams) =>
          `$${params.row.estimate.toFixed(2)}`
    },
    {
      field: '',
      headerName: 'Leftover',
      valueGetter: (params: GridValueGetterParams) =>
          params.row.estimate - params.row.sum
    }
  ];

  return (
    <div style={{ height: '80vh', width: '100%' }}>
      {/* <Chip label={caption} variant="outlined" />
      <Chip label={total} /> */}
        <DataGrid
            getRowId={(row) => row.category}
            rows={items}
            columns={columns}
            pageSize={pageSize}
            onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
            rowsPerPageOptions={[10, 20, 30]}
            pagination
            onRowClick={(params) => params.row.category === "Uncategorized" ? {} : onCategorySelected(params.row.category, params.row.estimate)}
        />
        
    </div>
  );
}

export default BudgetCategories;
