using BlendedAdmin.Js;
using BlendedAdmin.Services;
using BlendedJS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BlendedAdmin.Models.Items
{
    public class TableViewModel
    {
        public string Title { get; set; }
        public List<string> Columns { get; set; }
        public List<IDictionary<string, object>> Rows { get; set; }
        
        public int Page { get; set; }
        public bool NextPage { get; internal set; }
        public bool PrevioustPage { get; internal set; }

        public TableViewModel()
        {
        }
    }

    public class TableViewModelAssembler
    {
        private IUrlService _urlService;

        public TableViewModelAssembler(IUrlService urlService)
        {
            _urlService = urlService;
        }

        public TableViewModel ToModel(TableView tableView)
        {
            TableViewModel model = new TableViewModel();
            model.Title = tableView.GetValueOrDefault2("title").ToStringOrDefault();
            
            int? page = tableView.GetValueOrDefault2("page").ToIntOrDefault() ?? _urlService.GetQueryString("p").ToIntOrDefault();
            int pageSize = tableView.GetValueOrDefault2("pageSize").ToIntOrDefault(100);
            bool? previoustPage = tableView.GetValueOrDefault2("previoustPage").ToBoolOrDefault();
            bool? nextPage = tableView.GetValueOrDefault2("nextPage").ToBoolOrDefault();
            var data = GetData(tableView);

            model.Page = page.ToIntOrDefault(1);

            if (previoustPage.HasValue)
            {
                model.PrevioustPage = previoustPage.Value;
            }
            else
            {
                model.PrevioustPage = page.HasValue && page.Value > 1;
            }

            if (nextPage.HasValue)
            {
                model.NextPage = nextPage.Value;
            }
            else
            {
                model.NextPage = true;
            }
     
            model.Columns = data.Item1;
            model.Rows = data.Item2.Take(pageSize).ToList();
            
            return model;
        }

        public (List<string>,List<IDictionary<string, object>>) GetData(TableView tableView)
        {
            List<IDictionary<string, object>> rows = new List<IDictionary<string, object>>();
            Dictionary<string, object> columns = new Dictionary<string, object>();
            object rowsObject = tableView.GetValueOrDefault("rows");

            if (rowsObject == null)
                return (columns.Keys.ToList(), rows);

            if (rowsObject is object[])
            {
                object[] rowsArray = (object[])rowsObject;

                foreach (object row in rowsArray)
                {
                    if (row is HtmlView)
                    {
                        IDictionary<string, object> rowDictionary = new Dictionary<string, object>();
                        rowDictionary[""] = row;
                        rows.Add(rowDictionary);
                        columns.TryAdd("", null);
                    }
                    else if(row is IDictionary<string, object>)
                    {
                        IDictionary<string, object> rowDictionary = (IDictionary<string, object>)row;
                        rows.Add(rowDictionary);
                        foreach (var key in rowDictionary.Keys)
                            columns.TryAdd(key, null);
                    }
                    else if (row is IDictionary)
                    {
                        IDictionary<string, object> rowDictionary = ((IDictionary)row)
                            .Cast<DictionaryEntry>()
                            .ToDictionary(x => x.Key.ToString(), x => x.Value);
                        rows.Add(rowDictionary);
                        foreach (var key in rowDictionary.Keys)
                            columns.TryAdd(key, null);
                    }
                    else
                    {
                        IDictionary<string, object> rowDictionary = new Dictionary<string, object>();
                        rowDictionary[""] = row;
                        rows.Add(rowDictionary);
                        columns.TryAdd("", null);
                    }
                }
            }
            else
            {
                IDictionary<string, object> rowDictionary = new Dictionary<string, object>();
                rowDictionary[""] = rowsObject;
                rows.Add(rowDictionary);
                columns.TryAdd("", null);
            }

            return (columns.Keys.ToList(), rows);
        }
    }
}
