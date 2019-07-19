using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ComputerStore.Models.DbContexts;

namespace ComputerStore.Models
{
    public interface IProductPropertyRepository
    {
        IQueryable<ProductProperty> Properties { get; }
        void AddProperty(ProductProperty property);
        void EditProperty(ProductProperty property);
        ProductProperty GetProperty(long id);
        void DeleteProperty(long id);
    } 

    public class ProductPropertyRepository : IProductPropertyRepository
    {
        private readonly ApplicationDbContext context;

        public ProductPropertyRepository(ApplicationDbContext dbContext) => context = dbContext;

        public IQueryable<ProductProperty> Properties => context.ProductProperties;

        public ProductProperty GetProperty(long id) => Properties.FirstOrDefault(p => p.Id == id);

        public void AddProperty(ProductProperty property)
        {
            context.ProductProperties.Add(property);
            context.SaveChanges();
        }

        public void EditProperty(ProductProperty prop)
        {
            ProductProperty property = GetProperty(prop.Id);
            property.Name = prop.Name;
            property.Value = prop.Value;
            context.ProductProperties.Update(property);
            context.SaveChanges();
        }

        public void DeleteProperty(long id)
        {
            ProductProperty property = GetProperty(id);
            context.ProductProperties.Remove(property);
            context.SaveChanges();
        }
    }
}
