using System;
using System.Collections.Generic;

namespace TLA.Tasks.Api.Domain.Utils
{
    public class PagedList<T>
    {
        public IList<T> Items { get; }

        public int TotalItems { get; }
        public int PageCount { get; }
        public int PageSize { get; }
        public int CurrentPage { get; }

        protected PagedList() { }

        public PagedList(IList<T> items, int totalItems, int pageSize, int currentPage)
        {
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            TotalItems = totalItems;
            PageSize = pageSize;
            Items = items;
            CurrentPage = currentPage;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalItems / (double)PageSize));
        }
    }
}
