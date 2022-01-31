import React from 'react';
import { Route } from 'react-router-dom';  
import NavMenu from './NavMenu';


const Layout = ({ children }) => (
    <section>
        <NavMenu />
        {children}
    </section>
);

const LayoutRoute = ({ component: Component, ...rest }) => {
    return (
        <Route {...rest} render={matchProps => (
            <Layout>
                <Component {...matchProps} />
            </Layout>
        )} />
    )
}

export default LayoutRoute;