import { send } from './api.service'
export const categoryService = {
    getCategories,
    saveCategory
};

async function getCategories() {
    const request = {
        method: 'get'
    }
    return send(`/categories`, request)
}

async function saveCategory(data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/categories`, request)
}
