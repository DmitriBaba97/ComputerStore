using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ComputerStore.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ComputerStore.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Models
{
    public class Cart
    {
        [JsonIgnore]
        public ISession Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            Cart cart = session?.GetJson<Cart>("Cart") ?? new Cart();
            cart.Session = session;
            return cart;
        }

        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                            .Where(p => p.Product.Id == product.Id)
                            .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
            Session.SetJson("Cart", this);
        }

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.Id == product.Id);
            Session.SetJson("Cart", this);
        }
            

        public decimal ComputeTotalValue() =>
            lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public void Clear()
        {
            lineCollection.Clear();
            Session.Remove("Cart");
        } 

        public IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
