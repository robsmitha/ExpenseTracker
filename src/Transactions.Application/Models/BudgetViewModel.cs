using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Application.Models
{
    public class TransactionCategoryData
    {
        public string Category { get; set; }
        public decimal Sum { get; set; } = 0;
        public decimal Estimate { get; set; } = 0;
    }

    public class BudgetViewModel
    {
        public List<TransactionCategoryData> BudgetCategoryData { get; set; }
        public string BudgetName { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public List<ExpiredAccessItem> ExpiredAccessItems { get; set; }

        public bool HasExpiredAccessItems => ExpiredAccessItems?.Any() == true;
        public string ExpiredMessage => ExpiredAccessItems?.FirstOrDefault()?.Message;
        public decimal TransactionsTotal { get; set; }
        public BudgetViewModel(List<TransactionCategoryData> budgetCategoryData, string budgetName, DateTime startDate, DateTime endDate, 
            List<ExpiredAccessItem> expiredAccessItems, decimal transactionsTotal)
        {
            BudgetCategoryData = budgetCategoryData;
            BudgetName = budgetName;
            StartDate = startDate;
            EndDate = endDate;
            ExpiredAccessItems = expiredAccessItems;
            TransactionsTotal = transactionsTotal;
        }

        public string DateRange => $"{StartDate:MMMM} {StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy}";
    }
}
