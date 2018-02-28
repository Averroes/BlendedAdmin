using System.Collections.Generic;

namespace BlendedAdmin.DomainModel.Items
{
    public class Item
    {
        public Item()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
    }
}
