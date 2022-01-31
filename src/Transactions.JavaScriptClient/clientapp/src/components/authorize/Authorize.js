import React, { Component } from 'react';
import { UserContext, UserConsumer } from '../../contexts/UserContext';
import { Redirect } from 'react-router';
import { Container } from 'reactstrap';

export class Authorize extends Component {
    constructor(props) {
        super(props)
        this.state = {
            message: null
        }
    }

    componentDidMount() {
        let search = window.location.search;
        let params = new URLSearchParams(search);
        let token = params.get('id_token');
        if(token) {
            this.context.signIn(token)
        }
        else {
            this.setState({ message: 'Token not present.' })
        }
    }

    render() {
        const { message } = this.state;
        return (
            <UserConsumer>
                {({ auth }) => (
                    <Container>
                        {auth ? 
                        <Redirect to="/" />
                        : <p>{message}</p>}
                    </Container>
                )}
            </UserConsumer>
        );
    }
    
}
Authorize.contextType = UserContext