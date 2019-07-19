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
    public class FilterOptionController : Controller
    {
        private readonly IFilterOptionRepository filterOptionRepository;
        private readonly IFilterOptionValuesRepository filterOptionValuesRepository;

        public FilterOptionController(IFilterOptionRepository filterOptionRepos
            , IFilterOptionValuesRepository filterOptionValuesRepos)
        {
            
            filterOptionRepository = filterOptionRepos;
            filterOptionValuesRepository = filterOptionValuesRepos;
        }

        public ViewResult AddOption(long subcategoryId) => View(new FilterOption { SubcategoryID = subcategoryId });

        public JsonResult GetOptionsNames(long subcategoryId)
        {
            var names = from option in filterOptionRepository.Options
                          where option.SubcategoryID == subcategoryId
                          select option.Name;
            return Json(names);
        }


        public JsonResult GetOptions(long subcategoryId)
        {
            var options = from option in filterOptionRepository.Options
                          where option.SubcategoryID == subcategoryId
                          select new { option.Id, option.Name };
            return Json(options);
        }

        public JsonResult GetOptionValues(long optionId)
        {
            var values = from value in filterOptionValuesRepository.Values
                         where value.FilterOptionID == optionId
                         select new { value.Id, value.Value };
            return Json(values);
        }

        public IActionResult GetOptionName(long id)
        {
            FilterOption option = filterOptionRepository.GetOption(id);
            if (option == null)
            {
                return BadRequest();
            }
            return Json(option.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOption(string name, long subcategoryId)
        {
            if (string.IsNullOrEmpty(name) || subcategoryId == 0)
            {
                return BadRequest();
            }
            FilterOption option = new FilterOption { Name = name, SubcategoryID = subcategoryId };
            filterOptionRepository.AddOption(option);
            return Ok(new { option.Id, option.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOption(string name, long id)
        {
            FilterOption option = filterOptionRepository.GetOption(id);
            if (option == null)
            {
                return BadRequest();
            }
            option.Name = name;
            filterOptionRepository.EditOption(option);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOption(long id)
        {
            FilterOption option = filterOptionRepository.GetOption(id);
            if (option == null)
            {
                return BadRequest();
            }
            filterOptionRepository.DeleteOption(id);
            return Ok();
        }

        public ViewResult AddOptionValue(long optionId) => View(new FilterOptionValue { FilterOptionID = optionId });
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOptionValue(string value, long optionId)
        {
            if (string.IsNullOrEmpty(value) || optionId == 0)
            {
                return BadRequest();
            }
            FilterOptionValue optValue = new FilterOptionValue { Value = value, FilterOptionID = optionId };
            filterOptionValuesRepository.AddValue(optValue);
            return Ok(new { optValue.Id, optValue.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOptionValue(string value, long id)
        {
            FilterOptionValue optValue = filterOptionValuesRepository.GetValue(id);
            if (optValue == null)
            {
                return BadRequest();
            }
            optValue.Value = value;
            filterOptionValuesRepository.EditValue(optValue);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOptionValue(long id)
        {
            FilterOptionValue optValue = filterOptionValuesRepository.GetValue(id);
            if (optValue == null)
            {
                return BadRequest();
            }
            filterOptionValuesRepository.DeleteValue(id);
            return Ok();
        }
        

    }
}