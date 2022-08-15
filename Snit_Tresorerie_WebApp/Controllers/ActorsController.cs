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
using Snit_Tresorerie_WebApp.Wrapper;

namespace Snit_Tresorerie_WebApp.Controllers
{
    [Authorize]
    //[MultiplePoliciesAuthorize("readActorPolicy; writeActorPolicy; manageActorPolicy")]
    public class ActorsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public ActorsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/Actor";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [Authorize(Policy = "actor.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var actor = await _repository.Actor.GetByIdAsync(id);

            if (actor == null)
            {
                _logger.LogError($"Actor with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"{actor.FirstName} {actor.LastName}";

                _logger.LogInfo($"Returned actorResponse with id: {id}");

                var actorResponse = _mapper.Map<ActorResponse>(actor);

                if (!string.IsNullOrWhiteSpace(actorResponse.ImgLink)) actorResponse.ImgLink = $"{_baseURL}{actorResponse.ImgLink}";

                return View(actorResponse);
            }
        }


        private async Task<PagedResponse<IEnumerable<ActorResponse>>> ListActor([FromQuery] ActorParameters actorParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var actors = await _repository.Actor.GetPagedListAsync(actorParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(actors.MetaData));

            _logger.LogInfo($"Returned all actors from database.");

            var actorsResponse = _mapper.Map<IEnumerable<ActorResponse>>(actors);

            foreach (var actorResponse in actorsResponse)
            {
                if (!string.IsNullOrWhiteSpace(actorResponse.ImgLink)) actorResponse.ImgLink = $"{_baseURL}{actorResponse.ImgLink}";
            }

            var pagedActors = new PagedResponse<IEnumerable<ActorResponse>>(actorsResponse, actors.MetaData,5);

            ViewBag.Title = "Liste des Actors";
            return pagedActors;
        }


        [Authorize(Policy = "actor.read.policy")]
        public async Task<IActionResult> Index([FromQuery] ActorParameters actorParameters)
        {
            return View(await ListActor(actorParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "actor.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            if (id != null)
            {
                var actor = await _repository.Actor.GetByIdAsync(id.Value);

                if (actor == null)
                {
                    _logger.LogError($"Actor with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned actor with id: {id}");

                    var actorRequest = _mapper.Map<ActorRequest>(actor);
                    if (!string.IsNullOrWhiteSpace(actor.ImgLink)) actor.ImgLink = $"{_baseURL}{actor.ImgLink}";

                    return PartialView(actorRequest);
                }
            }

            return PartialView(new ActorRequest());
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
        [Authorize(Policy = "actor.write.policy")]
        public async Task<IActionResult> SaveActor(Guid? id, ActorRequest actor)
        {
            actor.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var actorEntity = _mapper.Map<Actor>(actor);

                    if (await _repository.Actor.ExistAsync(actorEntity))
                    {
                        ModelState.AddModelError("", "This Actor exists already");
                        return base.ValidationProblem(ModelState);
                    }


                    // upload file
                    if (actor.ImgFile != null)
                    {
                        actorEntity.Id = Guid.NewGuid();
                        var downloadLink = await UploadFile(actor.ImgFile, actorEntity.Id.ToString());

                        if (string.IsNullOrEmpty(downloadLink))
                        {
                            ModelState.AddModelError("", "file upload failed");
                            return ValidationProblem(ModelState);
                        }
                        actor.Id = Guid.NewGuid();
                        actor.ImgLink = downloadLink;
                    }

                    _mapper.Map(actor, actorEntity);

                    await _repository.Actor.CreateAsync(actorEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var actorEntity = await _repository.Actor.GetByIdAsync(id.Value);
                    if (actorEntity == null)
                    {
                        _logger.LogError($"Actor with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    // upload file
                    if (actor.ImgFile != null)
                    {
                        var downloadLink = await UploadFile(actor.ImgFile, actorEntity.Id.ToString());

                        if (string.IsNullOrEmpty(downloadLink))
                        {
                            ModelState.AddModelError("", "file upload failed");
                            return ValidationProblem(ModelState);
                        }

                        actor.ImgLink = downloadLink;
                    }

                    _mapper.Map(actor, actorEntity);

                    await _repository.Actor.UpdateAsync(actorEntity);
                    await _repository.SaveAsync();
                }

                var actorParameters = new ActorParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListActor(actorParameters)) });
            }

            _logger.LogError("Invalid actor object received.");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", actor) });
        }



        [Authorize(Policy = "actor.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var actor = await _repository.Actor.GetByIdAsync(id.Value);

            if (actor == null)
            {
                _logger.LogError($"Actor with id: {id}, hasn't been found.");
                return NotFound();
            }
            var actorResponse = _mapper.Map<ActorResponse>(actor);

            return PartialView(actorResponse);
        }



        [HttpPost]
        [Authorize(Policy = "actor.manage.policy")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            var actor = await _repository.Actor.GetByIdAsync(id);
            await _repository.Actor.DeleteAsync(actor);

            await _repository.SaveAsync();
            var actorParameters = new ActorParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListActor(actorParameters)) });
        }
    }
}
