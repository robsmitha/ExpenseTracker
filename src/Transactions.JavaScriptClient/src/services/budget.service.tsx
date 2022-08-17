import { send } from './api.service'
export const budgetService = {
    bulkUpdateTransactionCategory,
    saveBudget,
    getBudgets,
    getBudget,
    getTransactions,
    updateBudgetCategoryEstimate,
    setExcludedTransaction,
    setRestoredTransaction
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

async function setExcludedTransaction (data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/budgets/SetExcludedTransaction`, request)
}

async function setRestoredTransaction (data: any) {
    const request = {
        method: 'post',
        headers: { 
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return send(`/budgets/RestoreExcludedTransaction`, request)
}
