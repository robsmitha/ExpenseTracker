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
    IconButton
} from '@mui/material';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Box from '@mui/material/Box';

import { budgetService } from '../services/budget.service'
import BudgetCategories from '../components/BudgetCategories';
import AddBudgetCategoryEstimateDialog from '../components/AddBudgetCategoryEstimateDialog';

import { GridSelectionModel } from '@mui/x-data-grid';

import { categoryService } from './../services/category.service'
import TransactionList from './../components/TransactionList'
import ExcludedTransactionList from './../components/ExcludedTransactionList'
import CategoryAutoComplete from '../components/CategoriesAutocomplete'
import AccountList from './../components/AccountList'


interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
  }
  
  function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other } = props;
  
    return (
      <div
        role="tabpanel"
        hidden={value !== index}
        id={`simple-tabpanel-${index}`}
        aria-labelledby={`simple-tab-${index}`}
        {...other}
      >
        {value === index && (
          <Box>
            <Typography component={'span'}>{children}</Typography>
          </Box>
        )}
      </div>
    );
  }
  
  function a11yProps(index: number) {
    return {
      id: `simple-tab-${index}`,
      'aria-controls': `simple-tabpanel-${index}`,
    };
  }

export const Budget: FunctionComponent = () => {
    const navigate = useNavigate();
    const { budgetId } = useParams();
    const [budget, setBudget] = useState<any>();
    const [open, setOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState<string>('');
    const [selectedCategoryEstimate, setSelectedCategoryEstimate] = useState<number>(0);

    const [categories, setCategories] = useState<Array<any>>([]);

    const [errorText, setErrorText] = useState<string | null>(null);
    const [category, setCategory] = useState<any | null>(null);
    const [selectionModel, setSelectionModel] = useState<GridSelectionModel>([]);
    
    const [tabIndex, setTabIndex] = useState(0);

    useEffect(() => {
        getBudget();
        getCategories();
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

    const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
        setTabIndex(newValue);
    };

    async function setExcludedTransaction(transactionId: string){
        await budgetService.setExcludedTransaction({
            transactionId,
            budgetId
        })
        await getBudget()
    }

    async function setRestoredTransaction(transactionId: string){
        await budgetService.setRestoredTransaction({
            transactionId,
            budgetId
        })
        await getBudget()
    }

    return (
        <Grid container spacing={2}>
            {!budget 
                ? <Grid item xs={12}>
                    <Skeleton />
                </Grid>
                :  <Grid item xs={12}>
                <Box sx={{ width: '100%' }}>
                    <Box sx={{ borderBottom: 1, borderColor: 'divider', pb: 1, mb: 2 }}>
                        <Tabs value={tabIndex} onChange={handleTabChange} aria-label="basic tabs example">
                            <Tab label={budget.budgetName} {...a11yProps(0)} />
                            <Tab label="Transactions" {...a11yProps(1)} />
                            <Tab label="Excluded" {...a11yProps(2)} />
                            <Tab label="Accounts" {...a11yProps(3)} />
                        </Tabs>
                    </Box>
                    <TabPanel value={tabIndex} index={0}>
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
                    </TabPanel>
                    <TabPanel value={tabIndex} index={1}>
                        {selectionModel.length > 0 && <CategoryAutoComplete 
                            label="Select Category" 
                            value={category} 
                            setValue={onSetCategory} 
                            cateogories={categories} 
                            disabled={selectionModel.length === 0}
                            errorText={errorText}
                            />}
                        <TransactionList 
                            items={budget.transactions}
                            selectionModel={selectionModel} 
                            setSelectionModel={setSelectionModel} 
                            excludeTransaction={setExcludedTransaction}
                        />
                    </TabPanel>
                    <TabPanel value={tabIndex} index={2}>
                        <ExcludedTransactionList 
                            items={budget.excludedTransactions}
                            restoreTransaction={setRestoredTransaction}
                        />
                    </TabPanel>
                    <TabPanel value={tabIndex} index={3}>
                        <AccountList items={budget.budgetAccessItems} />
                    </TabPanel>
                </Box>
            </Grid>}
        </Grid>
    )
  };

  export default Budget;