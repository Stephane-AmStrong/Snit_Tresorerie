using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IFileRepository _fileRepository;
        private IAccountRepository _authenticationRepository;
        private IIntervenorRepository _intervenorRepository;
        private IAppUserRepository _appUser;
        private IPaymentOptionRepository _paymentOptionRepository;
        private ISiteRepository _siteRepository;
        private IOperationRepository _operationRepository;
        private IOperationTypeRepository _operationTypeRepository;
        //private IDinkToPdfRepository _dinkToPdfRepository;

        private IEmailSenderRepository _emailSender;
        private IWebHostEnvironment _webHostEnvironment;
        private IRoleRepository _role;

        //private readonly IConverter _converter;
        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        //private IOptions<EmailSettings> _emailSettings;

        private readonly ISortHelper<AppUser> _appUserSortHelper;
        private readonly ISortHelper<Intervenor> _intervenorSortHelper;
        private readonly ISortHelper<PaymentOption> _paymentOptionSortHelper;
        private readonly ISortHelper<Site> _siteSortHelper;
        private readonly ISortHelper<Operation> _operationSortHelper;
        private readonly ISortHelper<OperationType> _operationTypeSortHelper;
        private readonly ISortHelper<IdentityRole> _roleSortHelper;

        private RepositoryContext _repoContext;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly EmailConfiguration _emailConfig;

        private string filePath;
        public string Path
        {
            set { filePath = value; }
        }

        public IFileRepository File
        {
            get
            {
                if (_fileRepository == null)
                {
                    _fileRepository = new FileRepository(_webHostEnvironment, filePath);
                }
                return _fileRepository;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_authenticationRepository == null)
                {
                    _authenticationRepository = new AccountRepository(_repoContext, _userManager, _roleManager, _configuration, _httpContextAccessor);
                }
                return _authenticationRepository;
            }
        }

        public IIntervenorRepository Intervenor
        {
            get
            {
                if (_intervenorRepository == null)
                {
                    _intervenorRepository = new IntervenorRepository(_repoContext, _intervenorSortHelper);
                }
                return _intervenorRepository;
            }
        }

        public IPaymentOptionRepository PaymentOption
        {
            get
            {
                if (_paymentOptionRepository == null)
                {
                    _paymentOptionRepository = new PaymentOptionRepository(_repoContext, _paymentOptionSortHelper);
                }
                return _paymentOptionRepository;
            }
        }

        public IOperationRepository Operation
        {
            get
            {
                if (_operationRepository == null)
                {
                    _operationRepository = new OperationRepository(_repoContext, _operationSortHelper);
                }
                return _operationRepository;
            }
        }

        public IOperationTypeRepository OperationType
        {
            get
            {
                if (_operationTypeRepository == null)
                {
                    _operationTypeRepository = new OperationTypeRepository(_repoContext, _operationTypeSortHelper);
                }
                return _operationTypeRepository;
            }
        }

        public ISiteRepository Site
        {
            get
            {
                if (_siteRepository == null)
                {
                    _siteRepository = new SiteRepository(_repoContext, _siteSortHelper);
                }
                return _siteRepository;
            }
        }

        //public IDinkToPdfRepository PdfService
        //{
        //    get
        //    {
        //        if (_dinkToPdfRepository == null)
        //        {
        //            _dinkToPdfRepository = new DinkToPdfRepository(_converter);
        //        }
        //        return _dinkToPdfRepository;
        //    }
        //}

        public IAppUserRepository AppUser
        {
            get
            {
                if (_appUser == null)
                {
                    _appUser = new AppUserRepository(_repoContext, _appUserSortHelper, _userManager, _roleManager);
                }
                return _appUser;
            }
        }


        public IEmailSenderRepository EmailSender
        {
            get
            {
                if (_emailSender == null)
                {
                    _emailSender = new EmailSenderRepository(_emailConfig);
                }
                return _emailSender;
            }
        }


        public IRoleRepository Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_repoContext, _roleSortHelper, _roleManager);
                }
                return _role;
            }
        }





        public RepositoryWrapper(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            RepositoryContext repositoryContext,
            EmailConfiguration emailConfig,
            IOptions<EmailSettings> options,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            //IConverter converter,
            ISortHelper<AppUser> appUserSortHelper,
            ISortHelper<Intervenor> intervenorSortHelper,
            ISortHelper<PaymentOption> paymentOptionSortHelper,
            ISortHelper<Site> siteSortHelper,
            ISortHelper<IdentityRole> roleSortHelper,
            ISortHelper<Operation> operationSortHelper,
            ISortHelper<OperationType> operationTypeSortHelper,

            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_converter = converter;
            _configuration = configuration;
            _repoContext = repositoryContext;

            _appUserSortHelper = appUserSortHelper;
            _intervenorSortHelper = intervenorSortHelper;
            _paymentOptionSortHelper = paymentOptionSortHelper;
            _siteSortHelper = siteSortHelper;
            _roleSortHelper = roleSortHelper;
            _operationSortHelper = operationSortHelper;
            _operationTypeSortHelper = operationTypeSortHelper;

            _emailConfig = emailConfig;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
