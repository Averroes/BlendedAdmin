using System;
using Microsoft.AspNetCore.Mvc;
using BlendedAdmin.DomainModel;
using BlendedAdmin.Models.Environments;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Environment = BlendedAdmin.DomainModel.Environments.Environment;
using BlendedAdmin.DomainModel.Users;
using BlendedAdmin.Models.Users;

namespace BlendedAdmin.Controllers
{
    public class UsersController : Controller
    {
        private IDomainContext _domainContext;
        public UsersController(IDomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        [HttpGet]
        [Route("{environment}/users")]
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await _domainContext.Users.GetAll(null);
            var model = new UserModelAssembler().ToModel(users);
            return View(model);
        }

        [HttpGet]
        [Route("{environment}/users/create")]
        public IActionResult Create()
        {
            var model = new UserModelAssembler().ToModel(new ApplicationUser());
            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/users/create")]
        public async Task<IActionResult> Create(UserModel model)
        {
            ApplicationUser user = new ApplicationUser();
            new UserModelAssembler().Apply(user, model);


            return RedirectToAction("Edit", "Users", new { id = user.Id });
        }

        [HttpGet]
        [Route("{environment}/users/{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            ApplicationUser user = null;
            var model = new UserModelAssembler().ToModel(user);
            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/users/{id}/edit")]
        public async Task<IActionResult> Edit(UserModel model)
        {
            ApplicationUser user = null;
            new UserModelAssembler().Apply(user, model);

            if (model == null || ModelState.IsValid == false)
                return View("Edit", model);

            return RedirectToAction("Edit", "Users", new { id = user.Id });
        }

        [Route("{environment}/users/{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            return RedirectToAction("Index");
        }
    }
}