using Infrastracture.Constants;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Attributes;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute()
    {
    }

    public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString())
    {
    }
}
