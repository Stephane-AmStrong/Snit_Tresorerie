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
    //[MultiplePoliciesAuthorize("readPaymentTypePolicy; writePaymentTypePolicy; managePaymentTypePolicy")]
    public class PaymentTypesController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PaymentTypesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }


        [Authorize(Policy = "paymentType.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var paymentType = await _repository.PaymentType.GetByIdAsync(id);

            if (paymentType == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"Vue détaillée : {paymentType.Name}";
                _logger.LogInfo($"Returned paymentTypeResponse with id: {id}");

                var paymentTypeResponse = _mapper.Map<PaymentTypeResponse>(paymentType);
                return View(paymentTypeResponse);
            }
        }


        [Authorize(Policy = "paymentType.read.policy")]
        public async Task<PagedResponse<IEnumerable<PaymentTypeResponse>>> ListPaymentType([FromQuery] PaymentTypeParameters paymentTypeParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleName = User.FindFirstValue(ClaimTypes.Role);

            var paymentTypes = await _repository.PaymentType.GetPagedListAsync(paymentTypeParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paymentTypes.MetaData));

            _logger.LogInfo($"Returned all paymentTypes from database.");

            var paymentTypesResponse = _mapper.Map<IEnumerable<PaymentTypeResponse>>(paymentTypes);


            var pagedPaymentTypes = new PagedResponse<IEnumerable<PaymentTypeResponse>>(paymentTypesResponse, paymentTypes.MetaData, 5);

            ViewBag.Title = "Liste des PaymentTypes";
            return pagedPaymentTypes;
            //return paymentTypesResponse;
        }


        [Authorize(Policy = "paymentType.read.policy")]
        public async Task<IActionResult> Index([FromQuery] PaymentTypeParameters paymentTypeParameters)
        {
            return View(await ListPaymentType(paymentTypeParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "paymentType.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            if (id != null)
            {
                var paymentType = await _repository.PaymentType.GetByIdAsync(id.Value);

                if (paymentType == null)
                {
                    _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned paymentType with id: {id}");
                    var paymentTypeRequest = _mapper.Map<PaymentTypeRequest>(paymentType);
                    return PartialView(paymentTypeRequest);
                }
            }
            return PartialView(new PaymentTypeRequest());
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
        [Authorize(Policy = "paymentType.write.policy")]
        public async Task<IActionResult> SavePaymentType(Guid? id, PaymentTypeRequest paymentType)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var paymentTypeEntity = _mapper.Map<PaymentType>(paymentType);

                    if (await _repository.PaymentType.ExistAsync(paymentTypeEntity))
                    {
                        ModelState.AddModelError("", "This PaymentType exists already");
                        return base.ValidationProblem(ModelState);
                    }

                    await _repository.PaymentType.CreateAsync(paymentTypeEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var paymentTypeEntity = await _repository.PaymentType.GetByIdAsync(id.Value);
                    if (paymentTypeEntity == null)
                    {
                        _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    _mapper.Map(paymentType, paymentTypeEntity);

                    await _repository.PaymentType.UpdateAsync(paymentTypeEntity);
                    await _repository.SaveAsync();
                }

                var paymentTypeParameters = new PaymentTypeParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListPaymentType(paymentTypeParameters)) });
            }

            _logger.LogError("Invalid paymentType object received.");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", paymentType) });
        }



        [Authorize(Policy = "paymentType.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var paymentType = await _repository.PaymentType.GetByIdAsync(id.Value);

            ViewBag.Title = "PaymentType";

            if (paymentType == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }
            var paymentTypeResponse = _mapper.Map<PaymentTypeResponse>(paymentType);

            return PartialView(paymentTypeResponse);
        }



        [HttpPost]
        [Authorize(Policy = "paymentType.manage.policy")]
        public async Task<IActionResult> DeletePaymentType(Guid id)
        {
            var paymentType = await _repository.PaymentType.GetByIdAsync(id);
            await _repository.PaymentType.DeleteAsync(paymentType);

            await _repository.SaveAsync();
            var paymentTypeParameters = new PaymentTypeParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListPaymentType(paymentTypeParameters)) });
        }
    }
}
