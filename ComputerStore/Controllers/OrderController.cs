using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ComputerStore.Models;
using ComputerStore.Models.Repositories;
using ComputerStore.Models.DbContexts;

namespace ComputerStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly Cart cart;

        public OrderController(IOrderRepository orderRepos, Cart cartService,
            ApplicationDbContext dbContext)
        {
            orderRepository = orderRepos;
            cart = cartService;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                order.DateCreated = DateTime.Now;
                order.TotalPrice = cart.ComputeTotalValue();
                orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            return View(order);
        }

        public ViewResult Completed() 
        {
            cart.Clear();
            return View();
        }
        
    }
    
}