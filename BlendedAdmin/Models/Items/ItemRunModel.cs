using System.Collections.Generic;

namespace BlendedAdmin.Models.Items
{
    public class ItemRunModel
    {
        public int? Id { get; set; }
        //public string Name { get; set; }
        public ItemEditModel EditModel { get; set; }
        public Js.JsRunResult RunResult { get; set; }

        public ItemRunModel()
        {
        }
    }
}
