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
        public string BudgetName { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public List<TransactionCategoryData> BudgetCategoryData { get; set; }
        public List<TransactionModel> Transactions { get; set; }
        public List<TransactionModel> ExcludedTransactions { get; set; }
        public List<UserAccessItemModel> BudgetAccessItems { get; set; }
        public List<ExpiredAccessItem> ExpiredAccessItems { get; set; }

        public bool HasExpiredAccessItems => ExpiredAccessItems?.Any() == true;
        public string ExpiredMessage => ExpiredAccessItems?.FirstOrDefault()?.Message;
        public decimal TransactionsTotal => (decimal)(Transactions?.Sum(t => t.amount) ?? 0);

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
            
            Transactions = (from t in allTransactions
                            join c in transactionCategories on t.transaction_id equals c.TransactionId into tmpC
                            from c in tmpC.DefaultIfEmpty(new TransactionCategoryModel
                            {
                                CategoryId = -1,
                                CategoryName = "Uncategorized"
                            })
                            where !excludedTransactionIds.Contains(t.transaction_id)
                            select TransactionWithCategory(t, c)).ToList();

            ExcludedTransactions = allTransactions.Where(t => excludedTransactionIds.Contains(t.transaction_id)).ToList();

            budgetCategories.Insert(0, new BudgetCategoryModel
            {
                CategoryId = -1,
                CategoryName = "Uncategorized",
                Estimate = (decimal)Transactions.Where(t => t.Category.Id == -1).Sum(t => t.amount)
            });

            BudgetCategoryData = (from c in budgetCategories
                                  join t in Transactions on c.CategoryId equals t.Category.Id into tmpT
                                  from t in tmpT.DefaultIfEmpty(new TransactionModel
                                  {
                                      amount = 0
                                  })
                                  group t.amount by new { c.CategoryName, c.Estimate } into g
                                  select new TransactionCategoryData
                                  {
                                      Estimate = g.Key.Estimate,
                                      Category = g.Key.CategoryName,
                                      Sum = (decimal)g.Sum()
                                  }).ToList();
        }

        public string DateRange => $"{StartDate:MMMM} {StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy}";

        static TransactionModel TransactionWithCategory(TransactionModel t, TransactionCategoryModel c)
        {
            t.Category = new CategoryModel
            {
                Id = c.CategoryId,
                Name = c.CategoryName
            };
            return t;
        }
    }
}
