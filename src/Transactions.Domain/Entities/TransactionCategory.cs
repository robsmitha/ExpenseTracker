﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Domain.Entities
{
    public class TransactionCategory
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public int BudgetId { get; set; }
        [ForeignKey(nameof(BudgetId))]
        public Budget Budget { get; set; }
    }
}
