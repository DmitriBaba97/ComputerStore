using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ComputerStore.Infrastructure
{
    public interface IImageWriter
    {
        //Check if file has image format
        bool IsImageFile(IFormFile file);
        //Upload image file on the server 
        Task UploadImageAsync(IFormFile file, string path);
        //Delete image file from the server
        void DeleteImage(string path);
        //Rotate image according to orientation value
        void RotateImage(Image image, int orientationValue);
    }

    public class ImageWriter : IImageWriter
    {
        //Image extensions collection
        private ICollection<string> extensions = new HashSet<string>
        {
            ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".psd", ".webp", ".svg", ".ai", ".eps"
        };

        //Get orientation type of image file
        private RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }

        public bool IsImageFile(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(fileName);
            return extensions.Contains(extension);
        }

        public async Task UploadImageAsync(IFormFile file, string path)
        {
            if (file == null || string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }

            if (file.Length > 0)
            {
                try
                {
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException(ex.Message);
                }
            }
        }

        public void DeleteImage(string path)
        {
            FileInfo file = new FileInfo(path);
            file.Delete();
        }

        public void RotateImage(Image image, int orientationValue)
        {
            RotateFlipType rotateFlipType = GetOrientationToFlipType(orientationValue);
            image.RotateFlip(rotateFlipType);
        }
    }

}
