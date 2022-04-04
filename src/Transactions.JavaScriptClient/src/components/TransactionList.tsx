import * as React from 'react';
import { DataGrid, GridColDef, GridRowParams, GridSelectionModel } from '@mui/x-data-grid';

interface Props {
    items: Array<any>;
    columns: GridColDef[];
    selectionModel: GridSelectionModel;
    setSelectionModel: (selected: string[]) => void;
}

const TransactionList: React.FunctionComponent<Props> = ({ items, columns, selectionModel, setSelectionModel }) => {
    return (
        <div style={{ height: 400, width: '100%' }}>
        <DataGrid
            getRowId={(row) => row.transaction_id}
            rows={items}
            columns={columns}
            pageSize={5}
            rowsPerPageOptions={[5]}
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

