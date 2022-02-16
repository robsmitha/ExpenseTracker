import { FunctionComponent, useState, useEffect } from 'react';
import { Outlet } from "react-router-dom";
import AppBar from './AppBar';
import { UserContext } from './../context/UserContext'
import {
    Container
} from '@mui/material';

interface LayoutProps {
    
}
 
const Layout: FunctionComponent<LayoutProps> = (props) => {
    const [ authenticated, setAuthenticated] = useState<boolean>(() => {
        const appUser = JSON.parse(localStorage.getItem("appUser") || '{}')
        return appUser?.authenticated === true
    });

    return (
        <UserContext.Provider value={{
            authenticated,
            setAuthenticated
        }}>
            <AppBar />
            <Container>
                <Outlet />
            </Container>
        </UserContext.Provider>
    );
}
 
export default Layout;