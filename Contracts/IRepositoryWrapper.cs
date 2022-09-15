using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IFileRepository File { get; }

        IAccountRepository Account { get; }
        IIntervenorRepository Intervenor { get; }
        IAppUserRepository AppUser { get; }
        IPaymentOptionRepository PaymentOption { get; }
        IRoleRepository Role { get; }
        ISiteRepository Site { get; }
        IOperationRepository Operation { get; }
        IOperationTypeRepository OperationType { get; }

        //IDinkToPdfRepository PdfService { get; }
        IEmailSenderRepository EmailSender { get; }
        string Path { set; }

        Task SaveAsync();
    }
}
