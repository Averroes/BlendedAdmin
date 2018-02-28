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

namespace BlendedAdmin.Controllers
{
    public class HomeController : Controller
    {
        private IDomainContext _domainContext;

        public HomeController(IDomainContext domainContext)
        {
            _domainContext = domainContext;
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
