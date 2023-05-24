namespace Services
{
    using FluentValidation;
    using Models;
    using DotnetStandardQueryBuilder.Core;
    using Repositories;
    using Services.Validators;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Configuration.Options;

    public class ProductVariantService : BaseService<ProductVariant>, IProductVariantService
    {
        private readonly IProductService _productService;

        public ProductVariantService(IAppOptions appOptions, ICacheService<ProductVariant> cacheService, IProductVariantRepository productVariantRepository, IProductService productService)
            : base(appOptions, cacheService, productVariantRepository)
        {
            _productService = productService;
        }

        public new async Task BulkUpdateAsync(List<ProductVariant> productVariants)
        {
            await OnBulkUpdating(productVariants).ConfigureAwait(false);

            await base.BulkRemoveAsync(new Filter
            {
                Operator = FilterOperator.IsEqualTo,
                Property = nameof(ProductVariant.ProductId),
                Value = productVariants.Select(x => x.ProductId).FirstOrDefault()
            }).ConfigureAwait(false);

            await base.BulkUpdateAsync(productVariants).ConfigureAwait(false);
        }

        public new async Task<IResponse<ProductVariant>> PaginateAsync(IRequest request)
        {
            var productVariants = await base.PaginateAsync(request).ConfigureAwait(false);

            if (request.Select?.Contains(nameof(ProductVariant).ToLowerInvariant()) == false)
            {
                productVariants.Items = await UpdateVariants(productVariants.Items).ConfigureAwait(false);
            }

            return new Response<ProductVariant>
            {
                Count = productVariants.Count,
                Items = productVariants.Items
            };
        }

        public new async Task<List<ProductVariant>> GetAsync(IRequest request = null)
        {
            var productVariants = await base.GetAsync(request).ConfigureAwait(false);

            return await UpdateVariants(productVariants).ConfigureAwait(false);
        }

        public new async Task<ProductVariant> GetAsync(string id)
        {
            var productVariant = await base.GetAsync(id).ConfigureAwait(false);

            return (await UpdateVariants(new List<ProductVariant> { productVariant }).ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<List<Product>> UpdateProducts(List<Product> products)
        {
            var productIds = products.ConvertAll(x => x.Id);

            var productVariantDocuments = await GetAsync(new Request
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsContainedIn,
                    Property = nameof(ProductVariant.ProductId),
                    Value = productIds
                }
            }).ConfigureAwait(false);

            var allProductVariants = productVariantDocuments.ConvertAll(x => x);

            foreach (var product in products)
            {
                var productVariants = allProductVariants.Where(x => x.ProductId == product.Id && x.Type != ProductVariantType.Default).Select(x =>
                {
                    x.Values ??= new Dictionary<string, object>();
                    if (x.Values.ContainsKey(nameof(ProductVariant.Id)))
                    {
                        x.Values.Remove(nameof(ProductVariant.Id));
                    }
                    if (!x.Values.ContainsKey(nameof(ProductVariant.Id).ToLowerInvariant()))
                    {
                        x.Values.Add(nameof(ProductVariant.Id).ToLowerInvariant(), x.Id);
                    }
                    if (!x.Values.ContainsKey(nameof(ProductVariant.Type).ToLowerInvariant()))
                    {
                        x.Values.Add(nameof(ProductVariant.Type).ToLowerInvariant(), x.Type);
                    }
                    if (!x.Values.ContainsKey(nameof(ProductVariant.VariantPrice).ToLowerInvariant()))
                    {
                        x.Values.Add(nameof(ProductVariant.VariantPrice).ToLowerInvariant(), x.VariantPrice);
                    }
                    if (!x.Values.ContainsKey(nameof(ProductVariant.Price).ToLowerInvariant()))
                    {
                        x.Values.Add(nameof(ProductVariant.Price).ToLowerInvariant(), x.Price);
                    }
                    return x.Values;
                }).ToList();

                product.Metadata.Add(_productService.VariantsPropertyName.ToLowerInvariant(), productVariants);
            }

            return products;
        }

        protected override async Task OnBulkCreating(List<ProductVariant> productVariants)
        {
            var productVariantValidator = new ProductVariantValidator();
            foreach (var productVariant in productVariants)
            {
                productVariantValidator.ValidateAndThrow(productVariant);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }

        protected override async Task<ProductVariant> OnCreating(ProductVariant productVariant)
        {
            return await Task.FromResult(productVariant).ConfigureAwait(false);
        }

        private async Task<List<ProductVariant>> UpdateVariants(List<ProductVariant> productVariants)
        {
            var productIds = productVariants.ConvertAll(x => x.ProductId);

            var products = await _productService.GetAsync(new Request
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsContainedIn,
                    Property = nameof(Product.Id),
                    Value = productIds
                }
            }).ConfigureAwait(false);

            foreach (var productVariant in productVariants)
            {
                productVariant.Product = products.Find(x => x.Id == productVariant.ProductId);
                if (productVariant.Product != null)
                {
                    productVariant.Price = productVariant.VariantPrice ?? productVariant.Product.Price;
                }
            }

            return productVariants;
        }
    }
}
