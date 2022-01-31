import React, { FunctionComponent, useState } from 'react';
// @ts-ignore
import { UserConsumer } from '../../contexts/UserContext';
import {
    Container
} from 'reactstrap'
import PlaidLink from "../_helpers/PlaidLink";
// @ts-ignore
import { transactionService } from './../../services/transaction.service'

export const Home: FunctionComponent = () => {
    const [token, setToken] = useState<string | null>(null);
  
    // generate a link_token
    React.useEffect(() => {
      async function createLinkToken() {
        const response = await transactionService.createLinkToken();
        const { link_token } = response;
        setToken(link_token);
      }
      createLinkToken();
    }, []);
  
    return (
        <UserConsumer>
        {({ 
            // @ts-ignore
            auth 
        }) => (
            <Container>
                {auth ? 
                <div>
                    <h1>Hello Authenticated User!</h1>
                    {token === null 
                    ? <div>Loading..</div> 
                    : <PlaidLink token={token} />}
                </div> 
                : <h1>Hello World!</h1>}
            </Container>
        )}
        </UserConsumer> 
    )
  };