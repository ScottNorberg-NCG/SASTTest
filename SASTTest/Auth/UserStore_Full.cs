using Microsoft.AspNetCore.Identity;
using SASTTest.EF;

namespace SASTTest.Auth;

public class UserStore_Full : IUserStore<SiteUser>, IUserLockoutStore<SiteUser>
{
    public Task<IdentityResult> CreateAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<SiteUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SiteUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetAccessFailedCountAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetLockoutEnabledAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<DateTimeOffset?> GetLockoutEndDateAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedUserNameAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> IncrementAccessFailedCountAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ResetAccessFailedCountAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetLockoutEnabledAsync(SiteUser user, bool enabled, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetLockoutEndDateAsync(SiteUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(SiteUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserNameAsync(SiteUser user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(SiteUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
