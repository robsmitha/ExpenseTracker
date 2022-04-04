import { FunctionComponent, useEffect, useState } from 'react';
import { useUserContext } from './../context/UserContext';
import { useNavigate } from 'react-router-dom';
import { authService } from './../services/auth.service'
import {
    Typography,
    Grid,
    Button
} from '@mui/material';

export const Authorize: FunctionComponent = () => {
    const [b2cLoginUrl] = useState(process.env.REACT_APP_B2C_SIGN_UP_SIGN_IN_ENDPOINT || '');
    const [ error, setError ] = useState<string | null>();
    const { setAuthenticated } = useUserContext()
    const navigate = useNavigate();

    useEffect(() => {
        function setToken() {
            const search = window.location.search;
            const params = new URLSearchParams(search);
            const token = params.get('id_token');
            if(token) {
                setAuthenticated(true, token);
                authService.setAppUser({
                    token,
                    authenticated: true
                })
                navigate('/');
            }
            else {
                setError('Token not present');
            }
        }
        setToken();
      }, []);
    
    return (
        <Grid>
        <Grid item md={6}>
            <Typography variant="h3" component="div">
                Sign in Error
            </Typography>
            <Typography variant="h6" component="div">
                {error}
            </Typography>
            <Grid container spacing={2} pt={2}>
                <Grid item xs={6}>
                    <Button fullWidth variant="contained" href={b2cLoginUrl}>
                        Try Again
                    </Button>
                </Grid>
                <Grid item xs={6}>
                    <Button fullWidth variant="outlined" onClick={() => navigate('/')}>
                        Go Home
                    </Button>
                </Grid>
            </Grid>
        </Grid>
    </Grid>
        
    )
  };

  export default Authorize;