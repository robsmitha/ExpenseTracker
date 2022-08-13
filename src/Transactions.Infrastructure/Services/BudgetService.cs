using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Extensions;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;
using Transactions.Domain.Entities;
using Transactions.Infrastructure.Context;

namespace Transactions.Infrastructure.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BudgetService(ApplicationContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BudgetModel>> GetBudgetsAsync()
        {
            var budgets = await _context.Budgets.Where(b => b.UserId == _httpContextAccessor.GetUserId()).ToListAsync();
            return _mapper.Map<List<BudgetModel>>(budgets);
        }

        public async Task<BudgetModel> GetBudgetAsync(int budgetId)
        {
            var budget = await _context.Budgets.FindAsync(budgetId);
            var model = _mapper.Map<BudgetModel>(budget);
            return model;
        }

        public async Task<List<BudgetCategoryModel>> GetBudgetCategoriesAsync(int budgetId)
        {
            var budgetCategories = await _context.BudgetCategories
                .Include(i => i.Category).Where(b => b.BudgetId == budgetId).ToListAsync();
            var model = _mapper.Map<List<BudgetCategoryModel>>(budgetCategories);
            return model;
        }

        public async Task<BudgetModel> AddBudgetAsync(BudgetModel model)
        {
            var budget = new Budget
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                UserId = _httpContextAccessor.GetUserId()
            };
            await _context.AddAsync(budget);
            await _context.SaveChangesAsync();

            if (model.Categories?.Any() == true)
            {
                foreach (var category in model.Categories)
                {
                    await _context.AddAsync(new BudgetCategory
                    {
                        BudgetId = budget.Id,
                        CategoryId = category.Id
                    });
                }
                await _context.SaveChangesAsync();
            }

            if (model.BudgetAccessItems?.Any() == true)
            {
                foreach (var accessItem in model.BudgetAccessItems)
                {
                    await _context.AddAsync(new BudgetAccessItem
                    {
                        BudgetId = budget.Id,
                        UserAccessItemId = accessItem.UserAccessItemId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<BudgetModel>(budget);
        }

        public async Task<BudgetModel> UpdateBudgetAsync(BudgetModel model)
        {
            var budget = await _context.Budgets.FindAsync(model.Id);

            budget.Name = model.Name;
            budget.StartDate = model.StartDate;
            budget.EndDate = model.EndDate;

            var newCategoryIds = model.Categories.Select(c => c.Id);
            var oldCategories = await _context.BudgetCategories.Where(c => c.BudgetId == model.Id).ToListAsync();
            var oldCategoryIds = oldCategories.Select(c => c.CategoryId).ToList();

            if (model.Categories?.Count > 0)
            {
                var removeCategoryIds = oldCategoryIds.Except(newCategoryIds).ToList();
                foreach (var categoryId in removeCategoryIds)
                {
                    var budgetCategory = await _context.BudgetCategories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                    _context.Remove(budgetCategory);
                }

                var addCategoryIds = newCategoryIds.Except(oldCategoryIds).ToList();
                foreach (var categoryId in addCategoryIds)
                {
                    await _context.AddAsync(new BudgetCategory
                    {
                        BudgetId = budget.Id,
                        CategoryId = categoryId
                    });
                }
            }
            else if (oldCategories.Any())
            {
                _context.RemoveRange(oldCategories);
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<BudgetModel>(budget);
        }

        public async Task<BudgetCategoryModel> UpdateBudgetCategoryEstimateAsync(int budgetId, int categoryId, decimal estimate)
        {
            var budgetCategory = await _context.BudgetCategories
                .FirstOrDefaultAsync(b => b.BudgetId == budgetId && b.CategoryId == categoryId);
            budgetCategory.Estimate = estimate;
            await _context.SaveChangesAsync();
            return _mapper.Map<BudgetCategoryModel>(budgetCategory);
        }

        public async Task AddBudgetCategoryAsync(int budgetId, int categoryId, decimal? estimate = null)
        {
            await _context.BudgetCategories.AddAsync(new BudgetCategory
            {
                BudgetId = budgetId,
                CategoryId = categoryId,
                Estimate = estimate ?? 0
            });
            await _context.SaveChangesAsync();
        }

        public async Task<BudgetExcludedTransactionModel> SetExcludedTransactionAsync(int budgetId, string transactionId)
        {
            var excludedTransaction = await _context.BudgetExcludedTransactions.FirstOrDefaultAsync(t => t.BudgetId == budgetId && t.TransactionId == transactionId);
            if (excludedTransaction == null)
            {
                excludedTransaction = new BudgetExcludedTransaction
                {
                    BudgetId = budgetId,
                    TransactionId = transactionId
                };
            }

            await _context.BudgetExcludedTransactions.AddAsync(excludedTransaction);
            await _context.SaveChangesAsync();
            return _mapper.Map<BudgetExcludedTransactionModel>(excludedTransaction);
        }
    }
}
