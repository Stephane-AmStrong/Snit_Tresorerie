
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentOptionRepository
    {
        Task<PagedList<PaymentOption>> GetPagedListAsync(PaymentOptionParameters paymentOptionParameters);
        Task<PaymentOption> GetByIdAsync(Guid id);
        Task<PaymentOption> GetDetailsAsync(Guid id);
        Task<int> CountAsync();
        Task<bool> ExistAsync(PaymentOption paymentOption);
        Task CreateAsync(PaymentOption paymentOption);
        Task UpdateAsync(PaymentOption paymentOption);
        Task DeleteAsync(PaymentOption paymentOption);
    }
}
