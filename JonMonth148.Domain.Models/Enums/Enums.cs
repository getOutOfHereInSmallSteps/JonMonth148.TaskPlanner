using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonMonth148.Domain.Models.Enums
{
    public enum Priority
    {
        None,
        Low,
        Medium,
        High,
        Urgent,
    }

    public enum Complexity
    {
        None,
        Minutes, 
        Hours, 
        Days, 
        Weeks,
    }
}
