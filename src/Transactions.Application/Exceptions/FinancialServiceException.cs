using System;

namespace Transactions.Application.Exceptions
{
    public class FinancialServiceException : Exception
    {
        public FinancialServiceException(string message) : base(message)
        {

        }
    }
}
