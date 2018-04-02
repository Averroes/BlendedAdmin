using BlendedAdmin.DomainModel;
using BlendedAdmin.DomainModel.Items;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Services
{
    public interface ISiteMenuService
    {
        Task<List<SiteMenuItem>> GetMenuItems();
    }

    public class SiteMenuService : ISiteMenuService
    {
        private IDomainContext _domainContext;
        private IUrlService _urlService;
        public SiteMenuService(IDomainContext context, IUrlService urlService)
        {
            this._domainContext = context;
            this._urlService = urlService;
        }

        public async Task<List<SiteMenuItem>> GetMenuItems()
        {
            int currentItemId = _urlService.GetItemId();
            List<SiteMenuItem> menuItems = new List<SiteMenuItem>();
            List<Item> items = await this._domainContext.Items.GetAll();
            var groups = items.OrderBy(x => x.Name).GroupBy(x => x.Category);
            foreach(var group in groups)
            {
                if (string.IsNullOrEmpty(group.Key))
                {
                    foreach(var item in group)
                    {
                        menuItems.Add(new SiteMenuItem()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            IsActive = item.Id == currentItemId
                        });
                    }
                }
                else
                {
                    var category = new SiteMenuItem()
                    {
                        Id = 0,
                        Name = group.Key
                    };
                    foreach (var item in group)
                    {
                        category.Children.Add(new SiteMenuItem()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            IsActive = item.Id == currentItemId
                        });
                    }
                    category.IsActive = category.Children.Any(x => x.IsActive);
                    menuItems.Add(category);
                }
            }
            menuItems = menuItems.OrderBy(x => x.Children.Count == 0 ? 0 : 1).ThenBy(x => x.Name).ToList();
            return menuItems;
        }
    }

    public class SiteMenuItem
    {
        public SiteMenuItem()
        {
            Children = new List<SiteMenuItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IList<SiteMenuItem> Children { get; set; }
    }
}
