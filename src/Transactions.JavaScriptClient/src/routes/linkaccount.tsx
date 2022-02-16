import { FunctionComponent, useState, useEffect } from 'react';
import {
    Typography,
    Grid
} from '@mui/material';

import PlaidLink from "./../components/PlaidLink";
import { transactionService } from './../services/transaction.service'


export const Home: FunctionComponent = () => {

    const [token, setToken] = useState<string | null>(null);
  
    // generate a link_token
    useEffect(() => {
      async function createLinkToken() {
        const response = await transactionService.createLinkToken();
        const { link_token } = response;
        setToken(link_token);
      }
      createLinkToken();
    }, []);
  
    return (
        <Grid container spacing={2} pt={2}>
            <Grid item xs>
                <Typography variant='h3'>Link new account</Typography>
                    {token === null 
                    ? <div>Loading..</div> 
                    : <PlaidLink token={token} />}
            </Grid>
        </Grid>
        
    )
  };

  export default Home;