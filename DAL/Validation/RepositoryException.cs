using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Validation
{
    [Serializable]
    public class RepositoryExcpetion : Exception
    {
        public RepositoryExcpetion()
        {
        }

        public RepositoryExcpetion(string message)
            : base(message)
        {
        }

        public RepositoryExcpetion(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
