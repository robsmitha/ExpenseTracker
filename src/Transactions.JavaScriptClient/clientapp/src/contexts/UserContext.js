import React, { Component } from 'react';
import { authService } from '../services/auth.service';

const UserContext = React.createContext();

class UserProvider extends Component {


    constructor(props) {
        super(props) 
        this.state = {
            auth: authService.appUserValue !== null && authService.appUserValue.auth === true
        }
    }

    componentDidMount() {
        authService.appUser.subscribe(x => {
            if (x !== null && x.token) {
                this.setState({ auth: true })
            }
        })
    }

    signIn = (token) => {
        authService.setAppUser({token, auth: true})
        this.setState({ auth: true })
    }

    signOut = () => {
        authService.clearAppUser();
        this.setState({ auth: false })
    }
    
    render() {
        return (
            <UserContext.Provider value={{
                auth: this.state.auth,
                signIn: this.signIn,
                signOut: this.signOut
            }}>
                {this.props.children}
            </UserContext.Provider>
        )
    }
}

const UserConsumer = UserContext.Consumer

export { UserProvider, UserConsumer, UserContext }