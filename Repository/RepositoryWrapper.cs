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
        private IActorRepository _actorRepository;
        private IAppUserRepository _appUser;
        private IPaymentTypeRepository _paymentTypeRepository;
        private ISiteRepository _siteRepository;
        private ITransactionRepository _transactionRepository;
        //private IDinkToPdfRepository _dinkToPdfRepository;
        private IQrCodeRepository _qrCodeRepository;

        private IEmailSenderRepository _emailSender;
        private IWebHostEnvironment _webHostEnvironment;
        private IRoleRepository _role;

        //private readonly IConverter _converter;
        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        //private IOptions<EmailSettings> _emailSettings;

        private readonly ISortHelper<AppUser> _appUserSortHelper;
        private readonly ISortHelper<Actor> _actorSortHelper;
        private readonly ISortHelper<PaymentType> _paymentTypeSortHelper;
        private readonly ISortHelper<Site> _siteSortHelper;
        private readonly ISortHelper<Transaction> _transactionSortHelper;
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
        public IActorRepository Actor
        {
            get
            {
                if (_actorRepository == null)
                {
                    _actorRepository = new ActorRepository(_repoContext, _actorSortHelper);
                }
                return _actorRepository;
            }
        }

        public IPaymentTypeRepository PaymentType
        {
            get
            {
                if (_paymentTypeRepository == null)
                {
                    _paymentTypeRepository = new PaymentTypeRepository(_repoContext, _paymentTypeSortHelper);
                }
                return _paymentTypeRepository;
            }
        }

        public ITransactionRepository Transaction
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository = new TransactionRepository(_repoContext, _transactionSortHelper);
                }
                return _transactionRepository;
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

        public IQrCodeRepository QrCode
        {
            get
            {
                if (_qrCodeRepository == null)
                {
                    _qrCodeRepository = new QrCodeRepository();
                }
                return _qrCodeRepository;
            }
        }

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
            ISortHelper<Actor> actorSortHelper,
            ISortHelper<PaymentType> paymentTypeSortHelper,
            ISortHelper<Site> siteSortHelper,
            ISortHelper<IdentityRole> roleSortHelper,
            ISortHelper<Transaction> transactionSortHelper,

            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_converter = converter;
            _configuration = configuration;
            _repoContext = repositoryContext;

            _appUserSortHelper = appUserSortHelper;
            _actorSortHelper = actorSortHelper;
            _paymentTypeSortHelper = paymentTypeSortHelper;
            _siteSortHelper = siteSortHelper;
            _roleSortHelper = roleSortHelper;
            _transactionSortHelper = transactionSortHelper;

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
