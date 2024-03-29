﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Models;

namespace Transactions.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetCategoriesAsync();
        Task<CategoryModel> GetCategoryAsync(int categoryId);
        Task<CategoryModel> GetCategoryByNameAsync(string categoryName);
        Task<CategoryModel> AddCategoryAsync(CategoryModel model);
        Task<CategoryModel> UpdateCategoryAsync(CategoryModel model);
        Task<List<TransactionCategoryModel>> GetTransactionCategoriesAsync(int budgetId);
        Task<TransactionCategoryModel> SetTransactionCategoryAsync(string transactionId, int categoryId, int budgetId);
    }
}
