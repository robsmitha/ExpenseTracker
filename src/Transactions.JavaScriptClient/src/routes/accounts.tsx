import { FunctionComponent, useState, useEffect, Key, ReactChild, ReactFragment, ReactPortal } from 'react';
import {
    Typography,
    Grid
} from '@mui/material';

import { transactionService } from '../services/transaction.service'


export const Accounts: FunctionComponent = () => {

    const [accessItems, setAccessItems] = useState<Array<any>>();

    useEffect(() => {
      async function getUserAccessItems() {
        const response = await transactionService.getUserAccessItems();
        console.log(response)
        setAccessItems(response)
      }
      getUserAccessItems();
    }, []);

    const renderAccessItems = () => {
        return accessItems?.map(i => {
            return <div key={i.item.itemId}>
                <Typography variant='h5'>{i.institution.name}</Typography>
                {i.accounts.map((a: { account_id: Key | null | undefined; official_name: boolean | ReactChild | ReactFragment | ReactPortal | null | undefined; }) => {
                    return <Typography variant='h6' key={a.account_id}>{a.official_name}</Typography>
                })}
                </div>
        })
    }
    
    const renderLoading = () =>{
        return <div>Loading</div>;
    }
  
    return (
        <Grid container spacing={2} pt={2}>
            <Grid item xs>
                <Typography variant='h3'>Accounts</Typography>
                {!accessItems 
                ? renderLoading() 
                : renderAccessItems()}
            </Grid>
        </Grid>
    )
  };

  export default Accounts;