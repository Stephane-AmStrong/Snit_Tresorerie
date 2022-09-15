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
    //[MultiplePoliciesAuthorize("readPaymentOptionPolicy; writePaymentOptionPolicy; managePaymentOptionPolicy")]
    public class PaymentOptionsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PaymentOptionsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }


        [Authorize(Policy = "paymentOption.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var paymentOption = await _repository.PaymentOption.GetByIdAsync(id);

            if (paymentOption == null)
            {
                _logger.LogError($"PaymentOption with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"Vue détaillée : {paymentOption.Name}";
                _logger.LogInfo($"Returned paymentOptionResponse with id: {id}");

                var paymentOptionResponse = _mapper.Map<PaymentOptionResponse>(paymentOption);
                return View(paymentOptionResponse);
            }
        }


        [Authorize(Policy = "paymentOption.read.policy")]
        public async Task<PagedResponse<IEnumerable<PaymentOptionResponse>>> ListPaymentOption([FromQuery] PaymentOptionParameters paymentOptionParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleName = User.FindFirstValue(ClaimTypes.Role);

            var paymentOptions = await _repository.PaymentOption.GetPagedListAsync(paymentOptionParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paymentOptions.MetaData));

            _logger.LogInfo($"Returned all paymentOptions from database.");

            var paymentOptionsResponse = _mapper.Map<IEnumerable<PaymentOptionResponse>>(paymentOptions);


            var pagedPaymentOptions = new PagedResponse<IEnumerable<PaymentOptionResponse>>(paymentOptionsResponse, paymentOptions.MetaData, 5);

            ViewBag.Title = "Liste des Modes de Paiement";
            return pagedPaymentOptions;
        }


        [Authorize(Policy = "paymentOption.read.policy")]
        public async Task<IActionResult> Index([FromQuery] PaymentOptionParameters paymentOptionParameters)
        {
            return View(await ListPaymentOption(paymentOptionParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "paymentOption.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            if (id != null)
            {
                var paymentOption = await _repository.PaymentOption.GetByIdAsync(id.Value);

                if (paymentOption == null)
                {
                    _logger.LogError($"PaymentOption with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned paymentOption with id: {id}");
                    var paymentOptionRequest = _mapper.Map<PaymentOptionRequest>(paymentOption);
                    return PartialView(paymentOptionRequest);
                }
            }
            return PartialView(new PaymentOptionRequest());
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
        [Authorize(Policy = "paymentOption.write.policy")]
        public async Task<IActionResult> SavePaymentOption(Guid? id, PaymentOptionRequest paymentOption)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var paymentOptionEntity = _mapper.Map<PaymentOption>(paymentOption);

                    if (await _repository.PaymentOption.ExistAsync(paymentOptionEntity))
                    {
                        ModelState.AddModelError("", "This PaymentOption exists already");
                        return base.ValidationProblem(ModelState);
                    }

                    await _repository.PaymentOption.CreateAsync(paymentOptionEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var paymentOptionEntity = await _repository.PaymentOption.GetByIdAsync(id.Value);
                    if (paymentOptionEntity == null)
                    {
                        _logger.LogError($"PaymentOption with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    _mapper.Map(paymentOption, paymentOptionEntity);

                    await _repository.PaymentOption.UpdateAsync(paymentOptionEntity);
                    await _repository.SaveAsync();
                }

                var paymentOptionParameters = new PaymentOptionParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListPaymentOption(paymentOptionParameters)) });
            }

            _logger.LogError("Invalid paymentOption object received.");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", paymentOption) });
        }



        [Authorize(Policy = "paymentOption.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var paymentOption = await _repository.PaymentOption.GetByIdAsync(id.Value);

            if (paymentOption == null)
            {
                _logger.LogError($"PaymentOption with id: {id}, hasn't been found.");
                return NotFound();
            }
            var paymentOptionResponse = _mapper.Map<PaymentOptionResponse>(paymentOption);

            return PartialView(paymentOptionResponse);
        }



        [HttpPost]
        [Authorize(Policy = "paymentOption.manage.policy")]
        public async Task<IActionResult> DeletePaymentOption(Guid id)
        {
            var paymentOption = await _repository.PaymentOption.GetByIdAsync(id);
            await _repository.PaymentOption.DeleteAsync(paymentOption);

            await _repository.SaveAsync();
            var paymentOptionParameters = new PaymentOptionParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListPaymentOption(paymentOptionParameters)) });
        }
    }
}
