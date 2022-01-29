using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Domain.Responses
{
    public class ItemResponse
    {
        public Item item { get; set; }
        public string request_id { get; set; }
        public Status status { get; set; }

        public class Item
        {
            public List<string> available_products { get; set; }
            public List<string> billed_products { get; set; }
            public object consent_expiration_time { get; set; }
            public object error { get; set; }
            public string institution_id { get; set; }
            public string item_id { get; set; }
            public List<string> products { get; set; }
            public string update_type { get; set; }
            public string webhook { get; set; }
        }

        public class Transactions
        {
            public DateTime last_failed_update { get; set; }
            public DateTime last_successful_update { get; set; }
        }

        public class Status
        {
            public object last_webhook { get; set; }
            public Transactions transactions { get; set; }
        }
    }
}
