using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Attributes;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute()
    {
    }

    public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString())
    {
    }
}
