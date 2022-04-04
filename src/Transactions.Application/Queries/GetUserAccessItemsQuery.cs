using MediatR;
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
    public class GetUserAccessItemsQuery : IRequest<List<UserAccessItemModel>>
    {
        public string UserId { get; set; }
        public GetUserAccessItemsQuery(string userId)
        {
            UserId = userId;
        }
        public class Handler : IRequestHandler<GetUserAccessItemsQuery, List<UserAccessItemModel>>
        {
            private readonly IFinancialService _financialService;
            private readonly IAccessTokenService _accessTokenService;

            public Handler(IFinancialService financialService, IAccessTokenService accessTokenService)
            {
                _financialService = financialService;
                _accessTokenService = accessTokenService;
            }

            public async Task<List<UserAccessItemModel>> Handle(GetUserAccessItemsQuery request, CancellationToken cancellationToken)
            {
                var accessTokens = await _accessTokenService.GetAccessTokensAsync(request.UserId);

                var userAccessItems = new List<UserAccessItemModel>();
                foreach(var accessToken in accessTokens)
                {
                    try
                    {
                        var accounts = await _financialService.GetAccountsAsync(accessToken.AccessToken);
                        var item = await _financialService.GetItemAsync(accessToken.AccessToken);
                        var institution = await _financialService.GetInstitutionAsync(item.InstitutionId);
                        userAccessItems.Add(new UserAccessItemModel
                        {
                            Accounts = accounts,
                            Institution = institution,
                            Item = item
                        });
                    }
                    catch
                    {
                        // TODO: Add account name to user access item
                        // then show "action needed" when account require authentication
                        continue;
                    }
                }
                return userAccessItems;
            }
        }
    }
}
