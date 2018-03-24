using BlendedAdmin.DomainModel.Users;
using System.Collections.Generic;
using System.Linq;

namespace BlendedAdmin.Models.Users
{
    public class UserModelAssembler
    {
        public UserEditModel ToModel(ApplicationUser applicationUser)
        {
            UserEditModel model = new UserEditModel();
            model.Id = applicationUser.Id;
            model.Name = applicationUser.UserName;
            model.Email = applicationUser.Email;
            return model;
        }

        public UserChangePassowrdModel ToChangePasswordModel(ApplicationUser applicationUser)
        {
            UserChangePassowrdModel model = new UserChangePassowrdModel();
            model.Id = applicationUser.Id;
            model.Name = applicationUser.UserName;
            return model;
        }

        public List<UserEditModel> ToModel(List<ApplicationUser> users)
        {
            return users.Select(x => ToModel(x)).ToList();
        }

        public void Apply(ApplicationUser user, UserCreateModel model)
        {
            user.UserName = model.Name;
            user.Email = model.Email;
        }

        public void Apply(ApplicationUser user, UserEditModel model)
        {
            user.UserName = model.Name;
            user.Email = model.Email;
        }
    }
}
