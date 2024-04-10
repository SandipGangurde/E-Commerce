using Dapper;
using Dapper.Contrib.Extensions;
using RepositoryOperations.ApplicationModels.Common;
using RepositoryOperations.Contexts;
using RepositoryOperations.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryOperations.Services
{
    /// <summary>
    /// Generic Model created for Database Table.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DBContext _dbcontext;
        private readonly ILogger _logger;
        private readonly ITransactions _localTransaction;

        /// <summary>
        /// Constructor for Generic Repository Service.
        /// </summary>
        /// <param name="logger">Will be inject by IoC Container</param>
        /// <param name="dbcontext">will be inject by IoC Container</param>
        public GenericRepository(ILogger logger, DBContext dbcontext, ITransactions transactions)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _localTransaction = transactions;
        }

        /// <summary>
        /// Inserts a record in the database for generic models.
        /// </summary>
        /// <param name="model">Contains information that needs to be instered.</param>
        /// <returns>Primary Key value of the inserted record.</returns>
        public async Task<long> Insert(TEntity model, IDbTransaction transaction = null)
        {
            ValidateInputModel(model);

            long response = 0;
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response = await _dbcontext.Connection.InsertAsync(model, localtran);
                if (transaction == null && localtran != null)
                    localtran.Commit();

            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception.Message);
                throw;
            }
            return response;
        }

        /// <summary>
        /// Inserts multiple records at a time using generic list model.
        /// </summary>
        /// <param name="model">Accepts list of generic model having records to insert.</param>
        /// <returns></returns>
        public async Task<int> BulkInsert(List<TEntity> model, IDbTransaction transaction = null)
        {
            int response = 0;
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                if (model.Count > 0)
                {
                    TEntity obj = (TEntity)Activator.CreateInstance(typeof(TEntity));
                    string SQL = GenerateBulkInsertQuery(obj, model);
                    response = await _dbcontext.Connection.ExecuteAsync(SQL, transaction: localtran);

                    if (transaction == null && localtran != null)
                        localtran.Commit();
                }
            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception.Message);
                throw;
            }
            return response;
        }

        /// <summary>
        /// Updates a record in the database for generic models.
        /// </summary>
        /// <param name="model">Contains information that needs to be updated.</param>
        /// <returns>Boolean value. If true then record updated successfully else update failed.</returns>
        public async Task<bool> Update(TEntity model, IDbTransaction transaction = null)
        {
            ValidateInputModel(model);

            bool response = false;
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response = await _dbcontext.Connection.UpdateAsync(model, localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception.Message);
                throw;
            }

            return response;
        }

        /// <summary>
        /// Deletes a record from the database for generic models.
        /// </summary>
        /// <param name="model">Contains information that needs to be deleted.</param>
        /// <returns>Boolean value. If true then record deleted successfully else delete failed.</returns>
        public async Task<bool> Delete(TEntity model, IDbTransaction transaction = null)
        {
            ValidateInputModel(model);

            bool response = false;
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response = await _dbcontext.Connection.DeleteAsync(model, localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception.Message);
                throw;
            }
            return response;
        }

        /// <summary>
        /// Query database for the provided entity model.
        /// </summary>
        /// <param name="table">Accepts Entity object.</param>
        /// <param name="request">Accepts Request Model class object to prepare filter for the Entity object provided.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> Query(TEntity table, RequestModel request, IDbTransaction transaction = null)
        {
            ValidateInputModel(table);
            ValidateRequestModel(request);
            try
            {
                string SQL = GenerateQueryFromEntity(table, request);
                return await _dbcontext.Connection.QueryAsync<TEntity>(SQL);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Query database using custom sql query.
        /// </summary>
        /// <param name="SQL">Accepts custom sql query</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> Query(string SQL, IDbTransaction transaction = null)
        {
            ValidateStringSQL(SQL);
            try
            {
                return await _dbcontext.Connection.QueryAsync<TEntity>(SQL, transaction: transaction);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Query database using Stored Procedure.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <param name="SPparam">Accepts parameters for the stored procedure (Required)</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> QuerySP(string SPName, object SPparam, IDbTransaction transaction = null)
        {
            ValidateSPName(SPName);
            try
            {
                return await _dbcontext.Connection.QueryAsync<TEntity>(SPName, param: SPparam, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Query database using Stored Procedure. This is to be used when Stored Procedure does not accepts any parameter.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> QuerySP(string SPName, IDbTransaction transaction = null)
        {
            ValidateSPName(SPName);
            try
            {
                return await _dbcontext.Connection.QueryAsync<TEntity>(SPName, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes custom DML operations or Commands on database.
        /// </summary>
        /// <param name="SQL">Accepts custom sql query</param>
        /// <returns></returns>
        public async Task<int> Execute(string SQL, IDbTransaction transaction = null)
        {
            ValidateStringSQL(SQL);
            try
            {
                return await _dbcontext.Connection.ExecuteAsync(SQL, transaction: transaction);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes Stored Procedure on database.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <param name="SPparam">Accepts parameters for the stored procedure (Required)</param>
        /// <returns></returns>
        public async Task<int> ExecuteSP(string SPName, object SPparam, IDbTransaction transaction = null)
        {
            ValidateSPName(SPName);

            int response = 0;
            IDbTransaction localtran = null;
            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response = await _dbcontext.Connection.ExecuteAsync(SPName, SPparam, transaction: localtran, commandType: CommandType.StoredProcedure);
                if (transaction == null && localtran != null)
                    localtran.Commit();

            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception.Message);
                throw;
            }
            return response;
        }

        /// <summary>
        /// Executes Stored Procedure on database. This is to be used when Stored Procedure does not accepts any parameter.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <returns></returns>
        public async Task<int> ExecuteSP(string SPName, IDbTransaction transaction = null)
        {
            ValidateSPName(SPName);

            int response = 0;
            IDbTransaction localtran = null;
            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response = await _dbcontext.Connection.ExecuteAsync(SPName, transaction: localtran, commandType: CommandType.StoredProcedure);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception.Message);
                throw;
            }
            return response;
        }

        /// <summary>
        /// Executes Scalar response type Stored Procedure on database. This is to be used when Stored Procedure does not accepts any parameter.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarSP(string SPName, object SPparam, IDbTransaction transaction = null)
        {
            ValidateSPName(SPName);

            object response;
            IDbTransaction localtran = null;
            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response = await _dbcontext.Connection.ExecuteScalarAsync<object>(SPName, param: SPparam, transaction: localtran, commandType: CommandType.StoredProcedure);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception.Message);
                throw;
            }
            return response;
        }

        /// <summary>
        /// Query database using Stored Procedure with dynamic results.
        /// </summary>
        /// <param name="SPName">Accepts Name of the stored procedure (Required)</param>
        /// <param name="SPparam">Accepts parameters for the stored procedure (Required)</param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> QuerySPDynamicResult(string SPName, object SPparam, IDbTransaction transaction = null)
        {
            ValidateSPName(SPName);
            try
            {
                return await _dbcontext.Connection.QueryAsync(SPName, param: SPparam, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Query database to get dynamic results.
        /// </summary>
        /// <param name="SQL">Accepts string SQL query(Required)</param>
        /// <returns>dynamic</returns>
        public async Task<IEnumerable<dynamic>> QueryDynamicResult(string SQL, IDbTransaction transaction = null)
        {
            ValidateStringSQL(SQL);
            try
            {
                return await _dbcontext.Connection.QueryAsync(SQL, transaction: transaction);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Dynamic Scalar Query to database to get dynamic results.
        /// </summary>
        /// <param name="SQL">Accepts string SQL query(Required)</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> ScalarDynamicResult(string SQL, IDbTransaction transaction = null)
        {
            ValidateStringSQL(SQL);
            try
            {
                return await _dbcontext.Connection.ExecuteScalarAsync(SQL, transaction: transaction);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw;
            }
        }

        private void ValidateInputModel(TEntity model)
        {
            if (model == null)
            {
                _logger.Error("Parameter: model, No data provided for the operation.");
                throw new ArgumentNullException("model", "No data provided for the operation.");
            }
        }

        private void ValidateRequestModel(RequestModel request)
        {
            if (request == null)
            {
                _logger.Error("Parameter : request, Search parameter not provided.");
                throw new ArgumentNullException("request", "Search parameter not provided.");
            }
        }

        private void ValidateStringSQL(string SQL)
        {
            if (string.IsNullOrEmpty(SQL))
            {
                _logger.Error("Parameter : SQL, SQL query not provided.");
                throw new ArgumentNullException("SQL", "SQL query not provided.");
            }
        }

        private void ValidateSPName(string SPName)
        {
            if (string.IsNullOrEmpty(SPName))
            {
                _logger.Error("Parameter : SPName, Stored Procedure name not provided.");
                throw new ArgumentNullException("SPName", "Stored Procedure name not provided.");
            }
        }
        
        private string GenerateQueryFromEntity(TEntity model, RequestModel request)
        {
            StringBuilder sql = new StringBuilder();
            List<string> searchlist = new List<string>();
            List<string> multiSearchList = new List<string>();
            bool IsWhereIncluded = false;
            sql.Append("SELECT ");

            if (request != null && request.SelectColumns.Count > 0)
                sql.AppendJoin(",", request.SelectColumns);
            else
                sql.Append("*");

            sql.Append($" FROM {model.GetType().Name}");

            if (request != null && request.SearchInColumns.Count > 0 && !string.IsNullOrEmpty(request.SearchString))
            {
                sql.Append(" WHERE ");
                IsWhereIncluded = true;
                sql.Append(" ( ");
                foreach (var item in request.SearchInColumns)
                {
                    if (!string.IsNullOrEmpty(request.SearchType) && request.SearchType == "EQ")
                        searchlist.Add($@"{item} = '{request.SearchString}'");
                    else if (!string.IsNullOrEmpty(request.SearchType) && request.SearchType == "NOTEQ")
                        searchlist.Add($@"{item} <> '{request.SearchString}'");
                    else
                        searchlist.Add($@"{item} LIKE'%{request.SearchString}%'");
                }
                if (!string.IsNullOrEmpty(request.MultiSearchCondition) && request.MultiSearchCondition == "AND")
                    sql.AppendJoin(" AND ", searchlist);
                else
                    sql.AppendJoin(" OR ", searchlist);
                sql.Append(" ) ");
            }
            if (request != null && request.ListOfMultiSearchColumn.Count > 0)
            {
                if (!IsWhereIncluded)
                {
                    sql.Append(" WHERE ");
                    IsWhereIncluded = true;
                }
                else
                    sql.Append(" AND ");

                foreach (var item in request.ListOfMultiSearchColumn)
                {
                    if (!string.IsNullOrEmpty(request.SearchType) && request.SearchType == "EQ")
                        multiSearchList.Add($@"{item.ColumnName} = '{item.ColumnValue}'");
                    else if (!string.IsNullOrEmpty(request.SearchType) && request.SearchType == "NOTEQ")
                        multiSearchList.Add($@"{item.ColumnName} <> '{item.ColumnValue}'");
                    else
                        multiSearchList.Add($@"{item.ColumnName} LIKE '%{item.ColumnValue}%'");
                }
                sql.AppendJoin(" AND ", multiSearchList);
            }

            if (request != null && request.DateRangeFilter != null && request.DateRangeFilter.FromDate.HasValue && request.DateRangeFilter.ToDate.HasValue)
            {
                if (!IsWhereIncluded)
                {
                    sql.Append($@" WHERE CONVERT(DATE, {request.DateRangeFilter.SearchInColumn}) BETWEEN '{request.DateRangeFilter.FromDate.Value.ToString("yyyy-MM-dd")}' AND '{request.DateRangeFilter.ToDate.Value.ToString("yyyy-MM-dd")}'");
                }
                else
                    sql.Append($@" AND CONVERT(DATE, {request.DateRangeFilter.SearchInColumn}) BETWEEN '{request.DateRangeFilter.FromDate.Value.ToString("yyyy-MM-dd")}' AND '{request.DateRangeFilter.ToDate.Value.ToString("yyyy-MM-dd")}'");
            }

            if (request != null && request.Status)
            {
                if (!IsWhereIncluded)
                {
                    sql.Append(" WHERE Status = 1 ");
                }
                else
                    sql.Append(" AND Status = 1 ");
            }

            if (request != null && !string.IsNullOrEmpty(request.SortByColumn) && !string.IsNullOrEmpty(request.SortByColumnDirection))
                sql.Append($" ORDER BY {request.SortByColumn} {request.SortByColumnDirection}");
            else if (request != null && !string.IsNullOrEmpty(request.SortByColumn) && string.IsNullOrEmpty(request.SortByColumnDirection))
                sql.Append($" ORDER BY {request.SortByColumn} DESC");


            if (request != null && request.PageIndex.HasValue && request.PageSize.HasValue && request.PageIndex > 0 && request.PageSize > 0)
                sql.Append($" OFFSET {request.PageSize * (request.PageIndex - 1)} ROWS FETCH NEXT {request.PageSize} ROWS ONLY");

            return sql.ToString();
        }

        /*
        public string GenerateQueryFromEntity<T>(T model, RequestModel request, string Operator = "AND")
        {
            StringBuilder sql = new StringBuilder();
            bool IsWhereIncluded = false;
            string TableName = model.GetType().Name;
            List<string> SearchInColumns = null;
            List<MultiSearchInColumn> ListOfMultiSearchColumn = null;
            if (request != null && model != null)
            {
                var Columns = model.GetType().GetProperties().Select(p => p.Name.ToLower()).ToList();
                var ColumnsWithType = model.GetType().GetProperties().Select(p => new { p.Name, p.PropertyType }).ToList();
                if (request.SearchInColumns.Count > 0 && !string.IsNullOrEmpty(request.SearchString))
                {
                    foreach (var s in request.SearchInColumns.Where(x => Columns.Contains(x)))
                    {
                        if (SearchInColumns == null)
                            SearchInColumns = new List<string>();
                        SearchInColumns.Add(s);
                    }
                }
                if (request.ListOfMultiSearchColumn.Count > 0)
                {
                    foreach (var s in request.ListOfMultiSearchColumn.Where(x => Columns.Contains(x.ColumnName.ToLower())))
                    {
                        if (ListOfMultiSearchColumn == null)
                            ListOfMultiSearchColumn = new List<MultiSearchInColumn>();
                        ListOfMultiSearchColumn.Add(s);
                    }
                }
                sql.Append("SELECT ");
                if (request.SelectColumns.Count > 0)
                    sql.Append(string.Join(",", request.SelectColumns));
                else
                    sql.Append("*");
                sql.Append($" FROM {TableName}");
                if (SearchInColumns != null && SearchInColumns.Count > 0)
                {
                    sql.Append(" WHERE ");
                    IsWhereIncluded = true;
                    sql.Append(" ( ");
                    for (int i = 0; i < SearchInColumns.Count; i++)
                    {
                        var item = SearchInColumns[i];
                        if (i != SearchInColumns.Count - 1)
                        {
                            sql.Append($"{item} like ").Append($"%{request.SearchString}%").Append($" {Operator} ");
                        }
                        else
                        {
                            sql.Append($"{item} like ").Append($"%{request.SearchString}%");
                        }
                    }
                    sql.Append(" ) ");
                }
                if (ListOfMultiSearchColumn != null && ListOfMultiSearchColumn.Count > 0)
                {
                    if (!IsWhereIncluded)
                    {
                        sql.Append(" WHERE ");
                        IsWhereIncluded = true;
                    }
                    else
                        sql.Append($" {Operator} ");

                    for (int i = 0; i < ListOfMultiSearchColumn.Count; i++)
                    {

                        var item = ListOfMultiSearchColumn[i];
                        var type = ColumnsWithType.Where(cs => cs.Name.ToLower() == item.ColumnName.ToLower()).FirstOrDefault().PropertyType;
                        if (type == typeof(Int64) || type == typeof(Int32))
                        {
                            if (i != ListOfMultiSearchColumn.Count - 1)
                            {
                                sql.Append($"{item.ColumnName} = ").Append($"{item.ColumnValue}").Append($" {Operator} ");
                            }
                            else
                            {
                                sql.Append($"{item.ColumnName} = ").Append($"{item.ColumnValue}");
                            }
                        }
                        else if (type == typeof(DateTime))
                        {
                            if (i != ListOfMultiSearchColumn.Count - 1)
                            {

                                sql.Append($" DATEPART(yyyy, {item.ColumnName})= DATEPART(yyyy,").Append($"{item.ColumnValue}")
                                .Append($") AND DATEPART(dd, {item.ColumnName})= DATEPART(dd,").Append($"{item.ColumnValue}")
                                .Append($") AND DATEPART(mm, {item.ColumnName})= DATEPART(mm, ").Append($"{item.ColumnValue}").Append($") {Operator} ");
                            }
                            else
                            {
                                sql.Append($" DATEPART(yyyy, {item.ColumnName})= DATEPART(yyyy,").Append($"{item.ColumnValue}")
                                    .Append($") AND DATEPART(dd, {item.ColumnName})= DATEPART(dd,").Append($"{item.ColumnValue}")
                                    .Append($") AND DATEPART(mm, {item.ColumnName})= DATEPART(mm, ").Append($"{item.ColumnValue}").Append(")");
                            }
                        }
                        else
                        {
                            if (i != ListOfMultiSearchColumn.Count - 1)
                            {
                                sql.Append($"{item.ColumnName} like ").Append($"%{item.ColumnValue}%").Append($" {Operator} ");
                            }
                            else
                            {
                                sql.Append($"{item.ColumnName} like ").Append($"%{item.ColumnValue}%");
                            }
                        }
                    }
                }
                if (request.DateRangeFilter != null && request.DateRangeFilter.FromDate.HasValue
                    && request.DateRangeFilter.ToDate.HasValue && Columns.Contains(request.DateRangeFilter.SearchInColumn.ToLower()))
                {
                    if (!IsWhereIncluded)
                    {
                        sql.Append($" WHERE CONVERT(DATE, {request.DateRangeFilter.SearchInColumn}) BETWEEN ").Append($"{request.DateRangeFilter.FromDate.Value.ToString("yyyy-MM-dd")}").Append(" AND ").Append($"{request.DateRangeFilter.ToDate.Value.ToString("yyyy-MM-dd")}");
                        IsWhereIncluded = true;
                    }
                    else
                        sql.Append($" AND CONVERT(DATE, {request.DateRangeFilter.SearchInColumn}) BETWEEN ").Append($"{request.DateRangeFilter.FromDate.Value.ToString("yyyy-MM-dd")}").Append(" AND ").Append($"{request.DateRangeFilter.ToDate.Value.ToString("yyyy-MM-dd")}");
                }
                if (request.Status && Columns.Contains("Status"))
                {
                    if (!IsWhereIncluded)
                    {
                        sql.Append(" WHERE Status = 1 ");
                    }
                    else
                        sql.Append(" AND Status = 1 ");
                }
                if (!string.IsNullOrEmpty(request.SortByColumn) && Columns.Contains(request.SortByColumn.ToLower()) && !string.IsNullOrEmpty(request.SearchString))
                {
                    sql.Append($" ORDER BY {request.SortByColumn} DESC");
                }
                else if (!string.IsNullOrEmpty(request.SortByColumn) && Columns.Contains(request.SortByColumn.ToLower()))
                {
                    sql.Append($" ORDER BY {request.SortByColumn} DESC");
                    if (request.PageIndex.HasValue && request.PageSize.HasValue && request.PageIndex > 0 && request.PageSize > 0)
                        sql.Append($" OFFSET {request.PageSize * (request.PageIndex - 1)} ROWS FETCH NEXT {request.PageSize} ROWS ONLY");
                }
            }
            return sql.ToString();
        }
        */

        private static string GenerateBulkInsertQuery(TEntity model, List<TEntity> records)
        {
            StringBuilder sql = new StringBuilder();
            List<string> columns = new List<string>();

            var ArrpropertyInfo = model.GetType().GetProperties();
            foreach (var property in ArrpropertyInfo)
                columns.Add(property.Name);

            sql.Append($"INSERT INTO {model.GetType().Name} (");
            sql.AppendJoin(",", columns);
            sql.Append(") Values ");
            foreach (var item in records)
            {
                columns.Clear();
                var properties = item.GetType().GetProperties();
                sql.Append(Environment.NewLine);
                sql.Append("(");
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(int)
                        || property.PropertyType == typeof(long)
                        || property.PropertyType == typeof(decimal))
                    {
                        columns.Add($"{property.GetValue(item, null)}");
                    }

                    if (property.PropertyType == typeof(Nullable<int>)
                        || property.PropertyType == typeof(Nullable<long>)
                        || property.PropertyType == typeof(Nullable<decimal>))
                    {
                        var data = property.GetValue(item, null);
                        if (data == null)
                            columns.Add($"NULL");
                        else
                            columns.Add($"{property.GetValue(item, null)}");
                    }

                    if (property.PropertyType == typeof(string))
                    {
                        var data = property.GetValue(item, null);
                        if (data == null && string.IsNullOrEmpty(Convert.ToString(data)))
                            columns.Add($"NULL");
                        else
                            columns.Add($"'{property.GetValue(item, null)}'");
                    }


                    if (property.PropertyType == typeof(DateTime))
                        columns.Add($"'{property.GetValue(item, null)}'");

                    if (property.PropertyType == typeof(Nullable<DateTime>))
                    {
                        var data = property.GetValue(item, null);
                        if (data == null)
                            columns.Add($"NULL");
                        else
                            columns.Add($"'{property.GetValue(item, null)}'");
                    }

                    if (property.PropertyType == typeof(Boolean))
                    {
                        if (property.GetValue(item, null).ToString() == "True")
                            columns.Add($"1");

                        if (property.GetValue(item, null).ToString() == "False")
                            columns.Add($"0");
                    }

                    if (property.PropertyType == typeof(Nullable<Boolean>))
                    {
                        var data = property.GetValue(item, null);
                        if (data == null)
                            columns.Add($"NULL");
                        else
                        {
                            if (property.GetValue(item, null).ToString() == "True")
                                columns.Add($"1");

                            if (property.GetValue(item, null).ToString() == "False")
                                columns.Add($"0");
                        }
                    }
                }
                sql.AppendJoin(",", columns);
                sql.Append("),");

            }
            return sql.ToString().TrimEnd(',');
        }

        private static string GenerateInsertQuery(TEntity model)
        {
            StringBuilder sql = new StringBuilder();
            List<string> columns = new List<string>();

            var ArrpropertyInfo = model.GetType().GetProperties();
            foreach (var property in ArrpropertyInfo)
                columns.Add(property.Name);

            sql.Append($"INSERT INTO {model.GetType().Name} (");
            sql.AppendJoin(",", columns);
            sql.Append(")");
            sql.Append(Environment.NewLine);
            sql.Append("Values(");
            foreach (var property in ArrpropertyInfo)
            {
                if (property.PropertyType == typeof(int)
                    || property.PropertyType == typeof(long)
                    || property.PropertyType == typeof(decimal))
                {
                    columns.Add($"{property.GetValue(model, null)}");
                }

                if (property.PropertyType == typeof(Nullable<int>)
                    || property.PropertyType == typeof(Nullable<long>)
                    || property.PropertyType == typeof(Nullable<decimal>))
                {
                    var data = property.GetValue(model, null);
                    if (data == null)
                        columns.Add($"NULL");
                    else
                        columns.Add($"{property.GetValue(model, null)}");
                }

                if (property.PropertyType == typeof(string))
                {
                    var data = property.GetValue(model, null);
                    if (data == null && string.IsNullOrEmpty(Convert.ToString(data)))
                        columns.Add($"NULL");
                    else
                        columns.Add($"'{property.GetValue(model, null)}'");
                }


                if (property.PropertyType == typeof(DateTime))
                    columns.Add($"'{property.GetValue(model, null)}'");

                if (property.PropertyType == typeof(Nullable<DateTime>))
                {
                    var data = property.GetValue(model, null);
                    if (data == null)
                        columns.Add($"NULL");
                    else
                        columns.Add($"'{property.GetValue(model, null)}'");
                }

                if (property.PropertyType == typeof(Boolean))
                {
                    if (property.GetValue(model, null).ToString() == "True")
                        columns.Add($"1");

                    if (property.GetValue(model, null).ToString() == "False")
                        columns.Add($"0");
                }

                if (property.PropertyType == typeof(Nullable<Boolean>))
                {
                    var data = property.GetValue(model, null);
                    if (data == null)
                        columns.Add($"NULL");
                    else
                    {
                        if (property.GetValue(model, null).ToString() == "True")
                            columns.Add($"1");

                        if (property.GetValue(model, null).ToString() == "False")
                            columns.Add($"0");
                    }
                }
                sql.AppendJoin(",", columns);
                sql.Append("),");
            }
            return sql.ToString().TrimEnd(',');
        }
    }
}
