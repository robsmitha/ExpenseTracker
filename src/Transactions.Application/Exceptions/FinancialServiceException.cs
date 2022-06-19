using System;
using Transactions.Domain.Responses;

namespace Transactions.Application.Exceptions
{
    public class FinancialServiceException : Exception
    {
        public Error Error { get; set;}
        public FinancialServiceException(string message) : base(message)
        {
            Error = new Error();
        }

        public FinancialServiceException(Error error) : base(error.error_message)
        {
            Error = error ?? new Error();
        }
    }
}
