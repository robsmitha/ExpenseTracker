import { FunctionComponent, useContext, useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import {
    Typography,
    Grid,
    Alert,
    Button,
    AlertTitle,
    Skeleton,
    Toolbar
} from '@mui/material';
import { budgetService } from '../services/budget.service'
import BudgetCategories from '../components/BudgetCategories';
import AddBudgetCategoryEstimateDialog from '../components/AddBudgetCategoryEstimateDialog';

import { DataGrid, GridColDef, GridValueGetterParams, GridSelectionModel } from '@mui/x-data-grid';

import { categoryService } from './../services/category.service'
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

export const Dashboard: FunctionComponent = () => {
    const navigate = useNavigate();
    const { budgetId } = useParams();
    const [budget, setBudget] = useState<any>();
    const [open, setOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState<string>('');
    const [selectedCategoryEstimate, setSelectedCategoryEstimate] = useState<number>(0);

    const [transactions, setTransactions] = useState<Array<any>>([]);
    const [categories, setCategories] = useState<Array<any>>([]);

    const [errorText, setErrorText] = useState<string | null>(null);
    const [category, setCategory] = useState<any | null>(null);
    const [selectionModel, setSelectionModel] = useState<GridSelectionModel>([]);
    
    useEffect(() => {
        getBudget();
        getCategories();
        getTransactions();
    }, []);

    async function getBudget() {
        const response = await budgetService.getBudget(Number(budgetId));
        setBudget(response)
    }
    
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
        const response = await budgetService.getTransactions(Number(budgetId));
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
            await getBudget()

            // reset selections
            setSelectionModel([])

            // reset category
            setCategory(null)
        }
    }

    async function onSetCategory(value: any) {
        setErrorText(null)
        if(!value.id){
            const response = await categoryService.saveCategory(Object.assign({}, value, { budgetId: Number(budgetId) }));
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

    async function onCategorySaved(){
       await getBudget()
    }

    function onCategorySelected(category: string, estimate: number){
        setSelectedCategory(category)
        setSelectedCategoryEstimate(estimate)
        setOpen(true)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Toolbar>
                {budget 
                ? <Typography variant="h3" component="div" sx={{ flexGrow: 1 }}>
                    {budget.budgetName}
                </Typography>
                : <Skeleton /> }
                </Toolbar>
            </Grid>
            <Grid item xs={9}>
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
            <Grid item xs={3}>

                {budget && <BudgetCategories 
                items={budget.budgetCategoryData}
                total={budget.transactionsTotal} 
                onCategorySelected={onCategorySelected}
                caption={budget.dateRange} />}

                <AddBudgetCategoryEstimateDialog 
                    open={open} 
                    setOpen={setOpen} 
                    onCategorySaved={onCategorySaved}
                    category={selectedCategory} 
                    estimate={selectedCategoryEstimate}
                    setEstimate={setSelectedCategoryEstimate}
                    budgetId={Number(budgetId)} />
            </Grid>
        </Grid>
        
    )
  };

  export default Dashboard;