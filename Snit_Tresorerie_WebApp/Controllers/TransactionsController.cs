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
    //[MultiplePoliciesAuthorize("readTransactionPolicy; writeTransactionPolicy; manageTransactionPolicy")]
    public class TransactionsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private List<string> typesTransaction;

        public TransactionsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;

            /*
Type
             */


            typesTransaction = new List<string> {
                "Achat",
                "Vente"
            };

        }


        [Authorize(Policy = "transaction.read.policy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var transaction = await _repository.Transaction.GetByIdAsync(id);

            if (transaction == null)
            {
                _logger.LogError($"Transaction with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                ViewBag.Title = $"Vue détaillée : {transaction.Name}";
                _logger.LogInfo($"Returned transactionResponse with id: {id}");

                var transactionResponse = _mapper.Map<TransactionResponse>(transaction);
                return View(transactionResponse);
            }
        }


        [Authorize(Policy = "transaction.read.policy")]
        public async Task<PagedResponse<IEnumerable<TransactionResponse>>> ListTransaction([FromQuery] TransactionParameters transactionParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleName = User.FindFirstValue(ClaimTypes.Role);

            var transactions = await _repository.Transaction.GetPagedListAsync(transactionParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(transactions.MetaData));

            _logger.LogInfo($"Returned all transactions from database.");

            var transactionsResponse = _mapper.Map<IEnumerable<TransactionResponse>>(transactions);


            var pagedTransactions = new PagedResponse<IEnumerable<TransactionResponse>>(transactionsResponse, transactions.MetaData, 5);

            ViewBag.Title = "Liste des Transactions";
            return pagedTransactions;
            //return transactionsResponse;
        }


        [Authorize(Policy = "transaction.read.policy")]
        public async Task<IActionResult> Index([FromQuery] TransactionParameters transactionParameters)
        {
            return View(await ListTransaction(transactionParameters));
        }



        [NoDirectAccess]
        [Authorize(Policy = "transaction.write.policy")]
        public async Task<IActionResult> Form(Guid? id)
        {
            var actorParameters = new ActorParameters();
            var siteParameters = new SiteParameters();
            var paymentTypeParameters = new PaymentTypeParameters();

            if (id != null)
            {
                var transaction = await _repository.Transaction.GetByIdAsync(id.Value);

                if (transaction == null)
                {
                    _logger.LogError($"Transaction with id: {id}, hasn't been found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned transaction with id: {id}");
                    var transactionRequest = _mapper.Map<TransactionRequest>(transaction);

                    ViewData["Types"] = new SelectList(typesTransaction, transaction.Type);
                    ViewData["ActorId"] = new SelectList(from actor in await _repository.Actor.GetPagedListAsync(actorParameters) select new { actor.Id, FullName = actor.FirstName + " " + actor.LastName }, "Id", "FullName", transaction.ActorId);
                    ViewData["PaymentTypeId"] = new SelectList(await _repository.PaymentType.GetPagedListAsync(paymentTypeParameters), "Id", "Name", transaction.PaymentTypeId);
                    ViewData["SiteId"] = new SelectList(from site in await _repository.Site.GetPagedListAsync(siteParameters) select new { site.Id, FullName = site.Name + " " + site.Country + " " + site.Headquarters }, "Id", "FullName", transaction.SiteId);
                    return PartialView(transactionRequest);
                }
            }
            ViewData["Types"] = new SelectList(typesTransaction);
            ViewData["ActorId"] = new SelectList(from actor in await _repository.Actor.GetPagedListAsync(actorParameters) select new { actor.Id, FullName = actor.FirstName + " " + actor.LastName }, "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _repository.PaymentType.GetPagedListAsync(paymentTypeParameters), "Id", "Name");
            ViewData["SiteId"] = new SelectList(from site in await _repository.Site.GetPagedListAsync(siteParameters) select new { site.Id, FullName = site.Name + " " + site.Country + " " + site.Headquarters }, "Id", "FullName");
            return PartialView(new TransactionRequest());
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
        [Authorize(Policy = "transaction.write.policy")]
        public async Task<IActionResult> SaveTransaction(Guid? id, TransactionRequest transaction)
        {
            var actorParameters = new ActorParameters();
            var siteParameters = new SiteParameters();
            var paymentTypeParameters = new PaymentTypeParameters();

            transaction.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                //Insert
                if (id == null || id == new Guid())
                {
                    var transactionEntity = _mapper.Map<Transaction>(transaction);

                    if (await _repository.Transaction.ExistAsync(transactionEntity))
                    {
                        ModelState.AddModelError("", "This Transaction exists already");
                        return base.ValidationProblem(ModelState);
                    }

                    await _repository.Transaction.CreateAsync(transactionEntity);
                    await _repository.SaveAsync();
                }

                //Update
                else
                {
                    var transactionEntity = await _repository.Transaction.GetByIdAsync(id.Value);
                    if (transactionEntity == null)
                    {
                        _logger.LogError($"Transaction with id: {id}, hasn't been found.");
                        return NotFound();
                    }


                    _mapper.Map(transaction, transactionEntity);

                    await _repository.Transaction.UpdateAsync(transactionEntity);
                    await _repository.SaveAsync();
                }

                var transactionParameters = new TransactionParameters();

                return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListTransaction(transactionParameters)) });
            }

            _logger.LogError("Invalid transaction object received.");

            ViewData["Types"] = new SelectList(typesTransaction);
            ViewData["ActorId"] = new SelectList(from actor in await _repository.Actor.GetPagedListAsync(actorParameters) select new { actor.Id, FullName = actor.FirstName + " " + actor.LastName }, "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _repository.PaymentType.GetPagedListAsync(paymentTypeParameters), "Id", "Name");
            ViewData["SiteId"] = new SelectList(from site in await _repository.Site.GetPagedListAsync(siteParameters) select new { site.Id, FullName = site.Name + " " + site.Country + " " + site.Headquarters }, "Id", "FullName");

            return Json(new { isValid = false, html = RazorViewHelper.RenderRazorViewToString(this, "Form", transaction) });
        }



        [Authorize(Policy = "transaction.manage.policy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var transaction = await _repository.Transaction.GetByIdAsync(id.Value);

            ViewBag.Title = "Transaction";

            if (transaction == null)
            {
                _logger.LogError($"Transaction with id: {id}, hasn't been found.");
                return NotFound();
            }
            var transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return PartialView(transactionResponse);
        }



        [HttpPost]
        [Authorize(Policy = "transaction.manage.policy")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var transaction = await _repository.Transaction.GetByIdAsync(id);
            await _repository.Transaction.DeleteAsync(transaction);

            await _repository.SaveAsync();
            var transactionParameters = new TransactionParameters();
            return Json(new { isValid = true, html = RazorViewHelper.RenderRazorViewToString(this, "_ViewAll", await ListTransaction(transactionParameters)) });
        }
    }
}
