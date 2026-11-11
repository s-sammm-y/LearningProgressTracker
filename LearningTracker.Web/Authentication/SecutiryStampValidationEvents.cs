
using System.Security.Claims;
using LearningTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LearningTracker.Web.Authentication;

public class SecurityStampValidationEvents : CookieAuthenticationEvents
{
    private readonly IAccountRepository _accountRepository;

    public SecurityStampValidationEvents(
        IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public override async Task ValidatePrincipal(
        CookieValidatePrincipalContext context)
    {
        var accountIdClaim = context.Principal?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var securityStampClaim = context.Principal?
            .FindFirst("SecurityStamp")?.Value;

        if (!Guid.TryParse(accountIdClaim, out var accountId) ||
            string.IsNullOrWhiteSpace(securityStampClaim))
        {
            await RejectPrincipalAsync(context);
            return;
        }

        var account = await _accountRepository.GetByIdAsync(
            accountId,
            context.HttpContext.RequestAborted);

        if (account is null ||
            account.SecurityStamp != securityStampClaim)
        {
            await RejectPrincipalAsync(context);
        }
    }

    private static async Task RejectPrincipalAsync(
        CookieValidatePrincipalContext context)
    {
        context.RejectPrincipal();

        await context.HttpContext.SignOutAsync(
            "LearningTrackerCookie");
    }
}
