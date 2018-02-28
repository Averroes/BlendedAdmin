using System;
using System.Collections;
using System.Linq;
using System.Dynamic;
using BlendedJS;
//using System.Collections.Generic;

namespace BlendedAdmin.Js
{
    public class TableView : View
    {
        public TableView(object rows)
        {
            this["rows"] = rows;
        }
    }
}
