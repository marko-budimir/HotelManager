using HotelManager.Service.Common;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.WebApi.Authorization
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IProfileService _profileService;
        AuthorizationServerProvider(IProfileService profileservice)
        {
            _profileService = profileservice;
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
            identity.AddClaim(new Claim(ClaimTypes.Role, user.RoleId));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            identity.AddClaim(new Claim("Email", user.Email));
            context.Validated(identity);
            
        }
    }
}