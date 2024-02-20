using HotelManager.Repository;
using HotelManager.Service;
using HotelManager.Service.Common;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManager.WebApi.Authorization
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IProfileService _profileService;
        public AuthorizationServerProvider()
        {
            _profileService = new ProfileService(new ProfileRepository(), new RoleTypeRepository());
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = await _profileService.ValidateUserAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var role = await _profileService.GetRoleTypeByRoleIdAsync(user.RoleId);
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            context.Validated(identity);
            
        }
    }
}