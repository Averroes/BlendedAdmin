using BlendedAdmin.DomainModel.Items;
using System.Collections.Generic;
using System.Linq;

namespace BlendedAdmin.Models.Items
{
    public class ItemEditModelAssembler
    {
        public IList<ItemEditModel> ToModel(IList<Item> items)
        {
            return items.Select(x => ToModel(x)).ToList();
        }

        public ItemEditModel ToModel(Item item)
        {
            if (item == null)
                return new ItemEditModel();

            ItemEditModel model = new ItemEditModel();
            model.Id = item.Id;
            model.Name = item.Name;
            model.Category = item.Category;
            model.Code = item.Code;
            return model;
        }

        public void ApplayModel(Item item, ItemEditModel model)
        {
            if (model == null)
                return;

            item.Name = model.Name;
            item.Category = model.Category;
            item.Code = model.Code;
        }
    }
}
