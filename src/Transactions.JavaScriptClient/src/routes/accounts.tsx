import { FunctionComponent, useState, useEffect } from 'react';
import {
    Skeleton,
    Grid,
    Typography,
    Toolbar
} from '@mui/material';

import { accessItemService } from '../services/accessItem.service'
import AccountList from './../components/AccountList'
import PlaidLink from "./../components/PlaidLink";

export const Accounts: FunctionComponent = () => {
    const [accessItems, setAccessItems] = useState<Array<any>>();
    const [token, setToken] = useState<string | null>(null);

    useEffect(() => {
      async function createLinkToken() {
        const response = await accessItemService.createLinkToken();
        const { link_token } = response;
        setToken(link_token);
      }
      getUserAccessItems();
      createLinkToken();
    }, []);
  
    async function getUserAccessItems() {
      const response = await accessItemService.getUserAccessItems();
      setAccessItems(response)
    }
    const reloadAccessItems = async () => {
      await getUserAccessItems();
    };

    return (
        <Grid container spacing={2}>
          <Grid item xs={12}>
              <Toolbar>
                <Typography variant="h3" component="div" sx={{ flexGrow: 1 }}>
                  Accounts
                </Typography>
                {token !== null && <PlaidLink buttonText='Connect Bank Account' token={token} reloadAccessItems={reloadAccessItems} />}
              </Toolbar>
              {!accessItems 
              ? <Skeleton /> 
              : <AccountList items={accessItems} />}
          </Grid>
        </Grid>
    )
  };

  export default Accounts;