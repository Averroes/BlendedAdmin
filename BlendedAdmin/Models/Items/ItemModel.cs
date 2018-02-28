using BlendedAdmin.DomainModel.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Models.Items
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

    public class ItemModelAssembler
    {
        public List<ItemModel> ToModel(List<Item> items)
        {
            return items.Select(x => new ItemModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category
                }).ToList();
        }
    }
}
