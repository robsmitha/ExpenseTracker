import { FunctionComponent, useState, useEffect } from 'react';
import { Outlet } from "react-router-dom";
import NavMenu from './NavMenu';
import { UserContext } from '../context/UserContext'

interface LayoutProps {
    
}
 
const Layout: FunctionComponent<LayoutProps> = (props) => {
    const [ authenticated, setAuthenticated] = useState<boolean>(false);
    useEffect(() => {
        function checkIfAuthenticated() {
            const appUser = JSON.parse(localStorage.getItem("appUser") || '{}')
            setAuthenticated(appUser?.authenticated === true)
        }
        checkIfAuthenticated();
      }, []);

    return (
        <UserContext.Provider value={{
            authenticated,
            setAuthenticated
        }}>
            <NavMenu />
            <Outlet />
        </UserContext.Provider>
    );
}
 
export default Layout;