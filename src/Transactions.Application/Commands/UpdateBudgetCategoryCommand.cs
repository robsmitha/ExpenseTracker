using MediatR;
using Microsoft.Extensions.Logging;
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
    public class UpdateBudgetCategoryCommand : IRequest<BudgetCategoryModel>
    {
        public int BudgetId { get; }
        public string CategoryName { get; }
        public decimal Estimate { get; }

        public UpdateBudgetCategoryCommand(int budgetId, string categoryName, decimal estimate)
        {
            BudgetId = budgetId;
            CategoryName = categoryName;
            Estimate = estimate;
        }

        public class Handler : IRequestHandler<UpdateBudgetCategoryCommand, BudgetCategoryModel>
        {
            private readonly ILogger<UpdateBudgetCategoryCommand> _logger;
            private readonly IBudgetService _budgetService;
            private readonly ICategoryService _categoryService;

            public Handler(ILogger<UpdateBudgetCategoryCommand> logger, IBudgetService budgetService, ICategoryService categoryService)
            {
                _logger = logger;
                _budgetService = budgetService;
                _categoryService = categoryService;
            }

            public async Task<BudgetCategoryModel> Handle(UpdateBudgetCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = await _categoryService.GetCategoryByNameAsync(request.CategoryName);
                return await _budgetService.UpdateBudgetCategoryEstimateAsync(request.BudgetId, category.Id, request.Estimate);
            }
        }
    }
}
