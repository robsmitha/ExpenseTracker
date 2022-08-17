using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Entities;

namespace Transactions.Application.Models
{
    public class BudgetExcludedTransactionModel : IMapFrom<BudgetExcludedTransaction>
    {
        public string TransactionId { get; set; }
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
    }
}
