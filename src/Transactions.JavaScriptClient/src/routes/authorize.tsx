import { FunctionComponent, useEffect, useState } from 'react';
import { useUserContext } from './../context/UserContext';
import { useNavigate } from 'react-router-dom';
import {
    Typography
} from '@mui/material';

export const Authorize: FunctionComponent = () => {
    const [ error, setError ] = useState<string | null>();
    const { authenticated, setAuthenticated } = useUserContext()
    const navigate = useNavigate();

    useEffect(() => {
        function setToken() {
            const search = window.location.search;
            const params = new URLSearchParams(search);
            const token = params.get('id_token');
            if(token) {
                setAuthenticated(true, token);
                localStorage.setItem("appUser", JSON.stringify({
                    token,
                    authenticated: true
                }));
                navigate('/');
            }
            else {
                setError('Token not present');
            }
        }
        setToken();
      }, []);
    
    return (
        <Typography variant="h1">
            {error}
        </Typography>
        
    )
  };

  export default Authorize;