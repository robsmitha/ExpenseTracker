import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import TextField from '@mui/material/TextField';
import Grid from '@mui/material/Grid';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import CloseIcon from '@mui/icons-material/Close';
import Slide from '@mui/material/Slide';
import { TransitionProps } from '@mui/material/transitions';
import DatePicker from './CustomDatePicker'
import { budgetService } from '../services/budget.service'
import { categoryService } from '../services/category.service'
import CategoryAutoComplete from '../components/CategoriesAutocomplete'
import CategoriesAutocompleteMultiple from './../components/CategoriesAutocompleteMultiple'
import CategoriesCheckboxList from './../components/CategoriesCheckboxList'

const Transition = React.forwardRef(function Transition(
  props: TransitionProps & {
    children: React.ReactElement;
  },
  ref: React.Ref<unknown>,
) {
  return <Slide direction="up" ref={ref} {...props} />;
});

interface Props {
    open: boolean;
    setOpen: (value: boolean) => void;
    reloadBudgets: () => void;
    institutions: any[];
}
const AddBudgetDialog : React.FunctionComponent<Props> = ({ open, setOpen, reloadBudgets, institutions }) => {
    const date = new Date();
    const firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    const lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    const defaultBudgetName = `${date.toLocaleString('default', { month: 'long' })} ${date.getFullYear()}`;
    
    const [startDate, setStartDate] = React.useState<Date | undefined>(firstDay);
    const [endDate, setEndDate] = React.useState<Date | undefined>(lastDay);
    const [budgetName, setBudgetName] = React.useState<string | null>(defaultBudgetName);
    const [categories, setCategories] = React.useState<Array<any>>([]);
    const [checkedCategories, setCheckedCategories] = React.useState<any | null>([]);
    const [errorText, setErrorText] = React.useState<string | null>(null);

    const [checkedInstitutions, setCheckedInstitutions] = React.useState<any | null>([]);


    React.useEffect(() => {
        getCategories();
    }, []);

    const handleClose = () => {
        setBudgetName(defaultBudgetName)
        setStartDate(firstDay)
        setEndDate(lastDay)
        setCheckedCategories(categories)
        setErrorText(null)
        setOpen(false);
    };

    const handleSave = async () => {
        setErrorText(null)
        const budgetAccessItems = institutions.map((item: any) => {
            return {
                institutionName: item.name,
                userAccessItemId: item.id
            };
        });
        const data = {
            name: budgetName,
            startDate,
            endDate,
            categories: checkedCategories,
            budgetAccessItems
        };
        const response = await budgetService.saveBudget(data);
        if(response.errors) {
            const msg = Object.keys(response.errors)
                        .map((e: any) => response.errors[e].toString())
                        .join(", ")
            setErrorText(msg)
            return;
        } 
        handleClose();
        reloadBudgets();
    };

    async function getCategories() {
        const response = await categoryService.getCategories();
        const categories = response.map((c: any) => {
            return {
            ...c,
            label: c.name
            };
        });
        setCategories(categories)
        // select all by default
        setCheckedCategories(categories)
    }
    async function onSetCheckedCategories(value: any) {
        console.log("onSetCheckedCategories", value)
        setCheckedCategories(value)
    }

    async function onSetCheckedInstitutions(value: any) {
        console.log("onSetCheckedInstitutions", value)
        setCheckedInstitutions(value)
    }
    
    const handleBudgetNameChange = (e: any) => {
        setErrorText(null)
        setBudgetName(e.target.value);
    };

    return (
        <Dialog
        fullScreen
        open={open}
        onClose={handleClose}
        TransitionComponent={Transition}
        >
            <AppBar sx={{ position: 'relative' }}>
                <Toolbar>
                <IconButton
                    edge="start"
                    color="inherit"
                    onClick={handleClose}
                    aria-label="close"
                >
                    <CloseIcon />
                </IconButton>
                <Typography sx={{ ml: 2, flex: 1 }} variant="h6" component="div">
                    New Budget
                </Typography>
                <Button autoFocus color="inherit" onClick={handleSave}>
                    save
                </Button>
                </Toolbar>
            </AppBar>
            <DialogContent>
                <Grid sx={{ mb: 3}} container spacing={2}>
                    <Grid item xs={12} sm={12} md={4} lg={3}>
                        <TextField
                            value={budgetName}
                            autoFocus
                            id="budgetName"
                            label="Budget Name"
                            type="text"
                            fullWidth
                            variant="outlined"
                            onChange={handleBudgetNameChange}
                            error={errorText !== null}
                            helperText={errorText}
                        />
                    </Grid>
                </Grid>
                <Grid sx={{ mb: 3}} container spacing={2}>
                    <Grid item xs={12} sm="auto">
                        <DatePicker value={startDate} setValue={setStartDate} label="Start Date" />
                    </Grid>
                    <Grid item xs={12} sm="auto">
                        <DatePicker value={endDate} setValue={setEndDate} label="End Date" />
                    </Grid>
                </Grid>
                <Grid sx={{ mb: 3}} container spacing={2}>
                    <Grid item xs={12} sm={12} md={4} lg={3}>
                        {/* <CategoriesCheckboxList
                            label="" 
                            checked={checkedCategories} 
                            setChecked={onSetCheckedCategories} 
                            categories={categories}
                        /> */}
                        <CategoriesAutocompleteMultiple
                            label="Budget Categories" 
                            value={checkedCategories} 
                            setValue={onSetCheckedCategories} 
                            options={categories}
                        />
                    </Grid>
                </Grid>
                <Grid sx={{ mb: 3}} container spacing={2}>
                    <Grid item xs={12} sm={12} md={4} lg={3}>
                        <CategoriesAutocompleteMultiple
                            label="Budget Accounts" 
                            value={checkedInstitutions} 
                            setValue={onSetCheckedInstitutions} 
                            options={institutions}
                        />
                    </Grid>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}
export default AddBudgetDialog;