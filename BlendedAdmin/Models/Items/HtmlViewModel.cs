using BlendedAdmin.Js;
using BlendedJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Models.Items
{
    public class HtmlViewModel
    {
        public string Title { get; set; }
        public string Html { get; set; }
    }

    public class HtmlViewModelAssembler
    {
        public HtmlViewModel ToModel(HtmlView htmlView)
        {
            HtmlViewModel model = new HtmlViewModel();
            model.Html = htmlView.GetValueOrDefault("html").ToStringOrDefault();
            model.Title = htmlView.GetValueOrDefault("title").ToStringOrDefault();
            return model;
        }
    }
}
