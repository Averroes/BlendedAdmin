using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlendedAdmin.Models;
using BlendedAdmin.Models.Home;
using BlendedAdmin.DomainModel;
using BlendedAdmin.Models.Items;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace BlendedAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IDomainContext _domainContext;
        private ILogger<HomeController> _logger;

        public HomeController(IDomainContext domainContext, ILogger<HomeController> logger)
        {
            _domainContext = domainContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _domainContext.Items.GetAll();
            var model = new ItemModelAssembler().ToModel(items);
            return View(new HomeModel
            {
                Items = model
            });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        [Route("home/error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
