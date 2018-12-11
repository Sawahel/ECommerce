using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ECommerce.ProductCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductCatalog : StatefulService
    {
        private IProductRepository _repo;

        public ProductCatalog(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repo = new ServiceFabricProductRepository(this.StateManager);

            // Populate DB with some fake products

            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Pen",
                Description = "Pen used for writing",
                Price = 3.5,
                Availability = 30
            };

            var product2 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Pencil",
                Description = "Pencil used for drawing",
                Price = 1.5,
                Availability = 50
            };

            var product3 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Rubber",
                Description = "Rubber used for erasing",
                Price = 2,
                Availability = 24
            };

            var product4 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sharpner",
                Description = "Sharpner used for sharpening",
                Price = 2.5,
                Availability = 40
            };

            await _repo.AddProduct(product1);
            await _repo.AddProduct(product2);
            await _repo.AddProduct(product3);
            await _repo.AddProduct(product4);

            IEnumerable<Product> allProducts = await _repo.GetAllProducts();

            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            //var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            //while (true)
            //{
            //    cancellationToken.ThrowIfCancellationRequested();

            //    using (var tx = this.StateManager.CreateTransaction())
            //    {
            //        var result = await myDictionary.TryGetValueAsync(tx, "Counter");

            //        ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
            //            result.HasValue ? result.Value.ToString() : "Value does not exist.");

            //        await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

            //        // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
            //        // discarded, and nothing is saved to the secondary replicas.
            //        await tx.CommitAsync();
            //    }

            //    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            //}
        }
    }
}
