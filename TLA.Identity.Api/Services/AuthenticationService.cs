using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TLA.Identity.Api.Data;
using TLA.Identity.Api.WebApi.Extensions;
using TLA.Identity.Api.WebApi.Models;
using TLA.WebApi.Core.Identity;

namespace TLA.Identity.Api.Services
{
    public class AuthenticationService
    {
        public readonly UserManager<IdentityUser> UserManager;
        public readonly SignInManager<IdentityUser> SignInManager;

        private readonly AppTokenSettings _appTokenSettingsSettings;
        private readonly ApplicationDbContext _context;
        private readonly IAspNetUser _aspNetUser;
        private readonly IJwtService _jwksService;

        public AuthenticationService(UserManager<IdentityUser> userManager, IAspNetUser aspNetUser,
            IJwtService jwksService, IOptions<AppTokenSettings> appTokenSettingsSettings,
            ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            _aspNetUser = aspNetUser;
            _jwksService = jwksService;
            _appTokenSettingsSettings = appTokenSettingsSettings.Value;
            _context = context;
            SignInManager = signInManager;
        }

        public async Task<UserResponseLoginModel> GerarJwt(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var claims = await UserManager.GetClaimsAsync(user);

            var identityClaims = await ObterClaimsUsuario(claims, user);
            var encodedToken = await CodificarToken(identityClaims);

            var refreshToken = await GerarRefreshToken(email);

            return ObterRespostaToken(encodedToken, user, claims, refreshToken);
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await UserManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(),
                ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);

        private async Task<string> CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var currentIssuer =
                $"{_aspNetUser.ObterHttpContext().Request.Scheme}://{_aspNetUser.ObterHttpContext().Request.Host}";
            var key = await _jwksService.GetCurrentSigningCredentials();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<RefreshToken> GerarRefreshToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                Email = email,
                ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettingsSettings.RefreshTokenExpiration)
            };

            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.Email == email));
            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private UserResponseLoginModel ObterRespostaToken(string encodedToken, IdentityUser user,
            IEnumerable<Claim> claims, RefreshToken refreshToken)
        {
            return new UserResponseLoginModel
            {
                AccessToken = encodedToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        public IEnumerable<UserModel> ObterUsuarios()
        {
            var users = UserManager.Users.ToList();
            return users.Select(u => UserModel.ToUserModel(u));
        }
    }
}
