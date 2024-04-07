using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLA.Tasks.Api.Domain.Utils;

namespace TLA.Tasks.Api.Domain.Extension
{
    public static class PaginationExtension
    {
        public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var count = await query.CountAsync();

            if (page <= 0) page = 1;

            var result = await query
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(result, count, pageSize, page);
        }
    }
}
