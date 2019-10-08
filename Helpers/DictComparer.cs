using System.Collections.Generic;
using NetProductivity.Models;

namespace NetProductivity.Helpers
{
    public class DictComparer<T> : IComparer<T> where T:TaskP
    {
        public int Compare(T x, T y)
        {
            if (x.Priority > y.Priority)
            {
                return 1;
            }

            if (x.Priority < y.Priority)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
