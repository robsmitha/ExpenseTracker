import { FunctionComponent, useEffect } from 'react';
import { useUserContext } from './../context/UserContext';
import { useNavigate } from 'react-router-dom';
import {
    Typography,
    Button,
    Grid
} from '@mui/material';

export const SignOut: FunctionComponent = () => {
    const navigate = useNavigate();
    const { authenticated, setAuthenticated } = useUserContext()
    useEffect(() => {
        function checkIfLoggedIn() {
            if(!authenticated){
                navigate("/")
            }
        }
        checkIfLoggedIn();
      }, []);

    const signOut = () => {
        setAuthenticated(false);
        localStorage.removeItem("appUser");
        navigate("/");
    }

    const cancelSignOut = () => {
        navigate("/");
    }


    return (
        <Grid container spacing={2} pt={2}>
            <Grid item xs>
                
            </Grid>
            <Grid item xs={6}>
                <Typography variant="h3" component="div">
                    Expense Tracker
                </Typography>
                <Typography variant="h6" component="div">
                    Are you sure you want to go?
                </Typography>
                <Grid container spacing={2} pt={2}>
                    <Grid item xs={6}>
                        <Button fullWidth variant="contained" onClick={signOut}>
                            Yes, sign me out
                        </Button>
                    </Grid>
                    <Grid item xs={6}>
                        <Button fullWidth variant="outlined" onClick={cancelSignOut}>
                            No, keep me signed in
                        </Button>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs>
                
            </Grid>
        </Grid>
        
    )
  };

  export default SignOut;