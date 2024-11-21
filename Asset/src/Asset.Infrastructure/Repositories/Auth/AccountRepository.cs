using Asset.Domain.Common;
using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Interfaces.Auth;
using Asset.Domain.Interfaces.Common;
using Asset.Domain.Utilities;
using Asset.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;

namespace Asset.Infrastructure.Repositories.Auth;

internal class AccountRepository(UserManager<UserMaster> _userManager, IPathFinderService _pathFinderService) : IAccountRepository
{
    public async Task<bool> CreateUserAsync(UserMaster user, string password, CancellationToken cancellationToken = default)
        => await ConfigureAndCreateUser(user, password, cancellationToken);

    public async Task<IEnumerable<UserMaster>> GetUsersPagedAsync(QueryParams queryParams, CancellationToken cancellationToken = default)
    {
        var orderByExpression = queryParams.Ascending ? queryParams.SortColumn : $"{queryParams.SortColumn} DESC";

        return await _userManager.Users
            .Where(x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || (x.FirstName + x.LastName + x.Email + x.PhoneNumber + x.UserName).Contains(queryParams.SearchTerm))
            .OrderBy(orderByExpression)
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUsersCountAsync(QueryParams queryParams, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users
            .Where(x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || (x.FirstName + x.LastName + x.Email + x.PhoneNumber + x.UserName).Contains(queryParams.SearchTerm))
            .CountAsync(cancellationToken);
    }

    public async Task<UserMaster?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _userManager.FindByEmailAsync(email);

    public async Task<UserMaster?> GetUserByIdAsync(long id, CancellationToken cancellationToken = default)
        => await _userManager.FindByIdAsync(id.ToString());

    public async Task<UserMaster?> GetUserByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _userManager.FindByNameAsync(name);

    public async Task<UserMaster?> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

    public async Task<UserMaster?> GetUserByTokenAsync(string token, CancellationToken cancellationToken = default)
        => await _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token), cancellationToken);

    public async Task<bool> ConfirmEmailAsync(long userId, string token, CancellationToken cancellationToken = default)
        => await ConfirmEmail(userId, token, cancellationToken);

    public async Task<bool> SendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default)
        => await SendConfirmationEmail(email, cancellationToken);



    private async Task<bool> ConfirmEmail(long userId, string token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return false;
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }
    private async Task<bool> ConfigureAndCreateUser(UserMaster user, string password, CancellationToken cancellationToken = default)
    {
        user.PasswordHash = DbContextUtilities.HashPassword(user, password);

        user.NormalizedEmail = user.Email?.ToUpper();

        user.NormalizedUserName = user.UserName?.ToUpper();

        user.SecurityStamp = Guid.NewGuid().ToString();

        // By default confirm the email and contact number.
        user.IsActive = user.EmailConfirmed = user.PhoneNumberConfirmed = true;

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);
        }
        return result.Succeeded;
    }
    private async Task<bool> SendConfirmationEmail(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return false;
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationUrl = BuildConfirmationCallbackUrl(user.Id, token);

        var appName = ConfigurationHelper.GetProfile("AppName");

        var subject = $"{appName}: Confirm your email";

        string emailFilePath = Path.Combine(_pathFinderService.AssetsFolderPath, "EmailConfirmation.html");
        string emailContent = File.ReadAllText(emailFilePath);

        string body = emailContent.ToString();
        body = body
            .Replace("{{AppName}}", appName)
            .Replace("{{ConfirmationUrl}}", confirmationUrl);

        var sender = new SMTPMailSender(email, subject, body, true);
        var isSent = sender.Send();

        return isSent;
    }
    private string BuildConfirmationCallbackUrl(long userId, string token)
    {
        var baseUrl = ConfigurationHelper.GetURL("Api");
        var controllerUrl = "/api/v1/auth/accounts/confirm-email";

        var encodedToken = UrlEncoder.Default.Encode(token);

        return $"{baseUrl}{controllerUrl}?userId={userId}&token={encodedToken}";
    }

}
