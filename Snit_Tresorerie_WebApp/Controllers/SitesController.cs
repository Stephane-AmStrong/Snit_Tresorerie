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
using System.Security.Claims;
using System.Threading.Tasks;
using static Snit_Tresorerie_WebApp.Helpers.RazorViewHelper;
using Entities.Paging;

namespace Snit_Tresorerie_WebApp.Controllers
{
    [Authorize]
    //[MultiplePoliciesAuthorize("readSitePolicy; writeSitePolicy; manageSitePolicy")]
    public class SitesController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public SitesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }


        [Authorize(Policy = "site.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var site = await _repository.Site.GetByIdAsync(id);

            if (site == null)
            {
                _logger.LogError($"Site with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"Vue détaillée : {site.Name}";
                _logger.LogInfo($"Returned siteResponse with id: {id}");

                var siteResponse = _mapper.Map<SiteResponse>(site);
                return View(siteResponse);
            }
        }


        [Authorize(Policy = "site.read.policy")]
        public async Task<PagedResponse<IEnumerable<SiteResponse>>> ListSite([FromQuery] SiteParameters siteParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleName = User.FindFirstValue(ClaimTypes.Role);

            var sites = await _repository.Site.GetPagedListAsync(siteParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(sites.MetaData));

            _logger.LogInfo($"Returned all sites from database.");

            var sitesResponse = _mapper.Map<IEnumerable<SiteResponse>>(sites);


            var pagedSites = new PagedResponse<IEnumerable<SiteResponse>>(sitesResponse, sites.MetaData, 5);

            ViewBag.Title = "Liste des Sites";
            return pagedSites;
        }


        [Authorize(Policy = "site.read.policy")]
        public async Task<IActionResult> Index([FromQuery] SiteParameters siteParameters)
        {
            return View(await ListSite(siteParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "site.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            if (id != null)
            {
                var site = await _repository.Site.GetByIdAsync(id.Value);

                if (site == null)
                {
                    _logger.LogError($"Site with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned site with id: {id}");
                    var siteRequest = _mapper.Map<SiteRequest>(site);
                    return PartialView(siteRequest);
                }
            }
            return PartialView(new SiteRequest());
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
        [Authorize(Policy = "site.write.policy")]
        public async Task<IActionResult> SaveSite(Guid? id, SiteRequest site)
        {
            site.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var siteEntity = _mapper.Map<Site>(site);

                    if (await _repository.Site.ExistAsync(siteEntity))
                    {
                        ModelState.AddModelError("", "This Site exists already");
                        return base.ValidationProblem(ModelState);
                    }

                    await _repository.Site.CreateAsync(siteEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var siteEntity = await _repository.Site.GetByIdAsync(id.Value);
                    if (siteEntity == null)
                    {
                        _logger.LogError($"Site with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    _mapper.Map(site, siteEntity);

                    await _repository.Site.UpdateAsync(siteEntity);
                    await _repository.SaveAsync();
                }

                var siteParameters = new SiteParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListSite(siteParameters)) });
            }

            _logger.LogError("Invalid site object received.");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", site) });
        }



        [Authorize(Policy = "site.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var site = await _repository.Site.GetByIdAsync(id.Value);

            ViewBag.Title = "Site";

            if (site == null)
            {
                _logger.LogError($"Site with id: {id}, hasn't been found.");
                return NotFound();
            }
            var siteResponse = _mapper.Map<SiteResponse>(site);

            return PartialView(siteResponse);
        }



        [HttpPost]
        [Authorize(Policy = "site.manage.policy")]
        public async Task<IActionResult> DeleteSite(Guid id)
        {
            var site = await _repository.Site.GetByIdAsync(id);
            await _repository.Site.DeleteAsync(site);

            await _repository.SaveAsync();
            var siteParameters = new SiteParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListSite(siteParameters)) });
        }
    }
}
