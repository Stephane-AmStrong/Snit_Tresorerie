using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAccountRepository
    {
        Task<int> CountUsersAsync();
        Task<Authentication> LogInAsync(LoginModel loginRequest, string password);
        Task LogOutAsync();
        Task<Authentication> RegisterUserAsync(AppUser appUser, string password);
        Task<Authentication> ConfirmEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<string> EncodeTokenAsync(string token);
        Task<string> DecodeTokenAsync(string encodedToken);
        Task<AppUser> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<Authentication> ForgetPasswordAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password);
        Task<bool> IsEmailConfirmedAsync(AppUser appUser);
        Task<string> GetUserId(ClaimsPrincipal user);

        Task<ICollection<string>> GetUsersRolesAsync(AppUser user);
        Task<Authentication> AddToRoleAsync(AppUser user, IdentityRole role);
        Task<Authentication> RemoveFromRoleAsync(AppUser user, IdentityRole role);
    }
}
