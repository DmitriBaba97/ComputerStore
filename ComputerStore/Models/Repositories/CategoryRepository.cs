using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ComputerStore.Models.DbContexts;

namespace ComputerStore.Models.Repositories
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        void AddCategory(Category category);
        void EditCategory(Category category);
        Category GetCategory(long id);
        void DeleteCategory(long id);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext dbContext) => context = dbContext;

        public IQueryable<Category> Categories => context.Categories
            .Include(c => c.Products).ThenInclude(p => p.Images)
            .Include(c => c.Subcategories);

        public Category GetCategory(long id) => Categories.FirstOrDefault(c => c.Id == id);

        public void AddCategory(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void EditCategory(Category c)
        {
            Category category = GetCategory(c.Id);
            category.Name = c.Name;
            context.Categories.Update(category);
            context.SaveChanges();
        }

        public void DeleteCategory(long id)
        {
            Category category = GetCategory(id);
            context.Categories.Remove(category);
            context.SaveChanges();
        }
    }
}
