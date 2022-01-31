import React, { useState } from 'react';
import { UserConsumer } from '../../contexts/UserContext';
import {
    Navbar,
    NavItem,
    NavbarToggler,
    Collapse,
    NavLink,
    Nav,
    NavbarBrand
} from 'reactstrap';


function NavMenu(){
    const [isOpen, setIsOpen] = useState(false);
    const [b2cLoginUrl] = useState(process.env.REACT_APP_B2C_SIGN_UP_SIGN_IN_ENDPOINT);
    
    return(
        <UserConsumer>
        {({ auth }) => (
            <Navbar color="light" light expand="md">
                <NavbarBrand href="/">Expense Tracker</NavbarBrand>
                <NavbarToggler onClick={() => { setIsOpen(!isOpen) }} />
                <Collapse isOpen={isOpen} navbar>
                    <Nav className="mr-auto" navbar>
                        <NavItem>
                            <NavLink href="/">Home</NavLink>
                        </NavItem>
                        {!auth ? 
                        <NavItem>
                            <NavLink href={b2cLoginUrl}>Login</NavLink>
                        </NavItem>
                        : <NavItem>
                            <NavLink href="/sign-out">Sign Out</NavLink>
                        </NavItem>}
                    </Nav>
                </Collapse>
            </Navbar>
        )}
        </UserConsumer>  
    )
}

export default NavMenu;
