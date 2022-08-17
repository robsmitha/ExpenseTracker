import * as React from 'react';
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import IconButton from '@mui/material/IconButton';
import RestoreIcon from '@mui/icons-material/Restore';

interface Props {
    items: Array<any>;
    restoreTransaction: (transaction_id: string) => void;
}


const ExcludedTransactionList: React.FunctionComponent<Props> = ({ items, restoreTransaction }) => {
    const [pageSize, setPageSize] = React.useState<number>(5);
    
    const renderExcludeButton = (params: any) => {
        return (
            <IconButton aria-label="restore"
                onClick={() => {
                    restoreTransaction(params.row.transaction_id)
                }}>
                <RestoreIcon />
            </IconButton>
        )
      }
    
  const columns: GridColDef[] = [
    {
      field: 'date',
      headerName: 'Date',
      description: 'This date the transaction occurred on.',
      valueGetter: (params: GridValueGetterParams) =>
          params.row.authorized_date || params.row.date,
    },
    {
      field: 'name',
      headerName: 'Transaction',
      flex: 1,
    },
    {
      field: 'amount',
      headerName: 'Amount',
      valueGetter: (params: GridValueGetterParams) =>
          `$${params.row.amount.toFixed(2)}`
    },
    {
      field: 'account',
      headerName: 'Account',
      flex: 1,
      valueGetter: (params: GridValueGetterParams) =>
          params.row.account.name,
    },
    {
        field: 'transaction_id',
        headerName: '',
        renderCell: renderExcludeButton
    }
  ];


    return (
        <div style={{ height: '80vh', width: '100%' }}>
            <DataGrid
                getRowId={(row) => row.transaction_id}
                rows={items}
                columns={columns}
                pageSize={pageSize}
                onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
                rowsPerPageOptions={[5, 10, 20]}
                pagination
            />
        </div>
    )
  }
  
export default ExcludedTransactionList;

