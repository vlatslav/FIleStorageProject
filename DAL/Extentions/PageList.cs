using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extentions
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPage;

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int) Math.Ceiling(count / (double) pageSize);
            AddRange(items);
        }

        public static PageList<T> ToPagedList(List<T> sourse, int pageNumber, int pageSize)
        {
            var count = sourse.Count();
            var items =  sourse.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PageList<T>(items,count,pageNumber,pageSize);
        }
    }
}
