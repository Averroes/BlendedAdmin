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
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
        public int StartPage { get; internal set; }
        public int EndPage { get; internal set; }

        public TableViewModel()
        {
        }
    }

    public class TableViewModelAssembler
    {
        private IUrlServicecs _urlService;

        public TableViewModelAssembler(IUrlServicecs urlService)
        {
            _urlService = urlService;
        }

        public TableViewModel ToModel(TableView tableView)
        {
            var rows = GetRows(tableView);
            TableViewModel model = new TableViewModel();
            model.Rows = rows;
            model.Columns = GetColumns(tableView);
            model.Page = tableView.GetValueOrDefault2("page").ToIntOrDefault() ??
                        _urlService.GetQueryString("p").ToIntOrDefault(1);
            model.PageSize = tableView.GetValueOrDefault2("pageSize").ToIntOrDefault(50);
            model.PageCount = tableView.GetValueOrDefault2("pageCount").ToIntOrDefault() ??
                              (int)Math.Ceiling((double)model.Rows.Count / model.PageSize);
            model.Rows = rows.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList();
            model.Title = tableView.GetValueOrDefault2("title").ToStringOrDefault();

            var startPage = model.Page - 5;
            var endPage = model.Page + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > model.Page)
            {
                endPage = model.PageCount;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            model.StartPage = startPage;
            model.EndPage = endPage;

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
