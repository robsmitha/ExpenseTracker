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
    public class GetUserAccessItemQuery : IRequest<UserAccessItemModel>
    {
        public int UserAccessItemId { get; set; }
        public GetUserAccessItemQuery(int userAccessItemId)
        {
            UserAccessItemId = userAccessItemId;
        }

        public class Handler : IRequestHandler<GetUserAccessItemQuery, UserAccessItemModel>
        {
            private readonly IFinancialService _financialService;
            private readonly IAccessTokenService _accessTokenService;

            public Handler(IFinancialService financialService, IAccessTokenService accessTokenService)
            {
                _financialService = financialService;
                _accessTokenService = accessTokenService;
            }

            public async Task<UserAccessItemModel> Handle(GetUserAccessItemQuery request, CancellationToken cancellationToken)
            {
                var accessToken = await _accessTokenService.GetAccessTokenAsync(request.UserAccessItemId);
                var institution = await _financialService.GetInstitutionAsync(accessToken.InstitutionId);
                try
                {
                    var accounts = await _financialService.GetAccountsAsync(accessToken.AccessToken);
                    var item = await _financialService.GetItemAsync(accessToken.AccessToken);
                    return new UserAccessItemModel
                    {
                        Accounts = accounts,
                        Institution = institution,
                        Item = item,
                        UserAccessItemId = accessToken.Id
                    };
                }
                catch (FinancialServiceException fex)
                {
                    if (string.Equals(fex.Error?.error_code, ErrorCodes.ITEM_LOGIN_REQUIRED,
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new UserAccessItemModel
                        {
                            Institution = institution,
                            ExpiredAccessItem = new ExpiredAccessItem(accessToken.AccessToken, fex.Error.error_message, institution),
                            UserAccessItemId = accessToken.Id
                        };
                    }
                }
                return new UserAccessItemModel
                {
                    Institution = institution,
                    UserAccessItemId = accessToken.Id
                };
            }
        }
    }
}
