using DataCarrier.ApplicationModels.Common;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.DataContract.Contract
{
    public interface IImagesMasterRepository
    {
        Task<ApiGetResponseModel<List<Images>>> GetImageList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Images>> GetImageById(long imageId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveImage(Images image, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateImage(Images image, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteImage(long imageId, IDbTransaction transaction = null);
    }

}
