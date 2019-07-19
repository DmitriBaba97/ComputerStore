using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ComputerStore.Models;
using ComputerStore.Models.Repositories;

namespace ComputerStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISubcategoryRepository subcategoryRepository;
        private readonly IProductPropertyRepository productPropertyRepository;


        public ProductController(IProductRepository productRepos, ICategoryRepository categoryRepos, 
            ISubcategoryRepository subcategoryRepos, IProductPropertyRepository productPropertyRepos)
        {
            productRepository = productRepos;
            categoryRepository = categoryRepos;
            subcategoryRepository = subcategoryRepos;
            productPropertyRepository = productPropertyRepos;
        }

        public ViewResult Index() => View(productRepository.Products);

        public ViewResult AddProduct() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(long id)
        {
            Product product = productRepository.GetProduct(id);
            if (product == null)
            {
                return BadRequest();
            }
            productRepository.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditProduct(long id)
        {
            Product product = productRepository.GetProduct(id);
            if (product == null)
            {
                return BadRequest();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.EditProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}