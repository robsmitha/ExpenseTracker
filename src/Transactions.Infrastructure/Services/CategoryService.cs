using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;
using Transactions.Domain.Entities;
using Transactions.Infrastructure.Context;

namespace Transactions.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public CategoryService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryModel>>(categories);
        }

        public async Task<List<TransactionCategoryModel>> GetTransactionCategoriesAsync(List<string> transactionIds = null)
        {
            var transactionCategories = await _context.TransactionCategories
                .Include(c => c.Category)
                .Where(c => (transactionIds == null || transactionIds.Contains(c.TransactionId)))
                .ToListAsync();
            return _mapper.Map<List<TransactionCategoryModel>>(transactionCategories);
        }

        public async Task<TransactionCategoryModel> SetTransactionCategoryAsync(string transactionId, int categoryId)
        {
            TransactionCategory transactionCategory = null;
            try
            {
                // get existing transaction category if any
                transactionCategory = await _context.TransactionCategories
                    .SingleOrDefaultAsync(t => t.TransactionId == transactionId && t.CategoryId == categoryId);
            }
            catch (InvalidOperationException)
            {
                // multiple records, delete all and set new transaction category
                var duplicates = await _context.TransactionCategories
                    .Where(t => t.TransactionId == transactionId && t.CategoryId == categoryId)
                    .ToListAsync();
                _context.RemoveRange(duplicates);
                await _context.SaveChangesAsync(); 
            }
            finally
            {
                // create or update transactionCategory
                if (transactionCategory == null)
                {
                    transactionCategory = new TransactionCategory
                    {
                        TransactionId = transactionId,
                        CategoryId = categoryId
                    };
                    await _context.AddAsync(transactionCategory);
                }
                else
                {
                    transactionCategory.CategoryId = categoryId;
                }
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<TransactionCategoryModel>(transactionCategory);
        }
    }
}
