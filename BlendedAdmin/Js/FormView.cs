using BlendedJS;
using System.Collections.Generic;
using System.Dynamic;
namespace BlendedAdmin.Js
{
    public class FormView : View
    {
        public FormView()
        {
            this["method"] = "get";
            this["controls"] = new object[0];
        }

        public FormView(object controlsOrOptions)
        {
            if (controlsOrOptions is object[])
            {
                this["controls"] = (object[])controlsOrOptions;
            }
            if (controlsOrOptions is IDictionary<string, object>)
            {
                IDictionary<string, object> optionsModel = (IDictionary<string, object>)controlsOrOptions;
                this["method"] = optionsModel.GetValueOrDefault2("method", "get");
                this["title"] = optionsModel.GetValueOrDefault2("title", null);
                this["controls"] = optionsModel.GetValueOrDefault2("controls", new object[0]);
            }
        }
    }
}
