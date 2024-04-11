using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface IImagesMaster
    {
        Task<ApiGetResponseModel<List<ImagesVM>>> GetImageList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<ImagesVM>> GetImageById(long imageId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveImage(ImagesVM image, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateImage(ImagesVM image, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteImage(long imageId, IDbTransaction transaction = null);
    }
}
