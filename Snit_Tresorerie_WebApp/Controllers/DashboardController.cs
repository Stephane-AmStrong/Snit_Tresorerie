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
                    { "totalIntervenors", 0 },
                    { "totalAppUsers", 0},
                    { "totalSites", 0},
                    { "totalOperations", 0},
                };



            var intervenorParameters = new IntervenorParameters();
            var intervenors = await _repository.Intervenor.GetPagedListAsync(intervenorParameters);
            var totalIntervenors = intervenors.Count;

            while (intervenors.MetaData.HasNext)
            {
                intervenorParameters.PageNumber++;
                intervenors = await _repository.Intervenor.GetPagedListAsync(intervenorParameters);
                totalIntervenors += intervenors.Count;
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


            var operationParameters = new OperationParameters();
            var operations = await _repository.Operation.GetPagedListAsync(operationParameters);
            var totalOperations = operations.Count;

            while (operations.MetaData.HasNext)
            {
                operationParameters.PageNumber++;
                operations = await _repository.Operation.GetPagedListAsync(operationParameters);
                totalOperations += operations.Count;
            }



            statistics["totalIntervenors"] = totalIntervenors;
            statistics["totalAppUsers"] = totalAppUsers;
            statistics["totalSites"] = totalSites;
            statistics["totalOperations"] = totalOperations;

            ViewBag.Title = "Tableau de bord";
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