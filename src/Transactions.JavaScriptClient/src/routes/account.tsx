import { FunctionComponent, useEffect, useState } from 'react';
import {
    Typography,
    Grid
} from '@mui/material';
import { useParams } from 'react-router-dom';

export const Account: FunctionComponent = () => {    
    const { accountId } = useParams();
    return (
        <Grid>
            Account
        </Grid>
        
    )
  };

  export default Account;