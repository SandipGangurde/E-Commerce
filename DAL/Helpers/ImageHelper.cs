using DAL.DataContract.Contract;
using DataModel.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public ImageHelper(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }

        public Images SaveImage(Images image)
        {
            var filePath = _env.ContentRootPath;
            var docPath = Path.Combine(filePath, _configuration["DocumentsFolder"]);

            if (!Directory.Exists(docPath))
            {
                Directory.CreateDirectory(docPath);
            }

            var folderPath = Path.Combine(docPath, image.TableName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            byte[] imageArray = Convert.FromBase64String(image.FilePath);
            string fileName = $"{folderPath}\\{image.FileName}.png";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            System.IO.File.WriteAllBytes(fileName, imageArray);
            FileInfo fileInfo = new FileInfo(fileName);
            long sizeInBytes = fileInfo.Length;

            image.FilePath = fileName;

            return image;
        }

        public string GetBase64ImageData(string imagePath)
        {
            try
            {
                // Check if the file exists
                if (!File.Exists(imagePath))
                {
                    throw new FileNotFoundException("Image file not found", imagePath);
                }

                // Read the image file as bytes
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // Convert the byte array to a base64 string
                string base64Image = Convert.ToBase64String(imageBytes);

                // Create the data URL for the image
                string dataUrl = $"data:image/jpeg;base64,{base64Image}";

                return dataUrl;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public void DeleteImage(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
