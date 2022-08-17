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
        public List<TransactionModel> Transactions { get; set; }
        public List<TransactionModel> ExcludedTransactions { get; set; }
        public List<UserAccessItemModel> BudgetAccessItems { get; set; }
        public string BudgetName { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public List<ExpiredAccessItem> ExpiredAccessItems { get; set; }

        public bool HasExpiredAccessItems => ExpiredAccessItems?.Any() == true;
        public string ExpiredMessage => ExpiredAccessItems?.FirstOrDefault()?.Message;
        public decimal TransactionsTotal { get; set; }

        public BudgetViewModel(string name, DateTime startDate, DateTime endDate,
            List<BudgetCategoryModel> budgetCategories,
            List<TransactionCategoryModel> transactionCategories,
            List<TransactionModel> allTransactions,
            List<UserAccessItemModel> budgetAccessItems,
            List<ExpiredAccessItem> expiredAccessItems,
            List<BudgetExcludedTransactionModel> budgetExcludedTransactions)
        {
            BudgetName = name;
            StartDate = startDate;
            EndDate = endDate;
            BudgetAccessItems = budgetAccessItems;
            ExpiredAccessItems = expiredAccessItems;

            var excludedTransactionIds = budgetExcludedTransactions.Select(e => e.TransactionId).ToHashSet();
            var excludedTransactions = allTransactions.Where(t => excludedTransactionIds.Contains(t.transaction_id)).ToList();
            var includedTransactions = allTransactions.Where(t => !excludedTransactionIds.Contains(t.transaction_id)).ToList();

            Transactions = includedTransactions;
            ExcludedTransactions = excludedTransactions;
            TransactionsTotal = (decimal)includedTransactions.Sum(t => t.amount);

            var transactionCategoryData = from t in includedTransactions
                                          join c in transactionCategories on t.transaction_id equals c.TransactionId into tmpC
                                          from c in tmpC.DefaultIfEmpty()
                                          group t.amount by c?.CategoryName ?? "Uncategorized"
                    into g
                                          select new
                                          {
                                              Category = g.Key,
                                              Sum = (decimal)g.Sum()
                                          };

            var budgetCategoryData = new List<TransactionCategoryData>();

            var uncategorizedCategory = transactionCategoryData.FirstOrDefault(g => g.Category == "Uncategorized");
            if (uncategorizedCategory != null)
            {
                budgetCategoryData.Add(new TransactionCategoryData
                {
                    Category = uncategorizedCategory.Category,
                    Sum = uncategorizedCategory.Sum,
                    Estimate = uncategorizedCategory.Sum
                });
            }

            foreach (var budgetCategory in budgetCategories)
            {
                var transactionData = transactionCategoryData.FirstOrDefault(c => c.Category == budgetCategory.CategoryName);
                budgetCategoryData.Add(new TransactionCategoryData
                {
                    Estimate = budgetCategory.Estimate,
                    Category = budgetCategory.CategoryName,
                    Sum = transactionData?.Sum ?? 0
                });
            }

            BudgetCategoryData = budgetCategoryData;
        }

        public string DateRange => $"{StartDate:MMMM} {StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy}";
    }
}
