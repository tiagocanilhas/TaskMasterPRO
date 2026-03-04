using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskMasterPRO.Data.Domain
{
    public class CategoryDomain
    {
        public bool IsNameValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public bool IsColorValid(string color)
        {
            return Regex.IsMatch(color, "^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$"); // Example: #FFFFFF or #FFF
        }
    }
}
