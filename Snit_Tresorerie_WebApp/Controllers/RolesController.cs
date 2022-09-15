using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.RequestFeatures;
using AutoMapper;
using Newtonsoft.Json;
using static Snit_Tresorerie_WebApp.Helpers.RazorViewHelper;
using Snit_Tresorerie_WebApp.Helpers;
using System.Security.Claims;
using Repository.Seeds;
using Microsoft.AspNetCore.Identity;

namespace Snit_Tresorerie_WebApp.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public RolesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }



        [Authorize(Policy = "role.read.policy")]
        public async Task<IActionResult> Details(string id)
        {
            var role = await _repository.Role.GetByIdAsync(id);

            if (role == null)
            {
                _logger.LogError($"Role with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = role.Name;

                _logger.LogInfo($"Returned roleResponse with id: {id}");


                var existingRoleClaims = await _repository.Role.GetClaimsAsync(role);
                var allClaims = _mapper.Map<IList<ClaimResponse>>(ClaimsStore.AllClaims);

                var allAppUser = await _repository.AppUser.GetPagedListAsync(new AppUserParameters { WithRoleName = role.Name });
                _logger.LogInfo($"Returned allAppUsers from database");

                var appUserInRole = _mapper.Map<IEnumerable<AppUserResponse>>(allAppUser);
                var roleResponse = _mapper.Map<RoleResponse>(role);

                foreach (var claim in allClaims)
                {
                    if (existingRoleClaims.Any(x => x.Type == claim.Type)) claim.IsSelected = true;
                }

                roleResponse.AppUsers = appUserInRole.ToArray();
                roleResponse.Claims = allClaims;
                roleResponse.Claims.OrderBy(x => x.Value);

                return View(roleResponse);
            }
        }



        public async Task<IEnumerable<RoleResponse>> ListRole([FromQuery] RoleParameters roleParameters)
        {
            var roles = await _repository.Role.GetPagedListAsync(roleParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(roles.MetaData));


            _logger.LogInfo($"Returned all roles from database.");

            var rolesResponse = _mapper.Map<IEnumerable<RoleResponse>>(roles);

            ViewBag.Title = "Liste des Profils";
            return rolesResponse;
        }



        [Authorize(Policy = "role.read.policy")]
        public async Task<IActionResult> Index([FromQuery] RoleParameters roleParameters)
        {
            return View(await ListRole(roleParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "role.write.policy")]
        public async Task<IActionResult> Form(string id)
        {
            if (id != null)
            {
                var role = await _repository.Role.GetByIdAsync(id);

                if (role == null)
                {
                    _logger.LogError($"Role with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned role with id: {id}");

                    var roleRequest = _mapper.Map<RoleRequest>(role);
                    return PartialView(roleRequest);
                }
            }

            return PartialView(new RoleRequest());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "role.write.policy")]
        public async Task<IActionResult> SaveRole(string id, RoleRequest role)
        {
            if (ModelState.IsValid)
            {
                var roleParameters = new RoleParameters();

                //Insert
                if (string.IsNullOrEmpty(id))
                {
                    role.Id = Guid.NewGuid().ToString();
                    var roleEntity = _mapper.Map<IdentityRole>(role);

                    if (await _repository.Role.ExistAsync(roleEntity))
                    {
                        ModelState.AddModelError("", "This Role exists already");
                        return base.ValidationProblem(ModelState);
                    }

                    await _repository.Role.CreateAsync(roleEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var roleEntity = await _repository.Role.GetByIdAsync(id);
                    if (roleEntity == null)
                    {
                        _logger.LogError($"Role with id: {id}, hasn't been found.");
                        return NotFound();
                    }

                    _mapper.Map(role, roleEntity);

                    await _repository.Role.UpdateAsync(roleEntity);
                    await _repository.SaveAsync();
                }

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListRole(roleParameters)) });
            }

            _logger.LogError("Invalid role object received.");
            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", role) });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRoleClaims(string id, RoleResponse role)
        {
            if (ModelState.IsValid)
            {
                var roleParameters = new RoleParameters();

                //removeClaims
                var roleEntity = await _repository.Role.GetByIdAsync(id);
                if (roleEntity == null)
                {
                    _logger.LogError($"Role with id: {id}, hasn't been found.");
                    return NotFound();
                }

                var existingRoleClaims = await _repository.Role.GetClaimsAsync(roleEntity);
                var claimsAdministrationSucceeded = await _repository.Role.RemoveClaimsSucceededAsync(roleEntity, existingRoleClaims);

                if (claimsAdministrationSucceeded)
                {
                    _logger.LogError($"Claims removing succeeded");

                    //claimsAdministrationSucceeded = await _repository.Role.AddClaimsSucceededAsync(roleEntity,
                    //    role.Claims.Where(x => x.IsSelected).Select(claim => new Claim(claim.Type, claim.Value)));

                    claimsAdministrationSucceeded = await _repository.Role.AddClaimsSucceededAsync(roleEntity,
                        _mapper.Map<IEnumerable<Claim>>(role.Claims.Where(x => x.IsSelected)));

                    if (claimsAdministrationSucceeded) return RedirectToAction("Details", "Roles", new { id = id });
                }
            }

            _logger.LogError($"Claims removing failled");
            ModelState.AddModelError("", "Cannot remove claims from this role");
            return Json("Cannot remove claims from this role");
        }



        [Authorize(Policy = "role.manage.policy")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return BadRequest();

            var role = await _repository.Role.GetByIdAsync(id);

            if (role == null)
            {
                _logger.LogError($"Role with id: {id}, hasn't been found.");
                return NotFound();
            }
            var roleResponse = _mapper.Map<RoleResponse>(role);

            return PartialView(roleResponse);
        }



        // POST: Roles/DeleteConfirmed/5
        [HttpPost]
        [Authorize(Policy = "role.manage.policy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _repository.Role.GetByIdAsync(id);
            await _repository.Role.DeleteAsync(role);
            await _repository.SaveAsync();
            var roleParameters = new RoleParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListRole(roleParameters)) });
        }
    }
}
