import { FunctionComponent, useContext, useState, useEffect } from 'react';
import {
    Typography,
    Grid
} from '@mui/material';


export const Home: FunctionComponent = () => {

  
    return (
        <Grid container spacing={2} pt={2}>
            <Grid item xs>
                <Typography variant='h3'>Home</Typography>
            </Grid>
        </Grid>
        
    )
  };

  export default Home;