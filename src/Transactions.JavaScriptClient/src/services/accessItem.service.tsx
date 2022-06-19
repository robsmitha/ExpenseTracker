import { send } from './api.service'
export const accessItemService = {
    getUserAccessItems,
    createLinkToken,
    setAccessToken
};


async function getUserAccessItems() {
    const request = {
        method: 'get'
    }
    return send(`/accessItems/getUserAccessItems`, request)
}

async function createLinkToken () {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        }
    }
    return send(`/accessItems/createLinkToken`, request)
}

async function setAccessToken (publicToken: string) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ public_token: publicToken })
    }
    return send(`/accessItems/setAccessToken`, request)
}