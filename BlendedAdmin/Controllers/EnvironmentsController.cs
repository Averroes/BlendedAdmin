using System;
using Microsoft.AspNetCore.Mvc;
using BlendedAdmin.DomainModel;
using BlendedAdmin.Models.Environments;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Environment = BlendedAdmin.DomainModel.Environments.Environment;
using Microsoft.AspNetCore.Authorization;

namespace BlendedAdmin.Controllers
{
    [Authorize]
    public class EnvironmentsController : Controller
    {
        private IDomainContext _domainContext;
        public EnvironmentsController(IDomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        [HttpGet]
        [Route("{environment}/environments")]
        public async Task<IActionResult> Index()
        {
            List<Environment> environments = await _domainContext.Environments.GetAll();
            var model = new EnvironmentModelAssembler().ToModel(environments);
            return View(model);
        }

        [HttpGet]
        [Route("{environment}/environments/create")]
        public IActionResult Create()
        {
            var model = new EnvironmentModelAssembler().ToModel(new Environment());
            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/environments/create")]
        public async Task<IActionResult> Create(EnvironmentModel model)
        {
            Environment environment = new Environment();
            new EnvironmentModelAssembler().Apply(environment, model);
            environment.Index = await _domainContext.Environments.GetNextIndex();

            await Validate(environment);

            if (model == null || ModelState.IsValid == false)
                return View("Edit", model);

            _domainContext.Environments.Save(environment);
            await _domainContext.SaveAsync();

            return RedirectToAction("Edit", "Environments", new { id = environment.Id });
        }

        [HttpGet]
        [Route("{environment}/environments/{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var environment = await _domainContext.Environments.Get(id);
            var model = new EnvironmentModelAssembler().ToModel(environment);
            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/environments/{id}/edit")]
        public async Task<IActionResult> Edit(EnvironmentModel model)
        {
            var environment = await _domainContext.Environments.Get(model.Id);
            new EnvironmentModelAssembler().Apply(environment, model);

            await Validate(environment);

            if (model == null || ModelState.IsValid == false)
                return View("Edit", model);

            _domainContext.Environments.Save(environment);
            await _domainContext.SaveAsync();

            return RedirectToAction("Edit", "Environments", new { id = environment.Id });
        }

        [Route("{environment}/environments/{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _domainContext.Variables.Delete(id);
            await _domainContext.SaveAsync();
            return RedirectToAction("Index");
        }

        private async Task Validate(Environment environment)
        {
            Environment existingEnvironment = await _domainContext.Environments.GetByName(environment.Name);
            if (existingEnvironment != null && existingEnvironment.Id != environment.Id)
                ModelState.AddModelError("Name", string.Format("Environment '{0}' already exists.", environment.Name));
        }
    }
}