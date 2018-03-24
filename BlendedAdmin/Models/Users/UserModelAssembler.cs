using BlendedAdmin.DomainModel.Users;
using System.Collections.Generic;
using System.Linq;

namespace BlendedAdmin.Models.Users
{
    public class UserModelAssembler
    {
        public UserModel ToModel(ApplicationUser applicationUser)
        {
            UserModel model = new UserModel();
            model.Id = applicationUser.Id;
            model.Name = applicationUser.UserName;
            model.Email = applicationUser.Email;
            return model;
        }

        public List<UserModel> ToModel(List<ApplicationUser> users)
        {
            return users.Select(x => ToModel(x)).ToList();
        }

        public void Apply(ApplicationUser user, UserModel model)
        {
        }
    }
}
