import { FunctionComponent, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Typography,
    Grid,
    Alert,
    Button,
    AlertTitle,
    Toolbar
} from '@mui/material';
import { budgetService } from '../services/budget.service'
import { accessItemService } from '../services/accessItem.service'
import AddBudgetDialog from '../components/AddBudgetDialog';


export const Dashboard: FunctionComponent = () => {
    const navigate = useNavigate();
    const [budgets, setBudgets] = useState<any>([]);
    const [open, setOpen] = useState(false);
    const [institutions, setInstitutions] = useState<any>([]);

    useEffect(() => {
        getBudgets();
        getUserAccessItems();
    }, []);

    async function getBudgets() {
        const response = await budgetService.getBudgets();
        setBudgets(response)
    }

    async function getUserAccessItems() {
        const response = await accessItemService.getUserAccessItems();
        const data = response.map((item: any) => {
            return {
                name: item.institution.name,
                label: item.institution.name,
                id: item.userAccessItemId
            };
        });
        setInstitutions(data);
    }

    const handleClickOpen = () => {
        setOpen(true);
    };

    const reloadBudgets = () => {
        getBudgets();
    };
    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
              <Toolbar>
                <Typography variant="h3" component="div" sx={{ flexGrow: 1 }}>
                    Budgets
                </Typography>
                <Button variant="outlined" onClick={handleClickOpen}>
                    Add New
                </Button>
                <AddBudgetDialog open={open} setOpen={setOpen} reloadBudgets={reloadBudgets} institutions={institutions} />
              </Toolbar>
            </Grid>
            <Grid item xs>
                { budgets && budgets.map((b: any) => {
                    return <Button key={b.id} variant="text" onClick={() => navigate(`/budget/${b.id}`)}>{b.name}</Button>
                }) }
            </Grid>
        </Grid>
        
    )
  };

  export default Dashboard;