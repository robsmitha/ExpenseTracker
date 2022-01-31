import React, { Component } from 'react';
import './App.css';

import { withRouter } from 'react-router';

//Layouts
import LayoutRoute from './components/_layout/LayoutRoute';

import { Home } from './components/home/Home';
import { Authorize } from './components/authorize/Authorize';
import { SignOut } from './components/sign-out/SignOut';
import { UserProvider } from './contexts/UserContext'

class App extends Component {

  componentWillMount() {
    this.unlisten = this.props.history.listen((location, action) => {
      window.scrollTo(0,0)
    });
  }
  componentWillUnmount() {
      this.unlisten();
  }
  render(){
    return (
      <UserProvider>
        <LayoutRoute exact path='/' component={Home} />
        <LayoutRoute exact path='/Authorize' component={Authorize} />
        <LayoutRoute exact path='/sign-out' component={SignOut} />
      </UserProvider>
    )
  }
}
export default  withRouter(App)
