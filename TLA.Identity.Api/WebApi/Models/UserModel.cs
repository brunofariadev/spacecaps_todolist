using Microsoft.AspNetCore.Identity;

namespace TLA.Identity.Api.WebApi.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        public static UserModel ToUserModel(IdentityUser identityUser)
        {
            return new UserModel()
            {
                UserName = identityUser.UserName,
                Email = identityUser.Email
            };
        }
    }
}
