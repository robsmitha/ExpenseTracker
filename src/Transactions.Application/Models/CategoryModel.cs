using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Entities;

namespace Transactions.Application.Models
{
    public class CategoryModel : IMapFrom<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public bool IsExisting => Id > 0;
    }
}
