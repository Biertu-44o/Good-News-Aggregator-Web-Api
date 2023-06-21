using AutoMapper;
using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ControllerFactory;

namespace MVC.Controllers
{
    [Authorize(Roles="SuperAdmin")]
    public class AdminController : Controller
    {

        private readonly IServiceFactory _serviceFactory;


        public AdminController
        (IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }
        [HttpGet]
        public IActionResult GetAdminPage()
        {
            return View("Admin");
        }
        [HttpPost]
        public async Task<IActionResult> NewsAggregator()
        {
            await _serviceFactory.CreateArticlesService().AggregateArticlesAsync();
            return RedirectToAction("GetAdminPage");
        }
        [HttpGet]
        public async  Task<IActionResult> UserList()
        {
            return View("AllUsers", await _serviceFactory
                .CreateAdminService()
                .GetAllUsersWithRolesAsync());
        }
    }
}
