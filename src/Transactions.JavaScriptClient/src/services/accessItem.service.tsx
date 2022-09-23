import { send } from './api.service'
export const accessItemService = {
    getUserAccessItems,
    createLinkToken,
    setAccessToken,
    createLinkTokenForUpdate,
    getUserAccessItem
};


async function getUserAccessItems() {
    const request = {
        method: 'get'
    }
    return send(`/accessItems`, request)
}

async function getUserAccessItem(userAccessItemId: number) {
    const request = {
        method: 'get'
    }
    return send(`/accessItems/${userAccessItemId}`, request)
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

async function createLinkTokenForUpdate (accessToken: string) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        }
    }
    return send(`/accessItems/createLinkToken/${accessToken}`, request)
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