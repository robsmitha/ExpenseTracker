import * as React from 'react';
import { DataGrid, GridColDef, GridRowParams, GridSelectionModel, GridValueGetterParams } from '@mui/x-data-grid';

import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';

interface Props {
    items: Array<any>;
    selectionModel: GridSelectionModel;
    setSelectionModel: (selected: string[]) => void;
    excludeTransaction: (transaction_id: string) => void;
}


const TransactionList: React.FunctionComponent<Props> = ({ items, selectionModel, setSelectionModel, excludeTransaction }) => {

  
  const renderExcludeButton = (params: any) => {
    return (
        <IconButton aria-label="delete"
            onClick={() => {
              excludeTransaction(params.row.transaction_id)
            }}>
            <DeleteIcon />
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
      field: 'category',
      headerName: 'Category',
      flex: 1,
      valueGetter: (params: GridValueGetterParams) =>
          params.row.category?.name,
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
            pageSize={50}
            rowsPerPageOptions={[]}
            checkboxSelection
            disableSelectionOnClick
            isRowSelectable={(params: GridRowParams) => !params.row.pending}
            onSelectionModelChange={(transactionIds) => {
              const selectedIDs = new Set(transactionIds);
              const selectedRowData = items.filter((row) =>
                selectedIDs.has(row.transaction_id.toString())
              );
              setSelectionModel(selectedRowData.map(m => m.transaction_id));
            }}
            selectionModel={selectionModel}
          />
        </div>
    )
  }
  
export default TransactionList;

