using AutoMapper;
using Contracts;
using Entities.RequestFeatures;
using Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Snit_Tresorerie_WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public DashboardController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var statistics = new Dictionary<string, int>
                {
                    { "totalActors", 0 },
                    { "totalAppUsers", 0},
                    { "totalSites", 0},
                    { "totalTransactions", 0},
                };



            var actorParameters = new ActorParameters();
            var actors = await _repository.Actor.GetPagedListAsync(actorParameters);
            var totalActors = actors.Count;

            while (actors.MetaData.HasNext)
            {
                actorParameters.PageNumber++;
                actors = await _repository.Actor.GetPagedListAsync(actorParameters);
                totalActors += actors.Count;
            }


            var appUserParameters = new AppUserParameters();
            var appUsers = await _repository.AppUser.GetPagedListAsync(appUserParameters);
            var totalAppUsers = appUsers.Count;

            while (appUsers.MetaData.HasNext)
            {
                appUserParameters.PageNumber++;
                appUsers = await _repository.AppUser.GetPagedListAsync(appUserParameters);
                totalAppUsers += appUsers.Count;
            }


            var siteParameters = new SiteParameters();
            var sites = await _repository.Site.GetPagedListAsync(siteParameters);
            var totalSites = sites.Count;

            while (sites.MetaData.HasNext)
            {
                siteParameters.PageNumber++;
                sites = await _repository.Site.GetPagedListAsync(siteParameters);
                totalSites += sites.Count;
            }


            var transactionParameters = new TransactionParameters();
            var transactions = await _repository.Transaction.GetPagedListAsync(transactionParameters);
            var totalTransactions = transactions.Count;

            while (transactions.MetaData.HasNext)
            {
                transactionParameters.PageNumber++;
                transactions = await _repository.Transaction.GetPagedListAsync(transactionParameters);
                totalTransactions += transactions.Count;
            }



            statistics["totalActors"] = totalActors;
            statistics["totalAppUsers"] = totalAppUsers;
            statistics["totalSites"] = totalSites;
            statistics["totalTransactions"] = totalTransactions;

            return View(statistics);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}