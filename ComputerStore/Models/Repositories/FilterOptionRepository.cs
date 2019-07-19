using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerStore.Models.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Models.Repositories
{
    public interface IFilterOptionRepository
    {
        IQueryable<FilterOption> Options { get; }
        void AddOption(FilterOption option);
        void EditOption(FilterOption option);
        FilterOption GetOption(long id);
        void DeleteOption(long id);
    }
    public class FilterOptionRepository : IFilterOptionRepository
    {
        private readonly ApplicationDbContext context;

        public FilterOptionRepository(ApplicationDbContext dbContext) => context = dbContext;

        public IQueryable<FilterOption> Options => context.FilterOptions
            .Include(o => o.Subcategory)
            .Include(o => o.Values);

        public FilterOption GetOption(long id) => Options.FirstOrDefault(o => o.Id == id);

        public void AddOption(FilterOption option)
        {
            context.FilterOptions.Add(option);
            context.SaveChanges();
        }

        public void EditOption(FilterOption o)
        {
            FilterOption option = GetOption(o.Id);
            option.Name = o.Name;
            context.FilterOptions.Update(option);
            context.SaveChanges();
        }

        public void DeleteOption(long id)
        {
            FilterOption option = GetOption(id);
            context.FilterOptions.Remove(option);
            context.SaveChanges();
        }
    }
}
