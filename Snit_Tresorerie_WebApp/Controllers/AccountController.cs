using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Snit_Tresorerie_WebApp.Helpers;

using Microsoft.AspNetCore.Identity;
using Entities.ViewModels;

namespace Snit_Tresorerie_WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public AccountController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }




        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            var userCount = await _repository.Account.CountUsersAsync();
            if (userCount < 1) return RedirectToAction("RegisterAdmin", "Account"); 
            
            return View();
        }




        [AllowAnonymous]
        public async Task<ActionResult<AppUserResponse>> RegisterForm(AppUserViewModel appUser)
        {
            if (await GetUsersCount() < 1)
            {
                ModelState.AddModelError("", "create an adminUser account first");
                return View(ModelState);
            }

            var roles = new List<IdentityRole>();

            var organizerprofil = await _repository.Role.GetByNameAsync("Organisateur");
            var clientProfil = await _repository.Role.GetByNameAsync("Client");

            roles.Add(organizerprofil);
            roles.Add(clientProfil);

            ViewData["RoleId"] = new SelectList(roles, "Id", "Name");
            return PartialView("_RegisterForm");
        }





        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin()
        {
            if (await GetUsersCount() >= 1)
            {
                ModelState.AddModelError("", "Admin registration shortcut is no longer available");
                return View(ModelState);
            }
            return View();
        }




        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }





        private async Task<int> GetUsersCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var GivenName = User.FindFirstValue(ClaimTypes.GivenName);
            var Surname = User.FindFirstValue(ClaimTypes.Surname);

            _logger.LogInfo($"Count users of database.");
            return (await _repository.Account.CountUsersAsync());
        }





        #region EmailConfirmation



        [HttpGet]
        [AllowAnonymous]
        public IActionResult SuccessRegistration()
        {
            return View();
        }



        [HttpGet]
        public async Task<ActionResult> SendVerificationEmailTest()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("userId or token invalid");

            await SendVerificationEmail(userId);

            return Ok("Verification email sent successfully");
        }


        //POST api/authentications/login
        [HttpPost("resend-verification-email")]
        public async Task<ActionResult> ResendVerificationEmail()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("userId or token invalid");

            await SendVerificationEmail(userId);

            return Ok("Verification email sent successfully");
        }




        private async Task SendVerificationEmail(string userId)
        {
            var user = await _repository.AppUser.GetByIdAsync(userId);
            var token = await _repository.Account.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = await _repository.Account.EncodeTokenAsync(token);

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token = encodedToken, userId }, Request.Scheme);

            var userResponse = _mapper.Map<AppUserResponse>(user);

            var welcomeEmailVM = new WelcomeEmailViewModel
            {
                User = userResponse,
                ConfirmationLink = confirmationLink,
            };

            var htmlMessage = RazorViewHelper.RenderRazorViewToString(this, "WelcomeEmailTemplate", welcomeEmailVM);

            var message = new Message(new string[] { user.Email }, "Confirmation email link", htmlMessage, null);

            await _repository.EmailSender.SendAsync(message);
        }






        [HttpGet]
        [AllowAnonymous]
        public IActionResult WelcomeEmailTemplate(WelcomeEmailViewModel welcomeEmailVM)
        {
            return View(welcomeEmailVM);
        }





        //POST api/authentications/login
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest("userId or token invalid");

            var result = await _repository.Account.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return View("ConfirmEmail", $"{result.UserInfo["Email"]}");
            }

            return BadRequest(result);
        }

        #endregion


        //POST api/account/user/registration
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserResponse>> Register(AppUserViewModel appUser)
        {
            if (!ModelState.IsValid)
            {
                var roles = new List<IdentityRole>();

                var userProfil = await _repository.Role.GetByNameAsync("User");

                roles.Add(userProfil);

                ViewData["RoleId"] = new SelectList(roles, "Id", "Name");

                return View(appUser);
            }

            if (await GetUsersCount() < 1)
            {
                ModelState.AddModelError("", "create an adminUser account first");
                return View(appUser);
            }

            _logger.LogInfo($"Registration attempt by : {appUser.FirstName } {appUser.LastName }");

            var role = await _repository.Role.GetByIdAsync(appUser.RoleId);
            if (role == null) return NotFound("Role not found");

            appUser.Id = Guid.NewGuid().ToString();
            var userWithoutRole = _mapper.Map<AppUser>(appUser);
            var result = await _repository.Account.RegisterUserAsync(userWithoutRole, appUser.Password);

            var userResponse = _mapper.Map<AppUserResponse>(userWithoutRole);


            if (result.IsSuccess)
            {
                var roleAssignationResult = await _repository.Account.AddToRoleAsync(userWithoutRole, role);

                if (roleAssignationResult.IsSuccess)
                {
                    var userRoles = await _repository.Account.GetUsersRolesAsync(userWithoutRole);
                    //if (userRoles.Any()) userResponse.Role = _mapper.Map<IdentityRole, RoleResponse>(await _repository.Role.GetByNameAsync(userRoles.First()));

                    _logger.LogInfo($"Registration was successful");

                    //email verification should be enable later
                    await SendVerificationEmail(userResponse.Id);
                    return RedirectToAction("SuccessRegistration", "Account");
                    //return RedirectToAction("Login", "Account");
                }
                else
                {
                    var errorMessage = "";
                    foreach (var error in roleAssignationResult.ErrorDetails)
                    {
                        ModelState.AddModelError("", error);
                        errorMessage += error + ";";
                    }

                    _logger.LogError($"Registration failed ErrorMessage : {errorMessage}");
                    var roles = new List<IdentityRole>();

                    var studentProfil = await _repository.Role.GetByNameAsync("Etudiant");
                    var universityProfil = await _repository.Role.GetByNameAsync("Administrateur d'université");

                    roles.Add(studentProfil);
                    roles.Add(universityProfil);

                    ViewData["RoleId"] = new SelectList(roles, "Id", "Name");

                    return View(appUser);
                }
            }
            else
            {
                var errorMessage = "";
                foreach (var error in result.ErrorDetails)
                {
                    ModelState.AddModelError("", error);
                    errorMessage += error + ";";
                }
                _logger.LogError($"Registration failed ErrorMessage : {errorMessage}");
                //return ValidationProblem(ModelState);

                var roles = new List<IdentityRole>();

                var studentProfil = await _repository.Role.GetByNameAsync("Etudiant");
                var universityProfil = await _repository.Role.GetByNameAsync("Administrateur d'université");

                roles.Add(studentProfil);
                roles.Add(universityProfil);

                ViewData["RoleId"] = new SelectList(roles, "Id", "Name");

                return View(appUser);
            }
        }




        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserResponse>> RegisterAdmin(AppUserViewModel appUser)
        {
            if (await GetUsersCount() >= 1) return BadRequest("Admin registration shortcut is no longer available");

            if (!ModelState.IsValid) return View(appUser);

            var role = await _repository.Role.GetByNameAsync("SuperAdmin");
            if (role == null) return NotFound("Role not found");

            appUser.Id = Guid.NewGuid().ToString();
            var adminUser = _mapper.Map<AppUser>(appUser);
            var result = await _repository.Account.RegisterUserAsync(adminUser, appUser.Password);

            if (result.IsSuccess)
            {
                var roleAssignationResult = await _repository.Account.AddToRoleAsync(adminUser, role);

                if (roleAssignationResult.IsSuccess)
                {
                    _logger.LogInfo($"Registration was successful");

                    //email verification should be enable later
                    await SendVerificationEmail(adminUser.Id);
                    return RedirectToAction("SuccessRegistration", "Account");
                    //return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in roleAssignationResult.ErrorDetails)
                    {
                        ModelState.AddModelError(error, error);
                    }
                    //return ValidationProblem(ModelState);
                    return View(appUser);
                }
            }
            else
            {
                foreach (var error in result.ErrorDetails)
                {
                    ModelState.AddModelError(error, error);
                }
                //return ValidationProblem(ModelState);
                return View(appUser);
            }
        }




        //POST api/account/login
        [HttpPost, ActionName("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequest loginRequest, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInfo($"Authentication attempt");

                var loginModel = _mapper.Map<LoginModel>(loginRequest);
                var authenticationModel = await _repository.Account.LogInAsync(loginModel, loginModel.Password);

                if (authenticationModel.IsSuccess)
                {
                    _logger.LogInfo($"User Named: {authenticationModel.UserInfo["Name"]} has logged in successfully");

                    var authenticationResponse = _mapper.Map<AuthenticationResponse>(authenticationModel);

                    if (authenticationResponse.UserInfo["ImgUrl"] != null) authenticationResponse.UserInfo["ImgUrl"] = $"{_baseURL}{authenticationResponse.UserInfo["ImgUrl"]}";

                    var userCount = await _repository.Account.CountUsersAsync();

                    if (userCount == 1)
                    {
                        if (!string.IsNullOrEmpty(returnUrl)) return LocalRedirect(returnUrl);

                        return RedirectToAction("Index", "Dashboard");
                    }

                    if (!string.IsNullOrEmpty(returnUrl)) return LocalRedirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
                else if (authenticationModel.AppUser != null && !(await _repository.Account.IsEmailConfirmedAsync(authenticationModel.AppUser)))
                {
                    _logger.LogError($"Authentication failed : {authenticationModel.Message}");

                    ModelState.AddModelError("", "Veuillez confirmer votre email");
                    return View(loginRequest);
                }

                _logger.LogError($"Authentication failed : {authenticationModel.Message}");
                //return NotFound(authenticationModel.Message);
                ModelState.AddModelError("", authenticationModel.Message);
                return View(loginRequest);
            }

            //return ValidationProblem(ModelState);
            return View(loginRequest);
        }



        [HttpPost, ActionName("Logout")]

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _repository.Account.LogOutAsync();
            return RedirectToAction("Index", "Dashboard");
        }


        #region Forgot Password
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);

            var user = await _repository.Account.FindByEmailAsync(forgotPasswordModel.Email);

            if (user == null) return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _repository.Account.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _repository.EmailSender.SendAsync(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await _repository.Account.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await _repository.Account.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View();
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
    }
}
