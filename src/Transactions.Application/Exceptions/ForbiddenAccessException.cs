using System;
using System.Collections.Generic;
using System.Text;

namespace Transactions.Application.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base() { }
    }
}
