import { FunctionComponent, useState, useEffect } from 'react';
import {
    Skeleton,
    Grid,
    Typography
} from '@mui/material';

import { transactionService } from './../services/transaction.service'
import AccountList from './../components/AccountList'

export const Accounts: FunctionComponent = () => {
    const [accessItems, setAccessItems] = useState<Array<any>>();

    useEffect(() => {
      async function getUserAccessItems() {
        const response = await transactionService.getUserAccessItems();
        setAccessItems(response)
      }
      getUserAccessItems();
    }, []);
  
    return (
        <Grid container spacing={2}>
            <Grid item xs>
                <Typography variant='h3'>Accounts</Typography>
                {!accessItems 
                ? <Skeleton /> 
                : <AccountList items={accessItems} />}
            </Grid>
        </Grid>
    )
  };

  export default Accounts;