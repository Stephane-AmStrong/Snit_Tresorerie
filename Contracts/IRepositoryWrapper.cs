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
        IActorRepository Actor { get; }
        IAppUserRepository AppUser { get; }
        IPaymentTypeRepository PaymentType { get; }
        IRoleRepository Role { get; }
        ISiteRepository Site { get; }
        ITransactionRepository Transaction { get; }

        //IDinkToPdfRepository PdfService { get; }
        IQrCodeRepository QrCode { get; }
        IEmailSenderRepository EmailSender { get; }
        string Path { set; }

        Task SaveAsync();
    }
}
