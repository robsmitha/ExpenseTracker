import { send } from './api.service'
export const budgetService = {
    bulkUpdateTransactionCategory,
    saveBudget,
    getBudgets,
    getBudget,
    getTransactions,
    updateBudgetCategoryEstimate
};

async function getBudget(budgetId: number) {
    const request = {
        method: 'get'
    }
    return send(`/budgets/${budgetId}`, request)
}

async function getTransactions(budgetId: number) {
    const request = {
        method: 'get'
    }
    return send(`/budgets/${budgetId}/transactions`, request)
}

async function getBudgets() {
    const request = {
        method: 'get'
    }
    return send(`/budgets`, request)
}

async function bulkUpdateTransactionCategory (data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/budgets/BulkUpdateTransactionCategory`, request)
}

async function saveBudget(data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/budgets`, request)
}

async function updateBudgetCategoryEstimate (data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/budgets/UpdateBudgetCategoryEstimate`, request)
}
