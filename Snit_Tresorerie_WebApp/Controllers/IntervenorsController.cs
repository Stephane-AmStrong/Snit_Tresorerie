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
    //[MultiplePoliciesAuthorize("readIntervenorPolicy; writeIntervenorPolicy; manageIntervenorPolicy")]
    public class IntervenorsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public IntervenorsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/Intervenor";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [Authorize(Policy = "intervenor.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var intervenor = await _repository.Intervenor.GetDetailsAsync(id);

            if (intervenor == null)
            {
                _logger.LogError($"Intervenor with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"{intervenor.FirstName} {intervenor.LastName}";

                _logger.LogInfo($"Returned intervenorResponse with id: {id}");

                var intervenorResponse = _mapper.Map<IntervenorResponse>(intervenor);

                if (!string.IsNullOrWhiteSpace(intervenorResponse.ImgLink)) intervenorResponse.ImgLink = $"{_baseURL}{intervenorResponse.ImgLink}";

                return View(intervenorResponse);
            }
        }


        private async Task<PagedResponse<IEnumerable<IntervenorResponse>>> ListIntervenor([FromQuery] IntervenorParameters intervenorParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var intervenors = await _repository.Intervenor.GetPagedListAsync(intervenorParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(intervenors.MetaData));

            _logger.LogInfo($"Returned all intervenors from database.");

            var intervenorsResponse = _mapper.Map<IEnumerable<IntervenorResponse>>(intervenors);

            /*
                    IEnumerable<int> integers = new List<int>() { 1, 2, 3, 4, 5 };
                    IEnumerable<string> strings = integers.Select(i => i.ToString());
             */

            foreach (var intervenorResponse in intervenorsResponse)
            {
                if (!string.IsNullOrWhiteSpace(intervenorResponse.ImgLink)) intervenorResponse.ImgLink = $"{_baseURL}{intervenorResponse.ImgLink}";
            }

            var pagedIntervenors = new PagedResponse<IEnumerable<IntervenorResponse>>(intervenorsResponse, intervenors.MetaData,5);

            ViewBag.Title = "Liste des Intervenants";
            return pagedIntervenors;
        }


        [Authorize(Policy = "intervenor.read.policy")]
        public async Task<IActionResult> Index([FromQuery] IntervenorParameters intervenorParameters)
        {
            return View(await ListIntervenor(intervenorParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "intervenor.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            if (id != null)
            {
                var intervenor = await _repository.Intervenor.GetByIdAsync(id.Value);

                if (intervenor == null)
                {
                    _logger.LogError($"Intervenor with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned intervenor with id: {id}");

                    var intervenorRequest = _mapper.Map<IntervenorRequest>(intervenor);
                    if (!string.IsNullOrWhiteSpace(intervenor.ImgLink)) intervenor.ImgLink = $"{_baseURL}{intervenor.ImgLink}";

                    return PartialView(intervenorRequest);
                }
            }

            return PartialView(new IntervenorRequest());
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
        [Authorize(Policy = "intervenor.write.policy")]
        public async Task<IActionResult> SaveIntervenor(Guid? id, IntervenorRequest intervenor)
        {
            intervenor.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var intervenorEntity = _mapper.Map<Intervenor>(intervenor);

                    if (await _repository.Intervenor.ExistAsync(intervenorEntity))
                    {
                        ModelState.AddModelError("", "This Intervenor exists already");
                        return base.ValidationProblem(ModelState);
                    }


                    // upload file
                    if (intervenor.ImgFile != null)
                    {
                        intervenorEntity.Id = Guid.NewGuid();
                        var downloadLink = await UploadFile(intervenor.ImgFile, intervenorEntity.Id.ToString());

                        if (string.IsNullOrEmpty(downloadLink))
                        {
                            ModelState.AddModelError("", "file upload failed");
                            return ValidationProblem(ModelState);
                        }
                        intervenor.Id = Guid.NewGuid();
                        intervenor.ImgLink = downloadLink;
                    }

                    _mapper.Map(intervenor, intervenorEntity);

                    await _repository.Intervenor.CreateAsync(intervenorEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var intervenorEntity = await _repository.Intervenor.GetByIdAsync(id.Value);
                    if (intervenorEntity == null)
                    {
                        _logger.LogError($"Intervenor with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    // upload file
                    if (intervenor.ImgFile != null)
                    {
                        var downloadLink = await UploadFile(intervenor.ImgFile, intervenorEntity.Id.ToString());

                        if (string.IsNullOrEmpty(downloadLink))
                        {
                            ModelState.AddModelError("", "file upload failed");
                            return ValidationProblem(ModelState);
                        }

                        intervenor.ImgLink = downloadLink;
                    }

                    _mapper.Map(intervenor, intervenorEntity);

                    await _repository.Intervenor.UpdateAsync(intervenorEntity);
                    await _repository.SaveAsync();
                }

                var intervenorParameters = new IntervenorParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListIntervenor(intervenorParameters)) });
            }

            _logger.LogError("Invalid intervenor object received.");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", intervenor) });
        }



        [Authorize(Policy = "intervenor.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var intervenor = await _repository.Intervenor.GetByIdAsync(id.Value);

            if (intervenor == null)
            {
                _logger.LogError($"Intervenor with id: {id}, hasn't been found.");
                return NotFound();
            }
            var intervenorResponse = _mapper.Map<IntervenorResponse>(intervenor);

            return PartialView(intervenorResponse);
        }



        [HttpPost]
        [Authorize(Policy = "intervenor.manage.policy")]
        public async Task<IActionResult> DeleteIntervenor(Guid id)
        {
            var intervenor = await _repository.Intervenor.GetByIdAsync(id);
            await _repository.Intervenor.DeleteAsync(intervenor);

            await _repository.SaveAsync();
            var intervenorParameters = new IntervenorParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListIntervenor(intervenorParameters)) });
        }
    }
}
