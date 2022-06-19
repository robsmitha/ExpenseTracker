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
    public class SaveBudgetCommand : IRequest<BudgetModel>
    {
        public BudgetModel Budget { get; set; }
        public SaveBudgetCommand(BudgetModel budget)
        {
            Budget = budget;
        }
        public class Validator : AbstractValidator<SaveBudgetCommand>
        {
            private readonly IApplicationContext _context;
            public Validator(IApplicationContext context)
            {
                _context = context;

                RuleFor(v => v.Budget)
                    .NotEmpty()
                    .MustAsync(HaveUniqueNameAsync)
                        .WithMessage("Budget name already exists.");
            }

            public async Task<bool> HaveUniqueNameAsync(BudgetModel budget,
                CancellationToken cancellationToken)
            {
                return await _context.Budgets.AllAsync(c => !string.Equals(c.Name, budget.Name)
                    || (string.Equals(c.Name, budget.Name) && c.Id == budget.Id));
            }
        }

        public class Handler : IRequestHandler<SaveBudgetCommand, BudgetModel>
        {
            private readonly IBudgetService _budgetService;
            public Handler(IBudgetService budgetService)
            {
                _budgetService = budgetService;
            }
            public async Task<BudgetModel> Handle(SaveBudgetCommand request, CancellationToken cancellationToken)
            {

                return request.Budget.IsExisting
                    ? await _budgetService.UpdateBudgetAsync(request.Budget)
                    : await _budgetService.AddBudgetAsync(request.Budget);
            }

        }
    }
}
