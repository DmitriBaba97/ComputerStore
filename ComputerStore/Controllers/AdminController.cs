using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ComputerStore.Models;
using ComputerStore.Models.Repositories;
using Microsoft.Extensions.Logging;

namespace ComputerStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public AdminController(ICategoryRepository categoryRepos) 
        {
            categoryRepository = categoryRepos;
        }

        public ViewResult Index() => View();

    }
}