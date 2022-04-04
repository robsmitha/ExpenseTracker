import { FunctionComponent, useState, useEffect } from 'react';
import {
    Skeleton,
    Button,
    Grid
} from '@mui/material';
import { DataGrid, GridColDef, GridValueGetterParams, GridSelectionModel } from '@mui/x-data-grid';

import { transactionService } from './../services/transaction.service'
import { categoryService } from '../services/category.service'
import { useParams } from 'react-router-dom';
import TransactionList from './../components/TransactionList'
import CategoriesAutoComplete from './../components/CategoriesAutoComplete'

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
  }
];

export const Transactions: FunctionComponent = () => {
    const { itemId } = useParams();
    const [transactions, setTransactions] = useState<Array<any>>([]);
    const [categories, setCategories] = useState<Array<any>>([]);
    const [category, setCategory] = useState<string>();

    const [selectionModel, setSelectionModel] = useState<GridSelectionModel>([]);

    useEffect(() => {
      getCategories();
      getTransactions();
    }, []);
    
    async function getCategories() {
      if(itemId){
        const response = await categoryService.getCategories();
        setCategories(response.map((c: any) => {
          return {
            ...c,
            label: c.name
          };
        }))
      }
    }

    async function getTransactions() {
      if(itemId){
        const response = await transactionService.getTransactions(itemId);
        setTransactions(response)
      }
    }

    async function setTransactionsCategory(){
      const data = selectionModel?.map(transactionId => {
        return {
          transactionId: transactionId,
          categoryId: category
        }
      });
      const response = await categoryService.bulkUpdateTransactionCategory(data);
      if(response){
        await getTransactions()
        setSelectionModel([])
        setCategory('')
      }
    }

    function handleChange(_: any, value: any){
      setCategory(value.id)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs>
                {!transactions 
                ? <Skeleton /> 
                : <Grid container spacing={2}>
                    <Grid item xs={10}>
                      <CategoriesAutoComplete handleChange={handleChange} cateogories={categories} />
                    </Grid>
                    <Grid item xs={2}>
                      <Button fullWidth variant="contained" size="large" onClick={setTransactionsCategory}>
                        Update
                      </Button>
                    </Grid>
                    <Grid item xs={12}>
                      <TransactionList items={transactions} columns={columns} selectionModel={selectionModel} setSelectionModel={setSelectionModel} />
                    </Grid>
                  </Grid>}
            </Grid>
        </Grid>
    )
  };

  export default Transactions;