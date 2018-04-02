using System;
using Microsoft.AspNetCore.Mvc;
using BlendedAdmin.DomainModel;
using System.Threading.Tasks;
using BlendedAdmin.Models.Items;
using BlendedAdmin.DomainModel.Items;
using System.Collections.Generic;
using BlendedAdmin.Js;
using Microsoft.AspNetCore.Authorization;

namespace BlendedAdmin.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private IDomainContext _domainContext;

        public IJsService _jsService;

        public ItemsController(IDomainContext domainContext, IJsService jsService)
        {
            _domainContext = domainContext;
            _jsService = jsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/items/categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _domainContext.Items.GetAllCategories();
            return Json(categories);
        }

        [HttpGet]
        [Route("{environment}/items/create")]
        public IActionResult Create()
        {
            ItemEditModel model = new ItemEditModel();
            model.Code = @"function main(arg)
{
  	var htmlView = new HtmlView('<div>hello!</div>'); 
	return [htmlView];
}
main(arg);";
            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/items/create")]
        public async Task<IActionResult> Create(int? id, ItemEditModel model)
        {
            if (model == null || ModelState.IsValid == false)
                return View("Edit", model);

            var duplicateItem = await _domainContext.Items.GetByName(model.Name);
            if (duplicateItem != null)
            {
                ModelState.AddModelError("Name", string.Format("Item with the name '{0}' already exists.", model.Name));
                return View("Edit", model);
            }

            Item item = new Item();
            new ItemEditModelAssembler().ApplayModel(item, model);
            _domainContext.Items.Save(item);
            await _domainContext.SaveAsync();
            return RedirectToAction("Edit", new { id = item.Id });
        }

        [HttpGet]
        [Route("{environment}/items/{id}/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("New");

            Item item = await _domainContext.Items.Get(id.Value);
            ItemEditModel model = new ItemEditModelAssembler().ToModel(item);
            return View("Edit", model);
        }

        [HttpPost]
        [Route("{environment}/items/{id}/edit")]
        public async Task<IActionResult> Edit(int? id, ItemEditModel model)
        {
            if (model == null || ModelState.IsValid == false)
                return View("Edit", model);

            var duplicateItem = await _domainContext.Items.GetByName(model.Name);
            if (duplicateItem != null && duplicateItem.Id != model.Id)
            {
                ModelState.AddModelError("Name", string.Format("Item with the name '{0}' already exists.", model.Name));
                return View("Edit", model);
            }

            Item item = await _domainContext.Items.Get(id.GetValueOrDefault(0)) ?? new Item();
            new ItemEditModelAssembler().ApplayModel(item, model);
            _domainContext.Items.Save(item);
            await _domainContext.SaveAsync();
            return View("Edit", new ItemEditModelAssembler().ToModel(item));
        }

        [HttpGet]
        [Route("{environment}/items/{id}/delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                await _domainContext.Items.Delete(id.Value);
                await _domainContext.SaveAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("{environment}/items/{id}")]
        public async Task<IActionResult> Run(
            [FromRoute]int? id,
            ItemEditModel editModel,
            ItemSettingsModel settings
            )
        {
            this.ModelState.Clear();

            if (id > 0)
            {
                Item item = await _domainContext.Items.Get(id.Value) ?? new Item();
                editModel = new ItemEditModelAssembler().ToModel(item);
            }

            ItemRunModel m = new ItemRunModel();
            m.EditModel = editModel;
            try
            {
                m.RunResult = await _jsService.Run(editModel.Code);
                if (m.RunResult.Exception != null)
                    this.ModelState.AddModelError("", "Error, Line " + m.RunResult.LastExecutedLine + " :" + m.RunResult.Exception.Message);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }

            if (settings.RenderAs == "subView")
                return PartialView("_Run", m);
            return View(m);
        }

    }
}