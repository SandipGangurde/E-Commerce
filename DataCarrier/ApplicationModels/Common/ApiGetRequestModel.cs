using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ApplicationModels.Common
{
    public class ApiGetRequestModel
    {
        public ApiGetRequestModel()
        {
            SearchInColumns = new List<string>();
            SelectColumns = new List<string>();
            ListOfMultiSearchColumn = new List<MultiSearchInColumn>();
            DateRangeFilter = new DateRangeFilter();
        }
        public string SearchType { get; set; }
        public string MultiSearchCondition { get; set; }
        public string SearchString { get; set; }
        public List<string> SearchInColumns { get; set; }
        public List<string> SelectColumns { get; set; }
        public List<MultiSearchInColumn> ListOfMultiSearchColumn { get; set; }
        public string SortByColumn { get; set; }
        public string SortByColumnDirection { get; set; }
        public bool Status { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public DateRangeFilter DateRangeFilter { get; set; }
    }

    public class MultiSearchInColumn
    {
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
    }

    public class DateRangeFilter
    {
        public string SearchInColumn { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
