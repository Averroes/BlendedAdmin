using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BlendedAdmin.Models.Variables;
using System.Threading.Tasks;
using BlendedAdmin.DomainModel.Variables;
using Environment = BlendedAdmin.DomainModel.Environments;
using BlendedAdmin.DomainModel;
using Microsoft.AspNetCore.Authorization;

namespace BlendedAdmin.Controllers
{
    [Authorize]
    public class VariablesController : Controller
    {
        private IDomainContext _domainContext;
        public VariablesController(IDomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        [Route("{environment}/variables")]
        public async Task<IActionResult> Index()
        {
            var model = new VariableModelAssembler().ToModel(
                await _domainContext.Variables.GetAll(),
                await _domainContext.Environments.GetAll());

            return View(model);
        }

        [HttpGet]
        [Route("{environment}/variables/create")]
        public async Task<IActionResult> Create()
        {
            var model = new VariableModelAssembler().ToModel(
                new Variable(),
                await _domainContext.Environments.GetAll());

            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/variables/create")]
        public async Task<IActionResult> Create(VariableModel model)
        {
            Variable variable = new Variable();
            new VariableModelAssembler().ApplyModel(
                model, 
                variable,
                await _domainContext.Environments.GetAll());

            Validate(variable);
            
            if (model == null || ModelState.IsValid == false)
                return View("Edit", model);

            _domainContext.Variables.Save(variable);
            await _domainContext.SaveAsync();

            return RedirectToAction("Edit", "Variables", new { id = variable.Id });
        }

        [HttpGet]
        [Route("{environment}/variables/{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var variables = await _domainContext.Variables.GetById(id);
            var environments = await _domainContext.Environments.GetAll();
            var model = new VariableModelAssembler().ToModel(variables,environments);
            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/variables/{id}/edit")]
        public async Task<IActionResult> Edit(VariableModel model)
        {
            var variable = await _domainContext.Variables.GetById(model.Id);
            new VariableModelAssembler().ApplyModel(model,
                await _domainContext.Variables.GetById(model.Id),
                await _domainContext.Environments.GetAll());

            //Validate(variable);

            if (model == null || ModelState.IsValid == false)
                return View("Edit", model);

            _domainContext.Variables.Save(variable);
            await _domainContext.SaveAsync();

            return RedirectToAction("Edit", "Variables", new { id = variable.Id });
        }

        [Route("{environment}/variables/{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _domainContext.Variables.Delete(id);
            await _domainContext.SaveAsync();
            return RedirectToAction("Index");
        }

        private async void Validate(Variable variable)
        {
            Variable existingVariable = await _domainContext.Variables.GetByName(variable.Name);
            if (existingVariable != null && existingVariable.Id != variable.Id)
                ModelState.AddModelError("Name", string.Format("Variable '{0}' already exists.", variable.Name));
        }
    }
}