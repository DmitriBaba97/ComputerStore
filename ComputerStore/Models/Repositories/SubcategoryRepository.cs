using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ComputerStore.Models.DbContexts;

namespace ComputerStore.Models
{
    public interface ISubcategoryRepository
    {
        IQueryable<Subcategory> Subcategories { get; }
        void AddSubcategory(Subcategory subcategory);
        void EditSubcategory(Subcategory subcategory);
        Subcategory GetSubcategory(long id);
        void DeleteSubcategory(long id);
    }
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly ApplicationDbContext context;

        public SubcategoryRepository(ApplicationDbContext dbContext) => context = dbContext;

        public IQueryable<Subcategory> Subcategories => context.Subcategories
            .Include(sc => sc.Products).ThenInclude(p => p.Images)
            .Include(sc => sc.FilterOptions);

        public Subcategory GetSubcategory(long id) => Subcategories.FirstOrDefault(sc => sc.Id == id);

        public void AddSubcategory(Subcategory subcategory)
        {
            context.Subcategories.Add(subcategory);
            context.SaveChanges();
        }

        public void EditSubcategory(Subcategory sc)
        {
            Subcategory subcategory = GetSubcategory(sc.Id);
            subcategory.Name = sc.Name;
            context.Subcategories.Update(subcategory);
            context.SaveChanges();
        }

        public void DeleteSubcategory(long id)
        {
            Subcategory subcategory = GetSubcategory(id);
            context.Subcategories.Remove(subcategory);
            context.SaveChanges();
        }
    }
}
