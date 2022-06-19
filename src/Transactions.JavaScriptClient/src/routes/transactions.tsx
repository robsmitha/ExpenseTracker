import { FunctionComponent, useState, useEffect } from 'react';
import {
    Skeleton,
    Button,
    Grid,
    Typography
} from '@mui/material';
import { DataGrid, GridColDef, GridValueGetterParams, GridSelectionModel } from '@mui/x-data-grid';

import { categoryService } from './../services/category.service'
import { budgetService } from '../services/budget.service'
import { useParams } from 'react-router-dom';
import TransactionList from './../components/TransactionList'
import CategoryAutoComplete from '../components/CategoriesAutocomplete'

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
    const { budgetId } = useParams();
    const [transactions, setTransactions] = useState<Array<any>>([]);
    const [categories, setCategories] = useState<Array<any>>([]);

    const [errorText, setErrorText] = useState<string | null>(null);
    const [category, setCategory] = useState<any | null>(null);
    const [selectionModel, setSelectionModel] = useState<GridSelectionModel>([]);

    useEffect(() => {
      getCategories();
      getTransactions();
    }, []);
    
    async function getCategories() {
      const response = await categoryService.getCategories();
        setCategories(response.map((c: any) => {
          return {
            ...c,
            label: c.name
          };
        }))
    }

    async function getTransactions() {
      const response = await budgetService.getBudget(Number(budgetId));
      setTransactions(response.transactions)
    }

    async function setTransactionsCategory(category: any){
      const data = selectionModel?.map(transactionId => {
        return {
          transactionId: transactionId,
          categoryId: category.id,
          budgetId: budgetId
        }
      });
      const response = await budgetService.bulkUpdateTransactionCategory(data);
      if(response){
        // fetch updated transactions
        await getTransactions()

        // reset selections
        setSelectionModel([])

        // reset category
        setCategory(null)
      }
    }

    async function onSetCategory(value: any) {
      setErrorText(null)
      if(!value.id){
        const response = await categoryService.saveCategory(value);
        if(response.errors) {
          const msg = Object.keys(response.errors)
                        .map((e: any) => response.errors[e].toString())
                        .join(", ")
          setErrorText(msg)
          return;
        } 
        value.id = response.id;
        await getCategories();
      }
      setCategory(value)
      setTransactionsCategory(value);
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs>
                <Typography variant='h3'>
                  Transactions
                </Typography>
                {!transactions 
                ? <Skeleton /> 
                : <Grid container spacing={2}>
                  <Grid item xs={12}>
                    <CategoryAutoComplete 
                      label="Select Category" 
                      value={category} 
                      setValue={onSetCategory} 
                      cateogories={categories} 
                      disabled={selectionModel.length === 0}
                      errorText={errorText}
                    />
                  </Grid>
                    <Grid item xs={12}>
                      <TransactionList 
                        items={transactions} 
                        columns={columns} 
                        selectionModel={selectionModel} 
                        setSelectionModel={setSelectionModel} 
                      />
                    </Grid>
                  </Grid>}
            </Grid>
        </Grid>
    )
  };

  export default Transactions;