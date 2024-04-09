using RepositoryOperations.ApplicationModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class SqlQueryHelper
    {
        public static string GenerateQueryFromEntity(object model, RequestModel request)
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
            sql.Append($" WITH(NOLOCK) ");

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
                    multiSearchList.Add($@"{item.ColumnName} LIKE'%{item.ColumnValue}%'");
                }
                sql.AppendJoin(" AND ", multiSearchList);
            }
            if (request != null && request.DateRangeFilter != null && request.DateRangeFilter.FromDate.HasValue && request.DateRangeFilter.ToDate.HasValue)
            {
                if (!IsWhereIncluded)
                {
                    sql.Append($@" WHERE CONVERT(DATE, {request.DateRangeFilter.SearchInColumn}) BETWEEN '{request.DateRangeFilter.FromDate.Value.ToString("yyyy-MM-dd")}' AND '{request.DateRangeFilter.ToDate.Value.ToString("yyyy-MM-dd")}'");
                    IsWhereIncluded = true;
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

        public static string GenerateQueryForTotalRecordsFound(object model, RequestModel request)
        {
            StringBuilder sql = new StringBuilder();
            List<string> searchlist = new List<string>();
            List<string> multiSearchList = new List<string>();
            bool IsWhereIncluded = false;
            sql.Append("SELECT ");
            sql.Append($"COUNT(1) FROM {model.GetType().Name}");
            sql.Append($" WITH(NOLOCK) ");

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
                    multiSearchList.Add($@"{item.ColumnName} LIKE'%{item.ColumnValue}%'");
                }
                sql.AppendJoin(" AND ", multiSearchList);
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
            return sql.ToString();
        }
    }
}
