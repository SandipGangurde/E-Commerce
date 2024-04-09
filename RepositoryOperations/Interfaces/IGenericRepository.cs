using RepositoryOperations.ApplicationModels.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryOperations.Interfaces
{
    /// <summary>
    /// Generic Model created for Database Table.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Inserts a record in the database for generic models.
        /// </summary>
        /// <param name="model">Contains information that needs to be instered.</param>
        /// <returns>Primary Key value of the inserted record.</returns>
        Task<long> Insert(TEntity model, IDbTransaction transaction = null);

        /// <summary>
        /// Inserts multiple records at a time using generic list model.
        /// </summary>
        /// <param name="model">Accepts list of generic model having records to insert.</param>
        /// <returns></returns>
        Task<int> BulkInsert(List<TEntity> model, IDbTransaction transaction = null);

        /// <summary>
        /// Updates a record in the database for generic models.
        /// </summary>
        /// <param name="model">Contains information that needs to be updated.</param>
        /// <returns>Boolean value. If true then record updated successfully else update failed.</returns>
        Task<bool> Update(TEntity model, IDbTransaction transaction = null);

        /// <summary>
        /// Deletes a record from the database for generic models.
        /// </summary>
        /// <param name="model">Contains information that needs to be deleted.</param>
        /// <returns>Boolean value. If true then record deleted successfully else delete failed.</returns>
        Task<bool> Delete(TEntity model, IDbTransaction transaction = null);

        /// <summary>
        /// Query database for the provided entity model.
        /// </summary>
        /// <param name="table">Accepts Entity object.</param>
        /// <param name="request">Accepts Request Model class object to prepare filter for the Entity object provided.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> Query(TEntity table, RequestModel request, IDbTransaction transaction = null);

        /// <summary>
        /// Query database using custom sql query.
        /// </summary>
        /// <param name="SQL">Accepts custom sql query</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> Query(string SQL, IDbTransaction transaction = null);

        /// <summary>
        /// Query database using Stored Procedure.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <param name="SPparam">Accepts parameters for the stored procedure (Required)</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QuerySP(string SPName, object SPparam, IDbTransaction transaction = null);

        /// <summary>
        /// Query database using Stored Procedure. This is to be used when Stored Procedure does not accepts any parameter.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QuerySP(string SPName, IDbTransaction transaction = null);

        /// <summary>
        /// Executes custom DML operations or Commands on database.
        /// </summary>
        /// <param name="SQL">Accepts custom sql query</param>
        /// <returns></returns>
        Task<int> Execute(string SQL, IDbTransaction transaction = null);

        /// <summary>
        /// Executes Stored Procedure on database.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <param name="SPparam">Accepts parameters for the stored procedure (Required)</param>
        /// <returns></returns>
        Task<int> ExecuteSP(string SPName, object SPparam, IDbTransaction transaction = null);

        /// <summary>
        /// Executes Stored Procedure on database. This is to be used when Stored Procedure does not accepts any parameter.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <returns></returns>
        Task<int> ExecuteSP(string SPName, IDbTransaction transaction = null);

        /// <summary>
        /// Executes Scalar response type Stored Procedure on database. This is to be used when Stored Procedure does not accepts any parameter.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <returns></returns>
        Task<object> ExecuteScalarSP(string SPName, object SPparam, IDbTransaction transaction = null);

        /// <summary>
        /// Query database using Stored Procedure with dynamic results.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <param name="SPparam">Accepts parameters for the stored procedure (Required)</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QuerySPDynamicResult(string SPName, object SPparam, IDbTransaction transaction = null);

        /// <summary>
        /// Query database to get dynamic results.
        /// </summary>
        /// <param name="SQL">Accepts string SQL query(Required)</param>
        /// <returns>dynamic</returns>
        Task<IEnumerable<dynamic>> QueryDynamicResult(string SQL, IDbTransaction transaction = null);

        /// <summary>
        /// Dynamic Scalar Query to database to get dynamic results.
        /// </summary>
        /// <param name="SQL">Accepts string SQL query(Required)</param>
        /// <returns>dynamic</returns>
        Task<dynamic> ScalarDynamicResult(string SQL, IDbTransaction transaction = null);
    }
}
