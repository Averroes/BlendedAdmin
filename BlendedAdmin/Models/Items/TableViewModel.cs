using BlendedAdmin.Js;
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
        public int PageSize { get; set; }
        public int Page { get; set; }

        public TableViewModel()
        {
        }
    }

    public class TableViewModelAssembler
    {
        public TableViewModel ToModel(TableView tableView)
        {
            TableViewModel model = new TableViewModel();
            model.Rows = GetRows(tableView);
            model.Columns = GetColumns(tableView);
            model.Page = tableView.GetValueOrDefault2("page").ToIntOrDefault(1);
            model.PageSize = tableView.GetValueOrDefault2("pageSize").ToIntOrDefault(100);
            model.Title = tableView.GetValueOrDefault2("title").ToStringOrDefault();
            return model;
        }

        public List<IDictionary<string, object>> GetRows(TableView tableView)
        {
            var rows = tableView.GetValueOrDefault2("rows") as object[];
            if (rows != null)
            {
                return rows.Select(x => x as IDictionary<string, object>).Where(x => x != null).ToList();
            }
            return new List<IDictionary<string, object>>();
        }

        public List<string> GetColumns(TableView tableView)
        {
            var columns = new List<string>();
            var rows = tableView.GetValueOrDefault("rows") as object[];
            if (rows != null && rows.Length > 0)
            {
                if (rows[0] is IDictionary)
                {
                    foreach (var column in ((IDictionary)rows[0]).Keys)
                        columns.Add(column?.ToString());
                }
                if (rows[0] is IDictionary<string, object>)
                {
                    foreach (var column in ((IDictionary<string, object>)rows[0]).Keys)
                        columns.Add(column?.ToString());
                }
            }
            return columns;
        }


        //public IEnumerable<IDictionary<string, object>> GetRows2(string orderBy, string direction, int page)
        //{
        //    if (page <= 0)
        //        page = 1;

        //    var rows = Rows.OfType<IDictionary<string, object>>().Cast<IDictionary<string, object>>();
        //    //if (string.IsNullOrEmpty(orderBy) == false)
        //    //{
        //    //    if (direction == "desc")
        //    //        rows = rows.OrderByDescending(x => x.GetValueOrDefault(orderBy));
        //    //    else
        //    //        rows = rows.OrderBy(x => x.GetValueOrDefault(orderBy));
        //    //}

        //    rows = rows.Skip((page - 1) * PageSize).Take(PageSize);
        //    return rows.ToArray();
        //}
    }
}
