using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerStore.Models.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Models.Repositories
{
    public interface IFilterOptionValuesRepository
    {
        IQueryable<FilterOptionValue> Values { get; }
        void AddValue(FilterOptionValue value);
        void EditValue(FilterOptionValue value);
        FilterOptionValue GetValue(long id);
        void DeleteValue(long id);
    }

    public class FilterOptionValuesRepository : IFilterOptionValuesRepository
    {
        private readonly ApplicationDbContext dbContext;

        public FilterOptionValuesRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public IQueryable<FilterOptionValue> Values => dbContext.FilterOptionValues;

        public FilterOptionValue GetValue(long id) => Values.FirstOrDefault(v => v.Id == id);

        public void AddValue(FilterOptionValue value)
        {
            dbContext.FilterOptionValues.Add(value);
            dbContext.SaveChanges();
        }

        public void EditValue(FilterOptionValue v)
        {
            FilterOptionValue value = GetValue(v.Id);
            value.Value = v.Value;
            dbContext.FilterOptionValues.Update(value);
            dbContext.SaveChanges();
        }

        public void DeleteValue(long id)
        {
            FilterOptionValue value = GetValue(id);
            dbContext.FilterOptionValues.Remove(value);
            dbContext.SaveChanges();
        }
    }
}
