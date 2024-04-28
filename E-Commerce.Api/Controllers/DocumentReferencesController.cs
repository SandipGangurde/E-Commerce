using DataCarrier.ApplicationModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Business.Contract;
using DataCarrier.ViewModels;
using DataCarrier.ApplicationModels.DocumentReference.Request;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentReferencesController : ControllerBase
    {
        private readonly IDocumentReferencesMaster _documentReferencesMaster;
        public DocumentReferencesController(IDocumentReferencesMaster documentReferencesMaster)
        {
            _documentReferencesMaster = documentReferencesMaster;
        }

        [HttpPost("getDocumentReferenceList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<DocumentReferencesVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<VuDocumentReferences>>> GetAllDocumentReferenceList([FromBody] ApiGetRequestModel request)
        {
            ApiGetResponseModel<List<VuDocumentReferences>> response = new ApiGetResponseModel<List<VuDocumentReferences>>();
            response.Result = new List<VuDocumentReferences>();
            try
            {
                response = await _documentReferencesMaster.GetDocumentReferenceList(request);
            }
            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Result = default;
                response.TotalRecords = 0;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        [HttpPost("getDocumentReferenceById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<DocumentReferencesVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<DocumentReferencesVM>> GetDocumentReferenceById([FromBody] GetByIdVM request)
        {
            ApiGenericResponseModel<DocumentReferencesVM> response = new ApiGenericResponseModel<DocumentReferencesVM>();
            response.Result = new DocumentReferencesVM();
            try
            {
                response = await _documentReferencesMaster.GetDocumentReferenceById(request.Id);
            }
            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Result = default;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        [HttpPost("saveDocumentReference")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveDocumentReference([FromBody] DocumentReferencesVM data)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            try
            {
                response = await _documentReferencesMaster.SaveDocumentReference(data, transaction: null);
            }
            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Result = default;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        [HttpPost("updateDocumentReference")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateDocumentReference([FromBody] DocumentReferencesVM data)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                response = await _documentReferencesMaster.UpdateDocumentReference(data, transaction: null);
            }
            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Result = default;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        [HttpPost("deleteDocumentReference")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> DeleteDocumentReference([FromBody] DeleteDocumentReferenceRequestModel Data)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                response = await _documentReferencesMaster.DeleteDocumentReference(Data.TableName, Data.PrimaryKeyValue, Data.DocumentID);
            }
            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Result = default;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }
    }
}
