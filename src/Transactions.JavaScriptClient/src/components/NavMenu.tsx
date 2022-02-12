import { FunctionComponent, useContext,useState } from 'react';
import { UserContext } from './../context/UserContext'
import { useNavigate } from 'react-router-dom';
import { 
    AppBar, 
    Toolbar,
    Typography,
    Button,
    IconButton
} from '@mui/material';

import MenuIcon from '@mui/icons-material/Menu';


export const NavMenu: FunctionComponent = () => {
    const userContext = useContext(UserContext);
    const navigate = useNavigate();
    const [b2cLoginUrl] = useState(process.env.REACT_APP_B2C_SIGN_UP_SIGN_IN_ENDPOINT || '');
    
    return (
        <AppBar position="static">
            <Toolbar>
                <IconButton
                    size="large"
                    edge="start"
                    color="inherit"
                    aria-label="menu"
                    sx={{ mr: 2 }}
                >
                    <MenuIcon />
                </IconButton>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Expense Tracker
                </Typography>
                <Button onClick={() => navigate('/')} color="inherit">Home</Button>
                {!userContext!.authenticated ? 
                <Button href={b2cLoginUrl} variant="outlined" color="inherit">Sign in</Button>
                : <Button onClick={() => navigate('sign-out')} variant="outlined" color="inherit">Sign out</Button>}
                
            </Toolbar>
        </AppBar>
    );
}
export default NavMenu;