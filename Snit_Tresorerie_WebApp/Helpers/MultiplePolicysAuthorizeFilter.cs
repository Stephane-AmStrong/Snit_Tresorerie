using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snit_Tresorerie_WebApp.Helpers
{
    public class MultiplePoliciesAuthorizeAttribute : TypeFilterAttribute
    {
        public MultiplePoliciesAuthorizeAttribute(string policies, bool isAnd = false) : base(typeof(MultiplePoliciesAuthorizeFilter))
        {
            Arguments = new object[] { policies, isAnd };
        }
    }

    public class MultiplePoliciesAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorization;
        public string Policies { get; private set; }
        public bool IsAnd { get; private set; }

        public MultiplePoliciesAuthorizeFilter(string policies, bool isAnd, IAuthorizationService authorization)
        {
            Policies = policies;
            IsAnd = isAnd;
            _authorization = authorization;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var policies = Policies.Split(";").ToList();
            if (IsAnd)
            {
                foreach (var policy in policies)
                {
                    var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (!authorized.Succeeded)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }

                }
            }
            else
            {
                foreach (var policy in policies)
                {
                    var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (authorized.Succeeded)
                    {
                        return;
                    }

                }
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
