using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Exceptions
{
    public class BusinessConflictException : Exception
    {
        public BusinessConflictException(string message) : base(message) { }
    }
}
