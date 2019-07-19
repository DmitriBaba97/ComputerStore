using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ComputerStore.Models.Repositories;
using ComputerStore.Models;
using ComputerStore.Infrastructure;


namespace ComputerStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ImageController : Controller
    {
        private readonly IImageRepository imageRepository;
        private readonly IHostingEnvironment _appHostingEnv;
        private readonly IImageWriter imageWriter;
        private readonly UserManager<AppUser> userManager;
        private readonly IProductRepository productRepository;

        public ImageController(IImageRepository imageRepos, IHostingEnvironment hostingEnvironment,
            IImageWriter imgWriter, UserManager<AppUser> userMgr, IProductRepository productRepos)
        {
            imageRepository = imageRepos;
            _appHostingEnv = hostingEnvironment;
            imageWriter = imgWriter;
            userManager = userMgr;
            productRepository = productRepos;
        }

        public IActionResult GetImageId(string imageName)
        {
            Image image = imageRepository.Images.
                Where(i => i.Name == imageName).FirstOrDefault();
            if (image == null)
            {
                return BadRequest();
            }
            return Ok(image.Id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProductImages(IList<IFormFile> files, long productId)
        {
            if (files.Count == 0)
            {
                return BadRequest();
            }

            IList<Image> images = new List<Image>();
            foreach (IFormFile file in files)
            {
                if (imageWriter.IsImageFile(file))
                {
                    //Create unique file name with UUID and file extension
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(fileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + extension;

                    //Get file path in wwwroot folder
                    string imageUrl = "\\images\\products\\" + uniqueFileName;

                    //Get absolute path
                    string uploads = Path.Combine(_appHostingEnv.WebRootPath, "images", "products");
                    string path = Path.Combine(uploads, uniqueFileName);

                    //Upload image on the server
                    await imageWriter.UploadImageAsync(file, path);
                    imageRepository.AddImage(new Image { Name = uniqueFileName, Url = imageUrl, ProductID = productId });
                    //Add new image in list to return back to client
                    Image image = new Image
                    {
                        Name = uniqueFileName,
                        Url = imageUrl
                    };
                    images.Add(image);
                }
            }
            return Ok(images);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UploadUserPhoto(IFormFile file)
        {
            if ((file == null) || (!imageWriter.IsImageFile(file)))
            {
                return BadRequest();
            }

            //Create unique UUID file name
            string fileName = Path.GetFileName(file.FileName);
            string extension = Path.GetExtension(fileName);
            string uniqueFileName = Guid.NewGuid().ToString() + extension;
            //Image url on the server
            string imageUrl = "\\images\\users\\" + uniqueFileName;

            //Absolute path to the image on the server
            string uploads = Path.Combine(_appHostingEnv.WebRootPath, "images", "users");
            string path = Path.Combine(uploads, uniqueFileName);

            //Upload image on the server
            await imageWriter.UploadImageAsync(file, path);

            System.Drawing.Image image = System.Drawing.Image.FromFile(path);
            foreach (var prop in image.PropertyItems)
            {
                if (prop.Id == 0x0112)
                {
                    int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                    if (orientationValue != 1)
                    {
                        imageWriter.RotateImage(image, orientationValue);
                        imageWriter.DeleteImage(path);
                        image.Save(path, ImageFormat.Jpeg);
                        break;
                    }
                    break;
                }
            }

            return Ok(imageUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteImage(long imageId)
        {
            Image image = imageRepository.GetImage(imageId);
            if (image == null)
            {
                return BadRequest();
            }

            //Get absolute path of file
            string path = _appHostingEnv.WebRootPath + image.Url;
            //Delete image from server
            imageWriter.DeleteImage(path);
            //Delete image object from repository
            imageRepository.DeleteImage(imageId);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditImage(long imageId, long productId, bool forPreview)
        {
            Image image = imageRepository.GetImage(imageId);
            if (image == null)
            {
                return BadRequest();
            }

            //Get other images of the same product 
            Product product = productRepository.GetProduct(productId);
            IEnumerable<Image> images = product.Images.Where(i => i.Name != image.Name);

            //Set current image for preview true
            image.ForPreview = forPreview;
            imageRepository.EditImage(image);

            //Set other images of the same product for preview false
            foreach (Image img in images)
            {
                img.ForPreview = false;
                imageRepository.EditImage(image);
            }
            return Ok(images.Select(i => i.Id));
        }
    }
}