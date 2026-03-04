using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMasterPRO.Data.Domain
{
    public class TaskDomain
    {

        public bool IsTitleValid(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        public bool IsDeadlineValid(DateTime deadline)
        {
            return deadline > DateTime.Now;
        }
    }
}
