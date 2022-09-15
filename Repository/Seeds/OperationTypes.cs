using Contracts;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Repository.Seeds
{
    public static class OperationTypes
    {
        public static async Task<WebApplication> SeedDefaultOperationTypesAsync(this WebApplication webApp, IConfiguration config)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();

                var salesOperation = (await repository.OperationType.GetPagedListAsync(new OperationTypeParameters { Named = config["OperationTypeSeed:SalesOperation:Name"] })).FirstOrDefault();

                if (salesOperation == null)
                {
                    salesOperation = new OperationType
                    {
                        Name = config["OperationTypeSeed:SalesOperation:Name"],
                        Description = config["OperationTypeSeed:SalesOperation:Description"],
                    };

                    await repository.OperationType.CreateAsync(salesOperation);
                }

                var purchaseOperation = (await repository.OperationType.GetPagedListAsync(new OperationTypeParameters { Named = config["OperationTypeSeed:PurchaseOperation:Name"] })).FirstOrDefault();

                if (purchaseOperation == null)
                {
                    purchaseOperation = new OperationType
                    {
                        Name = config["OperationTypeSeed:PurchaseOperation:Name"],
                        Description = config["OperationTypeSeed:PurchaseOperation:Description"],
                    };

                    await repository.OperationType.CreateAsync(purchaseOperation);
                }

                await repository.SaveAsync();
            }

            return webApp;
        }
    }
}
