using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ComputerStore.Models;
using ComputerStore.Models.ViewModels;
using ComputerStore.Models.Repositories;

namespace ComputerStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private const int PageSize = 5;


        public CategoryController(ICategoryRepository categoryRepos)
        {
            categoryRepository = categoryRepos;
        }

        [Route("admin/categories")]
        public ViewResult Index() => View();

        public IActionResult GetCategories()
        {

        }

        public IActionResult GetCategoryId(string name)
        {
            Category category = categoryRepository.Categories.FirstOrDefault(c => c.Name == name);
            if (category == null)
            {
                return BadRequest();
            }
            return Json(category.Id);
        }

        public JsonResult GetCategoriesNames()
        {
            IEnumerable<string> names = categoryRepository.Categories.OrderBy(c => c.Name).Select(c => c.Name);
            return Json(names);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            Category category = new Category { Name = name };
            categoryRepository.AddCategory(category);
            Category resultCategory = new Category { Id = category.Id, Name = category.Name };
            return Ok(resultCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(long id)
        {
            Category category = categoryRepository.GetCategory(id);
            if (category == null)
            {
                return BadRequest();
            }
            categoryRepository.DeleteCategory(id);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory([FromBody] Category newCategory)
        {
            Category category = categoryRepository.GetCategory(newCategory.Id);
            if (category == null)
            {
                return BadRequest();
            }
            category.Name = newCategory.Name;
            categoryRepository.EditCategory(category);
            return Ok();
        }
    }
}