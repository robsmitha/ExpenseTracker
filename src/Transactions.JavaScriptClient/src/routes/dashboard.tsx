import { FunctionComponent, useContext, useState, useEffect } from 'react';
import {
    Typography,
    Grid
} from '@mui/material';


export const Dashboard: FunctionComponent = () => {

  
    return (
        <Grid container spacing={2}>
            <Grid item xs>
                <Typography variant='h3'>Dashboard</Typography>
            </Grid>
        </Grid>
        
    )
  };

  export default Dashboard;