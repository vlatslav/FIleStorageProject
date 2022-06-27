using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Validation
{
    public class AccessException : Exception
    {
        public AccessException()
        {
        }

        public AccessException(string message)
            : base(message)
        {
        }

        public AccessException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
