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

        public object[] getAllControls()
        {
            List<object> controls = new List<object>();
            object[] rows = this.GetValueOrDefault2("controls", new object[0]) as object[];
            foreach (var row in rows)
            {
                if (row is object[])
                    controls.AddRange((object[])row);
                
                else
                    controls.Add(row);
            }
            return controls.ToArray();
        }

        public object getControlByName(object name)
        {
            foreach (var control in getAllControls())
            {
                if (control.GetProperty("name").ToStringOrDefault() == name.ToStringOrDefault())
                    return control;
            }
            return null;
        }

        public object isValid()
        {
            foreach (var control in getAllControls())
            {
                if (string.IsNullOrWhiteSpace(control.GetProperty("error").ToStringOrDefault()) == false)
                    return false;
                if (control.GetProperty("required").ToBoolOrDefault(false) &&
                    string.IsNullOrWhiteSpace(control.GetProperty("value").ToStringOrDefault()))
                {
                    control.SetProperty("error", control.GetProperty("name") + " is required.");
                    return false;
                }
            }
            return true;
        }
    }
}
