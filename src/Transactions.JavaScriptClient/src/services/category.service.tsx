import { send } from './api.service'
export const categoryService = {
    getCategories,
    bulkUpdateTransactionCategory,
    saveCategory
};

async function getCategories() {
    const request = {
        method: 'get'
    }
    return send(`/categories`, request)
}

async function bulkUpdateTransactionCategory (data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/categories/BulkUpdateTransactionCategory`, request)
}

async function saveCategory(data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/categories/SaveCategory`, request)
}