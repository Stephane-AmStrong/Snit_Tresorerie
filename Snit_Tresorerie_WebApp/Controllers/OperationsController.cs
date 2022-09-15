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
    //[MultiplePoliciesAuthorize("readOperationPolicy; writeOperationPolicy; manageOperationPolicy")]
    public class OperationsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public OperationsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }


        [Authorize(Policy = "operation.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var operation = await _repository.Operation.GetByIdAsync(id);

            if (operation == null)
            {
                _logger.LogError($"Operation with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"Vue détaillée : {operation.Name}";
                _logger.LogInfo($"Returned operationResponse with id: {id}");

                var operationResponse = _mapper.Map<OperationResponse>(operation);
                return View(operationResponse);
            }
        }


        [Authorize(Policy = "operation.read.policy")]
        public async Task<PagedResponse<IEnumerable<OperationResponse>>> ListOperation([FromQuery] OperationParameters operationParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleName = User.FindFirstValue(ClaimTypes.Role);

            var operations = await _repository.Operation.GetPagedListAsync(operationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(operations.MetaData));

            _logger.LogInfo($"Returned all operations from database.");

            var operationsResponse = _mapper.Map<IEnumerable<OperationResponse>>(operations);


            var pagedOperations = new PagedResponse<IEnumerable<OperationResponse>>(operationsResponse, operations.MetaData, 5);

            ViewBag.Title = "Liste des Opérations";
            return pagedOperations;
        }


        [Authorize(Policy = "operation.read.policy")]
        public async Task<IActionResult> Index([FromQuery] OperationParameters operationParameters)
        {
            return View(await ListOperation(operationParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "operation.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            var intervenorParameters = new IntervenorParameters();
            var paymentOptionParameters = new PaymentOptionParameters();
            var siteParameters = new SiteParameters();
            var operationTypeParameters = new OperationTypeParameters();

            if (id != null)
            {
                var operation = await _repository.Operation.GetByIdAsync(id.Value);

                if (operation == null)
                {
                    _logger.LogError($"Operation with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned operation with id: {id}");
                    var operationRequest = _mapper.Map<OperationRequest>(operation);

                    ViewData["IntervenorId"] = new SelectList(from intervenor in await _repository.Intervenor.GetPagedListAsync(intervenorParameters) select new { intervenor.Id, FullName = $"{intervenor.FirstName} {intervenor.LastName}" }, "Id", "FullName", operation.IntervenorId);
                    ViewData["PaymentOptionId"] = new SelectList(await _repository.PaymentOption.GetPagedListAsync(paymentOptionParameters), "Id", "Name", operation.PaymentOptionId);
                    ViewData["SiteId"] = new SelectList(from site in await _repository.Site.GetPagedListAsync(siteParameters) select new { site.Id, FullName = site.Name + " " + site.Country + " " + site.City }, "Id", "FullName", operation.SiteId);
                    ViewData["OperationTypeId"] = new SelectList(await _repository.OperationType.GetPagedListAsync(operationTypeParameters), "Id", "Name", operation.OperationTypeId);
                    return PartialView(operationRequest);
                }
            }
            ViewData["IntervenorId"] = new SelectList(from intervenor in await _repository.Intervenor.GetPagedListAsync(intervenorParameters) select new { intervenor.Id, FullName = $"{intervenor.FirstName} {intervenor.LastName}" }, "Id", "FullName");
            ViewData["PaymentOptionId"] = new SelectList(await _repository.PaymentOption.GetPagedListAsync(paymentOptionParameters), "Id", "Name");
            ViewData["SiteId"] = new SelectList(from site in await _repository.Site.GetPagedListAsync(siteParameters) select new { site.Id, FullName = site.Name + " " + site.Country + " " + site.City }, "Id", "FullName");
            ViewData["OperationTypeId"] = new SelectList(await _repository.OperationType.GetPagedListAsync(operationTypeParameters), "Id", "Name");
            return PartialView(new OperationRequest());
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
        [Authorize(Policy = "operation.write.policy")]
        public async Task<IActionResult> SaveOperation(Guid? id, OperationRequest operation)
        {
            var intervenorParameters = new IntervenorParameters();
            var operationTypeParameters = new OperationTypeParameters();
            var siteParameters = new SiteParameters();
            var paymentOptionParameters = new PaymentOptionParameters();

            operation.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var operationEntity = _mapper.Map<Operation>(operation);

                    if (await _repository.Operation.ExistAsync(operationEntity))
                    {
                        ModelState.AddModelError("", "This Operation exists already");
                        return base.ValidationProblem(ModelState);
                    }

                    await _repository.Operation.CreateAsync(operationEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var operationEntity = await _repository.Operation.GetByIdAsync(id.Value);
                    if (operationEntity == null)
                    {
                        _logger.LogError($"Operation with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    _mapper.Map(operation, operationEntity);

                    await _repository.Operation.UpdateAsync(operationEntity);
                    await _repository.SaveAsync();
                }

                var operationParameters = new OperationParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListOperation(operationParameters)) });
            }

            _logger.LogError("Invalid operation object received.");

            ViewData["IntervenorId"] = new SelectList(from intervenor in await _repository.Intervenor.GetPagedListAsync(intervenorParameters) select new { intervenor.Id, FullName = $"{intervenor.FirstName} {intervenor.LastName}" }, "Id", "FullName", operation.IntervenorId);
            ViewData["PaymentOptionId"] = new SelectList(await _repository.PaymentOption.GetPagedListAsync(paymentOptionParameters), "Id", "Name", operation.PaymentOptionId);
            ViewData["SiteId"] = new SelectList(from site in await _repository.Site.GetPagedListAsync(siteParameters) select new { site.Id, FullName = site.Name + " " + site.Country + " " + site.City }, "Id", "FullName", operation.SiteId);
            ViewData["OperationTypeId"] = new SelectList(await _repository.OperationType.GetPagedListAsync(operationTypeParameters), "Id", "Name", operation.OperationTypeId);

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", operation) });
        }



        [Authorize(Policy = "operation.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var operation = await _repository.Operation.GetByIdAsync(id.Value);

            if (operation == null)
            {
                _logger.LogError($"Operation with id: {id}, hasn't been found.");
                return NotFound();
            }
            var operationResponse = _mapper.Map<OperationResponse>(operation);

            return PartialView(operationResponse);
        }



        [HttpPost]
        [Authorize(Policy = "operation.manage.policy")]
        public async Task<IActionResult> DeleteOperation(Guid id)
        {
            var operation = await _repository.Operation.GetByIdAsync(id);
            await _repository.Operation.DeleteAsync(operation);

            await _repository.SaveAsync();
            var operationParameters = new OperationParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListOperation(operationParameters)) });
        }
    }
}
