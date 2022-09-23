using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transactions.Application.Constants;
using Transactions.Application.Exceptions;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;

namespace Transactions.Application.Queries
{
    public class GetUserAccessItemsQuery : IRequest<List<UserAccessItemModel>>
    {
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
                var accessTokens = await _accessTokenService.GetAccessTokensAsync();
                var expiredAccessTokens = new List<ExpiredAccessItem>();
                var userAccessItems = new List<UserAccessItemModel>();
                foreach(var accessToken in accessTokens)
                {
                    var institution = await _financialService.GetInstitutionAsync(accessToken.InstitutionId);
                    try
                    {
                        var accounts = await _financialService.GetAccountsAsync(accessToken.AccessToken);
                        var item = await _financialService.GetItemAsync(accessToken.AccessToken);
                        userAccessItems.Add(new UserAccessItemModel
                        {
                            Accounts = accounts,
                            Institution = institution,
                            Item = item,
                            UserAccessItemId = accessToken.Id
                        });
                    }
                    catch (FinancialServiceException fex)
                    {
                        var errorAccessItem = new UserAccessItemModel
                        {
                            Institution = institution,
                            UserAccessItemId = accessToken.Id
                        };

                        if (string.Equals(fex.Error?.error_code, ErrorCodes.ITEM_LOGIN_REQUIRED, StringComparison.InvariantCultureIgnoreCase))
                        {
                            errorAccessItem.ExpiredAccessItem = new ExpiredAccessItem(accessToken.AccessToken, fex.Error.error_message, institution);
                        }

                        userAccessItems.Add(errorAccessItem);

                        continue;
                    }
                }
                return userAccessItems;
            }
        }
    }
}
