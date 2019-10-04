using System;
using System.Collections.Generic;
using System.Linq;

namespace FeedbackAPI.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalRecordCount { get; private set; }

        public bool HasPreviousPage
        {
            get { return CurrentPage > 1; }
        }

        public bool HasNextPage
        {
            get { return CurrentPage < TotalPages; }
        }

        PagedList(List<T> items, int recordCount, int currentPage, int pageSize)
        {
            TotalRecordCount = recordCount;
            PageSize = pageSize;
            CurrentPage = currentPage;

            TotalPages = (int)Math.Ceiling(TotalRecordCount / (double)PageSize);
            AddRange(items);
        }

        public static PagedList<T> Create<U>(IOrderedQueryable<U> source, int currentPage, int pageSize, Func<U, T> Convert)
        {
            int recordCount = source.Count();
            var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var convertedItems = items.Select(Convert).ToList();
            return new PagedList<T>(convertedItems, recordCount, currentPage, pageSize);
        }
    }
}
