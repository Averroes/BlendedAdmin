using System;
using BlendedAdmin.Js;
using BlendedJS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BlendedAdmin.Models.Items
{
    public class FormViewModel
    {
        public string Method { get; set; }
        public string Title { get; set; }
        public List<ParameterModel[]> Parameters { get; set; }

        public FormViewModel()
        {
        }
    }

    public class ParameterModel
    {
        public string Name { get; internal set; }
        public string Label { get; internal set; }
        public string Value { get; internal set; }
        public bool ReadOnly { get; internal set; }
        public ParmeterTypeModel Type { get; internal set; }
    }

    public enum ParmeterTypeModel
    {
        TextBox,
        Input,
        Select,
        DropDown,
        None
    }

    public class FormViewModelAssembler
    {
        public FormViewModel ToModel(FormView formView)
        {
            FormViewModel model = new FormViewModel();
            model.Parameters = new List<ParameterModel[]>();
            model.Method = formView.GetValueOrDefault2("method").ToStringOrDefault("get");
            model.Title = formView.GetValueOrDefault2("title").ToStringOrDefault();
            foreach (object row in ((formView.GetValueOrDefault2("controls") as IEnumerable) ?? new object[0] { }))
            {
                if (row is object[])
                {
                    var parameters =
                        ((object[])row)
                        .Select(x => CreateParameterModel(x as IDictionary<string, object>))
                        .ToArray();
                    model.Parameters.Add(parameters);
                }
                else
                {
                    model.Parameters.Add(new ParameterModel[1] { CreateParameterModel(row as IDictionary<string, object>) });
                }
            }
            return model;
        }

        public ParameterModel CreateParameterModel(IDictionary<string, object> parameter)
        {
            if (parameter is null)
                return new ParameterModel { Type = ParmeterTypeModel.None };
            
            string type = parameter.GetValueOrDefault2("type").ToStringOrDefault();
            ParmeterTypeModel parameterType = Enum.TryParse(type, true, out parameterType) ? parameterType : ParmeterTypeModel.TextBox;
            //IEnumerable options = model.GetValueOrDefault("options", null) as IEnumerable ?? new object[] { };
            //List<Option> optionsList = options.Cast<ExpandoObject>().Select(x =>
            //{
            //return new Option(x.FirstOrDefault().Value.ToStringOrDefault(), x.FirstOrDefault().Value.ToStringOrDefault());
            //}).ToList();
            return new ParameterModel
            {
                Name = parameter.GetValueOrDefault2("name").ToStringOrDefault(),
                Label = parameter.GetValueOrDefault2("label").ToStringOrDefault(),
                Value = parameter.GetValueOrDefault2("value").ToStringOrDefault(),
                ReadOnly = parameter.GetValueOrDefault2("readOnly").ToBoolOrDefault(false),
                Type = parameterType,
                //Options = optionsList
            };
        }

    }
}
