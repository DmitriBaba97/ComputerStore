using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ComputerStore.Models;
using ComputerStore.Models.Repositories;

namespace ComputerStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISubcategoryRepository subcategoryRepository;

        public HomeController(IProductRepository productRepos, ICategoryRepository categoryRepos,
            ISubcategoryRepository subcategoryRepos)
        {
            productRepository = productRepos;
            categoryRepository = categoryRepos;
            subcategoryRepository = subcategoryRepos;
        }

        public IActionResult Index() => View(productRepository.Products
            .OrderByDescending(p => p.Id).Take(10).
            Select(p => new Product { Id = p.Id, Name = p.Name, Price = p.Price, Description = p.Description, Images = p.Images.Where(i => i.ForPreview).Select(i => i).ToList()}));

        public IActionResult ProductPreview(long productId)
        {
            Product product = productRepository.GetProduct(productId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult CategoryProducts(long categoryId)
        {
            Category category = categoryRepository.GetCategory(categoryId);
            if (category == null)
            {
                return NotFound();
            }
            return View(category.Products);
        }

        public IActionResult SubcategoryProducts(long subcategoryId)
        {
            Subcategory subcategory = subcategoryRepository.GetSubcategory(subcategoryId);
            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory.Products);
        }
    }
}