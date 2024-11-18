using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DPD.HR.Infrastructure.Validation;
using DPD.HR.Infrastructure.Validation.Models.User;
using DPD.HR.Infrastructure.WebApi.Permission;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface;
using DPD.HumanResources.Utilities.Utils;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BCryptNet = BCrypt.Net.BCrypt;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UnitController> _logger;
    private readonly ValidatorProvider _validatorProvider;

    #endregion

    #region ===[ Constructor ]=================================================================

    public UserController(IUnitOfWork unitOfWork, ILogger<UnitController> logger,
        ValidatorProvider validatorProvider
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validatorProvider = validatorProvider;
    }

    #endregion

    #region ===[ UserController ]======================================================

    // HttpPost: /api/User/getUserAndRole
    [HttpGet("getUserAndRole")]
    public async Task<IActionResult>? GetUserAndRole(Guid idUser)
    {
        var result = await _unitOfWork.User.GetUserAndRole(idUser);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPost: /api/User/AddListRoleUser
    [HttpPost("addListRoleUser")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> AddListRoleUser(AddListRoleUserModel addListRoleUserModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.UserRole.InsertListUserRole(addListRoleUserModel.IdsRole!,
            addListRoleUserModel.IdUser, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // GET: api/User/GetListUserAvailable
    [HttpGet("getListUserAvailable")]
    public async Task<IActionResult> GetListUserAvailable(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.User.GetAllAvailable(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpDelete: /api/User/RemoveUserByList
    [HttpDelete("removeUserByList")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> RemoveUserByList(List<Guid> idUser)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.User.RemoveByList(idUser, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: /api/User/LockUserAccountByList
    [HttpPut]
    [Route("lockUserAccountByList")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> LockUserAccountByList(List<Guid> idUser, bool isLock)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.User.HideByList(idUser, isLock, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // GET: api/User/GetListUser
    [HttpGet("getListUser")]
    public async Task<IActionResult> GetListUser(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.User.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/User/GetUerById
    [HttpGet("getUerById")]
    public async Task<IActionResult> GetUerById(Guid idUser)
    {
        var templateApi = await _unitOfWork.User.GetById(idUser);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPut: /api/User/UpdateUser
    [HttpPut]
    [Route("updateUser")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateUser([FromForm] UserRequest userModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var userDto = userModel.Adapt<UserDto>();

        if (userModel.Fullname == "null")
        {
            userDto.Fullname = null;
        }
        else if (userModel.Description == "null")
        {
            userDto.Description = null;
        }
        else if (userModel.Phone == "null")
        {
            userDto.Phone = null;
        }
        else if (userModel.Address == "null")
        {
            userDto.Address = null;
        }

        // If directory does not exist, create it. 
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileAvatar))
        {
            Directory.CreateDirectory(AppSettings.ServerFileAvatar);
        }

        // check file exits
        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var fileContentType = file.ContentType;

                switch (fileContentType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/jpg":
                    {
                        var pathTo = AppSettings.ServerFileAvatar;
                        var idFile = userDto.Id + ".jpg";

                        var filename = Path.Combine(pathTo, Path.GetFileName(idFile));

                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }

                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        userDto.Avatar = idFile;
                        break;
                    }
                }
            }
        }

        var result = await _unitOfWork.User.Update(userDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPost: /api/User/InsertUser
    [HttpPost]
    [Route("insertUser")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertUser([FromForm] UserRequest userModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var userByEmail = await _unitOfWork.User.UserByEmail(userModel.Email);
        if (userByEmail != null && userByEmail.Id != Guid.Empty)
            return Ok(new TemplateApi<UserDto>(null, null, "Email này đã tồn tại !", false, 0, 0, 0, 0));

        var userDto = userModel.Adapt<UserDto>();

        userDto.Id = Guid.NewGuid();
        userDto.Status = userModel.Status.HasValue ? userModel.Status : 0;
        userDto.CreatedDate = DateTime.Now;
        userDto.IsLocked = false;
        userDto.IsDeleted = false;
        userDto.IsActive = userModel.IsActive;
        userDto.ActiveCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        userDto.UserCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        userDto.CreatedBy = idUserCurrent;

        string salt = BCryptNet.GenerateSalt();
        var password = BCryptNet.HashPassword(userModel.Password + AppSettings.SecretKey + salt);

        if (userModel.Fullname == "null")
        {
            userDto.Fullname = null;
        }
        else if (userModel.Description == "null")
        {
            userDto.Description = null;
        }
        else if (userModel.Phone == "null")
        {
            userDto.Phone = null;
        }
        else if (userModel.Address == "null")
        {
            userDto.Address = null;
        }

        // If directory does not exist, create it. 
        if (!Directory.Exists(AppSettings.Root))
        {
            Directory.CreateDirectory(AppSettings.Root);
        }

        if (!Directory.Exists(AppSettings.ServerFileAvatar))
        {
            Directory.CreateDirectory(AppSettings.ServerFileAvatar);
        }

        //save image user
        if (Request.Form.Files.Count > 0)
        {
            foreach (var file in Request.Form.Files)
            {
                var fileContentType = file.ContentType;

                switch (fileContentType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/jpg":
                    {
                        // prepare path to save file image
                        var pathTo = AppSettings.ServerFileAvatar;
                        // get extension form file name
                        var idFile = userDto.Id + ".jpg";

                        // set file path to save file
                        var filename = Path.Combine(pathTo, Path.GetFileName(idFile));

                        // save file
                        await using (var stream = System.IO.File.Create(filename))
                        {
                            await file.CopyToAsync(stream);
                        }

                        userDto.Avatar = idFile;
                        break;
                    }
                }
            }
        }

        //save to table user
        var result = await _unitOfWork.User.InsertUser(userDto, idUserCurrent, nameUserCurrent, password, salt);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion

    #region ===[ METHOD USER ]=================================================================

    // GET: api/User/generatePassWord
    [HttpGet("generatePassWord")]
    public IActionResult GeneratePassWord(string password)
    {
        string salt = BCryptNet.GenerateSalt();
        var hashed = BCryptNet.HashPassword(password + AppSettings.SecretKey + salt);

        string[] items = { password, salt, hashed };

        return Ok(items);
    }

    // HttpPost: /api/User/Register
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest userRegisterRequest)
    {
        var validationResult = _validatorProvider.Validate(userRegisterRequest);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Ok(new TemplateApi<UserDto>(null, null, errorMessages, false, 0, 0, 0, 0));
        }

        var userByEmail = await _unitOfWork.User.UserByEmail(userRegisterRequest.Email);

        var unit = await _unitOfWork.Unit.GetUnitByUnitCode(AppSettings.Unit);
        var userType = await _unitOfWork.UserType.GetTypeUser(AppSettings.UserTypeDefault);

        if (userByEmail is null)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));

        if (userByEmail.IsActive == false)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này chưa được kích hoạt !", false, 0, 0, 0, 0));

        var activeCode = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        string salt = BCryptNet.GenerateSalt();
        var passwordUser =
            BCryptNet.HashPassword(userRegisterRequest.Password + AppSettings.SecretKey + salt);

        var user = new UserDto()
        {
            Id = Guid.NewGuid(),
            Fullname = userRegisterRequest?.Fullname,
            Email = userRegisterRequest is not null ? userRegisterRequest.Email : "",
            CreatedDate = DateTime.Now,
            IsDeleted = false,
            IsLocked = false,
            IsActive = false,
            ActiveCode = activeCode,
            UserCode = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
            UserTypeId = userType.Id,
            UnitId = unit.Id,
            Address = userRegisterRequest?.Address,
            Phone = userRegisterRequest?.Phone,
        };
        await _unitOfWork.User.InsertUser(user, user.Id, user.Fullname ?? "", passwordUser, salt);

        // prepare content for mail
        var fromMail = AppSettings.FromMail;
        var passwordEmail = AppSettings.PasswordMail;
        const string subject = "Vui lòng xác thực tài khoản !";
        var body =
            $"<div style='max-width: 700px; margin: auto; border: 10px solid #ddd; padding: 50px 20px; font-size: 110%;'>" +
            $"<h2 style='text-align: center; text-transform: uppercase;color: teal;'> Chào mừng bạn đến với ứng dụng quản lí bán hàng </h2>" +
            $"<p>Chúc mừng bạn đã đăng kí thành công tài khoản. Hãy lấy mã và kích hoạt tài khoản của bạn !</p>" +
            $"<a style='background: crimson; text-decoration: none; color: white; padding: 10px 20px; margin-left: 300px; display: inline-block;text-align:center'>{activeCode}</a>";

        SendMail.SendMailAuto(fromMail, userRegisterRequest is not null ? userRegisterRequest.Password : "",
            passwordEmail,
            body,
            subject);

        return Ok(new TemplateApi<UserDto>(null, null, "Kiểm tra mail của bạn và kích hoạt tài khoản !", true, 0, 0, 0,
            0));
    }

    // HttpPost: /api/User/Login
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest userLoginRequest)
    {
        var validationResult = _validatorProvider.Validate(userLoginRequest);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Ok(new TemplateApi<UserDto>(null, null, errorMessages, false, 0, 0, 0, 0));
        }

        // get one user by email
        var userByEmail = await _unitOfWork.User.UserByEmail(userLoginRequest.Email);

        if (userByEmail is null)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));

        if (userByEmail is null && userByEmail.IsLocked)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này đã bị khóa !", false, 0, 0, 0, 0));

        if (userByEmail.IsActive == false)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này chưa được kích hoạt !", false, 0, 0, 0, 0));

        bool isPasswordValid =
            BCryptNet.Verify(userLoginRequest.Password + AppSettings.SecretKey + userByEmail?.Salt,
                userByEmail?.Password);
        if (!isPasswordValid)
            return Ok(new TemplateApi<UserDto>(null, null, "Thông tin đăng nhập không chính xác !", false, 0, 0, 0, 0));

        var listRoleOfUser = (await _unitOfWork.Role.GetRoleByIdUser(userByEmail!.Id)).ToList();

        var token = GenerateToken(userByEmail.Adapt<UserDto>());

        var data = new DataLoginUser()
        {
            Id = userByEmail.Id,
            Data = token,
            RoleList = listRoleOfUser,
            IsAdmin = listRoleOfUser.Any(e => e.IsAdmin)
        };

        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true
        });

        return Ok(new TemplateApi<DataLoginUser>(data, null, "Đăng nhập thành công !", true, 0, 0, 0, 0));
    }

    // GET: api/User/GetUser
    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser()
    {
        var jwt = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (jwt is null)
            return Ok(new TemplateApi<UserDto>(null, null, "Không tìm thấy token !", false, 0, 0, 0, 0));

        var handler = new JwtSecurityTokenHandler();
        var tokenS = handler.ReadToken(jwt) as JwtSecurityToken;
        var profile = tokenS!.Claims.First(claim => claim.Type == "email").Value;
        var expire = tokenS.Claims.First(claim => claim.Type == "exp").Value;

        var doubleVal = Convert.ToDouble(expire);
        var dateAfterConvert = UnixTimeStampToDateTime(doubleVal);

        if (dateAfterConvert < DateTime.Now)
            return Ok(new TemplateApi<UserDto>(null, null, "Token đã hết hạn !", false, 0, 0, 0, 0));

        var userByEmail = await _unitOfWork.User.UserByEmail(profile);
        if (userByEmail?.Id is not null && userByEmail.IsLocked)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản đã bị khóa !", false, 0, 0, 0, 0));

        var listRoleOfUser = (await _unitOfWork.Role.GetRoleByIdUser(userByEmail!.Id)).ToList();

        var data = new DataGetUser()
        {
            Data = userByEmail.Adapt<UserDto>(),
            RoleList = listRoleOfUser,
            IsAdmin = listRoleOfUser.Any(e => e.IsAdmin)
        };

        return Ok(new TemplateApi<DataGetUser>(data, null, "Xác thực thành công !", true, 0, 0, 0, 0));
    }

    // HttpPut: /api/User/ActiveUserByCode
    [HttpPut("activeUserByCode")]
    public async Task<IActionResult> ActiveUserByCode(string email, string code)
    {
        var userByEmail = await _unitOfWork.User.UserByEmail(email);

        if (userByEmail.Id == Guid.Empty)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));

        if (userByEmail.Id != Guid.Empty && userByEmail.IsActive)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này đã được kích hoạt !", false, 0, 0, 0, 0));

        if (userByEmail?.ActiveCode != code)
            return Ok(new TemplateApi<UserDto>(null, null, "Vui lòng nhập đúng mã code !", false, 0, 0, 0, 0));

        var result = await _unitOfWork.User.ActiveUserByCode(email, code);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: /api/User/SendAgainCode
    [HttpPut("sendAgainCode")]
    public async Task<IActionResult> SendAgainCode(string email)
    {
        var userByEmail = await _unitOfWork.User.UserByEmail(email);

        if (userByEmail?.Id is not null)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));

        if (userByEmail?.Id is not null && userByEmail.IsActive)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này đã được kích hoạt !", false, 0, 0, 0, 0));

        var codeActive = userByEmail!.ActiveCode;
        var fromMail = AppSettings.FromMail;
        var password = AppSettings.PasswordMail;
        const string subject = "Vui lòng xác thực tài khoản !";
        var body =
            $"<div style='max-width: 700px; margin: auto; border: 10px solid #ddd; padding: 50px 20px; font-size: 110%;'>" +
            $"<h2 style='text-align: center; text-transform: uppercase;color: teal;'> Chào mừng bạn đến với kỉ yếu tỉnh ủy </h2>" +
            $"<p>Chúc mừng bạn đã đăng kí thành công tài khoản. Hãy lấy mã và kích hoạt tài khoản của bạn !</p>" +
            $"<a style='background: crimson; text-decoration: none; color: white; padding: 10px 20px; margin-left: 300px; display: inline-block;text-align:center'>{codeActive}</a>";

        //send mail confirm
        SendMail.SendMailAuto(fromMail, email, password, body, subject);

        return Ok(new TemplateApi<UserDto>(null, null, "Vui lòng kiểm tra mail của bạn !", true, 0, 0, 0, 0));
    }

    // HttpPut: /api/User/SendCodeWithAccountActivated
    [HttpPut("sendCodeWithAccountActivated")]
    public async Task<IActionResult> SendCodeWithAccountActivated(string email)
    {
        var userByEmail = await _unitOfWork.User.UserByEmail(email);

        if (userByEmail?.Id is not null)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));

        if (userByEmail?.Id is not null && userByEmail.IsActive)
        {
            var codeActive = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var result = await _unitOfWork.User.UpdateActiveCode(codeActive, userByEmail.Email);

            if (!result.Success)
            {
                return Ok(new TemplateApi<UserDto>(null, null, "Không thể tạo mã code !", false, 0, 0, 0, 0));
            }

            var fromMail = AppSettings.FromMail;
            var password = AppSettings.PasswordMail;
            const string subject = "Vui lòng xác thực tài khoản !";
            var body =
                $"<div style='max-width: 700px; margin: auto; border: 10px solid #ddd; padding: 50px 20px; font-size: 110%;'>" +
                $"<h2 style='text-align: center; text-transform: uppercase;color: teal;'> Chào mừng bạn đến với kỉ yếu tỉnh ủy </h2>" +
                $"<p>Chúc mừng bạn đã lấy thành công mã kich hoạt tài khoản. Hãy lấy mã và kích hoạt tài khoản của bạn !</p>" +
                $"<a style='background: crimson; text-decoration: none; color: white; padding: 10px 20px; margin-left: 300px; display: inline-block;text-align:center'>{codeActive}</a>";

            //send mail confirm
            SendMail.SendMailAuto(fromMail, email, password, body, subject);

            return Ok(new TemplateApi<UserDto>(null, null, "Vui lòng kiểm tra mail của bạn !", true, 0, 0, 0, 0));
        }

        return Ok(new TemplateApi<UserDto>(null, null,
            "Tài khoản của bạn chưa được kích hoạt hoặc gmail không chính xác !", false, 0, 0, 0, 0));
    }

    // HttpPost: /api/User/VerifyCode
    [HttpPost("verifyCode")]
    public async Task<IActionResult> VerifyCode(string code, string email)
    {
        var userByEmail = await _unitOfWork.User.UserByEmail(email);
        if (userByEmail?.Id is not null)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));

        if (userByEmail != null && userByEmail.IsActive && userByEmail.ActiveCode == code)
            return Ok(new TemplateApi<UserDto>(null, null, "Xác thực thành công !", true, 0, 0, 0, 0));

        return Ok(new TemplateApi<UserDto>(null, null, "Vui lòng nhập chính xác thông tin !", false, 0, 0, 0, 0));
    }

    // HttpPut: /api/User/ForgotPassWord
    [HttpPut("forgotPassWord")]
    public async Task<IActionResult> ForgotPassWord(string email, string newPassword)
    {
        var userByEmail = await _unitOfWork.User.UserByEmail(email);

        if (userByEmail is null)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));
        if (userByEmail.IsActive == false)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này chưa được kích hoạt !", false, 0, 0, 0, 0));

        string salt = BCryptNet.GenerateSalt();
        var passwordUser =
            BCryptNet.HashPassword(newPassword + AppSettings.SecretKey + salt);

        var result = await _unitOfWork.User.UpdatePassword(email, passwordUser, salt);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: /api/User/ChangePassWord
    [HttpPut("changePassWord")]
    public async Task<IActionResult> ChangePassWord(string email, string oldPassword, string newPassword)
    {
        var userByEmail = await _unitOfWork.User.UserByEmail(email);

        if (userByEmail is null)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này không tồn tại !", false, 0, 0, 0, 0));
        if (userByEmail.IsActive == false)
            return Ok(new TemplateApi<UserDto>(null, null, "Tài khoản này chưa được kích hoạt !", false, 0, 0, 0, 0));
        if (!BCryptNet.Verify(oldPassword + AppSettings.SecretKey + userByEmail.Salt, userByEmail.Password))
        {
            return Ok(new TemplateApi<UserDto>(null, null, "Mật khẩu cũ không chính xác !" + oldPassword, false, 0, 0, 0, 0));
        }


        string salt = BCryptNet.GenerateSalt();
        var passwordUser =
            BCryptNet.HashPassword(newPassword + AppSettings.SecretKey + salt);

        var result = await _unitOfWork.User.UpdatePassword(email, passwordUser, salt);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion

    #region ===[ GENERATE TOKEN ]=================================================================

    private static string GenerateToken(UserDto user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secretKeyBytes = Encoding.UTF8.GetBytes(AppSettings.SecretKey);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Fullname ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                SecurityAlgorithms.HmacSha512Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescription);
        var accessToken = jwtTokenHandler.WriteToken(token);

        return accessToken;
    }

    private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    #endregion
}