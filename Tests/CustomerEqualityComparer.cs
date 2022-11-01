using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BAL.Entity;
using BAL.Entity.Auth;

namespace Tests
{
    internal class CategoryEqualityComparer : IEqualityComparer<Category>
    {
        public bool Equals([AllowNull] Category x, [AllowNull] Category y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.CategoryId == y.CategoryId
                && x.CategoryName == y.CategoryName;
        }

        public int GetHashCode([DisallowNull] Category obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class FileEqualityComparer : IEqualityComparer<Files>
    {
        public bool Equals([AllowNull] Files x, [AllowNull] Files y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.FileId == y.FileId
                && x.CategoryId == y.CategoryId
                && x.ContentType == y.ContentType
                && x.Date == y.Date
                && x.UserId == y.UserId
                && x.Description == y.Description
                && x.FileName == y.FileName
                && x.Title == y.Title
                && x.FilePath == y.FilePath;
        }

        public int GetHashCode([DisallowNull] Files obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.UserName == y.UserName;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }
}
