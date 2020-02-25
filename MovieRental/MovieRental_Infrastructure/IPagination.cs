using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRental_Infrastructure
{
    public interface IPagination<T> where T : class
    {
        IQueryable<T> GetPaginated(string filter, int initialPage, int pageSize, string order, out int totalRecords, out int recordsFiltered);
    }
}
