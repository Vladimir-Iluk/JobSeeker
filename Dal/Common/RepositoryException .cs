using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Common
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message, Exception? inner = null) : base(message, inner) { }
    }
}
