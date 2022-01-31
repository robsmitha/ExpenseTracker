import React, { Component } from 'react';
import { UserConsumer } from '../../contexts/UserContext';
import {
    Container
} from 'reactstrap'

export class Home extends Component {
    constructor(props){
        super(props)
    }

    render(){
        return (
            <UserConsumer>
            {({ auth }) => (
                <Container>{auth ? "Hello Authenticated User!" : "Hello World!"}</Container>
            )}
            </UserConsumer>  
        )
    }
}