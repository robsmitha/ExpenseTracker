using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;

namespace Transactions.Application.Commands
{
    public class SaveCategoryCommand : IRequest<CategoryModel>
    {
        public int BudgetId { get; set; }
        public decimal? Estimate { get; set; }
        public CategoryModel Category { get; set; }
        public SaveCategoryCommand(CategoryModel category, int budgetId, decimal? estimate = null)
        {
            Category = category;
            BudgetId = budgetId;
            Estimate = estimate;
        }
        public class Validator : AbstractValidator<SaveCategoryCommand>
        {
            private readonly IApplicationContext _context;
            public Validator(IApplicationContext context)
            {
                _context = context;

                RuleFor(v => v.Category)
                    .NotEmpty()
                    .MustAsync(HaveUniqueNameAsync)
                        .WithMessage("Category name already exists.");
            }

            public async Task<bool> HaveUniqueNameAsync(CategoryModel category,
                CancellationToken cancellationToken)
            {
                return await _context.Categories.AllAsync(c => !string.Equals(c.Name, category.Name)
                    || (string.Equals(c.Name, category.Name) && c.Id == category.Id));
            }
        }

        public class Handler : IRequestHandler<SaveCategoryCommand, CategoryModel>
        {
            private readonly ICategoryService _categoryService;
            private readonly IBudgetService _budgetService;
            private readonly IMapper _mapper;
            public Handler(ICategoryService categoryService, IBudgetService budgetService, IMapper mapper)
            {
                _categoryService = categoryService;
                _budgetService = budgetService;
                _mapper = mapper;
            }
            public async Task<CategoryModel> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
            {
                CategoryModel model;
                if (request.Category.IsExisting)
                {
                    model = await _categoryService.UpdateCategoryAsync(request.Category);
                }
                else
                {
                    model = await _categoryService.AddCategoryAsync(request.Category);
                }

                await _budgetService.AddBudgetCategoryAsync(request.BudgetId, model.Id, request.Estimate);

                return model;
            }

        }
    }
}
