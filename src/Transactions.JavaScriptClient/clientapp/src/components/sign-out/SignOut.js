import React, { Component } from 'react';
import { UserConsumer } from '../../contexts/UserContext';
import { Redirect } from 'react-router';
import { Container } from 'reactstrap';
import { Link } from 'react-router-dom';

export class SignOut extends Component {
    render() {
        return (
            <UserConsumer>
                {({ auth,signOut }) => (
                    <Container>
                        {!auth ? 
                        <Redirect to="/" />
                        : <div className="vh-100 d-flex align-items-stretch py-5">
                        <div className="container">
                            <div className="row">
                                <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                                    <h1 className="h3 mb-4 text-center">
                                        Expense Tracker
                                    </h1>
                                    <p className="lead mb-2 text-center text-muted">Are you sure you want to go?</p>
                                    <button type="button" onClick={signOut} className="btn btn-primary btn-block my-3">
                                        Yes, sign me out
                                    </button>
                                    <Link to='/' className="btn btn-link btn-block text-decoration-none">
                                        No, keep me signed in
                                    </Link>
                                </div>
                            </div>
                        </div>
                    </div>}
                    </Container>
                )}
            </UserConsumer>
        );
    }
    
}