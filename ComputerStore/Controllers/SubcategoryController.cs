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
    [Authorize(Roles = "Admin")]
    public class SubcategoryController : Controller
    {
        private readonly ISubcategoryRepository subcategoryRepository;
        private readonly ICategoryRepository categoryRepository;

        public SubcategoryController(ISubcategoryRepository subcategoryRepos, ICategoryRepository categoryRepos)
        {
            subcategoryRepository = subcategoryRepos;
            categoryRepository = categoryRepos;
        }

        public ViewResult Index() => View();

        public JsonResult GetSubcategoryId(string name) {
            Subcategory subcategory = subcategoryRepository.Subcategories.FirstOrDefault(sc => sc.Name == name);
            return Json(subcategory.Id);
        }

        public IActionResult GetSubcategoryName(long id)
        {
            Subcategory subcategory = subcategoryRepository.GetSubcategory(id);
            if (subcategory == null)
            {
                return BadRequest();
            }
            return Json(subcategory.Name);
        }

        public IActionResult GetSubcategories(string categoryName)
        {
            Category category = categoryRepository.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category == null)
            {
                return BadRequest();
            }
            var subcategories = from subcategory in category.Subcategories
                                orderby subcategory.Name ascending
                                select new { subcategory.Id, subcategory.Name };
            return Json(subcategories);
        }

        public IActionResult GetSubcategoriesNames(string categoryName)
        {
            Category category = categoryRepository.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category == null)
            {
                return BadRequest();
            }
            var names = from subcategory in category.Subcategories
                        orderby subcategory.Name ascending
                        select subcategory.Name;
            return Json(names);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubcategory(string name, long categoryId)
        {
            if (string.IsNullOrEmpty(name) || categoryId == 0)
            {
                return BadRequest();
            }
            Subcategory subcategory = new Subcategory { Name = name, CategoryID = categoryId };
            subcategoryRepository.AddSubcategory(subcategory);
            return Ok(new { subcategory.Id, subcategory.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSubcategory(long id)
        {
            Subcategory subcategory = subcategoryRepository.GetSubcategory(id);
            if (subcategory == null)
            {
                return BadRequest();
            }
            subcategoryRepository.DeleteSubcategory(id);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSubcategory(string name, long id)
        {
            Subcategory subcategory = subcategoryRepository.GetSubcategory(id);
            if (subcategory == null)
            {
                return BadRequest();
            }
            subcategory.Name = name;
            subcategoryRepository.EditSubcategory(subcategory);
            return Ok();
        }
    }
}