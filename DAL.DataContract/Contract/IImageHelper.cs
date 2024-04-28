using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContract.Contract
{
    public interface IImageHelper
    {
        Images SaveImage(Images image);
        string GetBase64ImageData(string imagePath);
        void DeleteImage(string filePath);
    }
}
