using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ComputerStore.Models.DbContexts;

namespace ComputerStore.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void AddProduct(Product product);
        void EditProduct(Product product);
        Product GetProduct(long id);
        void DeleteProduct(long id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext dbContext) => context = dbContext;

        public IQueryable<Product> Products => context.Products
            .Include(p => p.Images)
            .Include(p => p.Properties)
            .Include(p => p.Category).ThenInclude(c => c.Subcategories)
           .Include(p => p.Subcategory).ThenInclude(sc => sc.FilterOptions).ThenInclude(o => o.Values);

        public Product GetProduct(long id) => Products.FirstOrDefault(p => p.Id == id);

        public void AddProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }

        public void EditProduct(Product p)
        {
            Product product = GetProduct(p.Id);
            product.Name = p.Name;
            product.CategoryID = p.CategoryID;
            product.SubcategoryID = p.SubcategoryID;
            product.Price = p.Price;
            product.StockPrice = p.StockPrice;
            product.AmountInStock = p.AmountInStock;
            product.Description = p.Description;
            foreach (Image img in p.Images)
            {
                product.Images.Add(img);
            }
            product.Properties = p.Properties;
            context.Products.Update(product);
            context.SaveChanges();
        }

        public void DeleteProduct(long id)
        {
            Product product = GetProduct(id);
            context.Products.Remove(product);
            context.SaveChanges();
        }
    }
}
