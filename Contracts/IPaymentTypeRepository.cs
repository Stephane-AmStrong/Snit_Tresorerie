
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentTypeRepository
    {
        Task<PagedList<PaymentType>> GetPagedListAsync(PaymentTypeParameters paymentTypeParameters);
        Task<PaymentType> GetByIdAsync(Guid id);
        Task<PaymentType> GetDetailsAsync(Guid id);
        Task<int> CountAsync();
        Task<bool> ExistAsync(PaymentType paymentType);
        Task CreateAsync(PaymentType paymentType);
        Task UpdateAsync(PaymentType paymentType);
        Task DeleteAsync(PaymentType paymentType);
    }
}
