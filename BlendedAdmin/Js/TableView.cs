using System;
using System.Collections;
using System.Linq;
using System.Dynamic;
using BlendedJS;

namespace BlendedAdmin.Js
{
    public class TableView : View
    {
        public TableView(){}
        public TableView(object rows)
        {
            this["rows"] = rows;
        }
        public TableView(object rows, object arg2) : this (rows){}
        public TableView(object rows, object arg2, object arg3) : this(rows) { }
        public TableView(object rows, object arg2, object arg3, object arg4) : this(rows) { }
        public TableView(object rows, object arg2, object arg3, object arg4, object arg5) : this(rows) { }
    }
}
