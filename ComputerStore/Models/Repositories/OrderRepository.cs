using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerStore.Models.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Models.Repositories
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public IQueryable<Order> Orders => dbContext.Orders
            .Include(o => o.Lines).ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            dbContext.AttachRange(order.Lines.Select(l => l.Product));
            if (order.Id == 0)
            {
                dbContext.Orders.Add(order);
            }
            dbContext.SaveChanges();
        }
    }
    
}
