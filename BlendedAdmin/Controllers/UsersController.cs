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
using Microsoft.AspNetCore.Identity;

namespace BlendedAdmin.Controllers
{
    public class UsersController : Controller
    {
        private IDomainContext _domainContext;
        private UserManager<ApplicationUser> _userManager { get; }

        public UsersController(IDomainContext domainContext, UserManager<ApplicationUser> userManager)
        {
            _domainContext = domainContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("{environment}/users")]
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await _domainContext.Users.GetAll();
            var model = new UserModelAssembler().ToModel(users);
            return View(model);
        }

        [HttpGet]
        [Route("{environment}/users/create")]
        public IActionResult Create()
        {
            return View(new UserCreateModel());
        }

        [HttpPost]
        [Route("{environment}/users/create")]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid == false)
                return View("Edit", model);

            ApplicationUser entity = new ApplicationUser();
            new UserModelAssembler().Apply(entity, model);
            var result = await this._userManager.CreateAsync(entity, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Edit", "Users", new { id = entity.Id });
            }
            else
            {
                result.Errors.ToList().ForEach(x => ModelState.AddModelError(string.Empty, x.Description));
                return View(model);
            }
        }

        [HttpGet]
        [Route("{environment}/users/{id}/edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var entity = await this._domainContext.Users.Get(id);
            if (entity == null)
            {
                this.ModelState.AddModelError("", "Sorry, we cannot find the user.");
                return View(new ApplicationUser());
            }
            var model = new UserModelAssembler().ToModel(entity);
            return View(model);
        }

        [HttpPost]
        [Route("{environment}/users/{id}/edit")]
        public async Task<IActionResult> Edit(string id, UserEditModel model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var entity = await this._domainContext.Users.Get(id);
            new UserModelAssembler().Apply(entity, model);
            var result = await this._userManager.UpdateAsync(entity);
            if (result.Succeeded)
            {
                return RedirectToAction("Edit", "Users", new { id = entity.Id });
            }
            else
            {
                result.Errors.ToList().ForEach(x => ModelState.AddModelError(string.Empty, x.Description));
                return View(model);
            }
        }

        [HttpGet]
        [Route("{environment}/users/{id}/changepassword")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var entity = await this._domainContext.Users.Get(id);
            if (entity == null)
            {
                this.ModelState.AddModelError("", "Sorry, we cannot find the user.");
                return View(new ApplicationUser());
            }
            var model = new UserModelAssembler().ToChangePasswordModel(entity);
            return View(model);
        }

        [HttpPost]
        [Route("{environment}/users/{id}/changepassword")]
        public async Task<IActionResult> ChangePassword(string id, UserChangePassowrdModel model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var entity = await this._domainContext.Users.Get(id);
            var results = await _userManager.RemovePasswordAsync(entity);
            results.Errors.ToList().ForEach(x => ModelState.AddModelError(string.Empty, x.Description));
            results = await _userManager.AddPasswordAsync(entity, model.Password);
            results.Errors.ToList().ForEach(x => ModelState.AddModelError(string.Empty, x.Description));
            return View(model);
        }

        [Route("{environment}/users/{id}/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await this._domainContext.Users.Get(id);
            await _userManager.DeleteAsync(entity);
            return RedirectToAction("Index");
        }
    }
}