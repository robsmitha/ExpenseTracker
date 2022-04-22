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
        public CategoryModel Category { get; set; }
        public SaveCategoryCommand(CategoryModel category)
        {
            Category = category;
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
            public Handler(ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }
            public async Task<CategoryModel> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
            {
                return request.Category.IsExisting
                    ? await _categoryService.UpdateCategoryAsync(request.Category)
                    : await _categoryService.AddCategoryAsync(request.Category);
            }

        }
    }
}
