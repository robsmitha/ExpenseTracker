import { FunctionComponent, useState, useEffect } from 'react';
import { Outlet } from "react-router-dom";
import AppBar from './AppBar';
import { UserContext } from '../context/UserContext'
import { authService } from '../services/auth.service'
import {
    Box
} from '@mui/material';

interface LayoutProps {
    
}
 
const Layout: FunctionComponent<LayoutProps> = (props) => {
    const [ authenticated, setAuthenticated] = useState<boolean>(() => {
        const appUser = authService.getAppUser()
        return appUser?.authenticated === true
    });

    return (
        <UserContext.Provider value={{
            authenticated,
            setAuthenticated
        }}>
            <Box sx={{ display: 'flex', pt: 4 }}>
                <AppBar />
                <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
                    <Outlet />
                </Box>
            </Box>
        </UserContext.Provider>
    );
}
 
export default Layout;