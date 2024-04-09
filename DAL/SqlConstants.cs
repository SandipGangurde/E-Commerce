using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class SqlConstants
    {
        public const string SP_CategoriesByCategoryCode = "SP_CountryByCountryCode";


        public const string SP_CategoriesByCategoryId = "SP_CategoriesByCategoryId";


        public const string SP_DocumentByDocumentId = "SP_DocumentByDocumentId";
        public const string SP_DateRangeFilter = "SP_DateRangeFilter";
        public const string SP_DocumentByPrimaryKey = "SP_DocumentByPrimaryKey";
        public const string SP_DocumentReferenceByPrimaryKey = "SP_DocumentReferenceByPrimaryKey";
        public const string SP_DocumentReferenceByDocumentReferenceID = "SP_DocumentReferenceByDocumentReferenceID";
        public const string SP_DocumentReferenceByDocumentID = "SP_DocumentReferenceByDocumentID";
        public const string SP_DocumentReferenceByDocumentReferenceByPrimary = "SP_DocumentReferenceByDocumentReferenceByPrimary";
        public const string SP_DeleteDocumentReference = "SP_DeleteDocumentReference";
        public const string SP_DeleteDocument = "SP_DeleteDocument";

    }
}
