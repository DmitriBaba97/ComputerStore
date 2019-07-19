using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ComputerStore.Models;
using ComputerStore.Models.Repositories;
using ComputerStore.Models.ViewModels;

namespace ComputerStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly Cart cart;

        public CartController(IProductRepository productRepos, Cart cartService)
        {
            productRepository = productRepos;
            cart = cartService;
        }

        public ViewResult Index(string returnUrl) 
            => View(new CartIndexViewModel
        {
            Cart = cart,
            ReturnUrl = returnUrl
        });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(long productId)
        {
            Product product = productRepository.GetProduct(productId);
            if (product == null)
            {
                return BadRequest();
                
            }
            Product newProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Images = product.Images
            };
            cart.AddItem(newProduct, 1);
            return Ok(cart.Lines.Count());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(long productId)
        {
            Product product = productRepository.GetProduct(productId);
            if (product == null)
            {
                return BadRequest();
                
            }
            cart.RemoveLine(product);
            return Ok(new { TotalPrice = cart.ComputeTotalValue(), NumOfLines = cart.Lines.Count() });
        }
    }
}