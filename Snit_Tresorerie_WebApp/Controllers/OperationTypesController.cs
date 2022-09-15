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
    //[MultiplePoliciesAuthorize("readOperationTypePolicy; writeOperationTypePolicy; manageOperationTypePolicy")]
    public class OperationTypesController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public OperationTypesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }


        [Authorize(Policy = "operationType.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var operationType = await _repository.OperationType.GetByIdAsync(id);

            if (operationType == null)
            {
                _logger.LogError($"OperationType with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"Vue détaillée : {operationType.Name}";
                _logger.LogInfo($"Returned operationTypeResponse with id: {id}");

                var operationTypeResponse = _mapper.Map<OperationTypeResponse>(operationType);
                return View(operationTypeResponse);
            }
        }


        [Authorize(Policy = "operationType.read.policy")]
        public async Task<PagedResponse<IEnumerable<OperationTypeResponse>>> ListOperationType([FromQuery] OperationTypeParameters operationTypeParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleName = User.FindFirstValue(ClaimTypes.Role);

            var operationTypes = await _repository.OperationType.GetPagedListAsync(operationTypeParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(operationTypes.MetaData));

            _logger.LogInfo($"Returned all operationTypes from database.");

            var operationTypesResponse = _mapper.Map<IEnumerable<OperationTypeResponse>>(operationTypes);


            var pagedOperationTypes = new PagedResponse<IEnumerable<OperationTypeResponse>>(operationTypesResponse, operationTypes.MetaData, 5);

            ViewBag.Title = "Liste des Types d'Opération";
            return pagedOperationTypes;
            //return operationTypesResponse;
        }


        [Authorize(Policy = "operationType.read.policy")]
        public async Task<IActionResult> Index([FromQuery] OperationTypeParameters operationTypeParameters)
        {
            return View(await ListOperationType(operationTypeParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "operationType.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            if (id != null)
            {
                var operationType = await _repository.OperationType.GetByIdAsync(id.Value);

                if (operationType == null)
                {
                    _logger.LogError($"OperationType with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned operationType with id: {id}");
                    var operationTypeRequest = _mapper.Map<OperationTypeRequest>(operationType);
                    return PartialView(operationTypeRequest);
                }
            }
            return PartialView(new OperationTypeRequest());
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
        [Authorize(Policy = "operationType.write.policy")]
        public async Task<IActionResult> SaveOperationType(Guid? id, OperationTypeRequest operationType)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var operationTypeEntity = _mapper.Map<OperationType>(operationType);

                    if (await _repository.OperationType.ExistAsync(operationTypeEntity))
                    {
                        ModelState.AddModelError("", "This OperationType exists already");
                        return base.ValidationProblem(ModelState);
                    }

                    await _repository.OperationType.CreateAsync(operationTypeEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var operationTypeEntity = await _repository.OperationType.GetByIdAsync(id.Value);
                    if (operationTypeEntity == null)
                    {
                        _logger.LogError($"OperationType with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    _mapper.Map(operationType, operationTypeEntity);

                    await _repository.OperationType.UpdateAsync(operationTypeEntity);
                    await _repository.SaveAsync();
                }

                var operationTypeParameters = new OperationTypeParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListOperationType(operationTypeParameters)) });
            }

            _logger.LogError("Invalid operationType object received.");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", operationType) });
        }



        [Authorize(Policy = "operationType.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var operationType = await _repository.OperationType.GetByIdAsync(id.Value);

            //ViewBag.Title = "Type d'Operation";

            if (operationType == null)
            {
                _logger.LogError($"OperationType with id: {id}, hasn't been found.");
                return NotFound();
            }
            var operationTypeResponse = _mapper.Map<OperationTypeResponse>(operationType);

            return PartialView(operationTypeResponse);
        }



        [HttpPost]
        [Authorize(Policy = "operationType.manage.policy")]
        public async Task<IActionResult> DeleteOperationType(Guid id)
        {
            var operationType = await _repository.OperationType.GetByIdAsync(id);
            await _repository.OperationType.DeleteAsync(operationType);

            await _repository.SaveAsync();
            var operationTypeParameters = new OperationTypeParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListOperationType(operationTypeParameters)) });
        }
    }
}
