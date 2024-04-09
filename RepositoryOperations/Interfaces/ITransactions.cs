using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryOperations.Interfaces
{
    /// <summary>
    /// Interface for using Database realted Transaction operations like 
    /// Begin Transaction
    /// End Transaction
    /// Roll-Back Transaction
    /// </summary>
    public interface ITransactions
    {
        /// <summary>
        /// Begins the transaction for SQL Connection specified in DBConext class.
        /// </summary>
        IDbTransaction BeginTransaction();

        /// <summary>
        /// Ends the transaction for SQL Connection specified in DBConext class.
        /// </summary>
        void EndTransaction();

        /// <summary>
        /// Roll-Back the transaction for SQL Connection specified in DBConext class.
        /// </summary>
        void RollBack();
    }
}
