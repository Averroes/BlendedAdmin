using System;
using System.Collections;
using System.Linq;
using System.Dynamic;
using BlendedJS;
using System.Collections.Generic;

namespace BlendedAdmin.Js
{
    public class TableView : View
    {
        public TableView() {}
        public TableView(object rowsOrOptions)
        {
            if (rowsOrOptions is object[])
            {
                this["rows"] = rowsOrOptions;
            }
            else if (rowsOrOptions is IDictionary<string, object>)
            {
                foreach (var item in ((IDictionary<string, object>)rowsOrOptions))
                    this.TryAdd(item.Key, item.Value);
            }
            else
            {
                this["rows"] = rowsOrOptions;
            }
        }
        public TableView(object rows, object arg2) : this (rows){}
        public TableView(object rows, object arg2, object arg3) : this(rows) { }
        public TableView(object rows, object arg2, object arg3, object arg4) : this(rows) { }
        public TableView(object rows, object arg2, object arg3, object arg4, object arg5) : this(rows) { }
    }
}
