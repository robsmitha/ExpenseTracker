using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Application.Models
{
    public class TransactionCategoryData
    {
        public List<TransactionModel> Transactions { get; set; } = new();
        public double Sum { get; set; } = 0;
    }

    public class BudgetViewModel
    {
        public List<TransactionModel> Transactions { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public List<ExpiredAccessItem> ExpiredAccessItems { get; set; }

        public bool HasExpiredAccessItems => ExpiredAccessItems?.Any() == true;
        public string ExpiredMessage => ExpiredAccessItems?.FirstOrDefault()?.Message;
        public double TotalSum => Transactions?.Sum(t => t.amount) ?? 0;
        public BudgetViewModel(List<TransactionModel> transactions, DateTime startDate, DateTime endDate, List<ExpiredAccessItem> expiredAccessItems)
        {
            Transactions = transactions;
            StartDate = startDate;
            EndDate = endDate;
            ExpiredAccessItems = expiredAccessItems;
        }

        public Dictionary<string, TransactionCategoryData> TransactionsByCategory
        {
            get
            {
                var transactionsByCategory = new Dictionary<string, TransactionCategoryData>();
                foreach (var transaction in Transactions)
                {
                    var category = transaction.Category?.Name ?? "Uncategorized";
                    if (transactionsByCategory.TryGetValue(category, out var data))
                    {
                        data.Sum += transaction.amount;
                        data.Transactions.Add(transaction);

                        transactionsByCategory[category] = data;
                    }
                    else
                    {
                        transactionsByCategory.Add(category, new TransactionCategoryData
                        {
                            Sum = transaction.amount,
                            Transactions = new List<TransactionModel> { transaction }
                        });
                    } 
                }
                return transactionsByCategory;
            }
        }

        public string DateRange => $"{StartDate:MMMM} {StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy}";
    }
}
