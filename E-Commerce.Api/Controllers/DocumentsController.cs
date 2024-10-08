﻿using DataCarrier.ApplicationModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Business.Contract;
using DataCarrier.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsMaster _documentmaster;

        public DocumentsController(IDocumentsMaster documentmaster)
        {
            _documentmaster = documentmaster;
        }

        [HttpPost("getDocumentList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<DocumentsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<DocumentsVM>>> GetDocumentList([FromBody] ApiGetRequestModel request)
        {
            return await _documentmaster.GetDocumentList(request);
        }

        [HttpPost("getDocumentById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<DocumentsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<DocumentsVM>> GetDocumentById([FromBody] GetByIdVM request)
        {
            return await _documentmaster.GetDocumentById(request.Id);
        }

        [HttpPost("saveDocument")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveDocument([FromBody] DocumentsVM data)
        {
            return await _documentmaster.SaveDocument(data, transaction: null);
        }

        [HttpPost("updateDocument")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateDocument([FromBody] DocumentsVM data)
        {
            return await _documentmaster.UpdateDocument(data, transaction: null);
        }

        [HttpPost("deleteDocument")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> DeleteDocument([FromBody] GetByIdVM request)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                response = await _documentmaster.DeleteDocument(request.Id);
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
