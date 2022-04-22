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
        
        public async Task<CategoryModel> AddCategoryAsync(CategoryModel model)
        {
            var category = new Category
            {
                Name = model.Name
            };
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel> UpdateCategoryAsync(CategoryModel model)
        {
            var category = await _context.Categories.FindAsync(model.Id);
            category.Name = model.Name;
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryModel>(category);
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
                    .SingleOrDefaultAsync(t => t.TransactionId == transactionId);
            }
            catch (InvalidOperationException)
            {
                // multiple records, delete all and set new transaction category
                var duplicates = await _context.TransactionCategories
                    .Where(t => t.TransactionId == transactionId)
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
