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

namespace Transactions.Application.Queries
{
    public class GetCategoriesQuery : IRequest<List<CategoryModel>>
    {
        public class Handler : IRequestHandler<GetCategoriesQuery, List<CategoryModel>>
        {
            private readonly ICategoryService _categoryService;

            public Handler(ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }
            public async Task<List<CategoryModel>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
            {
                return await _categoryService.GetCategoriesAsync();
            }
        }
    }
}
