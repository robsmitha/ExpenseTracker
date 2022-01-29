using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Job.Interfaces
{
    public interface IRuntimeService
    {
        Task StartAsync();
        void Stop();
    }
}
