using BlendedJS;
using System.Collections.Generic;
using System;

namespace BlendedAdmin.Js
{
    public class JsRunResult
    {
        public JsRunResult()
        {
            Views = new List<object>();
            Logs = new List<Log>();
        }
        public List<object> Views { get; set; }
        public List<Log> Logs { get; set; }
        public Exception Exception { get; set; }
        public int LastExecutedLine { get; set; }
        public List<object> Errors { get; set; }
    }

}
