using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DPD.HR.Infrastructure.WebApi.Permission
{
    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly string _permission;
        private readonly IRoleRepository _roleRepository;

        public AuthorizeActionFilter(string permission, IRoleRepository roleRepository)
        {
            _permission = permission;
            _roleRepository = roleRepository;
        }
        /// <summary>
        /// this service uses to check permission of user that is requesting to the server 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //get current id user that's requesting to the server 
            //if the request don't have any token then return 00000000-0000-0000-0000-000000000000 (Guid.Empty in c#)
            var idUserCurrent = context.HttpContext.Items["UserId"] as Guid? ?? Guid.Empty;

            //getting all roles of this user
            var rolesOfUser = _roleRepository.GetRoleByIdUser(idUserCurrent);

            //if this user don't have any role then return 401 status
            if (!rolesOfUser.Result.Any())
            {
                context.Result =
                    new JsonResult(new TemplateApi<UserDto>(null, null, "Bạn cần đăng nhập tài khoản !",
                            false, 0, 0, 0, 0))
                        { StatusCode = StatusCodes.Status401Unauthorized };
            }

            //_permission value that get from filter controller
            //case 1 to check that's have admin role or not
            //case 2 to check that's have user role or not
            switch (_permission)
            {
                case ListRole.Admin:
                {
                    if (!rolesOfUser.Result.Any(e => e.IsAdmin))
                    {
                        context.Result =
                            new JsonResult(new TemplateApi<UserDto>(null, null, "Bạn cần đăng nhập tài khoản Admin !",
                                    false, 0, 0, 0, 0))
                                { StatusCode = StatusCodes.Status401Unauthorized };
                    }

                    break;
                }
                case ListRole.User:
                {
                    if (!rolesOfUser.Result.Any())
                    {
                        context.Result =
                            new JsonResult(new TemplateApi<UserDto>(null, null,
                                    "Bạn cần đăng nhập tài khoản của người dùng hoặc Admin !",
                                    false, 0, 0, 0, 0))
                                { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                
                    break;
                }
            }
        }
    }
}