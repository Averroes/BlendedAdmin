using BlendedAdmin.DomainModel.Users;
using System.Collections.Generic;
using System.Linq;

namespace BlendedAdmin.Models.Users
{
    public class UserModelAssembler
    {
        public EditModel ToModel(ApplicationUser applicationUser)
        {
            EditModel model = new EditModel();
            model.Id = applicationUser.Id;
            model.Name = applicationUser.UserName;
            model.Email = applicationUser.Email;
            return model;
        }

        public ChangePassowrdModel ToChangePasswordModel(ApplicationUser applicationUser)
        {
            ChangePassowrdModel model = new ChangePassowrdModel();
            model.Id = applicationUser.Id;
            model.Name = applicationUser.UserName;
            return model;
        }

        public List<EditModel> ToModel(List<ApplicationUser> users)
        {
            return users.Select(x => ToModel(x)).ToList();
        }

        public void Apply(ApplicationUser user, CreateModel model)
        {
            user.UserName = model.Name;
            user.Email = model.Email;
        }

        public void Apply(ApplicationUser user, EditModel model)
        {
            user.UserName = model.Name;
            user.Email = model.Email;
        }
    }
}
