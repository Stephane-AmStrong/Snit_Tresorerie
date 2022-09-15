using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PaymentOptionRepository : RepositoryBase<PaymentOption>, IPaymentOptionRepository
    {
        private ISortHelper<PaymentOption> _sortHelper;

        public PaymentOptionRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<PaymentOption> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<PaymentOption>> GetPagedListAsync(PaymentOptionParameters paymentOptionParameters)
        {
            var paymentOptions = Enumerable.Empty<PaymentOption>().AsQueryable();

            ApplyFilters(ref paymentOptions, paymentOptionParameters);

            var sortedPaymentOptions = _sortHelper.ApplySort(paymentOptions, paymentOptionParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<PaymentOption>.ToPagedList
                (
                    sortedPaymentOptions,
                    paymentOptionParameters.PageNumber,
                    paymentOptionParameters.PageSize)
                );
        }

        public async Task<PaymentOption> GetByIdAsync(Guid Id)
        {
            return await BaseFindByCondition(paymentOption => paymentOption.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<PaymentOption> GetDetailsAsync(Guid Id)
        {
            return await BaseFindByCondition(paymentOption => paymentOption.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(PaymentOption paymentOption)
        {
            return await BaseFindByCondition(x => x.Name == paymentOption.Name && x.Description == paymentOption.Description)
                .AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task CreateAsync(PaymentOption paymentOption)
        {
            await BaseCreateAsync(paymentOption);
        }

        public async Task UpdateAsync(PaymentOption paymentOption)
        {
            await BaseUpdateAsync(paymentOption);
        }

        public async Task DeleteAsync(PaymentOption paymentOption)
        {
            await BaseDeleteAsync(paymentOption);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<PaymentOption> paymentOptions, PaymentOptionParameters paymentOptionParameters)
        {
            paymentOptions = BaseFindAll();
            /*
            if (!string.IsNullOrWhiteSpace(paymentOptionParameters.AppUserId))
            {
                paymentOptions = paymentOptions.Where(x => x.AppUserId == paymentOptionParameters.AppUserId);
            }

            if (paymentOptionParameters.MinBirthday != null)
            {
                paymentOptions = paymentOptions.Where(x => x.Birthday >= paymentOptionParameters.MinBirthday);
            }

            if (paymentOptionParameters.MaxBirthday != null)
            {
                paymentOptions = paymentOptions.Where(x => x.Birthday < paymentOptionParameters.MaxBirthday);
            }

            if (paymentOptionParameters.MinCreateAt != null)
            {
                paymentOptions = paymentOptions.Where(x => x.CreateAt >= paymentOptionParameters.MinCreateAt);
            }

            if (paymentOptionParameters.MaxCreateAt != null)
            {
                paymentOptions = paymentOptions.Where(x => x.CreateAt < paymentOptionParameters.MaxCreateAt);
            }
            */
        }

        #endregion

    }
}
