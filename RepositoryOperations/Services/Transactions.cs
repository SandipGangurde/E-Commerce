using RepositoryOperations.Contexts;
using RepositoryOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryOperations.Services
{
    /// <summary>
    /// Transaction Service implementing ITransaction interface for using Database realted Transaction operations like 
    /// Begin Transaction
    /// End Transaction
    /// Roll-Back Transaction
    /// </summary>
    public class Transactions : ITransactions
    {
        private readonly DBContext _dbcontext;
        private IDbTransaction _transaction;

        /// <summary>
        /// Constructor for Transaction Service.
        /// </summary>
        /// <param name="logger">Will be inject by IoC Container</param>
        /// <param name="dbcontext">will be inject by IoC Container</param>
        public Transactions(DBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        /// <summary>
        /// Begins the transaction for SQL Connection specified in DBConext class.
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            OpenDbConnection();
            _transaction = _dbcontext.Connection.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// Ends the transaction for SQL Connection specified in DBConext class.
        /// </summary>
        public void EndTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
            CloseDbConnection();
        }

        /// <summary>
        /// Roll-Back the transaction for SQL Connection specified in DBConext class.
        /// </summary>
        public void RollBack()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            CloseDbConnection();
        }

        private void OpenDbConnection()
        {
            CloseDbConnection();
            _dbcontext.Connection.Open();
        }

        private void CloseDbConnection()
        {
            _dbcontext.Connection.Close();
        }
    }
}
