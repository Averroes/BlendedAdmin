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
        public string Name { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public bool ReadOnly { get; set; }
        public ParmeterTypeModel? Type { get; set; }
        public string Error { get; set; }
        public string Description { get; internal set; }
        public List<ParameterOptionModel> Options { get; internal set; }
    }

    public enum ParmeterTypeModel
    {
        TextBox,
        Input,
        Select,
        DropDown,
        CheckBox,
        Date,
        DateTime,
        None
    }

    public class ParameterOptionModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
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

            List<ParameterOptionModel> options = new List<ParameterOptionModel>();
            object optionsObject = parameter.GetValueOrDefault2("options", new object[0]);
            if (optionsObject is object[])
            {
                foreach(var option in (object[])optionsObject)
                {
                    if (option is IDictionary<string, object> optionDict)
                    {
                        string value = string.Empty;
                        if (optionDict.ContainsKey("value"))
                            value = optionDict.GetProperty("value").ToStringOrDefault();
                        else
                            if (optionDict.Keys.Count >= 1)
                                value = optionDict.ToList()[0].Value.ToStringOrDefault();

                        string text = string.Empty;
                        if (optionDict.ContainsKey("text"))
                            text = optionDict.GetProperty("text").ToStringOrDefault();
                        else
                            if (optionDict.Keys.Count >= 2)
                                value = optionDict.ToList()[1].Value.ToStringOrDefault();
                            else
                                if (optionDict.Keys.Count >= 1)
                                    value = optionDict.ToList()[0].Value.ToStringOrDefault();

                        options.Add(new ParameterOptionModel
                        {
                            Value = value,
                            Text = text,
                        });
                    }
                    else if (option is object[])
                    {
                        object[] optionList = (object[])option;
                        if (optionList.Length == 1)
                        {
                            options.Add(new ParameterOptionModel
                            {
                                Value = optionList[0].ToStringOrDefault(),
                                Text = optionList[0].ToStringOrDefault(),
                            });
                        }
                        if (optionList.Length > 1)
                        {
                            options.Add(new ParameterOptionModel
                            {
                                Value = optionList[0].ToStringOrDefault(),
                                Text = optionList[1].ToStringOrDefault(),
                            });
                        }
                    }
                    else
                    {
                        options.Add(new ParameterOptionModel
                        {
                            Value = option.ToStringOrDefault(),
                            Text = option.ToStringOrDefault(),
                        });
                    }
                }
            }

            return new ParameterModel
            {
                Name = parameter.GetValueOrDefault2("name").ToStringOrDefault(),
                Label = parameter.GetValueOrDefault2("label").ToStringOrDefault(),
                Value = parameter.GetValueOrDefault2("value").ToStringOrDefault(),
                ReadOnly = parameter.GetValueOrDefault2("readOnly").ToBoolOrDefault(false),
                Error = parameter.GetValueOrDefault2("error").ToStringOrDefault(),
                Description = parameter.GetValueOrDefault2("description").ToStringOrDefault(),
                Type = parameterType,
                Options = options
            };
        }

    }
}
