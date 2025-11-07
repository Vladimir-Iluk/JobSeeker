using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dal.Interfaces
{
    public interface ITransactionalRepository
    {
        IDbTransaction? Transaction { get; set; }
    }
}
