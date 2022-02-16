import { send } from './api.service'
import { authService } from './auth.service'
export const transactionService = {
    getUserAccessItems,
    createLinkToken,
    setAccessToken
};

async function getUserAccessItems() {
    const token = authService.getToken()
    var request = {
        method: 'get',
        headers: { 
            'Authorization': `Bearer ${token}` 
        }
    }
    return send(`/transactions/getUserAccessItems`, request)
}

async function createLinkToken () {
    const token = authService.getToken()
    var request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` 
        }
    }
    return send(`/transactions/createLinkToken`, request)
}

async function setAccessToken (publicToken: string) {
    const token = authService.getToken()
    var request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` 
        },
        body: JSON.stringify({ public_token: publicToken })
    }
    return send(`/transactions/setAccessToken`, request)
}