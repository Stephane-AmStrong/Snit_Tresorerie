using Snit_Tresorerie_WebApp.Helpers;
using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Snit_Tresorerie_WebApp.Helpers.RazorViewHelper;
using System.Security.Claims;
using Entities.ViewModels;

namespace Snit_Tresorerie_WebApp.Controllers
{
    [Authorize]
    public class AppUsersOldController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public AppUsersOldController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/AppUser";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }



        public async Task<IActionResult> Details(string id)
        {
            var appUser = await _repository.AppUser.GetDetailsAsync(id);

            if (appUser == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"{appUser.FirstName} {appUser.LastName}";

                _logger.LogInfo($"Returned appUserResponse with id: {id}");

                var appUserDto = _mapper.Map<AppUserResponse>(appUser);

                if (!string.IsNullOrWhiteSpace(appUserDto.ImgLink)) appUserDto.ImgLink = $"{_baseURL}{appUserDto.ImgLink}";

                return View(appUserDto);
            }
        }



        public async Task<IEnumerable<AppUserResponse>> ListAppUser([FromQuery] AppUserParameters appUserParameters)
        {
            var appUsers = await _repository.AppUser.GetPagedListAsync(appUserParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(appUsers.MetaData));


            _logger.LogInfo($"Returned all appUsers from database.");

            var appUsersResponse = _mapper.Map<IEnumerable<AppUserResponse>>(appUsers);

            foreach (var appUserResponse in appUsersResponse)
            {
                if (!string.IsNullOrWhiteSpace(appUserResponse.ImgLink)) appUserResponse.ImgLink = $"{_baseURL}{appUserResponse.ImgLink}";
            }

            return appUsersResponse;
        }



        public async Task<IActionResult> Index([FromQuery] AppUserParameters appUserParameters)
        {
            return View(await ListAppUser(appUserParameters));
        }



        [NoDirectAccess]
        public async Task<IActionResult> Form(string id)
        {
            var roleParameters = new RoleParameters();

            if (id != null)
            {
                var appUser = await _repository.AppUser.GetByIdAsync(id);

                if (appUser == null)
                {
                    _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned appUser with id: {id}");

                    ViewData["RoleId"] = new SelectList(await _repository.Role.GetPagedListAsync(roleParameters), "Id", "Name");

                    var appUserViewModel = _mapper.Map<AppUserViewModel>(appUser);
                    //
                    var userRoles = await _repository.Account.GetUsersRolesAsync(appUser);
                    if (userRoles.Any())
                    {
                        var roleResponse = _mapper.Map<RoleResponse>(await _repository.Role.GetByNameAsync(userRoles.First()));
                        appUserViewModel.RoleId = roleResponse.Id;
                        //appUserViewModel.RoleName = roleResponse.Name;


                        ViewData["RoleId"] = new SelectList(await _repository.Role.GetPagedListAsync(roleParameters), "Id", "Name", appUserViewModel.RoleId);
                    }

                    //
                    return PartialView(appUserViewModel);
                }
            }

            ViewData["RoleId"] = new SelectList(await _repository.Role.GetPagedListAsync(roleParameters), "Id", "Name");

            return PartialView(new AppUserViewModel { Password = "A la connexion que l'utilisateur passe par la procedure du mot de passe oublié" });
        }


        private async Task<string> UploadFile(IFormFile file, string fileName)
        {
            if (file != null)
            {
                _repository.File.FilePath = fileName;
                return await _repository.File.UploadFile(file);
            }
            return null;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAppUser(Guid? id, AppUserViewModel appUser)
        {
            if (await _repository.Account.CountUsersAsync() < 1)
            {
                ModelState.AddModelError("", "create an adminUser account first");
                return ValidationProblem(ModelState);
            }

            var roleParameters = new RoleParameters();
            
            if (ModelState.IsValid)
            {

                //Insert
                if (id == null || id == new Guid())
                {
                    _logger.LogInfo($"Registration attempt for : {appUser.FirstName } {appUser.LastName }");

                    var role = await _repository.Role.GetByIdAsync(appUser.RoleId);
                    if (role == null) return NotFound("Role not found");

                    appUser.Id = Guid.NewGuid().ToString();
                    var userWithoutRole = _mapper.Map<AppUser>(appUser);

                    ViewData["RoleId"] = new SelectList(await _repository.Role.GetPagedListAsync(roleParameters), "Id", "Name", appUser.RoleId);

                    // upload file
                    if (appUser.ImgFile != null)
                    {
                        appUser.Id = Guid.NewGuid().ToString();
                        var downloadLink = await UploadFile(appUser.ImgFile, appUser.Id.ToString());

                        if (string.IsNullOrEmpty(downloadLink))
                        {
                            ModelState.AddModelError("", "file upload failed");
                            return ValidationProblem(ModelState);
                        }
                        userWithoutRole.ImgLink = downloadLink;
                    }

                    var result = await _repository.Account.RegisterUserAsync(userWithoutRole, appUser.Password);

                    var userResponse = _mapper.Map<AppUserResponse>(userWithoutRole);


                    if (result.IsSuccess)
                    {
                        var roleAssignationResult = await _repository.Account.AddToRoleAsync(userWithoutRole, role);

                        if (roleAssignationResult.IsSuccess)
                        {
                            var userRoles = await _repository.Account.GetUsersRolesAsync(userWithoutRole);
                            //if (userRoles.Any()) userResponse.Role = _mapper.Map<RoleResponse>(await _repository.Role.GetByNameAsync(userRoles.First()));

                            _logger.LogInfo($"Registration was successful");

                            //email verification should be enable later
                            //await SendVerificationEmail(userResponse.Id);
                        }
                        else
                        {
                            foreach (var error in roleAssignationResult.ErrorDetails)
                            {
                                ModelState.AddModelError(error, error);
                            }
                            return ValidationProblem(ModelState);
                        }
                    }
                    else
                    {
                        foreach (var error in result.ErrorDetails)
                        {
                            ModelState.AddModelError(error, error);
                        }
                        _logger.LogError($"Registration failed ErrorMessage : {result.ErrorDetails}");
                        return ValidationProblem(ModelState);
                    }
                }

                //Update
                else
                {
                    _logger.LogInfo($"Registration attempt for : {appUser.FirstName } {appUser.LastName }");

                    var role = await _repository.Role.GetByIdAsync(appUser.RoleId);
                    if (role == null) return NotFound("Role not found");

                    var userWithoutRole = await _repository.AppUser.GetByIdAsync(id.ToString());
                    if (userWithoutRole == null)
                    {
                        _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                        return NotFound();
                    }

                    _mapper.Map(appUser, userWithoutRole);

                    ViewData["RoleId"] = new SelectList(await _repository.Role.GetPagedListAsync(roleParameters), "Id", "Name", appUser.RoleId);

                    // upload file
                    if (appUser.ImgFile != null)
                    {
                        var downloadLink = await UploadFile(appUser.ImgFile, appUser.Id.ToString());

                        if (string.IsNullOrEmpty(downloadLink))
                        {
                            ModelState.AddModelError("", "file upload failed");
                            return ValidationProblem(ModelState);
                        }
                        userWithoutRole.ImgLink = downloadLink;
                    }

                    var result = await _repository.Account.UpdateUserAsync(userWithoutRole);

                    var userResponse = _mapper.Map<AppUserResponse>(userWithoutRole);


                    if (result.IsSuccess)
                    {
                        var roleAssignationResult = await _repository.Account.AddToRoleAsync(userWithoutRole, role);

                        if (roleAssignationResult.IsSuccess)
                        {
                            var userRoles = await _repository.Account.GetUsersRolesAsync(userWithoutRole);
                            if (userRoles.Any()) userResponse.Role = _mapper.Map<RoleResponse>(await _repository.Role.GetByNameAsync(userRoles.First()));

                            _logger.LogInfo($"Registration was successful");

                            //email verification should be enable later
                            //await SendVerificationEmail(userResponse.Id);
                        }
                        else
                        {
                            foreach (var error in roleAssignationResult.ErrorDetails)
                            {
                                ModelState.AddModelError(error, error);
                            }
                            return ValidationProblem(ModelState);
                        }
                    }
                    else
                    {
                        foreach (var error in result.ErrorDetails)
                        {
                            ModelState.AddModelError(error, error);
                        }
                        _logger.LogError($"Registration failed ErrorMessage : {result.ErrorDetails}");
                        return ValidationProblem(ModelState);
                    }
                }

                var appUserParameters = new AppUserParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListAppUser(appUserParameters)) });
            }

            _logger.LogError("Invalid appUser object received.");

            ViewData["RoleId"] = new SelectList(await _repository.Role.GetPagedListAsync(roleParameters), "Id", "Name");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", appUser) });
        }



        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return BadRequest();

            var appUser = await _repository.AppUser.GetByIdAsync(id);

            if (appUser == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }
            var appUserResponse = _mapper.Map<AppUserResponse>(appUser);

            return PartialView(appUserResponse);
        }



        // POST: AppUsers/DeleteConfirmed/5
        [HttpPost]
        public async Task<IActionResult> DeleteAppUser(string id)
        {
            var appUser = await _repository.AppUser.GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(appUser.ImgLink))
            {
                appUser.ImgLink = $"{_baseURL}{appUser.ImgLink}";
                await _repository.File.DeleteFile(appUser.ImgLink);
            }

            await _repository.AppUser.DeleteAsync(appUser);
            await _repository.SaveAsync();
            //return Ok("OK");
            var appUserParameters = new AppUserParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListAppUser(appUserParameters)) });
        }
    }
}
