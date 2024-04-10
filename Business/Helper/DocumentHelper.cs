using AutoMapper;
using DataCarrier.ApplicationModels.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Helper
{
    public class DocumentHelper
    {

        //    public static async Task<ApiGenericResponseModel<long>> SaveDocuments(string PrimaryKeyValue, string TableName, List<DocumentsVM> documents, IDocumentsMasterRepository documentRepo, IDocumentReferencesMasterRepository documentRefRepo, IMapper map, IConfiguration configuration, IDbTransaction localtran, string ResourceDirectory = "Documents")
        //    {
        //        List<DocumentReferences> documentref = new List<DocumentReferences>();
        //        try
        //        {
        //            ApiGenericResponseModel<string> response = new ApiGenericResponseModel<string>();
        //            String blobConnectionString = configuration.GetValue<String>("BlobConnectionString");
        //            String blobFloderName = configuration.GetValue<String>("BlobFloderName");

        //            foreach (var document in documents)
        //            {
        //                BlobData blobData = new BlobData();

        //                if (document.FileBaseData.Contains(","))
        //                {
        //                    document.FileBaseData = document.FileBaseData.Split(",")[1];
        //                }
        //                document.FileData = Convert.FromBase64String(document.FileBaseData);
        //                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(document.FileName).ToLower();
        //                if (document.FileExt == "vnd.openxmlformats-officedocument.wordprocessingml.document")
        //                    document.FileExt = System.IO.Path.GetExtension(document.FileName).ToLower();

        //                blobData.fileBaseData = document.FileBaseData;
        //                blobData.filename = fileName;
        //                blobData.blobConnectionString = blobConnectionString;
        //                blobData.blobFloderName = blobFloderName;
        //                response = await BlobStorageHelper.UploadFileToBlob(blobData);

        //                document.FilePath = response.Result;
        //                document.DocumentURL = response.Result;

        //            }
        //            List<Documents> mapImagemodel = map.Map<List<Documents>>(documents);

        //            foreach (Documents document in mapImagemodel)
        //            {
        //                //  document.DocumentURL = $"{configuration["AppSettings:DownloadUrl"]}Resources/Documents/{document.FileName}";
        //                List<Documents> _tempDoumentList = new List<Documents>();
        //                _tempDoumentList.Add(document);
        //                var DocumentResponse = await documentRepo.saveDocument(_tempDoumentList, transaction: localtran);
        //                if (DocumentResponse != null && DocumentResponse.IsSuccess && DocumentResponse.Result > 0)
        //                {
        //                    /*Preparing Documents Reference for Ports*/
        //                    documentref.Add(new DocumentReferences()
        //                    {
        //                        DocumentID = DocumentResponse.Result,
        //                        TableName = TableName,
        //                        PrimaryKeyValue = PrimaryKeyValue
        //                    });
        //                }
        //                _tempDoumentList.Clear();
        //            }
        //            /*Storing Documents Reference for Ports*/
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionDispatchInfo.Capture(ex).Throw();
        //        }
        //        return await documentRefRepo.saveDocumentReference(documentref, transaction: localtran);
        //    }

        //    public static async Task<ApiGenericResponseModel<List<string>>> SaveDocumentsWithUrl(string PrimaryKeyValue, string TableName, List<DocumentsVM> documents, IDocumentsMasterRepository documentRepo, IDocumentReferencesMasterRepository documentRefRepo, IMapper map, IConfiguration configuration, IDbTransaction localtran, string ResourceDirectory = "Documents")
        //    {
        //        ApiGenericResponseModel<List<string>> response = new ApiGenericResponseModel<List<string>>();

        //        ApiGenericResponseModel<string> Blobresponse = new ApiGenericResponseModel<string>();
        //        String blobConnectionString = configuration.GetValue<String>("BlobConnectionString");
        //        String blobFloderName = configuration.GetValue<String>("BlobFloderName");
        //        response.Result = new List<string>();
        //        try
        //        {
        //            foreach (var document in documents)
        //            {
        //                BlobData blobData = new BlobData();
        //                if (document.FileBaseData.Contains(","))
        //                {
        //                    document.FileBaseData = document.FileBaseData.Split(",")[1];
        //                }
        //                document.FileData = Convert.FromBase64String(document.FileBaseData);
        //                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(document.FileName).ToLower();
        //                blobData.fileBaseData = document.FileBaseData;
        //                blobData.filename = Guid.NewGuid().ToString() + fileName;
        //                blobData.blobConnectionString = blobConnectionString;
        //                blobData.blobFloderName = blobFloderName;
        //                Blobresponse = await BlobStorageHelper.UploadFileToBlob(blobData);

        //                document.FilePath = Blobresponse.Result;
        //                document.DocumentURL = Blobresponse.Result;
        //            }
        //            List<Documents> mapImagemodel = map.Map<List<Documents>>(documents);
        //            List<DocumentReferences> documentref = new List<DocumentReferences>();
        //            foreach (Documents document in mapImagemodel)
        //            {
        //                //  document.DocumentURL = $"{configuration["AppSettings:DownloadUrl"]}Resources/Documents/{document.FileName}";
        //                response.Result.Add(document.DocumentURL);
        //                List<Documents> _tempDoumentList = new List<Documents>();
        //                _tempDoumentList.Add(document);
        //                var DocumentResponse = await documentRepo.saveDocument(_tempDoumentList, transaction: localtran);
        //                if (DocumentResponse != null && DocumentResponse.IsSuccess && DocumentResponse.Result > 0)
        //                {
        //                    /*Preparing Documents Reference for Ports*/
        //                    documentref.Add(new DocumentReferences()
        //                    {
        //                        DocumentID = DocumentResponse.Result,
        //                        TableName = TableName,
        //                        PrimaryKeyValue = PrimaryKeyValue
        //                    });
        //                }
        //                _tempDoumentList.Clear();
        //            }
        //            /*Storing Documents Reference for Ports*/
        //            await documentRefRepo.saveDocumentReference(documentref, transaction: localtran);
        //            response.IsSuccess = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            response.IsSuccess = false;
        //            response.ErrorMessage.Add(ex.Message);
        //        }
        //        return response;
        //    }
        //    public static async Task<ApiGenericResponseModel<bool>> DeleteBlobFile(DocumentsVM document, IMapper map, IConfiguration configuration, IDbTransaction localtran)
        //    {
        //        ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
        //        try
        //        {
        //            String blobConnectionString = configuration.GetValue<String>("BlobConnectionString");
        //            String blobFloderName = configuration.GetValue<String>("BlobFloderName");
        //            BlobData blobData = new BlobData();
        //            Uri u = new Uri(document.FilePath);

        //            string fileName = Path.GetFileName(u.AbsolutePath);


        //            blobData.filename = fileName;
        //            blobData.blobConnectionString = blobConnectionString;
        //            blobData.blobFloderName = blobFloderName;
        //            response = await BlobStorageHelper.DeleteBlobFile(blobData);

        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionDispatchInfo.Capture(ex).Throw();
        //        }
        //        return response;

        //    }
        //    public static async Task<ApiGenericResponseModel<long>> SaveUpdateAttachedDocuments(string PrimaryKeyValue, string TableName, List<DocumentsVM> Attachments, IDocumentsMasterRepository documentRepo, IDocumentReferencesMasterRepository documentRefRepo, IMapper map, IConfiguration configuration, IDbTransaction localtran, string ResourceDirectory = "Documents")
        //    {
        //        ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
        //        response.Result = new long();
        //        try
        //        {
        //            response.IsSuccess = true;
        //            /* Delete & Edit Attachment*/
        //            ApiGetResponseModel<List<Documents>> documentRespose = await documentRepo.getDocumentsListByPrimaryKey(Convert.ToInt16(PrimaryKeyValue), TableName, localtran);
        //            if (Attachments != null && Attachments.Count > 0)
        //            {
        //                if (documentRespose != null && documentRespose.IsSuccess && documentRespose.Result != null && documentRespose.Result.Count > 0)
        //                {
        //                    List<Documents> updateDocumentObj = new List<Documents>();
        //                    foreach (var Id in documentRespose.Result.Select(x => x.DocumentID))
        //                    {
        //                        var exitsDocuments = Attachments.FirstOrDefault(x => x.DocumentID == Id);
        //                        if (exitsDocuments != null)
        //                        {
        //                            var newUpdateObj = map.Map<Documents>(exitsDocuments);
        //                            var filedata = await documentRepo.getDocumentById(Id, transaction: localtran);
        //                            newUpdateObj.FileData = filedata.Result.FileData;
        //                            updateDocumentObj.Add(newUpdateObj);
        //                        }
        //                        else
        //                        {
        //                            var delDocRefStatus = await documentRefRepo.deleteDocumentReference(TableName, Convert.ToString(PrimaryKeyValue), Id, transaction: localtran);
        //                            if (delDocRefStatus != null && delDocRefStatus.IsSuccess && delDocRefStatus.Result)
        //                            {
        //                                await documentRepo.deleteDocument(Id, transaction: localtran);
        //                            }
        //                        }

        //                    }

        //                    if (updateDocumentObj != null && updateDocumentObj.Count > 0)
        //                    {
        //                        await documentRepo.updateDocument(updateDocumentObj, transaction: localtran);
        //                    }
        //                }

        //                /* Save New Attachment*/
        //                List<DocumentsVM> documentObj = new List<DocumentsVM>();
        //                foreach (var ele in Attachments)
        //                {
        //                    if (ele.DocumentID == 0)
        //                    {
        //                        documentObj.Add(ele);
        //                    }
        //                }
        //                if (documentObj != null && documentObj.Count > 0)
        //                {
        //                    foreach (var attachment in documentObj)
        //                    {
        //                        if (attachment.DocumentID > 0 && string.IsNullOrEmpty(attachment.FileBaseData))
        //                        {
        //                            var filedata = await documentRepo.getDocumentById(attachment.DocumentID, transaction: localtran);
        //                            Byte[] bytes = filedata.Result.FileData;
        //                            attachment.FileBaseData = Convert.ToBase64String(bytes);
        //                            attachment.DocumentID = 0;
        //                        }
        //                    }
        //                    response = await DocumentHelper.SaveDocuments(PrimaryKeyValue, TableName, documentObj, documentRepo, documentRefRepo, map, configuration, localtran);
        //                }

        //            }
        //            else
        //            {
        //                /* Remove All Attachment */
        //                if (documentRespose != null && documentRespose.IsSuccess && documentRespose.Result != null && documentRespose.Result.Count > 0)
        //                {
        //                    for (int i = 0; i < documentRespose.Result.Count; i++)
        //                    {
        //                        var delDocRefStatus = await documentRefRepo.deleteDocumentReference(TableName, PrimaryKeyValue, documentRespose.Result[i].DocumentID, localtran);
        //                        if (delDocRefStatus != null && delDocRefStatus.IsSuccess && delDocRefStatus.Result)
        //                        {
        //                            await documentRepo.deleteDocument(documentRespose.Result[i].DocumentID, localtran);
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            response.ErrorMessage.Add(ex.Message);
        //            response.IsSuccess = false;
        //            response.Result = default;
        //        }
        //        return response;
        //    }
    }
}
