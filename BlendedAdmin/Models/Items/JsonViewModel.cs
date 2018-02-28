using BlendedAdmin.Js;
using BlendedJS;
using System.Collections.Generic;

namespace BlendedAdmin.Models.Items
{
    public class JsonViewModel
    {
        public string Title { get; set; }
        public string Json { get; set; }

        public JsonViewModel()
        {
        }
    }

    public class JsonViewModelAssembler
    {
        public JsonViewModel ToModel(JsonView jsonView)
        {
            JsonViewModel model = new JsonViewModel();
            model.Json = jsonView.GetValueOrDefault("json").ToJsonOrString();
            model.Title = jsonView.GetValueOrDefault2("title").ToStringOrDefault();
            return model;
        }
    }
}
