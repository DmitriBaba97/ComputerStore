using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerStore.Models.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Models.Repositories
{
    public interface IImageRepository
    {
        IQueryable<Image> Images { get; }
        void AddImage(Image image);
        void DeleteImage(long id);
        void EditImage(Image img);
        Image GetImage(long id);
    }

    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public IQueryable<Image> Images => dbContext.Images.Include(i => i.Product);

        public Image GetImage(long id) => Images.FirstOrDefault(i => i.Id == id);

        public void EditImage(Image img)
        {
            Image image = GetImage(img.Id);
            image.Name = img.Name;
            image.Url = img.Url;
            image.ForPreview = img.ForPreview;

            dbContext.Images.Update(image);
            dbContext.SaveChanges();
        }

        public void AddImage(Image image)
        {
            dbContext.Images.Add(image);
            dbContext.SaveChanges();
        }

        public void DeleteImage(long id)
        {
            Image image = GetImage(id);
            dbContext.Images.Remove(image);
            dbContext.SaveChanges();
        }
    }
}
