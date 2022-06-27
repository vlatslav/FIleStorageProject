using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Validation
{
    [Serializable]
    public class FileExcpetion : Exception
    {
        public FileExcpetion()
        {
        }

        public FileExcpetion(string message)
            : base(message)
        {
        }

        public FileExcpetion(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
