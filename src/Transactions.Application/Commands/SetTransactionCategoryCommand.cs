using MediatR;
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
    public class SetTransactionCategoryCommand : IRequest<TransactionCategoryModel>
    {
        private string TransactionId { get; set; }
        private int CategoryId { get; set; }
        public SetTransactionCategoryCommand(string transactionId, int categoryId)
        {
            TransactionId = transactionId;
            CategoryId = categoryId;
        }
        public class Handler : IRequestHandler<SetTransactionCategoryCommand, TransactionCategoryModel>
        {
            private readonly ICategoryService _categoryService;
            public Handler(ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }
            public async Task<TransactionCategoryModel> Handle(SetTransactionCategoryCommand request, CancellationToken cancellationToken)
            {
                return await _categoryService.SetTransactionCategoryAsync(request.TransactionId, request.CategoryId);
            }
        }
    }
}
