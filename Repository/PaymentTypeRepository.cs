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
    public class PaymentTypeRepository : RepositoryBase<PaymentType>, IPaymentTypeRepository
    {
        private ISortHelper<PaymentType> _sortHelper;

        public PaymentTypeRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<PaymentType> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<PaymentType>> GetPagedListAsync(PaymentTypeParameters paymentTypeParameters)
        {
            var paymentTypes = Enumerable.Empty<PaymentType>().AsQueryable();

            ApplyFilters(ref paymentTypes, paymentTypeParameters);

            var sortedPaymentTypes = _sortHelper.ApplySort(paymentTypes, paymentTypeParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<PaymentType>.ToPagedList
                (
                    sortedPaymentTypes,
                    paymentTypeParameters.PageNumber,
                    paymentTypeParameters.PageSize)
                );
        }

        public async Task<PaymentType> GetByIdAsync(Guid Id)
        {
            return await BaseFindByCondition(paymentType => paymentType.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<PaymentType> GetDetailsAsync(Guid Id)
        {
            return await BaseFindByCondition(paymentType => paymentType.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(PaymentType paymentType)
        {
            return await BaseFindByCondition(x => x.Name == paymentType.Name && x.Description == paymentType.Description)
                .AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task CreateAsync(PaymentType paymentType)
        {
            await BaseCreateAsync(paymentType);
        }

        public async Task UpdateAsync(PaymentType paymentType)
        {
            await BaseUpdateAsync(paymentType);
        }

        public async Task DeleteAsync(PaymentType paymentType)
        {
            await BaseDeleteAsync(paymentType);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<PaymentType> paymentTypes, PaymentTypeParameters paymentTypeParameters)
        {
            paymentTypes = BaseFindAll();
            /*
            if (!string.IsNullOrWhiteSpace(paymentTypeParameters.AppUserId))
            {
                paymentTypes = paymentTypes.Where(x => x.AppUserId == paymentTypeParameters.AppUserId);
            }

            if (paymentTypeParameters.MinBirthday != null)
            {
                paymentTypes = paymentTypes.Where(x => x.Birthday >= paymentTypeParameters.MinBirthday);
            }

            if (paymentTypeParameters.MaxBirthday != null)
            {
                paymentTypes = paymentTypes.Where(x => x.Birthday < paymentTypeParameters.MaxBirthday);
            }

            if (paymentTypeParameters.MinCreateAt != null)
            {
                paymentTypes = paymentTypes.Where(x => x.CreateAt >= paymentTypeParameters.MinCreateAt);
            }

            if (paymentTypeParameters.MaxCreateAt != null)
            {
                paymentTypes = paymentTypes.Where(x => x.CreateAt < paymentTypeParameters.MaxCreateAt);
            }
            */
        }

        #endregion

    }
}
