import { send } from './api.service'
export const transactionService = {
    getUserAccessItems,
    getTransactions,
    createLinkToken,
    setAccessToken
};

async function getUserAccessItems() {
    const request = {
        method: 'get'
    }
    return send(`/transactions/getUserAccessItems`, request)
}

async function getTransactions(itemId: string, startDate: Date | null = null, endDate: Date | null = null) {
    const request = {
        method: 'get'
    }
    return send(`/transactions/${itemId}`, request)
}

async function createLinkToken () {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        }
    }
    return send(`/transactions/createLinkToken`, request)
}

async function setAccessToken (publicToken: string) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ public_token: publicToken })
    }
    return send(`/transactions/setAccessToken`, request)
}