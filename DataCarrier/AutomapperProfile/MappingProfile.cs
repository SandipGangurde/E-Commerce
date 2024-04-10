using AutoMapper;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.ApplicationModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.AutomapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*Entities to View Model Mapping*/
            CreateMap<ApiGetRequestModel, RequestModel>();
            CreateMap<ApplicationModels.Common.MultiSearchInColumn, RepositoryOperations.ApplicationModels.Common.MultiSearchInColumn>();
            CreateMap<ApplicationModels.Common.DateRangeFilter, RepositoryOperations.ApplicationModels.Common.DateRangeFilter>();
            CreateMap<Categories, CategoriesVM>();
            CreateMap<Documents, DocumentsVM>();
            CreateMap<DocumentReferences, DocumentReferencesVM>();

            /*View Model to Entities Mapping*/
            CreateMap<RequestModel, ApiGetRequestModel>();
            CreateMap<RepositoryOperations.ApplicationModels.Common.MultiSearchInColumn, ApplicationModels.Common.MultiSearchInColumn>();
            CreateMap<RepositoryOperations.ApplicationModels.Common.DateRangeFilter, ApplicationModels.Common.DateRangeFilter>();
            CreateMap<CategoriesVM, Categories>();
            CreateMap<DocumentsVM, Documents>();
            CreateMap<DocumentReferencesVM, DocumentReferences>();

            /*View Model to View Model*/
            //CreateMap<PortCallIssuedPdaAndWorkFlowVM, ExportPdfDocumentVM>();

        }
    }
}
